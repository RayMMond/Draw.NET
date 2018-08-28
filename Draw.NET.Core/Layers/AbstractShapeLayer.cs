/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       AbstractShapeLayer 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/2 15:13:34
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
    public abstract class AbstractShapeLayer : ILayer<AbstractShape>, IDisposable
    {
        protected ILayer<IPrimitive> __layer;
        protected List<AbstractShape> __shapes = new List<AbstractShape>();

        protected AbstractShapeLayer(ILayer<IPrimitive> layer)
        {
            __layer = layer;
        }



        #region ILayer接口
        public int Index
        {
            get
            {
                return __layer.Index;
            }

            set
            {
                __layer.Index = value;
            }
        }

        public string Name
        {
            get
            {
                return __layer.Name;
            }

            set
            {
                __layer.Name = value;
            }
        }

        public string UUID
        {
            get
            {
                return __layer.UUID;
            }
        }

        public bool Visible
        {
            get
            {
                return __layer.Visible;
            }

            set
            {
                __layer.Visible = value;
            }
        }

        public void Add(params AbstractShape[] shapes)
        {
            if (shapes != null)
            {
                foreach (var s in shapes)
                {
                    OnShapeAdded(s);
                    __shapes.Add(s);
                }
            }
        }


        public bool Remove(AbstractShape shape)
        {
            if (shape != null)
            {
                OnShapeRemoved(shape);
                return __shapes.Remove(shape);
            }
            return false;
        }


        public void Add(List<AbstractShape> shapes)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            __layer.Clear();
        }

        public RectangleF GetBounds()
        {
            return __layer.GetBounds();
        }

        public AbstractShape GetByID(string id)
        {
            return __shapes.FirstOrDefault(s => s.UUID == id);
        }

        public List<AbstractShape> GetAll()
        {
            return new List<AbstractShape>(__shapes);
        }


        public bool RemoveByID(string id)
        {
            throw new NotImplementedException();
        }

        #endregion

        protected abstract void OnShapeAdded(AbstractShape s);
        protected abstract void OnShapeRemoved(AbstractShape shape);



        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    foreach (var s in __shapes)
                    {
                        s.Dispose();
                    }
                    __shapes.Clear();
                }
                __layer = null;

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
