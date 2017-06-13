using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace ShutEye.Core
{
    public partial class GameCore
    {
        // private static readonly Dictionary<string, List<GameObject>> _mobs = new Dictionary<string, List<GameObject>>();

        private static readonly Dictionary<GameObject, List<GameObject>> _objects = new Dictionary<GameObject, List<GameObject>>();

        private static readonly Regex _atlasNumberRegex = new Regex("[\\s_]\\d+$");

        private static readonly Dictionary<string, Sprite> _cashadSprites = new Dictionary<string, Sprite>();

        private static readonly Dictionary<string, GameObject> _cashedPrefs = new Dictionary<string, GameObject>();

        public static T LoadObject<T>(string path, Vector3? pos = null, Quaternion? rotation = null) where T : Component
        {
            if (!_cashedPrefs.ContainsKey(path))
            {
                var prefab = Resources.Load<GameObject>(path);
                if (prefab == null)
                {
                    throw new Exception(string.Format("No prefab path {0}", path));
                }
                _cashedPrefs.Add(path, prefab);
            }
            var res = GetInstance(_cashedPrefs[path], pos, rotation);
            return res == null ? null : res.GetComponent<T>();
        }

        public static void SetInstance(GameObject go, string path)
        {
            var prefKey = _cashedPrefs[path];
            if (_objects.ContainsKey(prefKey))
            {
                _objects[prefKey].Add(go);
            }
            else
            {
                _objects.Add(prefKey, new List<GameObject>() { go });
            }
            //go.transform.position = new Vector3(-400, -400, -400);
            go.SetActive(false);
        }

        public static GameObject GetInstance(GameObject prefab, Vector3? pos = null, Quaternion? rotation = null)
        {
            if (_objects.ContainsKey(prefab))
            {
                var instances = _objects[prefab];
                if (instances.Any())
                {
                    var inst = instances[0];
                    instances.Remove(inst);
                    inst.SetActive(true);
                    inst.transform.position = pos ?? Vector3.zero;
                    inst.transform.rotation = rotation ?? Quaternion.identity;
                    return inst;
                }
            }
            return Instantiate(prefab, pos ?? Vector3.zero, rotation ?? Quaternion.identity) as GameObject;
        }

        public static void ClearPrafebPool()
        {
            foreach (var keyValuePair in _objects)
            {
                foreach (var o in keyValuePair.Value)
                {
                    Destroy(o);
                }
            }
            _objects.Clear();
            _cashadSprites.Clear();
            _cashedPrefs.Clear();
            Resources.UnloadUnusedAssets();
        }

        #region SpriteLoader

        internal void LoadSprite(string avatarSprite, Action<Sprite> onLoadSprite)
        {
            avatarSprite = avatarSprite.Replace("\r", "");
            if (_cashadSprites.ContainsKey(avatarSprite))
            {
                if (onLoadSprite != null)
                    onLoadSprite.Invoke(_cashadSprites[avatarSprite]);
            }
            else
            {
                if (onLoadSprite != null)
                    StartCoroutine(LoadSpriteCoroutine(avatarSprite, onLoadSprite));
            }
        }

        private IEnumerator LoadSpriteCoroutine(string url, Action<Sprite> Onfinish)
        {
            if (string.IsNullOrEmpty(url)) yield break;
            var www = new WWW(url);
            yield return www;
            if (string.IsNullOrEmpty(www.error))
            {
                var sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height),
                    new Vector2(0, 0));
                _cashadSprites.Add(url, sprite);
                Onfinish.Invoke(sprite);
            }
            else
            {
                Debug.LogError(url);
                Debug.LogError(www.error);
            }
            www.Dispose();
        }

        public static Sprite LoadSprite(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                return null;
            }
            path = path.Trim();

            var idx = path.IndexOf(":");

            var res = idx > 0
                ? LoadSpriteFromAtlas(path.Substring(0, idx), path.Substring(idx + 1, path.Length - idx - 1))
                : LoadSpriteManual(path);

            return res;
        }

        private static Sprite LoadSpriteManual(string path)
        {
            if (_cashadSprites.ContainsKey(path))
                return _cashadSprites[path];

            var sprite = Resources.Load<Sprite>(path);

            if (sprite != null && !_cashadSprites.ContainsKey(path))
                _cashadSprites.Add(path, sprite);

            return sprite;
        }

        private static Sprite LoadSpriteFromAtlas(string atlasPath, string spriteName)
        {
            var atlasName = Path.GetFileName(atlasPath);
            if (string.IsNullOrEmpty(atlasName))
                return null;

            var match = _atlasNumberRegex.Match(atlasName);

            Sprite loadedSprite = null;

            // Попробуем загрузить сразу
            loadedSprite = GetSpriteFromAtlas(atlasPath, spriteName).Sprite;
            if (loadedSprite != null)
                return loadedSprite;

            var baseAtlasPath = atlasPath;

            // Смотрим, может по ошибке указали номер атласа
            if (match.Success)
            {
                baseAtlasPath = atlasPath.Substring(0, atlasPath.Length - match.Length);
                loadedSprite = GetSpriteFromAtlas(baseAtlasPath, spriteName).Sprite;
                if (loadedSprite != null)
                    return loadedSprite;
            }

            // Ищем в спрайтах, номер которых отделен пробелом
            var withSpace = GetSpriteFromManyAtlases(baseAtlasPath, spriteName, " ");
            if (withSpace.Sprite != null)
                return withSpace.Sprite;

            // Ищем в спрайтах, номер которых отделен подчеркиванием
            var withUnderline = GetSpriteFromManyAtlases(baseAtlasPath, spriteName, "_");
            return withUnderline.Sprite;
        }

        private static GetSpriteFromAtlasResult GetSpriteFromManyAtlases(string atlasPath, string spriteName, string separator, int numb = 1)
        {
            var res = GetSpriteFromAtlas(String.Format("{0}{1}{2}", atlasPath, separator, numb), spriteName);
            if (res.Sprite != null || !res.AtlasFound)
                return res;
            return GetSpriteFromManyAtlases(atlasPath, spriteName, separator, numb + 1);
        }

        private static GetSpriteFromAtlasResult GetSpriteFromAtlas(string atlasPath, string spriteName)
        {
            var fullName = String.Format("{0}:{1}", atlasPath, spriteName);

            if (_cashadSprites.ContainsKey(fullName))
                return new GetSpriteFromAtlasResult(true, _cashadSprites[fullName]);

            Sprite[] sprites = Resources.LoadAll<Sprite>(atlasPath);
            if (!sprites.Any())
                return new GetSpriteFromAtlasResult(false, null);

            foreach (var sprite in sprites)
            {
                var loadedSpriteFullName = String.Format("{0}:{1}", atlasPath, sprite.name);
                if (!_cashadSprites.ContainsKey(loadedSpriteFullName))
                    _cashadSprites.Add(loadedSpriteFullName, sprite);

                if (sprite.name.Equals(spriteName))
                {
                    return new GetSpriteFromAtlasResult(true, sprite);
                }
            }
            return new GetSpriteFromAtlasResult(true, null);
        }

        internal static GameObject GetItem(string prefab)
        {
            return Resources.Load<GameObject>(prefab);
        }

        private struct GetSpriteFromAtlasResult
        {
            public bool AtlasFound { get; private set; }
            public Sprite Sprite { get; private set; }

            public GetSpriteFromAtlasResult(bool atlasFound, Sprite sprite)
            {
                AtlasFound = atlasFound;
                Sprite = sprite;
            }
        }
    }

    #endregion SpriteLoader
}