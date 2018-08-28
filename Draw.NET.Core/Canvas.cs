/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       Canvas 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/19 15:33:21
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using Draw.NET.Core.Layers;
using Draw.NET.Core.Mouse;
using Draw.NET.Core.Shapes;
using Draw.NET.Renderer;
using Draw.NET.Renderer.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Draw.NET.Core
{
    /// <summary>
    /// 画布对象
    /// </summary>
    public class Canvas : ICanvas
    {
        private bool disposedValue = false;

        private BackgroundLayer __bgLayer;
        private MouseManager __mouse;
        private TransformManager __trans;
        private MessagePipe __mp;
        private readonly IPrimitiveProvider primitiveProvider;


        /// <summary>
        /// 通过控件指针和大小初始化画布
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="initialSize"></param>
        public Canvas(IRenderer renderer, IPrimitiveProvider provider)
        {
            primitiveProvider = provider ?? throw new ArgumentNullException(nameof(provider));
            Renderer = renderer;
        }

        public event EventHandler<BeginTextingEventArgs> BeginEditingEvent
        {
            add { __mouse.BeginEditingEvent += value; }
            remove { __mouse.BeginEditingEvent -= value; }
        }

        //public event EventHandler<CursorChangedEventArgs> CursorChanged
        //{
        //    add { __mouse.CursorChanged += value; }
        //    remove { __mouse.CursorChanged -= value; }
        //}

        /// <summary>
        /// 消息管道监听
        /// </summary>
        public event EventHandler<DrawingFrameworkMessage> MessageListener
        {
            add { __mp.MessageListener += value; }
            remove { __mp.MessageListener -= value; }
        }

        /// <summary>
        /// 渲染器
        /// </summary>
        public IRenderer Renderer { get; private set; }

        public bool IsDisposed { get { return disposedValue; } }

        public void Load(IntPtr handle, SizeF initialSize)
        {
            Renderer.Initialize(handle, initialSize);
            Initialize(initialSize, primitiveProvider);

        }

        public void Load(IntPtr handle, Size initialSize)
        {
            Renderer.Initialize(handle, initialSize);
            Initialize(initialSize, primitiveProvider);
        }

        /// <summary>
        /// 将图层居中缩放
        /// </summary>
        /// <param name="layerID">需要Fit的图层ID，当为null时，Fit所有图层</param>
        public void Fit(string layerID = null)
        {
            if (IsDisposed)
            {
                return;
            }

            RectangleF bounds = RectangleF.Empty;
            if (!string.IsNullOrWhiteSpace(layerID))
            {
                var layer = __mouse.UserLayers.FirstOrDefault(l => l.UUID == layerID);
                if (layer != null)
                {
                    bounds = layer.GetBounds();
                }
            }
            else
            {
                __mouse.UserLayers.ForEach(l =>
                {
                    if (bounds == RectangleF.Empty)
                    {
                        bounds = l.GetBounds();
                    }
                    else
                    {
                        bounds = RectangleF.Union(bounds, l.GetBounds());
                    }

                });
            }
            var clientSize = Renderer.Configuration.ClientSize;

            var size = clientSize.GetResizeFactor(bounds.Size);

#if PerfMon
            System.Diagnostics.Debug.WriteLine($"bounds:{bounds}, size:{size}");
#endif
            __trans.Reset();
            __trans.MoveTo(-bounds.X, -bounds.Y);
            __trans.Scale(Point.Empty, size * 0.9f);
            __trans.MoveTo(clientSize.Width * size * 0.05f, clientSize.Height * size * 0.05f);
            Renderer.Render();

        }

        /// <summary>
        /// 清除画布，释放所有对象
        /// </summary>
        public void Clear()
        {
            __mouse.OperationLayer.Clear();
            __mouse.UserLayers.ForEach(l => l.Clear());
        }

        /// <summary>
        /// 改变显示大小，在上层控件大小改变时调用
        /// </summary>
        /// <param name="newSize">新的大小</param>
        public void ChangeSize(SizeF newSize)
        {
            if (IsDisposed)
            {
                return;
            }

            Renderer.ChangeSize(newSize);
        }


        #region 鼠标功能接口
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="actualPoint">实际坐标，可用于界面显示</param>
        /// <returns></returns>
        public AbstractShape MouseDown(PointF pt, out PointF actualPoint)
        {
            actualPoint = Point.Empty;
            if (IsDisposed)
            {
                return null;
            }

            actualPoint = __trans.GetActualPoint(pt);
            return __mouse.MouseDown(actualPoint);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="actualPoint"></param>
        /// <returns></returns>
        public List<AbstractShape> MouseMove(PointF pt, out PointF actualPoint)
        {
            actualPoint = Point.Empty;
            if (IsDisposed)
            {
                return null;
            }

            actualPoint = __trans.GetActualPoint(pt);
            var list = __mouse.MouseMove(actualPoint);
            if (__mouse.IsPressed)
            {
                Renderer.Render();
            }
            return list;
        }

        public void ResetMouse()
        {
            if (IsDisposed)
            {
                return;
            }

            __mouse.ResetMouse();
        }

        public void MouseUp(PointF pt, out PointF actualPoint)
        {
            actualPoint = Point.Empty;
            if (IsDisposed)
            {
                return;
            }

            actualPoint = __trans.GetActualPoint(pt);
            __mouse.MouseUp(actualPoint);
            Renderer.Render();
        }


        public AbstractShape MouseDoubleClick(PointF pt, out PointF actualPoint)
        {
            actualPoint = Point.Empty;
            if (IsDisposed)
            {
                return null;
            }

            actualPoint = __trans.GetActualPoint(pt);
            return __mouse.MouseDoubleClick(actualPoint);
        }

        #endregion

        #region 拖动、缩放整体
        /// <summary>
        /// 开始拖动时调用
        /// </summary>
        /// <param name="pt">开始拖动时的屏幕坐标</param>
        public void BeginDrag(PointF pt)
        {
            if (IsDisposed)
            {
                return;
            }

            //__mouse.SetCursor(__mouse.DRAGGING);
            __trans.BeginMove(pt);
        }
        /// <summary>
        /// 拖动中调用
        /// </summary>
        /// <param name="pt">拖动时的屏幕坐标</param>
        public void Dragging(PointF pt)
        {
            if (IsDisposed)
            {
                return;
            }

            __trans.Move(pt);
            Renderer.Render();
        }
        /// <summary>
        /// 停止拖动
        /// </summary>
        public void EndDrag()
        {
            if (IsDisposed)
            {
                return;
            }
            //__mouse.SetCursor(__mouse.DEFAULT);
            __trans.EndMove();
            Renderer.Render();

        }
        /// <summary>
        /// 以默认缩放比例进行缩放
        /// </summary>
        /// <param name="pt">缩放中心的屏幕坐标</param>
        /// <param name="zoomOut">当True时缩小，反之放大</param>
        public void Scale(PointF pt, bool zoomOut)
        {
            if (zoomOut)
            {
                Scale(pt, 1.1F);
            }
            else
            {
                Scale(pt, 0.9F);
            }

        }
        /// <summary>
        /// 缩放
        /// </summary>
        /// <param name="pt">缩放中心的屏幕坐标</param>
        /// <param name="scale">缩放比例</param>
        public void Scale(PointF pt, float scale)
        {
            if (IsDisposed)
            {
                return;
            }
            //if (scale >= 1F)
            //{
            //    __mouse.SetCursor(__mouse.ZOOM_OUT);
            //}
            //else
            //{
            //    __mouse.SetCursor(__mouse.ZOOM_IN);
            //}
            __trans.Scale(pt, scale);
            Renderer.Render();
            //__mouse.SetCursor(__mouse.DEFAULT);
        }

        #endregion

        #region 对齐功能
        public void AutoAlign()
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }

        public void AlignLeft()
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }

        public void AlignCenter()
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }
        public void AlignRight()
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }
        public void AlignTop()
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }
        public void AlignMiddle()
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }
        public void AlignBottom()
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }

        #endregion

        #region 自动排布位置
        public void AutoSpace()
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }

        public void DistributeVertically()
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }

        public void DistributeHorizonally()
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }
        #endregion

        #region 图层管理
        /// <summary>
        /// 根据需求实现
        /// </summary>
        public UserLayer GetNewUserLayer(string name = null)
        {
            if (IsDisposed)
            {
                return null;
            }
            //待实现
            throw new NotImplementedException();
        }
        #endregion

        #region 图形管理
        /// <summary>
        /// 移除指定图形
        /// </summary>
        /// <param name="shape"></param>
        public void RemoveShape(AbstractShape shape)
        {
            if (IsDisposed)
            {
                return;
            }

            throw new NotImplementedException();
        }
        /// <summary>
        /// 增加图形
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="layerID"></param>
        /// <param name="layerName"></param>
        public void AddShape(AbstractShape shape, string layerID = null, string layerName = null)
        {
            if (IsDisposed)
            {
                return;
            }

            if (!string.IsNullOrWhiteSpace(layerID))
            {
                __mouse.UserLayers.FirstOrDefault(l => l.UUID == layerID)?.Add(shape);
            }
            else if (!string.IsNullOrWhiteSpace(layerName))
            {
                __mouse.UserLayers.FirstOrDefault(l => l.Name == layerName)?.Add(shape);
            }
            else
            {
                __mouse.UserLayers.FirstOrDefault(l => l.Name == UserLayer.DEFAULT_NAME)?.Add(shape);
            }
            __mouse.OperationLayer.Add(shape);
        }

        public void AddShapes(string layerID = null, string layerName = null, params AbstractShape[] shapes)
        {
            if (shapes != null)
            {
                foreach (var shape in shapes)
                {
                    AddShape(shape, layerID, layerName);
                }
            }
        }

        /// <summary>
        /// 根据ID获取图形
        /// </summary>
        /// <param name="shapeID"></param>
        /// <param name="layerID"></param>
        /// <param name="layerName"></param>
        /// <returns></returns>
        public AbstractShape GetShapeByID(string shapeID, string layerID = null, string layerName = null)
        {
            if (!string.IsNullOrWhiteSpace(shapeID))
            {
                if (IsDisposed)
                {
                    return null;
                }

                if (!string.IsNullOrWhiteSpace(layerID))
                {
                    return __mouse.UserLayers.First(l => l.UUID == layerID).GetByID(shapeID);
                }
                else if (!string.IsNullOrWhiteSpace(layerName))
                {
                    return __mouse.UserLayers.First(l => l.Name == layerName).GetByID(shapeID);
                }
                else
                {
                    return __mouse.UserLayers.First(l => l.Name == UserLayer.DEFAULT_NAME).GetByID(shapeID);
                }

            }
            return null;
        }

        /// <summary>
        /// 取消选中所有图形
        /// </summary>
        public void UnselectAll()
        {
            __mouse.SelectedShapes.ForEach(s => s.UnSelect());
            __mouse.SelectedShapes.Clear();
        }

        /// <summary>
        /// 根据ID选择图形
        /// </summary>
        /// <param name="shapeID"></param>
        public void SelectShape(string shapeID)
        {
            var shape = GetShapeByID(shapeID);
            shape.Select();
            __mouse.SelectedShapes.Add(shape);
        }


        #endregion

        #region 私有



        private void Initialize(SizeF initialSize, IPrimitiveProvider provider)
        {
            if (Renderer.Configuration.ClientSize != initialSize)
            {
                Renderer.Configuration.ChangeSize(initialSize);
            }
            Renderer.MessageListener += AllMessageListener;
            __trans = new TransformManager(Renderer.Configuration);
            __bgLayer = new BackgroundLayer(Renderer.GetNewLayer(0, BackgroundLayer.NAME), initialSize);
            __mp = new MessagePipe(this);
            var ul = new List<UserLayer>
            {
                new UserLayer(Renderer.GetNewLayer(1, UserLayer.DEFAULT_NAME))
            };
            var ol = new OperationLayer(Renderer.GetNewLayer(99, OperationLayer.NAME), provider);
            __mouse = new MouseManager(ul, ol);
            __mouse.MessageListener += AllMessageListener;
            //__mouse.CursorChanged += Mouse_CursorChanged;
        }


        private void AllMessageListener(object sender, DrawingFrameworkMessage e)
        {
            __mp.OnMessageSend(e);
        }

        //private void Mouse_CursorChanged(object sender, CursorChangedEventArgs e)
        //{
        //    Renderer.Render();
        //}

        #endregion

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Renderer.Dispose();
                    __mouse.Dispose();
                    __bgLayer.Dispose();
                    __trans.Dispose();
                    __mp.Dispose();
                }
                Renderer = null;
                __mouse = null;
                __bgLayer = null;
                __trans = null;
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
