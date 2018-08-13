using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer.Styles
{
    public interface IStyle
    {
        BorderPattern BorderPattern { get; }

        FillPattern FillPattern { get; set; }

    }
}
