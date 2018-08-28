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


namespace Draw.NET.Core.Styles
{
    internal static class HandleStyles
    {
        public static BorderPattern DefaultHandleBorder { get; set; }
        public static FillPattern DefaultHandleFill { get; set; }

        public static void ApplyDefaultPatternTo(IStyle s)
        {
            s.BorderPattern.Load(DefaultHandleBorder);
            if (s.FillPattern.FillType != DefaultHandleFill.FillType)
            {
                s.FillPattern.Dispose();
                switch (DefaultHandleFill.FillType)
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
            s.FillPattern.Load(DefaultHandleFill);
        }

        static HandleStyles()
        {
            DefaultHandleBorder = new BorderPattern()
            {
                Color = System.Drawing.Color.Black,
            };

            DefaultHandleFill = new FillPattern.Solid()
            {
                Color = System.Drawing.Color.White
            };

        }
    }
}
