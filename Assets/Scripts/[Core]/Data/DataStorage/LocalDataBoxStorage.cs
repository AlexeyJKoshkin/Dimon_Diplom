using System;
using System.Collections.Generic;
using System.Linq;

namespace ShutEye.Data.Provider
{
    public partial class LocalDataBoxStorage : AbstractDataStorage, IDataEditorStorage
    {
        #region DataProvider

        public void RegisterProvider(IDataBox provider)
        {
            if (_Members.ContainsKey(provider.ObjectType)) return;
            _Members.Add(provider.ObjectType, provider);

            //  Debug.Log(string.Format("Load {0} Count {1}", provider.ObjectType, provider.Count));
        }

        public override IEnumerable<T> All<T>()
        {
            var member = GetMember<T>();
            return member.Objects;
        }

        public override int Count<T>()
        {
            return GetMember<T>().Count;
        }

        public void Add<T>(T entety) where T : DataObject
        {
            GetMember<T>().UpdateData(entety);
        }

        public override T First<T>()
        {
            var member = GetMember<T>();
            return member.Objects.FirstOrDefault();
        }

        public override T ById<T>(long id)
        {
            var member = GetMember<T>();
            return member.Objects.FirstOrDefault(o => o.Id == id);
        }

        #endregion DataProvider

        protected IDataBox<T> GetMember<T>() where T : DataObject
        {
            try
            {
                var res = (IDataBox<T>)_Members.FirstOrDefault(m => m.Key == typeof(T)).Value;
                if (res == default(IDataBox<T>))
                {
                    throw new ArgumentException(String.Format("Provider for {0} not registered", typeof(T)));
                }
                return res;
            }
            catch (Exception)
            {
                throw new ArgumentException(String.Format("Provider for {0} not registered", typeof(T)));
            }
        }

        public bool Exists<T>(long id) where T : DataObject
        {
            return GetMember<T>().Objects.Any(o => o.Id == id);
        }
    }
}