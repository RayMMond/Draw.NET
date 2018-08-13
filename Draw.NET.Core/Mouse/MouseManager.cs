/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       CursorManager 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/21 17:14:13
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using Draw.NET.Core.Layers;
using Draw.NET.Core.Shapes;
using Draw.NET.Renderer;

namespace Draw.NET.Core.Mouse
{
    public class MouseManager : IDisposable, INotifyPropertyChanged, IMessagePipe
    {
        #region 预定义光标样式
        //public readonly Cursor ENTERING_SHAPE;
        //public readonly Cursor MOVING_SHAPE;
        //public readonly Cursor DRAGGING;
        //public readonly Cursor DEFAULT;
        //public readonly Cursor ZOOM_OUT;
        //public readonly Cursor ZOOM_IN;
        #endregion

        private bool disposedValue = false;
        private bool __multiSelect = false;
        private bool __moveToCopy = false;
        private bool __resizeFixRadio = true;


        //private Cursor __cur;
        private List<UserLayer> __userLayers;
        private OperationLayer __opLayer;
        private MouseState __state;
        private UserLayer __defaultLayer;
        private MessagePipe __messagePipe;
        private PointF __lastPoint;
        private List<AbstractShape> __selectedShapes;


        #region 构造
        MouseManager()
        {
            __selectedShapes = new List<AbstractShape>();
            __lastPoint = PressedPoint = PointF.Empty;
            //ENTERING_SHAPE = new Cursor(Cursors.Arrow.CopyHandle());
            //MOVING_SHAPE = new Cursor(Cursors.SizeAll.CopyHandle());
            //DRAGGING = new Cursor(Cursors.Hand.CopyHandle());
            //ZOOM_IN = new Cursor(Cursors.Cross.CopyHandle());
            //ZOOM_OUT = new Cursor(Cursors.Cross.CopyHandle());
            //__cur = DEFAULT = new Cursor(Cursors.Arrow.CopyHandle());
            __state = StateManager.Normal;
            __messagePipe = new MessagePipe(this);
            PropertyChanged += MouseManager_PropertyChanged;
        }

        internal MouseManager(List<UserLayer> userLayers, OperationLayer operationLayer) : this()
        {
            __defaultLayer = userLayers[0];
            __userLayers = userLayers;
            __opLayer = operationLayer;
        }
        #endregion

        #region 属性


        public MouseState State
        {
            get { return __state; }
            set
            {
                if (__state != value)
                {
                    __state = value;
                    OnPropertyChanged("State");
                }
            }
        }

        public bool IsPressed { get; set; } = false;
        public bool IsDoubleClicked { get; set; } = false;


        public bool MultiSelect
        {
            get { return __multiSelect; }
            set
            {
                if (__multiSelect != value)
                {
                    __multiSelect = value;
                    OnMultiSelectChanged(value);
                }
            }
        }

        public bool MoveToCopy
        {
            get { return __moveToCopy; }
            set
            {
                if (__moveToCopy != value)
                {
                    __moveToCopy = value;
                    OnMoveToCopyChagned(value);
                }
            }
        }

        public bool ResizeFixRadio
        {
            get { return __resizeFixRadio; }
            set
            {
                if (__resizeFixRadio != value)
                {
                    __resizeFixRadio = value;
                    OnResizeFixRadioChanged(value);
                }
            }
        }

        public PointF PressedPoint { get; set; }

        public PointF LastPoint { get { return __lastPoint; } }

        public List<AbstractShape> SelectedShapes
        {
            get
            {
                __selectedShapes = __selectedShapes.Distinct().ToList();
                return __selectedShapes;
            }
        }

        public List<UserLayer> UserLayers { get { return __userLayers; } }

        public UserLayer DefaultUserLayer { get { return __defaultLayer; } }

        public OperationLayer OperationLayer { get { return __opLayer; } }


        public MessagePipe Message { get { return __messagePipe; } }


        #endregion

        //public event EventHandler<CursorChangedEventArgs> CursorChanged;

        public event PropertyChangedEventHandler PropertyChanged;

        public event EventHandler<BeginTextingEventArgs> BeginEditingEvent;

        /// <summary>
        /// 消息管道监听
        /// </summary>
        public event EventHandler<DrawingFrameworkMessage> MessageListener
        {
            add { __messagePipe.MessageListener += value; }
            remove { __messagePipe.MessageListener -= value; }
        }


        #region 公共方法


        public List<AbstractShape> MouseMove(PointF pt)
        {
            if (pt != __lastPoint)
            {
                var s = State.MouseMove(this, pt);

                __lastPoint = pt;

                return s;
            }
            return null;
        }

        public AbstractShape MouseDown(PointF pt)
        {
            IsPressed = true;
            __lastPoint = PressedPoint = pt;

            var s = State.MouseDown(this, pt);

            return s;
        }

        public void MouseUp(PointF pt)
        {
            IsPressed = false;
            State.MouseUp(this, pt);
            __lastPoint = pt;
            PressedPoint = PointF.Empty;
        }

        public AbstractShape MouseDoubleClick(PointF pt)
        {
            IsDoubleClicked = true;
            AbstractShape shape = HitShape(pt);
            if (shape != null)
            {
                if(shape is Text)
                {
                    Text text = shape as Text;
                    BeginEditingEvent?.Invoke(this, new BeginTextingEventArgs(text.GetBounds(), text, text.TextString));
                }
                State = StateManager.GetStateByShape(shape, false, true);
                if (State != null)
                {
                    State.MouseDown(this, pt);
                }
                else
                {
                    State = StateManager.Normal;
                }
            }
            return shape;
        }

        public void ResetMouse()
        {
            IsPressed = false;
            IsDoubleClicked = false;
            __lastPoint = PressedPoint = PointF.Empty;
            State = StateManager.Normal;
        }


        public AbstractShape HitShape(PointF pt)
        {
            var s = __opLayer.IsVisible(pt);
            if (s != null)
            {
                return s;
            }
            else
            {
                return __userLayers.IsVisible(pt);
            }
        }

        public List<AbstractShape> HitAllShape(RectangleF rect)
        {
            return __userLayers.IsVisible(rect);
        }


        //public void SetCursor(Cursor cursor)
        //{
        //    OnCursorChanged(cursor);
        //}


        //public void SetcursorByCornerType(CornerType type)
        //{
        //    switch (type)
        //    {
        //        case CornerType.LeftTop:
        //            SetCursor(Cursors.SizeNWSE);
        //            break;
        //        case CornerType.LeftMiddle:
        //            SetCursor(Cursors.SizeWE);
        //            break;
        //        case CornerType.LeftBottom:
        //            SetCursor(Cursors.SizeNESW);
        //            break;
        //        case CornerType.Top:
        //            SetCursor(Cursors.SizeNS);
        //            break;
        //        case CornerType.Bottom:
        //            SetCursor(Cursors.SizeNS);
        //            break;
        //        case CornerType.RightTop:
        //            SetCursor(Cursors.SizeNESW);
        //            break;
        //        case CornerType.RightMiddle:
        //            SetCursor(Cursors.SizeWE);
        //            break;
        //        case CornerType.RightBottom:
        //            SetCursor(Cursors.SizeNWSE);
        //            break;
        //        case CornerType.Center:
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

        #region 私有方法


        private void OnResizeFixRadioChanged(bool value)
        {
            throw new NotImplementedException();
        }

        private void OnMoveToCopyChagned(bool value)
        {
            throw new NotImplementedException();
        }

        private void OnMultiSelectChanged(bool value)
        {
            throw new NotImplementedException();
        }

        //private void OnCursorChanged(Cursor cur)
        //{
        //    CursorChanged?.Invoke(this, new CursorChangedEventArgs(cur));
        //}

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private void MouseManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "State":
                    //TODO:SYP 完成鼠标状态转换
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    //ENTERING_SHAPE.Dispose();
                    //MOVING_SHAPE.Dispose();
                    //DRAGGING.Dispose();
                    //DEFAULT.Dispose();
                    //ZOOM_OUT.Dispose();
                    //ZOOM_IN.Dispose();
                    __selectedShapes.Clear();
                    __messagePipe.Dispose();
                    __userLayers.ForEach(l => l.Dispose());
                    __userLayers.Clear();
                    __opLayer.Clear();
                    __opLayer.Dispose();
                }

                //__cur = null;
                __userLayers = null;
                __opLayer = null;
                __defaultLayer = null;
                __messagePipe = null;

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }

        #endregion

    }



    //public class CursorChangedEventArgs : EventArgs
    //{
    //    public readonly Cursor NewCursor;

    //    public CursorChangedEventArgs(Cursor cur)
    //    {
    //        NewCursor = cur;
    //    }
    //}
}
