/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       GDIPlusTools 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/3 12:32:06
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
    internal class GDIPlusTools : IDisposable
    {
        private IStyle style;
        private bool disposedValue = false;
        private bool rendering = false;
        private IMessagePipe mp;
        //private Region clip;
        private FillPattern fillPattern;


        public GDIPlusTools(IStyle style, IMessagePipe mp, RectangleF rect)
        {
            this.style = style;
            this.mp = mp;

            fillPattern = style.FillPattern;

            DrawingPen = GetNewPen(style);
            DrawingBrush = GetNewBrush(fillPattern);

            fillPattern.PropertyChanged += Pattern_PropertyChanged;
            style.BorderPattern.PropertyChanged += BorderPattern_PropertyChanged;

            ExpandRect(ref rect);
            Bounds = rect;
        }


        public Pen DrawingPen { get; private set; }

        public Brush DrawingBrush { get; private set; }

        public RectangleF Bounds { get; private set; }

        public bool IsRendering { get { return rendering; } }

        public event DrawingEvent Drawing;

        //public event PrepareEvent Preparing;


        public void Prepare(object state)
        {
            if (disposedValue)
                return;

            rendering = true;
            try
            {
                if (state is PrepareArgs arg)
                {

                    //if (clip != null)
                    //{
                    //    //if (!arg.RenderAll)
                    //    //{
                    //    //    arg.Graphics.SetClip(clip, System.Drawing.Drawing2D.CombineMode.Union);
                    //    //}
                    //    clip.Dispose();
                    //    clip = null;
                    //}
                    //Preparing?.Invoke(arg);
                }
                else
                {
                    throw new ArgumentNullException("state");
                }
            }
            finally
            {
                rendering = false;
            }

        }

        public void Draw(object state)
        {
            if (disposedValue)
                return;

            rendering = true;
            try
            {
                if (state is DrawingArgs arg)
                {
                    Drawing?.Invoke(arg);
                }
                else
                {
                    throw new ArgumentNullException("state");
                }
            }
            finally
            {
                rendering = false;
            }

        }

        public void RecordClip(RectangleF rect)
        {
            //if (clip == null)
            //  {  ExpandRect(ref Bounds);
            //clip = new Region(Bounds);}
            Bounds = rect;
            ExpandRect(ref rect);
            //clip.Union(rect);
        }

        public void WaitForRendering()
        {
            while (IsRendering)
                Thread.Sleep(2);
        }

        public void UpdateFillPattern()
        {
            WaitForRendering();
            if (fillPattern != null) fillPattern.PropertyChanged -= Pattern_PropertyChanged;
            fillPattern = style.FillPattern;
            fillPattern.PropertyChanged += Pattern_PropertyChanged;

            DrawingBrush = GetNewBrush(style.FillPattern);
        }

        private void Pattern_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            WaitForRendering();
            DrawingBrush = GetNewBrush(style.FillPattern);
        }

        private void BorderPattern_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case nameof(BorderPattern.Color):
                    DrawingPen.Color = style.BorderPattern.Color;
                    break;
                case nameof(BorderPattern.BackArrowSize):

                    break;
                case nameof(BorderPattern.BackArrowType):

                    break;
                case nameof(BorderPattern.DashCap):
                    DrawingPen.DashCap = style.BorderPattern.DashCap;
                    break;
                case nameof(BorderPattern.DashCapVisible):

                    break;
                case nameof(BorderPattern.DashStyle):
                    DrawingPen.DashStyle = style.BorderPattern.DashStyle;
                    break;
                case nameof(BorderPattern.FrontArrowSize):

                    break;
                case nameof(BorderPattern.FrontArrowType):

                    break;
                case nameof(BorderPattern.Width):
                    DrawingPen.Width = style.BorderPattern.Width;
                    break;
                default:
                    break;
            }
        }


        private Pen GetNewPen(IStyle s)
        {
            var pen = new Pen(s.BorderPattern.Color, s.BorderPattern.Width)
            {
                Alignment = System.Drawing.Drawing2D.PenAlignment.Center,
                //pen.CustomEndCap = border.DashCap;
                // pen.CustomStartCap
                DashCap = s.BorderPattern.DashCap,
                //pen.DashOffset = 
                //pen.DashPattern = 
                DashStyle = s.BorderPattern.DashStyle
            };
            //pen.EndCap
            //pen.LineJoin=
            //pen.MiterLimit =
            //pen.PenType
            //pen.StartCap
            return pen;
        }

        private Brush GetNewBrush(FillPattern fillPattern)
        {
            switch (fillPattern.FillType)
            {
                case FillType.None:
                    return Brushes.Transparent;
                case FillType.Solid:
                    return new SolidBrush(style.FillPattern.Color);
                case FillType.LinearGradient:
                    throw new NotImplementedException();
                case FillType.Texture:
                    throw new NotImplementedException();
            }

            throw new ArgumentOutOfRangeException("图元的填充类型无效！");
        }


        private static void ExpandRect(ref RectangleF rect)
        {
            rect.Location -= new SizeF(2, 2);
            rect.Height += 6;
            rect.Width += 6;
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //if (clip != null)
                    //{
                    //    clip.Dispose();
                    //}
                    DrawingPen.Dispose();
                    DrawingBrush.Dispose();
                    style.BorderPattern.PropertyChanged -= BorderPattern_PropertyChanged;
                }


                mp = null;
                style = null;
                Drawing = null;
                //Preparing = null;


                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}
