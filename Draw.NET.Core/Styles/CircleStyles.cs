/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       DefaultStyles 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/20 17:13:19
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draw.NET.Renderer.Primitives;
using Draw.NET.Renderer.Styles;

namespace Draw.NET.Core.Styles
{
    internal static class CircleStyles
    {
        public readonly static BorderPattern DefaultCircleBorder;
        public readonly static BorderPattern BoundsPatternBorder;
        public readonly static FillPattern DefaultCircleFill;
        public readonly static FillPattern BoundsPatternFill;

        public static void ApplyDefaultPatternTo(IStyle s)
        {
            s.BorderPattern.Load(DefaultCircleBorder);
            if (s.FillPattern.FillType != DefaultCircleFill.FillType)
            {
                s.FillPattern.Dispose();
                switch (DefaultCircleFill.FillType)
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
            s.FillPattern.Load(DefaultCircleFill);
        }

        public static void ApplyBoundsPatternTo(IStyle s)
        {
            s.BorderPattern.Load(BoundsPatternBorder);
            if (s.FillPattern.FillType != BoundsPatternFill.FillType)
            {
                s.FillPattern.Dispose();
                switch (BoundsPatternFill.FillType)
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
            s.FillPattern.Load(BoundsPatternFill);
        }

        static CircleStyles()
        {
            DefaultCircleBorder = new BorderPattern()
            {
                Color = System.Drawing.Color.Black,
            };

            BoundsPatternBorder = new BorderPattern()
            {
                Color = System.Drawing.Color.Gray,
                DashStyle = System.Drawing.Drawing2D.DashStyle.Dash
            };

            DefaultCircleFill = new FillPattern.Solid()
            {
                Color = System.Drawing.Color.White
            };

            BoundsPatternFill = new FillPattern.None();


        }
    }
}
