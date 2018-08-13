/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       GDIPlusBrokenLine 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/3 11:22:33
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Draw.NET.Renderer.GDIPlus.Primitive
{
    public class GDI_BrokenLine : Primitives.AbstractBrokenLine
    {
        GDIPlusTools tools;

        public GDI_BrokenLine(List<PointF> pts) : base(pts)
        {
            tools = new GDIPlusTools(this, this, pts.GetBounds());
            tools.Drawing += Tools_Drawing;
            //tools.Preparing += Tools_Preparing;
            PropertyChanged += GDI_BrokenLine_PropertyChanged;
        }

        public GDI_BrokenLine() : this(new List<PointF>()) { }


        public override void Draw(object state)
        {
            tools.Draw(state);
        }

        public override void Prepare(object state)
        {
            tools.Prepare(state);
        }



        //private void Tools_Preparing(PrepareArgs arg)
        //{
        //}

        private void Tools_Drawing(DrawingArgs arg)
        {
            //if (arg.RenderAll || arg.Graphics.Clip.IsVisible(tools.Bounds))
            //{
            if (arg.Transform != null)
            {
                var copy = new List<PointF>(__points).ToArray();
                arg.Transform.TransformPoints(copy);
                arg.Graphics.DrawLines(tools.DrawingPen, copy);
            }
            else
            {
                arg.Graphics.DrawLines(tools.DrawingPen, __points.ToArray());
            }
            //}
        }


        private void GDI_BrokenLine_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            tools.WaitForRendering();
            switch (e.PropertyName)
            {
                case nameof(Description):
                    break;
                case nameof(Layer):
                    break;
                case nameof(FillPattern):
                    tools.UpdateFillPattern();
                    break;
                case nameof(Points):
                case nameof(Visible):
                    var rect = __points.GetBounds();
                    if (rect.Size != SizeF.Empty)
                    {
                        tools.RecordClip(rect);
                    }
                    break;
                default:
                    break;
            }
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                tools.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
