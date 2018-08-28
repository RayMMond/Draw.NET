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
using System.Drawing;
using Draw.NET.Core.Shapes;


namespace Draw.NET.Core.Styles
{
    internal static class RectStyles
    {
        public static BorderPattern DefaultRectBorder { get; set; }

        /// <summary>
        /// 选中后的边框样式(2018-06-29 by zsw 添加)
        /// </summary>
        public static BorderPattern SelectedRectBorder { get; set; }
        public static FillPattern DefaultRectFill { get; set; }

        public static readonly string COLOR_SELECTION_OUTLINE = "#ABABABAB";
        public static readonly string COLOR_SELECTION_FILL = "#0FE8E8E8";
        public static readonly string COLOR_SELECTED_OUTLINE = "#C8ABABAB";

        public static void ApplyDefaultPatternTo(IStyle s)
        {
            s.BorderPattern.Load(DefaultRectBorder);
            if (s.FillPattern.FillType != DefaultRectFill.FillType)
            {
                s.FillPattern.Dispose();
                switch (DefaultRectFill.FillType)
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
            s.FillPattern.Load(DefaultRectFill);
        }

        public static void ApplySelectionPatternTo(IStyle s)
        {
            s.BorderPattern.DashStyle = System.Drawing.Drawing2D.DashStyle.Solid;
            s.BorderPattern.Width = 1;
            s.BorderPattern.Color = ColorTranslator.FromHtml(COLOR_SELECTION_OUTLINE);


            if (s.FillPattern.FillType != FillType.Solid)
            {
                s.FillPattern.Dispose();
                s.FillPattern = new FillPattern.Solid();
            }
            s.FillPattern.Color = ColorTranslator.FromHtml(COLOR_SELECTION_OUTLINE);
        }


        /// <summary>
        /// 设置选中后的状态(2018-06-29 By zsw 添加)
        /// </summary>
        /// <param name="s"></param>
        public static void ApplySelectedPatternTo(IStyle s)
        {
            s.BorderPattern.DashStyle = System.Drawing.Drawing2D.DashStyle.Dash;
            s.BorderPattern.Width = 1;
            s.BorderPattern.Color = ColorTranslator.FromHtml(COLOR_SELECTED_OUTLINE);

            if (s.FillPattern.FillType != FillType.None)
            {
                s.FillPattern.Dispose();
                s.FillPattern = new FillPattern.None();
            }
        }




        static RectStyles()
        {
            DefaultRectBorder = new BorderPattern()
            {
                Color = System.Drawing.Color.Black,
            };

            DefaultRectFill = new FillPattern.Solid()
            {
                Color = System.Drawing.Color.White
            };
        }
    }
}
