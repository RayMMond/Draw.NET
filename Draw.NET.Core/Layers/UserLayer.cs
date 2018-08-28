/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       UserLayer 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/20 14:15:58
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Draw.NET.Core.Shapes;
using Draw.NET.Renderer;
using Draw.NET.Renderer.Primitives;

namespace Draw.NET.Core.Layers
{
    public class UserLayer : AbstractShapeLayer
    {
        public const string DEFAULT_NAME = "layer0";





        public UserLayer(ILayer<IPrimitive> layer)
            : base(layer)
        {

        }




        public AbstractShape IsVisible(PointF pt)
        {
            for (int i = __shapes.Count - 1; i >= 0; i--)
            {
                if (__shapes[i].IsVisible(pt))
                {
                    return __shapes[i];
                }
            }
            return null;
        }

        public List<AbstractShape> IsVisible(RectangleF rect)
        {
            var list = new List<AbstractShape>();
            for (int i = __shapes.Count - 1; i >= 0; i--)
            {
                if (__shapes[i].IsVisible(rect))
                {
                    list.Add(__shapes[i]);
                }
            }
            return list;
        }

        protected override void OnShapeAdded(AbstractShape s)
        {
            s.AddToLayer(__layer);
        }

        protected override void OnShapeRemoved(AbstractShape shape)
        {
            shape.RemoveFromLayer(__layer);
        }
        #region 层叠管理
        //TODO:SYP 层叠管理


        #endregion

        #region 对齐功能
        //TODO:SYP 对齐功能

        #endregion
    }

}
