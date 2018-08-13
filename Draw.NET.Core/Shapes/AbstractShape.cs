/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       AbstractShape 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/20 14:29:48
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Draw.NET.Renderer.Primitives;
using Draw.NET.Renderer.Styles;

namespace Draw.NET.Core.Shapes
{
    public abstract class AbstractShape : IDisposable, IStyle
    {
        private bool disposedValue = false;
        protected AbstractRectangle __bound;
        protected PointF __defaultRotatePoint;
        protected PointF __location;
        protected AbstractPrimitive __primaryPrimitive;
        protected Pen __widenPen;
        protected bool __visible = true;


        public const string TOP_ANCHOR = "TopAnchor";
        public const string LEFT_ANCHOR = "LeftAnchor";
        public const string RIGHT_ANCHOR = "RightAnchor";
        public const string BOTTOM_ANCHOR = "BottomAnchor";
        
        public event EventHandler<EventArgs> EndEditingEvent;
        public event EventHandler<EventArgs> SelectedEvent;
        public event EventHandler<EventArgs> UnSelectedEvent;
        protected AbstractShape()
        {
            UUID = Guid.NewGuid().ToString();
            Anchors = new List<AnchorHandle>();
            ResizeHandles = new List<AbstractHandle>();
            CanSelect = false;
            Selected = false;
            Location = new PointF();
            IsGraphicsPathChanged = true;
        }




        #region 属性
        protected AbstractPrimitive PrimaryPrimitive
        {
            get
            {
                if (__primaryPrimitive == null)
                    throw new InvalidOperationException("未调用InitializePrimaryPrimitive方法初始化！");
                return __primaryPrimitive;
            }
        }

        public string Name { get; set; }

        public bool Visible
        {
            get
            {
                return __visible;
            }
            set
            {
                if (__visible != value)
                {
                    __visible = value;
                    OnVisibleChanged();
                }
            }
        }

        public string UUID { get; protected set; }

        public string TypeString
        {
            get
            {
                return GetType().Name;
            }
        }

        public bool CanSelect { get; set; }


        public bool Selected { get; protected set; }


        public virtual PointF Location
        {
            get
            {
                return __location;
            }
            set
            {
                if (value != __location)
                {
                    var diff = value.ToSizeF() - __location.ToSizeF();
                    __location = value;
                    __defaultRotatePoint = __location;
                    if (__bound != null)
                        __bound.Location = __location;
                    foreach (var a in Anchors)
                    {
                        a.Move(diff);
                    }
                    foreach (var a in ResizeHandles)
                    {
                        a.Move(diff);
                    }
                    IsGraphicsPathChanged = true;
                    OnLocationChanged(diff);
                }
            }
        }

        public BorderPattern BorderPattern
        {
            get
            {
                return PrimaryPrimitive.BorderPattern;
            }
        }


        public FillPattern FillPattern
        {
            get
            {
                return PrimaryPrimitive.FillPattern;
            }
            set
            {
                PrimaryPrimitive.FillPattern.PropertyChanged -= PropertyChanged;
                PrimaryPrimitive.FillPattern = value;
                PrimaryPrimitive.FillPattern.PropertyChanged += PropertyChanged;
            }
        }


        public Region Region { get; protected set; }

        public Pen WidenPen
        {
            get
            {
                if (__widenPen == null)
                {
                    __widenPen = OnCreateWidenPen();
                }
                return __widenPen;
            }
        }

        public bool IsGraphicsPathChanged { get; set; }


        #endregion


        #region Anchor
        public List<AnchorHandle> Anchors { get; protected set; }

        public AnchorHandle GetAnchorByName(string name)
        {
            return Anchors.FirstOrDefault(a => a.Name == name);
        }
        #endregion


        #region RectResizeHandle
        public List<AbstractHandle> ResizeHandles { get; protected set; }



        #endregion


        #region 公共方法

        #region 图形操作
        /// <summary>
        /// 以默认点旋转特定角度
        /// </summary>
        /// <param name="angle">0~359度</param>
        public virtual void Rotate(float angle)
        {
            RotateAt(__defaultRotatePoint, angle);
        }

        /// <summary>
        /// 以特定点旋转特定角度
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="angle">0~359度</param>
        public abstract void RotateAt(PointF pt, float angle);

        /// <summary>
        /// 移动图形
        /// </summary>
        /// <param name="vector"></param>
        public void Move(SizeF vector)
        {
            MoveShape(vector);
        }

        public void Move(PointF vector)
        {
            MoveShape(vector.ToSizeF());
        }


        public abstract void Resize(CornerType type, PointF targetPoint, bool resizeFixRadio);

        public abstract void Edit();
        #endregion


        /// <summary>
        /// 选中此图形
        /// </summary>
        public virtual void Select()
        {
            if (CanSelect)
            {
                Selected = true;
                ResizeHandles.ForEach(h => h.Visible = true);
                if (__bound != null)
                    __bound.Visible = true;
                SelectedEvent?.Invoke(this, null);
            }
        }

        public virtual void UnSelect()
        {
            Selected = false;
            ResizeHandles.ForEach(h => h.Visible = false);
            if (__bound != null)
                __bound.Visible = false;
            UnSelectedEvent?.Invoke(this, null);
        }


        protected abstract void OnLocationChanged(SizeF diff);

        /// <summary>
        /// 获取图形边界
        /// </summary>
        /// <returns></returns>
        public abstract RectangleF GetBounds();


        /// <summary>
        /// 添加到指定Layer中
        /// </summary>
        /// <param name="layer"></param>
        public virtual void AddToLayer(Renderer.ILayer<AbstractPrimitive> layer)
        {
            layer.Add(PrimaryPrimitive);
        }

        /// <summary>
        /// 从指定Layer中删除
        /// </summary>
        /// <param name="layer"></param>
        public virtual void RemoveFromLayer(Renderer.ILayer<AbstractPrimitive> layer)
        {
            layer.Remove(PrimaryPrimitive);
        }


        public virtual void AddOperationShapeToLayer(Renderer.ILayer<AbstractPrimitive> layer)
        {
            if (__bound != null)
            {
                layer.Add(__bound);
            }
            Anchors.ForEach(a => a.AddToLayer(layer));
            ResizeHandles.ForEach(a => a.AddToLayer(layer));
        }

        /// <summary>
        /// 从指定Layer中删除
        /// </summary>
        /// <param name="layer"></param>
        public virtual void RemoveOperationShapeFromLayer(Renderer.ILayer<AbstractPrimitive> layer)
        {
            Anchors.ForEach(a => a.RemoveFromLayer(layer));
            ResizeHandles.ForEach(a => a.RemoveFromLayer(layer));
            if (__bound != null)
            {
                layer.Remove(__bound);
            }
        }
        

        public void OnNodifyToRemoveTextBox()
        {
            EndEditingEvent?.Invoke(this, null);
        }

        public virtual bool IsVisible(PointF pt)
        {
            if (Visible)
            {
                if (IsGraphicsPathChanged || Region == null)
                {
                    Region?.Dispose();
                    Region = OnRegionRecreate();
                    IsGraphicsPathChanged = false;
                }

                return Region.IsVisible(pt);
            }
            else
                return false;
        }


        public bool IsVisible(RectangleF rect)
        {
            if (Visible)
            {
                if (CanvasConfig.FullSelectionMode)
                {
                    return rect.Contains(GetBounds());
                }
                else
                {
                    if (IsGraphicsPathChanged || Region == null)
                    {
                        Region?.Dispose();
                        Region = OnRegionRecreate();
                        IsGraphicsPathChanged = false;
                    }
                    return Region.IsVisible(rect);
                }

            }
            else
                return false;

        }

        public override string ToString()
        {
            return $"{Name}:{TypeString} at ({Location}). IsVisible:{Visible}. Selected:{Selected}";
        }
        #endregion

        protected void UpdateBound(PointF location, float w, float h)
        {
            __bound.Location = GetBounds().Location;
            __bound.RectWidth = w;
            __bound.RectHeight = h;
        }

        protected abstract Pen OnCreateWidenPen();

        protected abstract Region OnRegionRecreate();

        protected void InitializePrimaryPrimitiveAndBound(AbstractPrimitive pri, AbstractRectangle b)
        {
            if (pri != null)
            {
                if (__primaryPrimitive != null)
                {
                    __primaryPrimitive.FillPattern.PropertyChanged -= PropertyChanged;
                    __primaryPrimitive.BorderPattern.PropertyChanged -= PropertyChanged;
                    __primaryPrimitive.Dispose();
                }

                __primaryPrimitive = pri;
                __primaryPrimitive.FillPattern.PropertyChanged += PropertyChanged;
                __primaryPrimitive.BorderPattern.PropertyChanged += PropertyChanged;
            }

            if (b != null)
            {
                if (__bound != null)
                {
                    __bound.FillPattern.PropertyChanged -= PropertyChanged;
                    __bound.BorderPattern.PropertyChanged -= PropertyChanged;
                    __bound.Dispose();
                }

                __bound = b;
                __bound.FillPattern.PropertyChanged += PropertyChanged;
                __bound.BorderPattern.PropertyChanged += PropertyChanged;
            }
        }

        protected void InitializeDefaultAnchor(RectangleF rect)
        {
            var a = new AnchorHandle(rect.GetCornerPoint(CornerType.Top), this, TOP_ANCHOR);
            Anchors.Add(a);
            a = new AnchorHandle(rect.GetCornerPoint(CornerType.LeftMiddle), this, LEFT_ANCHOR);
            Anchors.Add(a);
            a = new AnchorHandle(rect.GetCornerPoint(CornerType.RightMiddle), this, RIGHT_ANCHOR);
            Anchors.Add(a);
            a = new AnchorHandle(rect.GetCornerPoint(CornerType.Bottom), this, BOTTOM_ANCHOR);
            Anchors.Add(a);
        }


        protected abstract void InitializeResizeHandle();

        protected virtual void OnVisibleChanged()
        {
            __primaryPrimitive.Visible = __visible;
        }

        protected abstract void UpdateResizeHandle();


        protected virtual void MoveShape(SizeF vector)
        {
            Location += vector;
        }

        protected void DisposeAnchorsAndResizeHandles()
        {
            foreach (var a in Anchors)
            {
                a.Dispose();
            }
            Anchors.Clear();
            foreach (var a in ResizeHandles)
            {
                a.Dispose();
            }
            ResizeHandles.Clear();
        }

        protected void PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            IsGraphicsPathChanged = true;
        }

        #region IDisposable Support 

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                    if (Region != null)
                    {
                        Region.Dispose();
                    }

                    if (__widenPen != null)
                    {
                        __widenPen.Dispose();
                    }

                    if (__primaryPrimitive != null)
                    {
                        __primaryPrimitive.Dispose();
                    }

                    if (__bound != null)
                    {
                        __bound.Dispose();
                    }

                    DisposeAnchorsAndResizeHandles();

                }

                FillPattern = null;
                Region = null;
                __primaryPrimitive = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion



    }

    public class EndTextingEvengArgs : EventArgs
    {
        public string Content { get; set; }
        public AbstractShape Shape { get; set; }
    }

    public class BeginTextingEventArgs : EventArgs
    {
        public RectangleF Rect { get; set; }
        public AbstractShape Shape { get; set; }
        public string TextString { get; set; }

        public BeginTextingEventArgs(RectangleF rect, AbstractShape shape, string textString)
        {
            Rect = rect;
            Shape = shape;
            TextString = textString;
        }
    }
}
