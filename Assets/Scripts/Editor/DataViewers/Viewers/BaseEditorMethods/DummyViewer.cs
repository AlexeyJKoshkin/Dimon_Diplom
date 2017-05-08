namespace ShutEye.EditorsScripts.ViewerData
{
    public class DummyViewer<T> : BaseViewer<T> where T : DataObject
    {
        public override void SetCurrent(DataObject item)
        {
            throw new System.NotImplementedException();
        }

        protected override void PrepareCurrentEntity()
        {
            throw new System.NotImplementedException();
        }
    }
}