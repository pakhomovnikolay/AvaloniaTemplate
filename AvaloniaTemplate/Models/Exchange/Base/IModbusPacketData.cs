namespace AvaloniaTemplate.Models.Exchange.Base
{
    public interface IModbusPacketData
    {
        #region Стартовый адрес пакета
        /// <summary>
        /// Стартовый адрес пакета
        /// </summary>
        ushort StartAddress { get; set; }
        #endregion

        #region Буфер
        /// <summary>
        /// Буфер
        /// </summary>
        ushort[] Data { get; set; }
        #endregion

        #region Размер буфера
        /// <summary>
        /// Размер буфера
        /// </summary>
        int Length { get; }
        #endregion
    }
}