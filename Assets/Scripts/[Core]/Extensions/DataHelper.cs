using ShutEye.Data;
using ShutEye.Data.Provider;
using ShutEye.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static partial class AssemblyReflectionHelper
{
    /// <summary>
    /// Создает и возвращает экземпляры всех IDataMember в решении
    /// </summary>
    /// <returns></returns>
    public static ICollection<DataBox> CreateAllDataMebersInSoluton()
    {
        var res = new List<DataBox>();
        foreach (var type in GetAllTypesInSolution<DataBox>())
        {
            var obj = (DataBox)Activator.CreateInstance(type);
            res.Add(obj);
        }
        return res;
    }

    public static byte[] Serialise(object obj)
    {
        byte[] res;
        //Serialize the data to a binary stream
        using (var stream = new MemoryStream())
        {
            (new BinaryFormatter()).Serialize(stream, obj);
            stream.Flush();
            res = stream.ToArray();
        }
        return res;
    }

    public static T Deserialse<T>(byte[] data) where T : class
    {
        using (var stream = new MemoryStream(data))
        {
            return (new BinaryFormatter()).Deserialize(stream) as T;
        }
    }

    public static void SavePicture(string pathImage, string namePicture)
    {
        var folderToSave = Application.persistentDataPath;
        try
        {
            Debug.Log(folderToSave);
            Texture2D pict = Resources.Load(pathImage) as Texture2D;

            if (Directory.Exists(folderToSave))
            {
                if (File.Exists(Path.Combine(folderToSave, namePicture + ".jpg")))
                {
                    File.Delete(Path.Combine(folderToSave, namePicture + ".jpg"));
                }
            }
            else
            {
                Directory.CreateDirectory(folderToSave);
                Debug.Log(File.Exists(Path.Combine(folderToSave, "")) ? Path.Combine(folderToSave, "NEW DIRECTORY PATH") : "nety takogo pyti");
            }

            byte[] bytes = pict.EncodeToJPG();
            File.WriteAllBytes(Path.Combine(folderToSave, namePicture + ".jpg"), bytes);
            Debug.Log("___+" + Path.Combine(folderToSave, namePicture));
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    /// <summary>
    /// Все не абстрактные типы классов реализующие T в решении
    /// </summary>
    /// <returns></returns>
    public static ICollection<Type> GetAllTypesInSolution<T>(bool IncludingAbstract = false)
    {
        List<Assembly> scriptAssemblies = AppDomain.CurrentDomain.GetAssemblies().ToList();
        if (!scriptAssemblies.Contains(Assembly.GetExecutingAssembly()))
            scriptAssemblies.Add(Assembly.GetExecutingAssembly());
        return (from assembly in scriptAssemblies from type in assembly.GetTypes().Where(t => t.IsClass && typeof(T).IsAssignableFrom(t)) where IncludingAbstract || !type.IsAbstract select type).ToList();
    }

#if UNITY_EDITOR

    public static IDataEditorStorage LoadAllPrividers(IDataEditorStorage storage = null)
    {
        storage = storage ?? new LocalDataBoxStorage();
        AllDataBox().ForEach(provider =>
          {
              storage.RegisterProvider(provider);
          });
        return storage;
    }

    public static IEnumerable<DataBox> AllDataBox()
    {
        return UnityEditor.AssetDatabase.FindAssets("", new[] { "Assets/DATA" }).
            Select(guid =>
           {
               var path = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
               return (DataBox)UnityEditor.AssetDatabase.LoadAssetAtPath(path, typeof(DataBox));
           });
    }

    public static T CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        string path = UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject);
        if (path == "")
        {
            path = "Assets";
        }
        else if (Path.GetExtension(path) != "")
        {
            path = path.Replace(Path.GetFileName(UnityEditor.AssetDatabase.GetAssetPath(UnityEditor.Selection.activeObject)), "");
        }

        string assetPathAndName = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path + "/New " + typeof(T).ToString() + ".asset");

        UnityEditor.AssetDatabase.CreateAsset(asset, assetPathAndName);

        UnityEditor.AssetDatabase.SaveAssets();
        UnityEditor.EditorUtility.FocusProjectWindow();
        UnityEditor.Selection.activeObject = asset;

        return asset;
    }

    public static string GetUniqueAssetPathNameOrFallback(string filename)
    {
        string path;
        try
        {
            // Private implementation of a file naming function which puts the file at the selected path.
            System.Type assetdatabase = typeof(UnityEditor.AssetDatabase);
            path = (string)assetdatabase.GetMethod("GetUniquePathNameAtSelectedPath",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Invoke(assetdatabase, new object[] { filename });
        }
        catch
        {
            // Protection against implementation changes.
            path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath("Assets/" + filename);
        }
        return path;
    }

#endif
}