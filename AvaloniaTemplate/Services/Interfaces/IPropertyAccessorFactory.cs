using System;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface IPropertyAccessorFactory
    {
        #region Создание кеша для фильтрации
        /// <summary>
        /// Создание кеша для фильтрации
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        Func<object, object?> Create(Type type, string? path);
        #endregion
    }
}
