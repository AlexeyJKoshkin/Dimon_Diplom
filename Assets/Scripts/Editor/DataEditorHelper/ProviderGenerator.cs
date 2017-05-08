using UnityEditor;
using UnityEngine;

public class ProviderGenerator : CodeGenerators
{
    protected override string PathToFolder { get { return _pathToProviderFolder; } }
    private string _pathToProviderFolder;
    public override string FullPathToFile { get { return PathToFolder + "/" + FileName + ".cs"; } }
    protected override string FileName { get { return _typeName + "Provider"; } }
    protected override string Using { get { return "using GameKit.Data.ProviderData;\n\n"; } }

    private string ClassHeader()
    {
        return string.Format("public class {0} : DataBox<{1}>", FileName, _typeName) + "\n{}";
    }

    public ProviderGenerator()
    {
        _pathToProviderFolder = EditorPrefs.GetString("Path_Scripts_Generate", "");
        if (string.IsNullOrEmpty(PathToFolder))
        {
            Debug.LogWarning("No path for Provider pathes");

            _pathToProviderFolder = Application.dataPath + "/Scripts/Data" + "/Providers";
            EditorPrefs.SetString("Path_Scripts_Generate", _pathToProviderFolder);
            Debug.LogWarningFormat("use default {0}", PathToFolder);
        }
        else
            _pathToProviderFolder += "/Providers";
    }

    public override string Content()
    {
        return Using + "\n \n"
        + ClassHeader();
    }
}