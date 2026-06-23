using System.ComponentModel;

namespace AvaloniaTemplate.Models.Enums
{
    public enum CurrentBorderStyleType
    {
        None,

        [Description("Нижняя граница")]
        Bottom,
        Top,
        Left,
        Right,
        All,
        Outside,
        ThickOutside,
        DoubleBottom,
        ThickBottom,
        TopBottom,
    }
}
