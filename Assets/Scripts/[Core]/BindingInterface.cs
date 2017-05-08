using Entitas;

namespace GameKit
{
    public interface IDataBinding
    {
        void RefreshView();
    }

    public interface IDataBinding<TData> : IDataBinding
    {
        void UpdateDataView(TData newdata);

        TData CurrentData { get; }
    }

    public interface IEntityBinging<out T> : IDataBinding<Entity> where T : Entity
    {
    }
}