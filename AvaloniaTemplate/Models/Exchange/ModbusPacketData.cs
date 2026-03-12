using AvaloniaTemplate.Models.Exchange.Base;

namespace AvaloniaTemplate.Models.Exchange
{
    public class ModbusPacketData : IModbusPacketData
    {
        #region Стартовый адрес пакета
        /// <summary>
        /// Стартовый адрес пакета
        /// </summary>
        public ushort StartAddress { get; set; }
        #endregion

        #region Буфер
        /// <summary>
        /// Буфер
        /// </summary>
        public ushort[] Data { get; set; }
        #endregion

        #region Размер буфера
        /// <summary>
        /// Размер буфера
        /// </summary>
        public int Length { get => Data.Length; }
        #endregion
    }
}
