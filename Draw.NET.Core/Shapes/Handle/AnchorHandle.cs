/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       Anchor 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/20 15:30:05
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Draw.NET.Renderer;
using Draw.NET.Renderer.Primitives;



namespace Draw.NET.Core.Shapes
{
    /// <summary>
    /// 图形的锚点，用于图形之间的连线
    /// </summary>
    public class AnchorHandle : AbstractHandle
    {
        private List<Line> __lines;

        public AnchorHandle(PointF location, AbstractShape parent, string name, IPrimitiveProvider primitiveProvider)
            : base(location, parent, primitiveProvider)
        {
            __lines = new List<Line>();
            Name = name;
        }



        #region 与锚点连接的连线

        public void AddLine(Line l, AnchorLocation al)
        {
            __lines.Add(l);
            if (al == AnchorLocation.Front)
            {
                l.FrontAnchor = this;
            }
            else
            {
                l.BackAnchor = this;
            }
        }

        public void RemoveLine(Line l, AnchorLocation al)
        {
            if (al == AnchorLocation.Front)
            {
                l.FrontAnchor = null;
            }
            else
            {
                l.BackAnchor = null;
            }
            __lines.Remove(l);
        }

        public List<Line> GetLines()
        {
            return new List<Line>(__lines);
        }

        #endregion


        protected override void MoveShape(SizeF vector)
        {
            base.MoveShape(vector);
            //TODO:移动描点的逻辑


        }


        public override void Select()
        {
            //TODO: 被选中的逻辑
            base.Select();
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                __lines.Clear();
            }
            base.Dispose(disposing);
        }

        public override void Edit()
        {
            return;
        }

        public override void Resize(CornerType type, PointF targetPoint, bool resizeFixRadio)
        {
            throw new NotImplementedException();
        }

        protected override void InitializeResizeHandle(IPrimitiveProvider provider)
        {
            throw new NotImplementedException();
        }

        protected override void UpdateResizeHandle()
        {
            throw new NotImplementedException();
        }
    }
}
