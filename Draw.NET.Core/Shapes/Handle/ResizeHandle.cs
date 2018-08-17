/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       ResizeHandle 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/21 15:33:20
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using Draw.NET.Renderer.Primitives;
using System;
using System.Drawing;

namespace Draw.NET.Core.Shapes
{
    public class RectResizeHandle : AbstractHandle
    {

        public CornerType Type { get; private set; }

        public RectResizeHandle(PointF location, AbstractShape parent, CornerType type, IPrimitiveProvider provider)
            : base(location, parent, provider)
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

        public void ShowTip()
        {

        }

        public void ChangeMouse()
        {
            
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
