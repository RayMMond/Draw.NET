/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       Rectangle 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/21 10:09:48
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using Draw.NET.Renderer;
using Draw.NET.Renderer.Primitives;
using Draw.NET.Renderer.Styles;



namespace Draw.NET.Core.Shapes
{
    public class Rect : AbstractShape, IRectangle, IText
    {
        private AbstractRectangle __rect { get { return PrimaryPrimitive as AbstractRectangle; } }


        public Rect(IPrimitiveProvider primitiveProvider)
            : this(new PointF(0, 0), new SizeF(1, 1), primitiveProvider)
        { }

        public Rect(PointF location, SizeF size, IPrimitiveProvider primitiveProvider)
        {
            if (primitiveProvider == null) throw new ArgumentNullException(nameof(primitiveProvider));

            __location = location;

            var rect = primitiveProvider.GetPrimitive<AbstractRectangle>(location, size);
            var bound = primitiveProvider.GetPrimitive<AbstractRectangle>(location, size);

            rect.Visible = true;
            bound.Visible = false;
            Styles.RectStyles.ApplyDefaultPatternTo(rect);
            Styles.RectStyles.ApplySelectedPatternTo(bound);

            InitializePrimaryPrimitiveAndBound(rect, bound);
            var b = rect.GetBounds();
            InitializeDefaultAnchor(b);
            InitializeResizeHandle();
            CanSelect = true;
        }



        #region 属性

        public PointF Center
        {
            get
            {
                return ((IRectangle)__rect).Center;
            }
            set
            {
                ((IRectangle)__rect).Center = value;
            }
        }

        public float RectHeight
        {
            get
            {
                return ((IRectangle)__rect).RectHeight;
            }

            set
            {
                ((IRectangle)__rect).RectHeight = value;
                IsGraphicsPathChanged = true;
            }
        }

        public SizeF RectSize
        {
            get
            {
                return ((IRectangle)__rect).RectSize;
            }
        }

        public float RectWidth
        {
            get
            {
                return ((IRectangle)__rect).RectWidth;
            }

            set
            {
                ((IRectangle)__rect).RectWidth = value;
                IsGraphicsPathChanged = true;
            }
        }

        public float Left
        {
            get
            {
                return ((IRectangle)__rect).Left;
            }
        }

        public float Right
        {
            get
            {
                return ((IRectangle)__rect).Right;
            }
        }

        public float Top
        {
            get
            {
                return ((IRectangle)__rect).Top;
            }
        }

        public float Bottom
        {
            get
            {
                return ((IRectangle)__rect).Bottom;
            }
        }

        public StringAlignment Alignment
        {
            get
            {
                return ((IText)__rect).Alignment;
            }

            set
            {
                ((IText)__rect).Alignment = value;
            }
        }

        public string Content
        {
            get
            {
                return ((IText)__rect).Content;
            }

            set
            {
                ((IText)__rect).Content = value;
            }
        }

        public string FamilyName
        {
            get
            {
                return ((IText)__rect).FamilyName;
            }

            set
            {
                ((IText)__rect).FamilyName = value;
            }
        }

        public float FontSize
        {
            get
            {
                return ((IText)__rect).FontSize;
            }

            set
            {
                ((IText)__rect).FontSize = value;
            }
        }

        public FontStyle FontStyle
        {
            get
            {
                return ((IText)__rect).FontStyle;
            }

            set
            {
                ((IText)__rect).FontStyle = value;
            }
        }

        public StringAlignment LineAlignment
        {
            get
            {
                return ((IText)__rect).LineAlignment;
            }

            set
            {
                ((IText)__rect).LineAlignment = value;
            }
        }

        public SizeF StringSize
        {
            get
            {
                return ((IText)__rect).StringSize;
            }

            set
            {
                ((IText)__rect).StringSize = value;
            }
        }

        public StringTrimming Trimming
        {
            get
            {
                return ((IText)__rect).Trimming;
            }

            set
            {
                ((IText)__rect).Trimming = value;
            }
        }


        public Color TextColor
        {
            get
            {
                return ((IText)__rect).TextColor;
            }

            set
            {
                ((IText)__rect).TextColor = value;
            }
        }

        public LocationType LocationType
        {
            get
            {
                return ((IText)__rect).LocationType;
            }

            set
            {
                ((IText)__rect).LocationType = value;
            }
        }

        public DrawModel DrawModel
        {
            get
            {
                return ((IRectangle)__rect).DrawModel;
            }

            set
            {
                ((IRectangle)__rect).DrawModel = value;
            }
        }

        public Font Font
        {
            get
            {
                return ((IText)__rect).Font;
            }
        }
        #endregion


        protected override void MoveShape(SizeF vector)
        {
            base.MoveShape(vector);
            //TODO:移动矩形逻辑
        }

        public override string ToString()
        {
            return $"{base.ToString()} Size:{RectSize}";
        }

        public override void Resize(CornerType type, PointF targetPoint, bool resizeFixRadio)
        {
            //图形变换
            var rect = GetBounds().Resize(type, targetPoint, resizeFixRadio);
            RectHeight = rect.Height;
            RectWidth = rect.Width;
            Location = rect.Location;

            UpdateBound(rect.Location, rect.Width, rect.Height);
            UpdateResizeHandle();
            IsGraphicsPathChanged = true;
        }


        public override void Edit()
        {
            //TODO:编辑逻辑
            throw new NotImplementedException();
        }

        public override void RotateAt(PointF pt, float angle)
        {
            //TODO:Rotate 的逻辑
            throw new NotImplementedException();
        }

        public override RectangleF GetBounds()
        {
            return __rect.GetBounds();
        }

        protected override void OnLocationChanged(SizeF diff)
        {
            __rect.Location = __location;
            __defaultRotatePoint = __rect.Center;
        }
        protected override Pen OnCreateWidenPen()
        {
            return new Pen(Color.Black, BorderPattern.Width + 1);
        }

        protected override Region OnRegionRecreate()
        {
            using (var gp = new GraphicsPath())
            {
                gp.AddRectangle(GetBounds());
                if (FillPattern.FillType == FillType.None)
                {
                    gp.Widen(WidenPen);
                }
                return new Region(gp);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeAnchorsAndResizeHandles();
            }

            base.Dispose(disposing);
        }

        protected override void InitializeResizeHandle()
        {
            var list = Enum.GetValues(typeof(CornerType))
             .Cast<CornerType>().Where(t => t != CornerType.Center);
            var rect = __bound.GetBounds();
            foreach (var item in list)
            {
                var r = new RectResizeHandle(rect.GetCornerPoint(item), this, item);
                ResizeHandles.Add(r);
            }
        }
        protected override void UpdateResizeHandle()
        {
            foreach (RectResizeHandle h in ResizeHandles)
            {
                h.Location = __bound.GetBounds().GetCornerPoint(h.Type);
            }
        }


        private void ResetDefaultAnchorLocation()
        {
            var a = GetAnchorByName(TOP_ANCHOR);
            if (a != null)
            {
                a.Location = new PointF(Location.X + __rect.RectWidth, Top);
            }
            a = GetAnchorByName(LEFT_ANCHOR);
            if (a != null)
            {
                a.Location = new PointF(Left, Location.Y + __rect.RectHeight);
            }
            a = GetAnchorByName(RIGHT_ANCHOR);
            if (a != null)
            {
                a.Location = new PointF(Right, Location.Y + __rect.RectHeight);
            }
            a = GetAnchorByName(BOTTOM_ANCHOR);
            if (a != null)
            {
                a.Location = new PointF(Location.X + __rect.RectWidth, Bottom);
            }

        }

    }

}
