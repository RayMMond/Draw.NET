using System;

namespace Test.WPF
{
    static class WPFExtensions
    {
        public static System.Drawing.SizeF ToSizeF(this System.Windows.Size s)
        {
            return new System.Drawing.SizeF((float)s.Width, (float)s.Height);
        }

        public static System.Drawing.Size ToSize(this System.Windows.Size s)
        {
            return new System.Drawing.Size((int)Math.Ceiling(s.Width), (int)Math.Ceiling(s.Height));
        }

        public static System.Drawing.PointF ToPointF(this System.Windows.Point s)
        {
            return new System.Drawing.PointF((float)s.X, (float)s.Y);
        }

        public static System.Drawing.Point ToPoint(this System.Windows.Point s)
        {
            return new System.Drawing.Point((int)Math.Ceiling(s.X), (int)Math.Ceiling(s.Y));
        }
    }
}
