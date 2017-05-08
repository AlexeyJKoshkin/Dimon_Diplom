using System;
using System.Collections;
using System.Collections.Generic;

namespace ShutEye.Data
{
    public interface IDataBox<T> : IDataBox where T : DataObject
    {
        IList<T> Objects { get; }

        void UpdateData(T entety);
    }

    public interface IDataBox
    {
        string CollectionName { get; }

        Type ObjectType { get; }

        void Reload();

        int Count { get; }

        ArrayList ArrayListData { get; }
    }

    public interface IDataBoxStorage
    {
        IEnumerable<T> All<T>() where T : DataObject;

        int CountProviders { get; }

        T First<T>() where T : DataObject;

        T ById<T>(long id) where T : DataObject;

        int Count<T>() where T : DataObject;
    }

    public interface IDataEditorStorage : IDataBoxStorage
    {
        void Update<T>(T Entety) where T : DataObject;

        void Remove<T>(T obj) where T : DataObject;

        bool Exists<T>(long id) where T : DataObject;

        void Add<T>(T entety) where T : DataObject;

        DataObject ById(Type type, long id);

        IEnumerable<DataObject> All(Type key);

        void RegisterProvider(IDataBox member);
    }
}