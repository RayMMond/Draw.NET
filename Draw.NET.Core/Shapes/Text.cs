using Draw.NET.Renderer.Primitives;
using Draw.NET.Renderer.Styles;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Draw.NET.Core.Shapes
{
    public class Text : AbstractShape, IText
    {
        private AbstractText __text { get { return PrimaryPrimitive as AbstractText; } }

        #region 接口
        public Text(PointF location, SizeF size, IPrimitiveProvider primitiveProvider)
        {

            if (primitiveProvider == null) throw new ArgumentNullException(nameof(primitiveProvider));
            var c = primitiveProvider.GetPrimitive<AbstractText>();
            __location = location;
            c.Location = Point.Ceiling(location);
            c.StringSize = size;
            var bound = primitiveProvider.GetPrimitive<AbstractRectangle>(location, size);


            c.Visible = true;
            bound.Visible = false;

            Styles.TextStyles.ApplyDefaultPatternTo(c);
            Styles.RectStyles.ApplySelectedPatternTo(bound);
            InitializePrimaryPrimitiveAndBound(c, bound);

            var b = c.GetBounds();
            InitializeDefaultAnchor(b, primitiveProvider);
            InitializeResizeHandle(primitiveProvider);
            CanSelect = true;
        }


        public override RectangleF GetBounds()
        {
            return __text.GetBounds();
        }


        public override void UnSelect()
        {
            base.UnSelect();
            OnNodifyToRemoveTextBox();
        }

        public override void RotateAt(PointF pt, float angle)
        {
            throw new NotImplementedException();
        }

        public override void Resize(CornerType type, PointF targetPoint, bool resizeFixRadio)
        {
            var rect = GetBounds().Resize(type, targetPoint, resizeFixRadio);
            StringSize = new SizeF(rect.Width, rect.Height);
            Location = rect.Location;

            UpdateBound(rect.Location, rect.Width, rect.Height);
            UpdateResizeHandle();
            IsGraphicsPathChanged = true;
        }

        public override void Edit()
        {
            throw new NotImplementedException();
        }

        protected override void MoveShape(SizeF vector)
        {
            base.MoveShape(vector);
        }



        protected override void OnLocationChanged(SizeF diff)
        {
            __text.Location = __location;
            __defaultRotatePoint = __location;
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

        protected override void UpdateResizeHandle()
        {
            foreach (RectResizeHandle h in ResizeHandles)
            {
                h.Location = __bound.GetBounds().GetCornerPoint(h.Type);
            }
        }
        protected override void InitializeResizeHandle(IPrimitiveProvider provider)
        {
            var list = Enum.GetValues(typeof(CornerType))
             .Cast<CornerType>().Where(t => t != CornerType.Center);
            var rect = __bound.GetBounds();
            foreach (var item in list)
            {
                var r = new RectResizeHandle(rect.GetCornerPoint(item), this, item, provider);
                ResizeHandles.Add(r);
            }
        }
        #endregion

        #region 属性

        public string TextString
        {
            get
            {
                return __text.Content;
            }
            set
            {
                __text.Content = value;
            }
        }

        public StringAlignment Alignment
        {
            get
            {
                return __text.Alignment;
            }
            set
            {
                __text.Alignment = value;
            }
        }

        public StringAlignment LineAlignment
        {
            get
            {
                return __text.LineAlignment;
            }
            set
            {
                __text.LineAlignment = value;
            }
        }

        public StringTrimming Trimming
        {
            get
            {
                return __text.Trimming;
            }
            set
            {
                __text.Trimming = value;
            }
        }

        public float FontSize
        {
            get
            {
                return __text.FontSize;
            }

            set
            {
                __text.FontSize = value;
            }
        }

        public string Content
        {
            get
            {
                return ((IText)__text).Content;
            }

            set
            {
                ((IText)__text).Content = value;
            }
        }

        public string FamilyName
        {
            get
            {
                return ((IText)__text).FamilyName;
            }

            set
            {
                ((IText)__text).FamilyName = value;
            }
        }

        public FontStyle FontStyle
        {
            get
            {
                return ((IText)__text).FontStyle;
            }

            set
            {
                ((IText)__text).FontStyle = value;
            }
        }

        public SizeF StringSize
        {
            get
            {
                return ((IText)__text).StringSize;
            }

            set
            {
                ((IText)__text).StringSize = value;
            }
        }

        public Color TextColor
        {
            get
            {
                return ((IText)__text).TextColor;
            }

            set
            {
                ((IText)__text).TextColor = value;
            }
        }

        public LocationType LocationType
        {
            get
            {
                return ((IText)__text).LocationType;
            }

            set
            {
                ((IText)__text).LocationType = value;
            }
        }

        public Font Font
        {
            get
            {
                return ((IText)__text).Font;
            }
        }

        #endregion
    }
}
