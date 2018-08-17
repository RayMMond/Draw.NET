/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       Circle 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/25 11:50:21
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

namespace Draw.NET.Core.Shapes
{
    public class Ellipse : AbstractShape, IEllipse
    {
        private AbstractEllipse __ellipse { get { return PrimaryPrimitive as AbstractEllipse; } }


        public Ellipse(PointF location, SizeF sz, IPrimitiveProvider primitiveProvider)
        {
            if (primitiveProvider == null) throw new ArgumentNullException(nameof(primitiveProvider));
            __location = location;
            var ellipse = primitiveProvider.GetPrimitive<AbstractEllipse>(location, sz);
            var bound = primitiveProvider.GetPrimitive<AbstractRectangle>(location, sz);


            ellipse.Visible = true;
            bound.Visible = false;

            Styles.CircleStyles.ApplyDefaultPatternTo(ellipse);
            Styles.RectStyles.ApplySelectedPatternTo(bound);

            InitializePrimaryPrimitiveAndBound(ellipse, bound);

            var b = bound.GetBounds();
            InitializeDefaultAnchor(b, primitiveProvider);
            InitializeResizeHandle(primitiveProvider);

            CanSelect = true;

        }

        #region 属性


        public float Height
        {
            get
            {
                return ((IEllipse)__ellipse).Height;
            }

            set
            {
                ((IEllipse)__ellipse).Height = value;
                IsGraphicsPathChanged = true;
            }
        }

        public float Width
        {
            get
            {
                return ((IEllipse)__ellipse).Width;
            }

            set
            {
                ((IEllipse)__ellipse).Width = value;
                IsGraphicsPathChanged = true;
            }
        }

        public SizeF Size
        {
            get
            {
                return ((IEllipse)__ellipse).Size;
            }
        }

        #endregion

        #region 公开方法
        public override void Edit()
        {
            //TODO: 待实现
            throw new NotImplementedException();
        }

        public override RectangleF GetBounds()
        {
            return __ellipse.GetBounds();
        }

        public override void Resize(CornerType type, PointF targetPoint, bool resizeFixRadio)
        {
            //图形变换
            var rect = GetBounds().Resize(type, targetPoint, resizeFixRadio);
            Location = rect.Location;
            Width = rect.Width;
            Height = rect.Height;

            UpdateBound(rect.Location, rect.Width, rect.Height);
            UpdateResizeHandle();
            IsGraphicsPathChanged = true;
        }

        public override void RotateAt(PointF pt, float angle)
        {
            //TODO: 待实现
            throw new NotImplementedException();
        }

        protected override void OnLocationChanged(SizeF diff)
        {
            __ellipse.Location = __location;
        }


        protected override void MoveShape(SizeF vector)
        {
            base.MoveShape(vector);
        }

        public override void Select()
        {
            base.Select();
        }


        public override string ToString()
        {
            return $"{base.ToString()} Size:{Size}";
        }
        #endregion


        #region 内部方法
        protected override Region OnRegionRecreate()
        {
            using (var gp = new GraphicsPath())
            {
                gp.AddEllipse(__ellipse.GetBounds());
                if (FillPattern.FillType == Renderer.Styles.FillType.None)
                {
                    gp.Widen(WidenPen);
                }
                return new Region(gp);
            }
        }

        protected override Pen OnCreateWidenPen()
        {
            return new Pen(Color.Black, BorderPattern.Width + 1);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

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

        protected override void UpdateResizeHandle()
        {
            foreach (RectResizeHandle h in ResizeHandles)
            {
                h.Location = __bound.GetBounds().GetCornerPoint(h.Type);
            }
        }
        #endregion
    }
}
