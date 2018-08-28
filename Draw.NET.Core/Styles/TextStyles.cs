
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draw.NET.Core.Styles
{
    public class TextStyles
    {
        static FillPattern DefaultFontText { get; set; }
        public static void ApplyDefaultPatternTo(IStyle s)
        {
            if (s.FillPattern.FillType != DefaultFontText.FillType)
            {
                s.FillPattern.Dispose();
                switch (DefaultFontText.FillType)
                {
                    case FillType.None:
                        s.FillPattern = new FillPattern.None();
                        break;
                    case FillType.Solid:
                        s.FillPattern = new FillPattern.Solid();
                        break;
                    case FillType.LinearGradient:
                        s.FillPattern = new FillPattern.LinearGradient();
                        break;
                    case FillType.Texture:
                        s.FillPattern = new FillPattern.Texture();
                        break;
                }
            }
            s.FillPattern.Load(DefaultFontText);
        }

        static TextStyles()
        {
            DefaultFontText = new FillPattern.Solid()
            {
                Color = System.Drawing.Color.Black
            };
        }
    }
}

