/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       GDIPlusDefinations 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/3 11:19:36
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer.GDIPlus.Primitive
{
    internal struct PrepareArgs
    {
        public readonly bool RenderAll;
        public readonly Graphics Graphics;

        public PrepareArgs(Graphics graphics, bool renderAll)
        {
            Graphics = graphics;
            RenderAll = renderAll;
        }
    }

    internal struct DrawingArgs
    {
        public readonly Graphics Graphics;
        public readonly Matrix Transform;

        public DrawingArgs(Graphics graphics, Matrix tran)
        {
            Transform = tran;
            Graphics = graphics;
        }

        public DrawingArgs(Graphics graphics)
        {
            Transform = null;
            Graphics = graphics;
        }

    }


    internal delegate void PrepareEvent(PrepareArgs arg);

    internal delegate void DrawingEvent(DrawingArgs arg);

}
