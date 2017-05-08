using UnityEngine;

public class ViewerGenerator : CodeGenerators
{
    protected override string Using
    {
        get
        {
            return "using UnityEditor;\n" +
                    "using UnityEngine;\n";
        }
    }

    protected override string FileName
    {
        get
        {
            return _typeName + "Viewer";
        }
    }

    protected override string PathToFolder
    {
        get
        {
            return _pathToViewerFolder;
        }
    }

    public override string FullPathToFile
    {
        get
        {
            return PathToFolder + "/" + FileName + ".cs";
        }
    }

    private string _pathToViewerFolder;

    public ViewerGenerator()
    {
        _pathToViewerFolder = UnityEditor.EditorPrefs.GetString("Path_Scripts_Viewers", "");
        if (string.IsNullOrEmpty(PathToFolder))
        {
            Debug.LogWarning("No path for Viewers pathes");

            _pathToViewerFolder = Application.dataPath + "/Scripts/Data/Editor" + "/Viewers";
            Debug.LogWarningFormat("use default {0}", PathToFolder);
        }
        else
            _pathToViewerFolder += "/Viewers";
    }

    public override string Content()
    {
        return Using
                + "\n \n"
                + string.Format("[ViewerData (typeof({0}}))]", _typeName)
                + ClassHeader();
    }

    private string ClassHeader()
    {
        return
            string.Format("public class {0} : BaseViewerData<{0}>\n", _typeName)
                + "{}";
    }
}