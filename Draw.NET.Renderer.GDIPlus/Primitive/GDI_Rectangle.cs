/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       GDI_Rectangle 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/3 12:52:44
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Draw.NET.Renderer.Primitives;

namespace Draw.NET.Renderer.GDIPlus.Primitive
{
    public class GDI_Rectangle : Primitives.AbstractRectangle, IText
    {
        GDIPlusTools tools;
        RectangleF rect;
        GDI_Text text;


        public GDI_Rectangle(PointF location, SizeF size) : base(location, size)
        {
            rect = new RectangleF(location, size);
            text = new GDI_Text();

            tools = new GDIPlusTools(this, this, rect);
            tools.Drawing += Tools_Drawing;
            //tools.Preparing += Tools_Preparing;
            PropertyChanged += GDI_Rectangle_PropertyChanged;
        }


        public StringAlignment Alignment
        {
            get
            {
                return ((IText)text).Alignment;
            }

            set
            {
                ((IText)text).Alignment = value;
            }
        }

        public string Content
        {
            get
            {
                return ((IText)text).Content;
            }

            set
            {
                ((IText)text).Content = value;
            }
        }

        public string FamilyName
        {
            get
            {
                return ((IText)text).FamilyName;
            }

            set
            {
                ((IText)text).FamilyName = value;
            }
        }

        public float FontSize
        {
            get
            {
                return ((IText)text).FontSize;
            }

            set
            {
                ((IText)text).FontSize = value;
            }
        }

        public FontStyle FontStyle
        {
            get
            {
                return ((IText)text).FontStyle;
            }

            set
            {
                ((IText)text).FontStyle = value;
            }
        }

        public StringAlignment LineAlignment
        {
            get
            {
                return ((IText)text).LineAlignment;
            }

            set
            {
                ((IText)text).LineAlignment = value;
            }
        }


        public SizeF StringSize
        {
            get
            {
                return ((IText)text).StringSize;
            }

            set
            {
                ((IText)text).StringSize = value;
            }
        }

        public StringTrimming Trimming
        {
            get
            {
                return ((IText)text).Trimming;
            }

            set
            {
                ((IText)text).Trimming = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return ((IText)text).TextColor;
            }

            set
            {
                ((IText)text).TextColor = value;
            }
        }

        public LocationType LocationType
        {
            get
            {
                return ((IText)text).LocationType;
            }

            set
            {
                ((IText)text).LocationType = value;
            }
        }

        public Font Font
        {
            get
            {
                return ((IText)text).Font;
            }
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
            RectangleF rectToDraw = rect;
            if (arg.Transform != null)
            {
                if (drawModel == DrawModel.Center)
                {
                    var center = new PointF[] { Center };
                    arg.Transform.TransformPoints(center);
                    rectToDraw = new RectangleF(center[0].X - rect.Width / 2, center[0].Y - rect.Height / 2, rect.Width, rect.Height);
                }
                else
                {
                    var ltrb = new PointF[] { rect.Location, new PointF(rect.Right, rect.Bottom) };
                    arg.Transform.TransformPoints(ltrb);
                    rectToDraw = RectangleF.FromLTRB(ltrb[0].X, ltrb[0].Y, ltrb[1].X, ltrb[1].Y);
                }
            }
            if (tools.DrawingBrush != Brushes.Transparent)
            {
                arg.Graphics.FillRectangle(tools.DrawingBrush, rectToDraw);
            }
            arg.Graphics.DrawRectangles(tools.DrawingPen, new RectangleF[] { rectToDraw });


            if (!string.IsNullOrWhiteSpace(Content)
                && arg.Transform.Elements[0] >= 0.6)
            {
                if (LocationType == LocationType.Relative)
                {
                    text.StringSize = arg.Graphics.MeasureString(Content, text.Font);
                    int x = 0;
                    if (Alignment == StringAlignment.Far)
                    {
                        x = -(int)Math.Ceiling(text.StringSize.Width);
                    }
                    text.Location = new Point(x, (int)Math.Ceiling(-text.StringSize.Height));
                    text.Location = Point.Ceiling(rectToDraw.GetCornerPoint(CornerType.Top) + new SizeF(text.Location));
                }

                text.DrawText(new RectangleF(text.Location, text.StringSize), arg.Graphics);
            }
        }


        private void GDI_Rectangle_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            tools.WaitForRendering();
            try
            {
                switch (e.PropertyName)
                {
                    case nameof(Description):
                        //TODO change Description
                        break;
                    case nameof(Layer):
                        break;
                    case nameof(Location):
                    case nameof(RectHeight):
                    case nameof(RectWidth):
                    case nameof(Visible):
                        rect = Rectangle.Ceiling(GetBounds());
                        if (rect.Size != SizeF.Empty)
                        {
                            tools.RecordClip(rect);
                        }

                        break;
                    case nameof(FillPattern):
                        tools.UpdateFillPattern();
                        break;

                    //case nameof(TextColor):

                    //    break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                __messagePipe.OnMessageSend("GDI_Rectangle_PropertyChanged failed.", DrawingMessageLevel.Error, ex);
            }

        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                tools.Dispose();
                text.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
