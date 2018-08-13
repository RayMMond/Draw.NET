/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       AbstractHandle 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/21 15:46:34
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Drawing;
using Draw.NET.Renderer;
using Draw.NET.Renderer.Primitives;
using Draw.NET.Renderer.Styles;

namespace Draw.NET.Core.Shapes
{
    public abstract class AbstractHandle : AbstractShape
    {
        protected AbstractShape __parentShape;


        public static int HANDLE_SIZE = 2;


        public AbstractShape ParentShape
        {
            get
            {
                return __parentShape;
            }
        }

        protected AbstractHandle(PointF location, AbstractShape parent, IPrimitiveProvider primitiveProvider)
        {
            if (primitiveProvider == null) throw new ArgumentNullException(nameof(primitiveProvider));
            __location = location;
            __parentShape = parent;


            var rect = primitiveProvider.GetPrimitive<AbstractRectangle>(
                new PointF(location.X - HANDLE_SIZE, location.Y - HANDLE_SIZE),
                new SizeF(HANDLE_SIZE * 2 + 1, HANDLE_SIZE * 2 + 1));
            rect.DrawModel = DrawModel.Center;

            InitializePrimaryPrimitiveAndBound(rect, null);

            CanSelect = false;
            Visible = false;
            Styles.HandleStyles.ApplyDefaultPatternTo(rect);
        }



        /// <summary>
        /// 不可旋转
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="angle"></param>
        public override void RotateAt(PointF pt, float angle)
        {
            return;
        }


        protected override void OnLocationChanged(SizeF diff)
        {
            var rect = PrimaryPrimitive as AbstractRectangle;
            if (rect != null)
            {
                rect.Location = new PointF(Location.X - HANDLE_SIZE, Location.Y - HANDLE_SIZE);
            }
        }

        protected override Region OnRegionRecreate()
        {
            using (var gp = new System.Drawing.Drawing2D.GraphicsPath())
            {
                gp.AddRectangle(__primaryPrimitive.GetBounds());
                return new Region(gp);
            }
        }

        protected override Pen OnCreateWidenPen()
        {
            return new Pen(Color.Black);
        }


        public override RectangleF GetBounds()
        {
            return PrimaryPrimitive.GetBounds();
        }



        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            __parentShape = null;
            base.Dispose(disposing);
        }


    }
}
