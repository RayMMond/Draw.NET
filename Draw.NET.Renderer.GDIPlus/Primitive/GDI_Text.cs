/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       GDI_Text 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/3 13:03:41
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Draw.NET.Renderer.GDIPlus.Primitive
{
    public class GDI_Text : Renderer.Primitives.AbstractText
    {
        GDIPlusTools tools;
        Font font;
        StringFormat sf;
        Brush brush;

        public GDI_Text() : base()
        {
            tools = new GDIPlusTools(this, this, new RectangleF(location, stringSize));
            tools.Drawing += Tools_Drawing;
            //tools.Preparing += Tools_Preparing;
            PropertyChanged += GDI_Text_PropertyChanged;
            font = new Font(FamilyName, FontSize, GraphicsUnit.Pixel);
            sf = new StringFormat();
            sf.Alignment = alignment;
            sf.LineAlignment = lineAlignment;
            sf.Trimming = trimming;
            brush = new SolidBrush(textColor);
        }


        public override void Draw(object state)
        {
            tools.Draw(state);
        }

        public override void Prepare(object state)
        {
            tools.Prepare(state);
        }

        public override Font Font
        {
            get { return font; }
        }

        public void DrawText(RectangleF rect, Graphics g, float fontSize = 0)
        {
            if (fontSize != 0 && fontSize != font.Size)
            {
                var f = new Font(font.FontFamily, fontSize, FontStyle.Regular);
                font.Dispose();
                font = f;
            }
            g.DrawString(Content, font, brush, rect, sf);
        }

        private void Tools_Drawing(DrawingArgs arg)
        {
            RectangleF rect = tools.Bounds;
            if (arg.Transform != null)
            {
                var ltrb = new PointF[] { rect.Location, new PointF(rect.Right, rect.Bottom) };
                arg.Transform.TransformPoints(ltrb);
                rect = RectangleF.FromLTRB(ltrb[0].X, ltrb[0].Y, ltrb[1].X, ltrb[1].Y);
            }

            arg.Graphics.DrawString(Content, font, tools.DrawingBrush, rect, sf);
        }

        private void GDI_Text_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            tools.WaitForRendering();
            try
            {
                switch (e.PropertyName)
                {
                    case nameof(FamilyName):
                    case nameof(FontSize):
                        using (var tmp = font)
                        {
                            font = new Font(FamilyName, Math.Max(9, FontSize), GraphicsUnit.Pixel);
                        }
                        break;
                    case nameof(FillPattern):
                        tools.UpdateFillPattern();
                        break;
                    case nameof(Location):
                    case nameof(StringSize):
                    case nameof(Visible):
                        if (StringSize != SizeF.Empty)
                        {
                            tools.RecordClip(new RectangleF(Location, StringSize));
                        }
                        break;
                    case nameof(Alignment):
                        sf.Alignment = alignment;
                        break;
                    case nameof(LineAlignment):
                        sf.LineAlignment = lineAlignment;
                        break;
                    case nameof(Trimming):
                        sf.Trimming = trimming;
                        break;
                    case nameof(TextColor):
                        brush.Dispose();
                        brush = new SolidBrush(textColor);
                        break;
                }
            }
            catch (Exception ex)
            {
                __messagePipe.OnMessageSend("GDI_Text_PropertyChanged failed", DrawingMessageLevel.Error, ex);
            }


        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                font.Dispose();
                tools.Dispose();
                sf.Dispose();
                brush.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
