using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Draw.NET.Renderer;

namespace Draw.NET.Renderer.GDIPlus.Primitive
{
    public class GDIPlusRenderer : AbstractRenderer
    {
        private Graphics __graphics;
        private BufferedGraphics __bufferedGraphics;
        private BufferedGraphicsContext __context;
        private readonly bool __disposeContext = true;

        private IntPtr __handle;


        public static readonly bool DOUBLE_BUFFERED = true;
        private bool __initialized;

        public GDIPlusRenderer(IntPtr handle, SizeF initialSize) : base()
        {

            if (DOUBLE_BUFFERED)
            {
                __context = new BufferedGraphicsContext();
                __disposeContext = true;
            }

            if (__handle == null)
            {
                throw new ArgumentNullException("handle");
            }

            __handle = handle;

            if (DOUBLE_BUFFERED)
            {
                __bufferedGraphics = CreateGraphics(handle, initialSize, __context);
                __graphics = __bufferedGraphics.Graphics;
            }
            else
            {
                __graphics = Graphics.FromHwnd(handle);
            }

            __graphics.CompositingMode = CompositingMode.SourceOver;
            __graphics.CompositingQuality = CompositingQuality.HighSpeed;
            __graphics.InterpolationMode = InterpolationMode.Default;
            __graphics.PixelOffsetMode = PixelOffsetMode.Default;
            __graphics.SmoothingMode = SmoothingMode.Default;
            //__graphics.TextContrast
            __graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault;
        }



        /// <summary>
        /// 渲染一次
        /// </summary>
        protected override void OnRendering(RectangleF clip)
        {

            __graphics.Clear(Configuration.BackgroundColor);
            if (Configuration.ScaleLineWidth)
            {
                __graphics.Transform = Configuration.TransformMatrix;
            }

            DrawingArgs state = default(DrawingArgs);
            if (Configuration.ScaleLineWidth)
            {
                state = new DrawingArgs(__graphics);
            }
            else
            {
                state = new DrawingArgs(__graphics, Configuration.TransformMatrix);
            }
            OnLayerRendering(state);

#if PerfMon
            var pts1 = new PointF[] { Point.Empty };
            Configuration.TransformMatrix.TransformPoints(pts1);
            System.Diagnostics.Debug.WriteLine($"Elements: {(string.Join(",", Configuration.TransformMatrix.Elements))}");
            System.Diagnostics.Debug.WriteLine($"Tranformed:{pts1[0]}");
#endif
            //__graphics.Flush(FlushIntention.Flush);
            if (DOUBLE_BUFFERED)
            {
                __bufferedGraphics.Render();
            }
            if (!__initialized)
            {
                __initialized = true;
            }
        }

        /// <summary>
        /// 准备渲染的数据
        /// </summary>
        protected override void Prepare(RectangleF clip)
        {
            if (!__initialized)
            {
                __graphics.ResetClip();
                __graphics.Clear(Color.LightGray);
            }
            else
            {
                if (clip != default(RectangleF))
                {
                    __graphics.SetClip(clip, CombineMode.Replace);
                }
                else
                {
                    __graphics.ResetClip();
                }
            }

            //var state = new PrepareArgs(__graphics, !__initialized);
            //OnPrepare(state);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                __graphics.Dispose();
                if (DOUBLE_BUFFERED)
                {
                    __bufferedGraphics.Dispose();
                    if (__disposeContext)
                    {
                        __context.Dispose();
                    }
                }
            }
            __graphics = null;
            __handle = IntPtr.Zero;

        }

        protected override void OnRenderSizeChanged(SizeF newSize)
        {
            var task = Task.Factory.StartNew(RecreateGraphics);
            var complete = task.ContinueWith(t =>
             {
                 if (t.IsFaulted)
                 {
                     __messagePipe.OnMessageSend(t.Exception.Message, DrawingMessageLevel.Error, t.Exception);
                 }

                 Render();
             });
            complete.Wait();
        }




        private BufferedGraphics CreateGraphics(IntPtr handle, SizeF initialSize, BufferedGraphicsContext bgc)
        {
            bgc.MaximumBuffer = Size.Ceiling(initialSize);
            return bgc.Allocate(Graphics.FromHwnd(handle), new Rectangle(new Point(), bgc.MaximumBuffer));
        }

        private void RecreateGraphics()
        {
            __graphics.Dispose();
            if (DOUBLE_BUFFERED)
            {
                __bufferedGraphics.Dispose();
                __bufferedGraphics = CreateGraphics(__handle, Configuration.ClientSize, __context);
                __graphics = __bufferedGraphics.Graphics;
            }
            else
            {
                __graphics = Graphics.FromHwnd(__handle);
            }
            __initialized = false;
        }

    }

}
