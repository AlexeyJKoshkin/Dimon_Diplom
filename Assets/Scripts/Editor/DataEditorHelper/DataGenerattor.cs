using UnityEditor;
using UnityEngine;

public class DataGenerator : CodeGenerators
{
    protected override string FileName
    {
        get
        {
            return _typeName;
        }
    }

    public override string FullPathToFile
    {
        get
        {
            return PathToFolder + "/" + _typeName + ".cs";
        }
    }

    protected override string PathToFolder { get { return _pathToDataFolder; } }

    private string _pathToDataFolder;

    public DataGenerator()
    {
        _pathToDataFolder = EditorPrefs.GetString("Path_Scripts_Generate", "");
        if (string.IsNullOrEmpty(PathToFolder))
        {
            Debug.LogWarning("No path for Provider pathes");

            _pathToDataFolder = Application.dataPath + "/Scripts/Data" + "/Objects";
            Debug.LogWarningFormat("use default {0}", PathToFolder);
        }
        else
        {
            _pathToDataFolder += "/Objects";
        }
    }

    protected override string Using { get { return "using System;"; } }

    private string ClassHeader()
    {
        return string.Format("public class {0} : {1}", _typeName, _parentType) + "\n{}";
    }

    public override string Content()
    {
        return Using + "\n \n"
            + "[Serializable]\n"
            + ClassHeader();
    }
}