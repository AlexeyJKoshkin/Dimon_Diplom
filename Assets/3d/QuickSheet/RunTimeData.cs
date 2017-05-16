///////////////////////////////////////////////////////////////////////////////
///
/// BaseMachine.cs
/// 
/// (c)2014 Kim, Hyoun Woo
///
///////////////////////////////////////////////////////////////////////////////

using System;
using UnityEngine;
using System.Collections.Generic;

namespace UnityQuickSheet
{
    /// <summary>
    /// A class which stores various settings for a worksheet which is imported.
    /// </summary>
    [Serializable]
    public class BaseRuntimeMachine 
    {
        public readonly static string ImportSettingFilename = "New Import Setting.asset";

        [SerializeField]
        private string sheetName;
        public string SpreadSheetName
        {
            get { return sheetName; }
            set { sheetName = value; }
        }

        [SerializeField]
        private string workSheetName;
        public string WorkSheetName
        {
            get { return workSheetName; }
            set { workSheetName = value; }
        }

        [System.NonSerialized]
        public bool onlyCreateDataClass = false;

        public List<ColumnHeader> ColumnHeaderList
        {
            get { return columnHeaderList; }
            set { columnHeaderList = value; }
        }

        [SerializeField]
        protected List<ColumnHeader> columnHeaderList;

        /// <summary>
        /// Return true, if the list is instantiated and has any its item more than one.
        /// </summary>
        /// <returns></returns>
        public bool HasColumnHeader()
        {
            if (columnHeaderList != null && columnHeaderList.Count > 0)
                return true;

            return false;
        }

        protected void OnEnable()
        {
            if (columnHeaderList == null)
                columnHeaderList = new List<ColumnHeader>();
        }

        /// <summary>
        /// Initialize with default value whenever the asset file is enabled.
        /// </summary>
        public void ReInitialize()
        {
            
            // reinitialize. it does not need to be serialized.
            onlyCreateDataClass = false;
        }
    }

    [Serializable]
    public class GoogleRuntimeMachine : BaseRuntimeMachine
    {
        [SerializeField]
        public static string generatorAssetPath = "Assets/QuickSheet/GDataPlugin/Tool/";
        [SerializeField]
        public static string assetFileName = "GoogleMachine.asset";

        // excel and google plugin have its own template files, 
        // so we need to set the different path when the asset file is created.
        private readonly string gDataTemplatePath = "QuickSheet/GDataPlugin/Templates";

        public string AccessCode = "";
    }
}