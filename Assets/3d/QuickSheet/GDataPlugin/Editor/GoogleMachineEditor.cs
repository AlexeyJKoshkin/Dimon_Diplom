///////////////////////////////////////////////////////////////////////////////
///
/// GoogleMachineEditor.cs
/// 
/// (c)2013 Kim, Hyoun Woo
///
///////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using System.Linq;

// to resolve TlsException error.
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;

using GDataDB;
using GDataDB.Linq;

using GDataDB.Impl;
using Google.GData.Client;
using Google.GData.Spreadsheets;
using ShutEye.Core;

namespace UnityQuickSheet
{
    /// <summary>
    /// An editor script class of GoogleMachine class.
    /// </summary>
    [CustomEditor(typeof(GoogleMachine))]
    public class GoogleMachineEditor : BaseMachineEditor
    {

        /// <summary>
        /// Create new account setting asset file if there is already one then select it.
        /// </summary>
        [MenuItem("Assets/Create/QuickSheet/Setting/GoogleData Setting")]
        public static void CreateGoogleDataSetting()
        {
            Debug.LogError("Создать настройки");
            //AssemblyReflectionHelper.Create();
        }

        /// <summary>
        /// A menu item which create a 'GoogleMachine' asset file.
        /// </summary>
        [MenuItem("Assets/Create/QuickSheet/Tools/Google")]
        public static void CreateGoogleMachineAsset()
        {
            GoogleMachine inst = ScriptableObject.CreateInstance<GoogleMachine>();
            string path = AssemblyReflectionHelper.GetUniqueAssetPathNameOrFallback(BaseMachine.ImportSettingFilename);
            AssetDatabase.CreateAsset(inst, path);
            AssetDatabase.SaveAssets();
            Selection.activeObject = inst;
        }

        ///// <summary>
        ///// Select currently exist account setting asset file.
        ///// </summary>
        //[MenuItem("Edit/Project Settings/QuickSheet/Select Google Data Setting")]
        //public static void Edit()
        //{
        //    Selection.activeObject = GameCore.GoogleSettings;
        //    if (Selection.activeObject == null)
        //    {
        //        Debug.LogError("No GoogleDataSettings.asset file is found. Create setting file first.");
        //    }
        //}

        protected override void OnEnable()
        {
            base.OnEnable();

            // resolve TlsException error
            UnsafeSecurityPolicy.Instate();

            machine = target as GoogleMachine;
            if (machine != null)
            {
                machine.ReInitialize();
            }
        }

        /// <summary>
        /// Draw custom UI.
        /// </summary>
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            GUILayout.Label("Google Spreadsheet Settings:", headerStyle);

            EditorGUILayout.Separator();

            GUILayout.Label("Script Path Settings:", headerStyle);
            machine.SpreadSheetName = EditorGUILayout.TextField("SpreadSheet Name: ", machine.SpreadSheetName);
            machine.WorkSheetName = EditorGUILayout.TextField("WorkSheet Name: ", machine.WorkSheetName);

            EditorGUILayout.Separator();

            GUILayout.BeginHorizontal();

            if (machine.HasColumnHeader())
            {
                if (GUILayout.Button("Update"))
                    Import();

                if (GUILayout.Button("Reimport"))
                    Import(true);
            }
            else
            {
                if (GUILayout.Button("Import"))
                    Import();
            }

            GUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            DrawHeaderSetting(machine);

            EditorGUILayout.Separator();

            GUILayout.Label("Path Settings:", headerStyle);

            machine.TemplatePath = EditorGUILayout.TextField("Template: ", machine.TemplatePath);
            machine.RuntimeClassPath = EditorGUILayout.TextField("Runtime: ", machine.RuntimeClassPath);
            machine.EditorClassPath = EditorGUILayout.TextField("Editor:", machine.EditorClassPath);

            machine.onlyCreateDataClass = EditorGUILayout.Toggle("Only DataClass", machine.onlyCreateDataClass);

            EditorGUILayout.Separator();

            // force save changed type.
            if (GUI.changed)
            {
               // EditorUtility.SetDirty(GameCore.GoogleSettings);
                EditorUtility.SetDirty(machine);
            }
        }

        /// <summary>
        /// A delegate called on each of a cell query.
        /// </summary>
        delegate void OnEachCell(CellEntry cell);

        /// <summary>
        /// Connect to google-spreadsheet with the specified account and password 
        /// then query cells and call the given callback.
        /// </summary>
        private void DoCellQuery(OnEachCell onCell)
        {
            Debug.LogError("Писюн");
            //// first we need to connect to the google-spreadsheet to get all the first row of the cells
            //// which are used for the properties of data class.
            //var client = new DatabaseClient(GameCore.GoogleSettings);

            //if (string.IsNullOrEmpty(machine.SpreadSheetName))
            //    return;
            //if (string.IsNullOrEmpty(machine.WorkSheetName))
            //    return;

            //string error = string.Empty;
            //var db = client.GetDatabase(machine.SpreadSheetName, ref error);
            //if (db == null)
            //{
            //    string message = string.Empty;
            //    if (string.IsNullOrEmpty(error))
            //        message = @"Unknown error.";
            //    else
            //        message = string.Format(@"{0}", error);

            //    message += "\n\nOn the other hand, see 'GoogleDataSettings.asset' file and check the oAuth2 setting is correctly done.";
            //    EditorUtility.DisplayDialog("Error", message, "OK");
            //    return;
            //}

            //// retrieves all cells
            //var worksheet = ((Database)db).GetWorksheetEntry(machine.WorkSheetName);

            //// Fetch the cell feed of the worksheet.
            //CellQuery cellQuery = new CellQuery(worksheet.CellFeedLink);
            //var cellFeed = client.SpreadsheetService.Query(cellQuery);

            //// Iterate through each cell, printing its value.
            //foreach (CellEntry cell in cellFeed.Entries)
            //{
            //    if (onCell != null)
            //        onCell(cell);
            //}
        }

        /// <summary>
        /// Connect to the google spreadsheet and retrieves its header columns.
        /// </summary>
        protected override void Import(bool reimport = false)
        {
            Regex re = new Regex(@"\d+");

            Dictionary<string, ColumnHeader> headerDic = null;
            if (reimport)
                machine.ColumnHeaderList.Clear();
            else
                headerDic = machine.ColumnHeaderList.ToDictionary(k => k.name);

            List<ColumnHeader> tmpColumnList = new List<ColumnHeader>();

            int order = 0;
            // query the first columns only.
            DoCellQuery((cell) =>
            {

                // get numerical value from a cell's address in A1 notation
                // only retrieves first column of the worksheet 
                // which is used for member fields of the created data class.
                Match m = re.Match(cell.Title.Text);
                if (int.Parse(m.Value) > 1)
                    return;

                // check the column header is valid
                if (!IsValidHeader(cell.Value))
                {
                    string error = string.Format(@"Invalid column header name {0}. Any c# keyword should not be used for column header. Note it is not case sensitive.", cell.Value);
                    EditorUtility.DisplayDialog("Error", error, "OK");
                    return;
                }

                ColumnHeader column = ParseColumnHeader(cell.Value, order++);
                if (headerDic != null && headerDic.ContainsKey(cell.Value))
                {
                    // if the column is already exist, copy its name and type from the exist one.
                    ColumnHeader h = machine.ColumnHeaderList.Find(x => x.name == column.name);
                    if (h != null)
                    {
                        column.type = h.type;
                        column.isArray = h.isArray;
                    }
                }

                tmpColumnList.Add(column);
            });

            // update (all of settings are reset when it reimports)
            machine.ColumnHeaderList = tmpColumnList;

            EditorUtility.SetDirty(machine);
            AssetDatabase.SaveAssets();
        }



        /// 
        /// Create utility class which has menu item function to create an asset file.
        /// 
        protected override void CreateAssetCreationScript(BaseMachine m, ScriptPrescription sp)
        {
            sp.className = machine.WorkSheetName;
            sp.spreadsheetName = machine.SpreadSheetName;
            sp.worksheetClassName = machine.WorkSheetName;
            sp.assetFileCreateFuncName = "Create" + machine.WorkSheetName + "AssetFile";
            sp.template = GetTemplate("AssetFileClass");

            // write a script to the given folder.		
            using (var writer = new StreamWriter(TargetPathForAssetFileCreateFunc(machine.WorkSheetName)))
            {
                writer.Write(new ScriptGenerator(sp).ToString());
                writer.Close();
            }
        }

    }
}