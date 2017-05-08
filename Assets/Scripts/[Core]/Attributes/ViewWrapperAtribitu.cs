using System;

namespace ShutEye.Extensions.Attributes
{
    /// <summary>
    /// Атрибут указываект какой тип данных может редактировать данный  экземпляр класса
    /// </summary>
    public class ViewWrapperAttribute : Attribute
    {
        public Type WrapperType;

        public ViewWrapperAttribute(Type wrapperType)
        {
            WrapperType = wrapperType;
        }
    }
}