using System;
using System.Collections.Generic;

namespace ShutEye.Data.Provider
{
    public abstract class AbstractDataStorage : IDataBoxStorage
    {
        protected readonly Dictionary<Type, IDataBox> _Members;
        public int CountProviders { get { return _Members.Count; } }

        protected AbstractDataStorage()
        {
            _Members = new Dictionary<Type, IDataBox>();
        }

        public abstract IEnumerable<T> All<T>() where T : DataObject;

        public abstract T First<T>() where T : DataObject;

        public abstract T ById<T>(long id) where T : DataObject;

        public abstract void Update<T>(T Entety) where T : DataObject;

        public abstract int Count<T>() where T : DataObject;
    }
}