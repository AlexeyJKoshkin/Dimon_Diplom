using ShutEye.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShutEye.Core
{
    public partial class GameCore
    {
        public static ICollection<IEnumerator> LoadLevelScenes(IEnumerable<string> sScene, IEnumerable<string> dScene = null)
        {
            var res = new List<IEnumerator>();
            sScene.ForEach(name => res.Add(LoadAsync(name, LoadSceneMode.Additive)));
            if (dScene != null)
                dScene.ForEach(name => res.Add(LoadAsync(name, LoadSceneMode.Additive)));
            return res;
        }

        public static ICollection<IEnumerator> UnloadLevelScenes(IEnumerable<string> sScene, IEnumerable<string> dScene = null)
        {
            var res = new List<IEnumerator>();
            sScene.ForEach(name => res.Add(UnloadScene(name)));
            if (dScene != null)
                dScene.ForEach(name => res.Add(UnloadScene(name)));
            return res;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneInfo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private static IEnumerator LoadAsync(string sceneInfo, LoadSceneMode type = LoadSceneMode.Single)
        {
            Debug.Log("[SCENE] " + sceneInfo + " loading");
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneInfo, type); // начинаем грузить
            while (!operation.isDone)
            {
                yield return operation.isDone;
                //Debug.Log("[ScL]: " + operation.progress);
            }
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(sceneInfo)); //активируем
            Debug.Log("[SCENE]\"" + sceneInfo + "\" load done");
        }

        private static IEnumerator UnloadScene(string sceneName, bool clearRes = false)
        {
            yield return SceneManager.UnloadSceneAsync(sceneName);
            if (clearRes)
                Resources.UnloadUnusedAssets();
            yield return null;
        }
    }
}