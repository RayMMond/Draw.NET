using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer.Primitives
{
    public abstract class AbstractPolygon : AbstractBrokenLine
    {
        

        protected AbstractPolygon(List<PointF> pts)
            : base(pts)
        {
        }

    }
}
