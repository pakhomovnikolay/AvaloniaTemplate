using Avalonia.Controls;
using Avalonia.Controls.Primitives;
using System;

namespace AvaloniaTemplate.Resources.CustomResourcesDictionary.Base
{
    public abstract partial class BaseTemplatedControl : TemplatedControl
    {
        private TemplateAppliedEventArgs EventArgs { get; set; }

        #region Текущий аргумент события применения шаблона
        /// <summary>
        /// Текущий аргумент события применения шаблона
        /// </summary>
        public TemplateAppliedEventArgs CurrTemplateAppliedEventArgs => EventArgs;
        #endregion

        #region Задать текущий аргумент события применения шаблона
        /// <summary>
        /// Задать текущий аргумент события применения шаблона
        /// </summary>
        public void SetTemplateAppliedEventArgs(TemplateAppliedEventArgs e)
        {
            if (e is null)
                return;

            EventArgs = e;
        }
        #endregion

        #region Найти элемент управления в шаблоне по идентификатору
        /// <summary>
        /// Найти элемент управления в шаблоне по идентификатору
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="e"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="InvalidOperationException"></exception>
        protected T FindPartById<T>(TemplateAppliedEventArgs e, string Id) where T : Control
        {
            EventArgs = e;
            return e.NameScope.Find<T>(Id)
                ?? throw new InvalidOperationException($"PART '{Id}' not found in template of {GetType().Name}");
        }
        #endregion
    }
}