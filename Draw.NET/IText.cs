using System.Drawing;

namespace Draw.NET
{
    /// <summary>
    /// 文本对象定义
    /// </summary>
    public interface IText
    {
        StringAlignment Alignment { get; set; }
        string Content { get; set; }
        string FamilyName { get; set; }
        float FontSize { get; set; }
        FontStyle FontStyle { get; set; }
        StringAlignment LineAlignment { get; set; }
        LocationType LocationType { get; set; }
        SizeF StringSize { get; set; }
        StringTrimming Trimming { get; set; }

        Color TextColor { get; set; }

        Font Font { get; }

        RectangleF GetBounds();
    }

    public enum LocationType
    {
        Relative,
        Absolute
    }
}