using AvaloniaTemplate.Models.Table.Model;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AvaloniaTemplate.Services.Interfaces
{
    public interface ITableGenerateFactory
    {
        #region Коллекция моделей
        /// <summary>
        /// Коллекция моделей
        /// </summary>
        ObservableCollection<ModelTable> Models { get; set; }
        #endregion

        #region Выбранная модель
        /// <summary>
        /// Выбранная модель
        /// </summary>
        ModelTable SelectedModel { get; set; }
        #endregion

        #region Выбранные модели
        /// <summary>
        /// Выбранные модели
        /// </summary>
        ObservableCollection<ModelTable> SelectedModels { get; set; }
        #endregion

        #region Создать модель
        /// <summary>
        /// Создать модель
        /// </summary>
        ModelTable CreateModel();
        #endregion

        #region Удалить модель
        /// <summary>
        /// Удалить модель
        /// </summary>
        void DeleteModel();

        /// <summary>
        /// Удалить модель
        /// </summary>
        /// <param name="model"></param>
        void DeleteModel(ModelTable model);

        /// <summary>
        /// Удалить модель
        /// </summary>
        /// <param name="models"></param>
        void DeleteModel(ObservableCollection<ModelTable> models);
        #endregion

        #region Создать модель колонки
        /// <summary>
        /// Создать модель колонки
        /// </summary>
        /// <param name="index"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        ModelColumn CreateModelColumn(int index, string header);
        #endregion

        #region Создать модель колонок
        /// <summary>
        /// Создать модель колонок
        /// </summary>
        /// <param name="startCol"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ModelColumn> CreateModelColumns(int startCol, int count);
        #endregion

        #region Удалить модель колонки
        /// <summary>
        /// Удалить модель колонки
        /// </summary>
        void DeleteModelColumn();

        /// <summary>
        /// Удалить модель колонки
        /// </summary>
        /// <param name="column"></param>
        void DeleteModelColumn(ModelColumn column);

        /// <summary>
        /// Удалить модель колонки
        /// </summary>
        /// <param name="columns"></param>
        void DeleteModelColumn(ObservableCollection<ModelColumn> columns);
        #endregion

        #region Создать модель строки
        /// <summary>
        /// Создать модель строки
        /// </summary>
        /// <param name="index"></param>
        /// <param name="header"></param>
        /// <returns></returns>
        ModelRow CreateModelRow(int index, string header);
        #endregion

        #region Создать модель строк
        /// <summary>
        /// Создать модель строк
        /// </summary>
        /// <param name="startRow"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ModelRow> CreateModelRows(int startRow, int count);
        #endregion

        #region Удалить модель строки
        /// <summary>
        /// Удалить модель строки
        /// </summary>
        void DeleteModelRowRow();

        /// <summary>
        /// Удалить модель строки
        /// </summary>
        /// <param name="row"></param>
        void DeleteModelRowRow(ModelRow row);

        /// <summary>
        /// Удалить модель строки
        /// </summary>
        /// <param name="rows"></param>
        void DeleteModelRowRow(ObservableCollection<ModelRow> rows);
        #endregion

        #region Создать модель ячейкм
        /// <summary>
        /// Создать модель ячейкм
        /// </summary>
        /// <param name="indexCol"></param>
        /// <param name="indexRow"></param>
        /// <returns></returns>
        ModelCell CreateModelCell(int indexCol, int indexRow);
        #endregion

        #region Создать модель ячеек
        /// <summary>
        /// Создать модель ячеек
        /// </summary>
        /// <param name="indexRow"></param>
        /// <param name="startCell"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        List<ModelCell> CreateModelCells(int indexRow, int startCell, int count);
        #endregion

        #region Удалить модель ячейки
        /// <summary>
        /// Удалить модель ячейки
        /// </summary>
        void DeleteCells();

        /// <summary>
        /// Удалить модель ячейки
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cell"></param>
        void DeleteCells(ModelRow row, ModelCell cell);

        /// <summary>
        /// Удалить модель ячейки
        /// </summary>
        /// <param name="row"></param>
        /// <param name="cells"></param>
        void DeleteCells(ModelRow row, ObservableCollection<ModelCell> cells);
        #endregion

        #region Обновить визульное пространство
        /// <summary>
        /// Обновить визульное пространство
        /// </summary>
        void UpdateViewport();
        #endregion
    }
}