///////////////////////////////////////////////////////////////////////////////
///
/// ScriptPrescription.cs
///
/// (c)2013 Kim, Hyoun Woo
///
///////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace UnityQuickSheet
{
    [Serializable]
    public class ScriptPrescription
    {
        public string className = string.Empty;
        public string spreadsheetName = string.Empty;
        public string worksheetClassName = string.Empty; // used for ScriptableObject class name.
        public string dataClassName = string.Empty;
        public string assetFileCreateFuncName = string.Empty;
        public string template = string.Empty;

        public string importedFilePath = string.Empty; // should start with "Assets" not full path
        public string assetFilepath = string.Empty; // should start with "Assets" not full path
        public string assetPostprocessorClass = string.Empty;

        public MemberFieldData[] memberFields;

        /// <summary>
        /// Reserved for future usage to make it easy for explicitly converting.
        /// </summary>
        public Dictionary<string, string> mStringReplacements = new Dictionary<string, string>();
    }

    /// <summary>
    /// Represent type of an each cell.
    /// </summary>
    public enum CellType
    {
        Undefined,
        String,
        Short,
        Int,
        Long,
        Float,
        Double,
        Enum,
        Bool,
    }

    public class MemberFieldData
    {
        public CellType type = CellType.Undefined;
        private string name;

        public static bool Valid(string title)
        {
            return Regex.IsMatch(title, @"\s*(:string:integer)", RegexOptions.IgnoreCase);
        }

        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public string Type
        {
            get
            {
                switch (type)
                {
                    case CellType.String:
                        return "string";

                    case CellType.Short:
                        return "short";

                    case CellType.Int:
                        return "int";

                    case CellType.Long:
                        return "long";

                    case CellType.Float:
                        return "float";

                    case CellType.Double:
                        return "double";

                    case CellType.Enum:
                        return "enum";

                    case CellType.Bool:
                        return "bool";

                    default:
                        return "string";
                }
            }
        }

        public bool IsArrayType { get; set; }

        public MemberFieldData()
        {
            name = "";
            type = CellType.Undefined;
        }
    }
}