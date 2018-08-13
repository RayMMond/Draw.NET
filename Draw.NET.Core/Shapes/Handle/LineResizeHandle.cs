/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       LineResizeHandle 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/2 14:02:22
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using Draw.NET.Renderer.Primitives;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.NET.Core.Shapes
{
   public class LineResizeHandle : AbstractHandle
    {
        public LineResizeType Type { get; private set; }

        public LineResizeHandle(PointF location, AbstractShape parent, LineResizeType type, IPrimitiveProvider primitiveProvider)
            : base(location, parent, primitiveProvider)
        {
            Type = type;
        }

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

        public override void Edit()
        {
            return;
        }

        public override void Resize(CornerType type, PointF targetPoint, bool resizeFixRadio)
        {
            throw new NotImplementedException();
        }

        protected override void InitializeResizeHandle()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateResizeHandle()
        {
            throw new NotImplementedException();
        }
    }
}
