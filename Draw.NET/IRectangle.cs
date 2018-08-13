using System.Drawing;

namespace Draw.NET
{
    /// <summary>
    /// 矩形定义
    /// </summary>
    public interface IRectangle
    {
        PointF Center { get; set; }
        PointF Location { get; }
        float RectHeight { get; set; }
        SizeF RectSize { get; }
        float RectWidth { get; set; }
        float Left { get; }
        float Right { get; }
        float Top { get; }
        float Bottom { get; }

        DrawModel DrawModel { get; set; }
    }

    public enum DrawModel
    {
        LeftTop,
        Center,
    }
}