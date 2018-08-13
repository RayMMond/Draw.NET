/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       GDI_Ellipse 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/3 12:25:26
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
    public class GDI_Ellipse : Renderer.Primitives.AbstractEllipse
    {
        GDIPlusTools tools;
        RectangleF rect;

        public GDI_Ellipse(PointF location, SizeF size) : base(location, size)
        {
            rect = new RectangleF(location, size);
            tools = new GDIPlusTools(this, this, rect);
            tools.Drawing += Tools_Drawing;
            PropertyChanged += GDI_Ellipse_PropertyChanged;
            //tools.Preparing += Tools_Preparing;
        }


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
            RectangleF rectToDraw = rect;
            if (arg.Transform != null)
            {
                var ltrb = new PointF[] { rect.Location, new PointF(rect.Right, rect.Bottom) };
                arg.Transform.TransformPoints(ltrb);
                rectToDraw = RectangleF.FromLTRB(ltrb[0].X, ltrb[0].Y, ltrb[1].X, ltrb[1].Y);
            }
            if (tools.DrawingBrush != Brushes.Transparent)
            {
                arg.Graphics.FillEllipse(tools.DrawingBrush, rectToDraw);
            }
            arg.Graphics.DrawEllipse(tools.DrawingPen, rectToDraw);

            //}
        }


        private void GDI_Ellipse_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
                case nameof(Location):
                case nameof(Width):
                case nameof(Height):
                case nameof(Visible):
                    rect = GetBounds();
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
