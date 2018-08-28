using Draw.NET;
using Draw.NET.Core;
using Draw.NET.Renderer.Primitives;
using System;
using System.Drawing;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;

namespace Test.WPF
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        ICanvas canvas;
        bool beginDrag = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Initialized(object sender, EventArgs e)
        {
            var handle = new WindowInteropHelper(this).Handle;
            canvas = App.Container.GetInstance<ICanvas>();
            canvas.Load(handle, new System.Drawing.Size((int)Width, (int)Height));
            canvas.MessageListener += Canvas_MessageListener;
            canvas.Renderer.Configuration.BackgroundColor = System.Drawing.Color.White;
            canvas.Renderer.Configuration.Mode = Draw.NET.RenderMode.OnRenderCall;
            var rect = new Draw.NET.Core.Shapes.Rect(App.Container.GetInstance<IPrimitiveProvider>())
            {
                Location = new PointF(100, 100),
                RectWidth = 200,
                RectHeight = 200,
                FillPattern = new FillPattern.Solid() { Color = System.Drawing.Color.Red }
            };
            canvas.AddShape(rect);
            canvas.Renderer.Render();
        }

        private void Canvas_MessageListener(object sender, DrawingFrameworkMessage e)
        {

        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Released)
            {
                canvas.MouseMove(e.GetPosition(this).ToPoint(), out PointF point);
            }
            else
            {
                canvas.EndDrag();
            }
        }

        private void Window_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            canvas.Scale(e.GetPosition(this).ToPoint(), e.Delta > 0);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                canvas.MouseDown(e.GetPosition(this).ToPoint(), out PointF point);
            }
            else
            {
                canvas.BeginDrag(e.GetPosition(this).ToPoint());
                beginDrag = true;
            }
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            if (beginDrag)
            {
                canvas.Dragging(e.GetPosition(this).ToPoint());
            }
            else
            {
                canvas.MouseMove(e.GetPosition(this).ToPoint(), out PointF point);
            }
        }

        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            canvas.ChangeSize(e.NewSize.ToSize());
        }
    }
}
