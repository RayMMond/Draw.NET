using Draw.NET.Renderer.Primitives;
using System.Drawing;

namespace Draw.NET.Renderer.GDIPlus.Primitive
{
    public class GDIPrimitiveProvider : IPrimitiveProvider
    {
        public T GetPrimitive<T>(PointF location, SizeF size) where T : AbstractPrimitive
        {
            if (typeof(T) == typeof(AbstractEllipse))
            {
                return new GDI_Ellipse(location, size) as T;
            }
            else if (typeof(T) == typeof(AbstractRectangle))
            {
                return new GDI_Rectangle(location, size) as T;
            }
            else
            {
                return default(T);
            }
        }

        public T GetPrimitive<T>() where T : AbstractPrimitive
        {
            if (typeof(T) == typeof(AbstractText))
            {
                return new GDI_Text() as T;
            }
            else if (typeof(T) == typeof(AbstractPolygon))
            {
                return new GDI_Polygon() as T;
            }
            else if (typeof(T) == typeof(AbstractBrokenLine))
            {
                return new GDI_BrokenLine() as T;
            }
            else
            {
                return default(T);
            }
        }
    }
}
