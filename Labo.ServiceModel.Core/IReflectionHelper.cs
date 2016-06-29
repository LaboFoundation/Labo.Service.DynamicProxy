namespace Labo.ServiceModel.Core
{
    using System;
    using System.Reflection;

    public interface IReflectionHelper
    {
        void SetFieldValue(object instance, string fieldName, object value);

        object GetFieldValue(object instance, string fieldName);

        void SetPropertyValue(object instance, string propertyName, object value);

        void SetPropertyValue(object instance, PropertyInfo propertyInfo, object value);

        object GetPropertyValue(object instance, string propertyName);

        object CreateInstance(Type type);
    }
}
