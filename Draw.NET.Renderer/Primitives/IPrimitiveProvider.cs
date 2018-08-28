using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Draw.NET.Renderer.Primitives
{
    public interface IPrimitiveProvider
    {
        T GetPrimitive<T>(PointF location, SizeF size) where T : AbstractPrimitive;

        T GetPrimitive<T>() where T : AbstractPrimitive;
    }
}
