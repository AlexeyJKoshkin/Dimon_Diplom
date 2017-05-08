using ShutEye.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ShutEye.Data.Provider
{
    public partial class LocalDataBoxStorage : AbstractDataStorage, IDataEditorStorage
    {
        public override void Update<T>(T Entety)
        {
            GetMember<T>().UpdateData(Entety);
        }

        public void Remove<T>(T obj) where T : DataObject
        {
            ((DataBox)GetMember<T>()).Remove(obj.Id);
        }

        public void Add<T>(IEnumerable<T> enteties) where T : DataObject
        {
            var member = GetMember<T>();
            enteties.ForEach(e => member.UpdateData(e));
        }

        public DataObject ById(Type type, long id)
        {
            var res = _Members.FirstOrDefault(m => m.Key == type).Value;
            if (res == null) return null;
            return res.ArrayListData.Cast<DataObject>().FirstOrDefault(o => o.Id == id);
        }

        public IEnumerable<DataObject> All(Type type)
        {
            var res = _Members.FirstOrDefault(m => m.Key == type).Value;
            if (res == null) return null;
            return res.ArrayListData.Cast<DataObject>();
        }
    }
}