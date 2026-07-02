using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvaloniaTemplate.Models.Enums
{
    public class BorderStyleType
    {
        /// <summary>
        /// Стиль верхней границы
        /// </summary>
        public BorderLineStyleType Bottom { get; set; } = BorderLineStyleType.None;

        /// <summary>
        /// Стиль нижней границы
        /// </summary>
        public BorderLineStyleType Top { get; set; } = BorderLineStyleType.None;

        /// <summary>
        /// Стиль левой границы
        /// </summary>
        public BorderLineStyleType Left { get; set; } = BorderLineStyleType.None;

        /// <summary>
        /// Стиль правой границы
        /// </summary>
        public BorderLineStyleType Right { get; set; } = BorderLineStyleType.None;

        /// <summary>
        /// Стиль внутренней границы по горизонтали
        /// </summary>
        public BorderLineStyleType InsideHorizontal { get; set; } = BorderLineStyleType.None;

        /// <summary>
        /// Стиль внутренней границы по вертикали
        /// </summary>
        public BorderLineStyleType InsideVertical { get; set; } = BorderLineStyleType.None;
    }
}
