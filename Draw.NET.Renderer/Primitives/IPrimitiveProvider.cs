using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Draw.NET.Renderer.Primitives
{
    public interface IPrimitiveProvider
    {
        T GetPrimitive<T>(PointF location, SizeF size);

        T GetPrimitive<T>();
    }
}
