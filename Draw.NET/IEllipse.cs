using System.Drawing;

namespace Draw.NET
{
    /// <summary>
    /// 椭圆定义
    /// </summary>
    public interface IEllipse
    {
        /// <summary>
        /// 与椭圆相切的矩形的高度
        /// </summary>
        float Height { get; set; }
        /// <summary>
        /// 与椭圆相切的矩形的左上角坐标
        /// </summary>
        PointF Location { get; set; }
        /// <summary>
        /// 与椭圆相切的矩形的宽度
        /// </summary>
        float Width { get; set; }

        /// <summary>
        /// 与椭圆相切的矩形的大小
        /// </summary>
        SizeF Size { get; }
    }
}