namespace Labo.ServiceModel.Core
{
    using System;
    using System.Reflection;

    using Labo.Common.Reflection;

    public sealed class DefaultReflectionHelper : IReflectionHelper
    {
        public void SetFieldValue(object instance, string fieldName, object value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            Type objectType = instance.GetType();
            FieldInfo fieldInfo = objectType.GetField(fieldName);
            fieldInfo.SetValue(instance, value);
        }

        public object GetFieldValue(object instance, string fieldName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            Type objectType = instance.GetType();
            FieldInfo fieldInfo = objectType.GetField(fieldName);
            return fieldInfo.GetValue(instance);
        }

        public void SetPropertyValue(object instance, string propertyName, object value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            ReflectionHelper.SetPropertyValue(instance, propertyName, value);
        }

        public void SetPropertyValue(object instance, PropertyInfo propertyInfo, object value)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            ReflectionHelper.SetPropertyValue(instance, propertyInfo, value);
        }

        public object GetPropertyValue(object instance, string propertyName)
        {
            if (instance == null)
            {
                throw new ArgumentNullException("instance");
            }

            return ReflectionHelper.GetPropertyValue(instance, propertyName);
        }

        public object CreateInstance(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException("type");
            }

            return ReflectionHelper.CreateInstance(type);
        }
    }
}