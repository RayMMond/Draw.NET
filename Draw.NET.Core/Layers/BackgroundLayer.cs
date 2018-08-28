/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       BackgroundLayer 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/19 16:23:54
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
    /// 背景图层，用于显示背景，处于最底层
    /// </summary>
    public class BackgroundLayer : AbstractShapeLayer
    {
        private SizeF __size;
        public const string NAME = "BackgroundLayer";


        public BackgroundLayer(ILayer<IPrimitive> layer, SizeF initialSize)
            : base(layer)
        {
            __size = initialSize;
            UpdateLayer(layer, initialSize);
        }

        #region 属性


        public SizeF ClientSize
        {
            get
            {
                return __size;
            }
            set
            {
                ChangeSize(value);
            }
        }
        #endregion



        public void ChangeSize(SizeF size)
        {
            __size = size;
            UpdateLayer(__layer, __size);
        }


        private void UpdateLayer(ILayer<IPrimitive> layer, SizeF size)
        {
            layer.Clear();
            //TODO:根据背景属性，添加背景图元

            //TODO:根据显示区域大小，添加网格线
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnShapeAdded(AbstractShape s)
        {
            throw new NotImplementedException();
        }

        protected override void OnShapeRemoved(AbstractShape shape)
        {
            throw new NotImplementedException();
        }
    }
}
