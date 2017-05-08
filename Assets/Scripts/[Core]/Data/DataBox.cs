using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

namespace ShutEye.Data
{
    [Serializable]
    public class DataBox<T> : DataBox, IDataBox<T> where T : DataObject
    {
        public override void OnBeforeSerialize()
        {
            _serializedCollection = SEJsonConverter.Serialize(_tempList);
        }

        public override void OnSave()
        {
        }

        public override void OnAfterDeserialize()
        {
            if (string.IsNullOrEmpty(_serializedCollection)) return;
            _tempList = SEJsonConverter.Deserialize<List<T>>(_serializedCollection);
            if (_tempList == null)
                Debug.LogError("Missin data");
        }

        public override void LoadFromFile()
        {
            string fullpathLog = string.Format("{0}/{1}.txt", Application.dataPath, CollectionName);
            _tempList = SEJsonConverter.Deserialize<List<T>>(File.ReadAllText(fullpathLog));
        }

        public override void SaveToFile()
        {
            string fullpathLog = string.Format("{0}/{1}.txt", Application.dataPath, CollectionName);
            Debug.Log(fullpathLog);
            if (!File.Exists(fullpathLog))
            {
                Debug.LogWarning("CREARE lOG : " + fullpathLog);
                var file = File.Create(fullpathLog);
                file.Close();
                file.Dispose();
            }
            try
            {
                File.WriteAllText(fullpathLog, _serializedCollection);
            }
            catch (Exception ex)
            {
                Debug.LogError("Error while OPEN file " + ex.Message);
            }
        }

        public override string SerializedCollection
        {
            get { return _serializedCollection; }
            set { _serializedCollection = value; }
        }

        [SerializeField]
        private string _serializedCollection;

        public override string CollectionName { get { return string.Format("{0}_Data", ObjectType.Name); } }
        public override Type ObjectType { get { return typeof(T); } }
        public override int Count { get { return _tempList == null ? 0 : _tempList.Count; } }

        public IList<T> Objects { get { return _tempList; } }
        private List<T> _tempList = new List<T>();

        public override ArrayList ArrayListData
        {
            get
            {
                return _tempList == null ? new ArrayList() : new ArrayList(_tempList);
            }
        }

        private int nextId
        {
            get
            {
                return _tempList.Any()
                    ? _tempList.Max(o => o.Id) + 1
                    : 1;
            }
        }

        public void UpdateData(T data)
        {
            var oldindex = _tempList.FindIndex(o => o.Id == data.Id);
            if (oldindex < 0)
            {
                data.Id = nextId;
                _tempList.Add(data);
            }
            else
            {
                _tempList.RemoveAt(oldindex);
                _tempList.Insert(oldindex, data);
            }
            OnBeforeSerialize();
        }

        public void Clear()
        {
            _tempList.Clear();
        }

        public override void Reload()
        {
            OnAfterDeserialize();
        }

        public override void Remove(long currentID)
        {
            _tempList.RemoveAll(o => o.Id == currentID);
            OnBeforeSerialize(); //Сохраняем данные про удаленные объект
        }

        public override DataObject CreateNew()
        {
            T res = Activator.CreateInstance<T>();
            res.Id = -1;
            UpdateData(res);
            return res;
        }
    }

    [Serializable]
    public abstract class DataBox : ScriptableObject, IDataBox, ISerializationCallbackReceiver
    {
        public virtual string CollectionName
        {
            get { return "Emty"; }
        }

        public virtual Type ObjectType
        {
            get { return null; }
        }

        public virtual int Count
        {
            get { return 0; }
        }

        public abstract void Reload();

        public abstract void Remove(long currentID);

        public abstract DataObject CreateNew();

        public abstract ArrayList ArrayListData { get; }

        public abstract void OnBeforeSerialize();

        public abstract void OnAfterDeserialize();

        public abstract void SaveToFile();

        public abstract void LoadFromFile();

        public abstract string SerializedCollection { get; set; }

        public abstract void OnSave();
    }
}