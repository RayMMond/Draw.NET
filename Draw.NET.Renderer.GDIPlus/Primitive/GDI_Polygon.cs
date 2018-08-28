/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       GDI_Polygon 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/3 12:55:27
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
    public class GDI_Polygon : Renderer.Primitives.AbstractPolygon
    {
        GDIPlusTools tools;

        public GDI_Polygon(List<PointF> pts) : base(pts)
        {
            tools = new GDIPlusTools(this, this, pts.GetBounds());
            tools.Drawing += Tools_Drawing;
            //tools.Preparing += Tools_Preparing;
            PropertyChanged += GDI_Polygon_PropertyChanged;
        }

        public GDI_Polygon() : this(new List<PointF>()) { }


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
            PointF[] points = __points.ToArray();
            if (arg.Transform != null)
            {
                arg.Transform.TransformPoints(points);
            }

            if (tools.DrawingBrush != Brushes.Transparent)
            {
                arg.Graphics.FillPolygon(tools.DrawingBrush, points);
            }
            arg.Graphics.DrawPolygon(tools.DrawingPen, points);
            //}
        }

        private void GDI_Polygon_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
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
