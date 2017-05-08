using System;
using System.IO;
using UnityEngine;

public abstract class CodeGenerators
{
    protected abstract string Using { get; }

    protected abstract string FileName { get; }
    protected abstract string PathToFolder { get; }

    public abstract string Content();

    public abstract string FullPathToFile { get; }

    protected string _typeName;

    protected Type _parentType;

    public void SetType(string newNameType, Type parent)
    {
        _typeName = newNameType;
        _parentType = parent;
    }

    public bool Check()
    {
        if (!Directory.Exists(PathToFolder))
        {
            Directory.CreateDirectory(PathToFolder);
            return true;
        }

        if (File.Exists(FullPathToFile))
        {
            Debug.LogWarningFormat("{0} exists", FullPathToFile);
            return false;
        }
        return true;
    }

    public bool Generate()
    {
        if (Check())
        {
            File.WriteAllText(FullPathToFile, Content());
            return true;
        }
        return false;
    }
}