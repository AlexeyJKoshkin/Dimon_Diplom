using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SpLink_Scene
{
    None,
    This,
    All
}

public class SpLink_Wizard : ScriptableWizard
{
    private SpAtlas atlas;

    private SpLink_Scene inScenes = SpLink_Scene.All;

    private bool inPrefabs = true;

    private bool inScriptableObjects = true;

    private bool inAnimationClips = true;

    private Sprite[] sourceSprites;

    private Sprite[] packedSprites;

    private Vector2 scrollPosition;

    public static SpLink_Wizard Open(SpAtlas atlas)
    {
        if (atlas != null)
        {
            var path = AssetDatabase.GUIDToAssetPath(atlas.Identifier);
            var texture = AssetDatabase.LoadMainAssetAtPath(path) as Texture2D;

            if (texture != null)
            {
                var allSourceSprites = new List<Sprite>();
                var allPackedSprites = new List<Sprite>();

                for (var i = 0; i < atlas.Sources.Count; i++)
                {
                    var source = atlas.Sources[i];
                    var sourceSprites = source.Sprites;

                    if (sourceSprites.Count > 0)
                    {
                        allSourceSprites.AddRange(sourceSprites);
                        allPackedSprites.AddRange(source.FindSprites(atlas.Sprites, sourceSprites));
                    }
                }

                if (allSourceSprites.Count > 0)
                {
                    var linker = ScriptableWizard.DisplayWizard<SpLink_Wizard>("Sprite Linker", "Link", "Cancel");

                    linker.atlas = atlas;
                    linker.sourceSprites = allSourceSprites.ToArray();
                    linker.packedSprites = allPackedSprites.ToArray();

                    return linker;
                }
            }
        }

        return null;
    }

    public void OnGUI()
    {
        if (atlas != null)
        {
            EditorGUILayout.HelpBox("This tool will replace all your non-atlas sprites with these atlas sprites (e.g. in your prefabs & scripts)", MessageType.Info);

            EditorGUILayout.Separator();

            inScenes = (SpLink_Scene)EditorGUILayout.EnumPopup("In Scenes", inScenes);

            inPrefabs = EditorGUILayout.Toggle("In Prefabs", inPrefabs);

            inScriptableObjects = EditorGUILayout.Toggle("In Scriptable Objects", inScriptableObjects);

            inAnimationClips = EditorGUILayout.Toggle("In Animation Clips", inAnimationClips);

            EditorGUILayout.Separator();

            EditorGUILayout.LabelField("Replace", EditorStyles.boldLabel);

            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
            {
                for (var i = 0; i < sourceSprites.Length; i++)
                {
                    var rect = SpHelper.Reserve();
                    var rect0 = rect; rect0.xMax = rect0.center.x - 20;
                    var rect1 = rect; rect1.xMin = rect1.center.x + 20;
                    var rect2 = rect; rect2.xMin = rect2.center.x - 20; rect2.xMax = rect2.center.x + 20;

                    if (sourceSprites[i] == null)
                    {
                        GUI.Box(rect0, "", SpHelper.RedBox);
                    }

                    sourceSprites[i] = (Sprite)EditorGUI.ObjectField(rect0, sourceSprites[i], typeof(Sprite), true);
                    packedSprites[i] = (Sprite)EditorGUI.ObjectField(rect1, packedSprites[i], typeof(Sprite), true);

                    EditorGUI.LabelField(rect2, "With");
                }
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Separator();

            DrawButtons();
        }
        else
        {
            Close();
        }
    }

    private Scene currentScene = SceneManager.GetActiveScene();

    private void DrawButtons()
    {
        var rect = SpHelper.Reserve();
        var rect0 = rect; rect0.xMax = rect0.center.x - 1;
        var rect1 = rect; rect1.xMin = rect1.center.x + 1;

        if (GUI.Button(rect0, "Link Sprites") == true)
        {
            switch (inScenes)
            {
                case SpLink_Scene.This:
                    {
                        LinkInScene();
                    }
                    break;

                case SpLink_Scene.All:
                    {
                        if (EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                        {
                            currentScene = SceneManager.GetActiveScene();
                            var sceneGuids = AssetDatabase.FindAssets("t:scene");

                            foreach (var sceneGuid in sceneGuids)
                            {
                                var scenePath = AssetDatabase.GUIDToAssetPath(sceneGuid);

                                if (EditorSceneManager.OpenScene(scenePath) != default(Scene))
                                {
                                    LinkInScene();

                                    EditorSceneManager.SaveOpenScenes();
                                }
                            }

                            EditorSceneManager.OpenScene(currentScene.name);
                        }
                    }
                    break;
            }

            if (inPrefabs == true)
            {
                LinkInPrefabs();
            }

            if (inScriptableObjects == true)
            {
                LinkInScriptableObjects();
            }

            if (inAnimationClips == true)
            {
                LinkInAnimationClips();
            }

            EditorSceneManager.MarkSceneDirty(currentScene);
        }

        if (GUI.Button(rect1, "Cancel") == true)
        {
            Close();
        }
    }

    private void LinkInScene()
    {
        var gameObjects = Object.FindObjectsOfType<GameObject>();

        foreach (var gameObject in gameObjects)
        {
            var transform = gameObject.transform;

            if (transform.parent == null)
            {
                LinkInTransform(transform);
            }
        }
    }

    private void LinkInPrefabs()
    {
        var guids = AssetDatabase.FindAssets("t:prefab");

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var prefab = AssetDatabase.LoadMainAssetAtPath(path) as GameObject;

            if (prefab != null)
            {
                LinkInTransform(prefab.transform);
            }
        }
    }

    private void LinkInScriptableObjects()
    {
        var guids = AssetDatabase.FindAssets("t:scriptableobject");

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var scriptableObject = AssetDatabase.LoadMainAssetAtPath(path) as ScriptableObject;

            if (scriptableObject != null)
            {
                if (LinkInObject(scriptableObject) == true)
                {
                    EditorUtility.SetDirty(scriptableObject);
                }
            }
        }
    }

    private void LinkInAnimationClips()
    {
        var guids = AssetDatabase.FindAssets("t:animationclip");

        foreach (var guid in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);
            var animationClip = AssetDatabase.LoadMainAssetAtPath(path) as AnimationClip;

            if (animationClip != null)
            {
                var bindings = AnimationUtility.GetObjectReferenceCurveBindings(animationClip);

                foreach (var binding in bindings)
                {
                    var keys = AnimationUtility.GetObjectReferenceCurve(animationClip, binding);
                    var dirty = false;

                    for (var i = 0; i < keys.Length; i++)
                    {
                        var key = keys[i];
                        var sprite = key.value as Sprite;

                        if (Link(ref sprite) == true)
                        {
                            key.value = sprite;

                            keys[i] = key;

                            dirty = true;
                        }
                    }

                    if (dirty == true)
                    {
                        AnimationUtility.SetObjectReferenceCurve(animationClip, binding, keys); EditorUtility.SetDirty(animationClip);
                    }
                }
            }
        }
    }

    private void LinkInTransform(Transform t)
    {
        var components = t.GetComponents<Component>();

        foreach (var component in components)
        {
            LinkInComponent(component);
        }

        for (var i = 0; i < t.childCount; i++)
        {
            LinkInTransform(t.GetChild(i));
        }
    }

    private void LinkInComponent(Component c)
    {
        // Sprite renderers don't store the sprite in a field, so manually replace it
        var spriteRenderer = c as SpriteRenderer;

        if (spriteRenderer != null)
        {
            var sprite = spriteRenderer.sprite;

            if (Link(ref sprite) == true)
            {
                spriteRenderer.sprite = sprite; EditorUtility.SetDirty(spriteRenderer);
            }

            return;
        }

        // Images don't store the sprite in a field, so manually replace it
        var image = c as UnityEngine.UI.Image;

        if (image != null)
        {
            var sprite = image.sprite;

            if (Link(ref sprite) == true)
            {
                image.sprite = sprite; EditorUtility.SetDirty(image);
            }

            return;
        }

        if (LinkInObject(c) == true)
        {
            EditorUtility.SetDirty(c);
        }
    }

    private bool LinkInObject(Object o)
    {
        var dirty = false;

        if (o != null)
        {
            var fields = o.GetType().GetFields(System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);

            foreach (var field in fields)
            {
                var type = field.FieldType;

                if (type == typeof(Sprite))
                {
                    var sprite = (Sprite)field.GetValue(o);

                    if (Link(ref sprite) == true)
                    {
                        field.SetValue(o, sprite);

                        dirty = true;
                    }
                }
            }
        }

        return dirty;
    }

    private bool Link(ref Sprite sprite)
    {
        if (sprite != null)
        {
            var index = System.Array.IndexOf(sourceSprites, sprite);

            if (index != -1)
            {
                sprite = packedSprites[index];

                if (sprite != null)
                {
                    return true;
                }
            }
        }

        return false;
    }
}