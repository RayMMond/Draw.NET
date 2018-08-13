/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       OperationLayer 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/20 14:10:56
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
    /// <summary>
    /// 操作图层，用于显示选中框，高亮等操作性的图形
    /// 处于最高层
    /// </summary>
    public class OperationLayer : AbstractShapeLayer
    {
        private Rect __selection;

        public const string NAME = "OperationLayer";

        public OperationLayer(ILayer<AbstractPrimitive> layer, IPrimitiveProvider primitiveProvider)
            : base(layer)
        {
            if (primitiveProvider == null) throw new ArgumentNullException(nameof(primitiveProvider));
            __selection = new Rect(primitiveProvider);
            Styles.RectStyles.ApplySelectionPatternTo(__selection);
            __selection.AddToLayer(__layer);
            Add(__selection);
        }

        #region 操作图层的图形
        public Rect Selection { get { return __selection; } }

        /// <summary>
        /// 创建选择矩形
        /// </summary>
        /// <returns></returns>
        public Rect GetSelectionRectangle()
        {
            __selection.Visible = true;
            return __selection;
        }

        public void DeleteSelectionRectangle()
        {
            __selection.Visible = false;
        }

        public AbstractShape IsVisible(PointF pt)
        {
            for (int i = __shapes.Count - 1; i >= 0; i--)
            {
                if (__shapes[i].Visible && __shapes[i].Selected)
                {
                    for (int j = 0; j < __shapes[i].ResizeHandles.Count; j++)
                    {
                        if (__shapes[i].ResizeHandles[j].IsVisible(pt))
                        {
                            return __shapes[i].ResizeHandles[j];
                        }
                    }
                    for (int j = 0; j < __shapes[i].ResizeHandles.Count; j++)
                    {
                        if (__shapes[i].ResizeHandles[j].IsVisible(pt))
                        {
                            return __shapes[i].ResizeHandles[j];
                        }
                    }
                    for (int j = 0; j < __shapes[i].Anchors.Count; j++)
                    {
                        if (__shapes[i].Anchors[j].IsVisible(pt))
                        {
                            return __shapes[i].Anchors[j];
                        }
                    }
                }
            }
            return null;
        }

        public List<AbstractShape> IsVisible(RectangleF rect)
        {
            var list = new List<AbstractShape>();
            for (int i = __shapes.Count - 1; i >= 0; i--)
            {
                for (int j = 0; j < __shapes[i].ResizeHandles.Count; j++)
                {
                    if (__shapes[i].ResizeHandles[j].IsVisible(rect))
                    {
                        list.Add(__shapes[i].ResizeHandles[j]);
                    }
                }
                for (int j = 0; j < __shapes[i].Anchors.Count; j++)
                {
                    if (__shapes[i].Anchors[j].IsVisible(rect))
                    {
                        list.Add(__shapes[i].Anchors[j]);
                    }
                }
            }
            return list;
        }
        #endregion

        protected override void OnShapeAdded(AbstractShape s)
        {
            s.AddOperationShapeToLayer(__layer);
        }

        protected override void OnShapeRemoved(AbstractShape shape)
        {
            shape.RemoveOperationShapeFromLayer(__layer);
        }



        #region Dispose
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (__selection != null)
                {
                    __selection.RemoveFromLayer(__layer);
                    Remove(__selection);
                    __selection.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #endregion

    }
}
