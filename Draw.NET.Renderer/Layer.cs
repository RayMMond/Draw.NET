/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       Layer 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/13 16:58:10
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using Draw.NET.Renderer.Primitives;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer
{
    /// <summary>
    /// 图层，所有图元都属于一个图层，图层是用于管理图元对象的组织结构
    /// </summary>
    public class Layer : IMessagePipe, ILayer<AbstractPrimitive>
    {
        protected MessagePipe __messagePipe;
        private bool disposedValue = false;

        /// <summary>
        /// 图层中的图元
        /// </summary>
        protected List<AbstractPrimitive> primitives;
        private ConcurrentQueue<Action> __cmdList;
        private RenderConfig __config;


        /// <summary>
        /// Layer 构造函数，对外隐藏
        /// 使用Render 中的GetNewLayer方法
        /// </summary>
        internal Layer(ConcurrentQueue<Action> cmdList, RenderConfig config)
        {
            UUID = Guid.NewGuid().ToString();
            __cmdList = cmdList;
            __config = config;
            primitives = new List<AbstractPrimitive>();
            __messagePipe = new MessagePipe(this);
        }




        /// <summary>
        /// 消息管道监听
        /// </summary>
        public event EventHandler<DrawingFrameworkMessage> MessageListener
        {
            add { __messagePipe.MessageListener += value; }
            remove { __messagePipe.MessageListener -= value; }
        }


        #region 属性

        /// <summary>
        /// 图层唯一ID
        /// </summary>
        public string UUID { get; }
        /// <summary>
        /// 图层索引，从0起始
        /// </summary>
        public int Index { get; set; }

        /// <summary>
        /// 图层名称
        /// </summary>
        public string Name { get; set; }


        /// <summary>
        /// 图层是否可见
        /// </summary>
        public bool Visible { get; set; } = true;

        public bool IsDisposed { get { return disposedValue; } }

        #endregion


        #region 公共


        /// <summary>
        /// 添加图元对象
        /// </summary>
        /// <param name="primitives">图元对象</param>
        public void Add(params AbstractPrimitive[] primitives)
        {
            if (primitives != null)
            {
                foreach (var primitive in primitives)
                {
                    primitive.Layer = this;
                    primitive.MessageListener += Primitive_MessageListener;
                    this.primitives.Add(primitive);
                }
            }
        }


        /// <summary>
        /// 添加图元对象
        /// </summary>
        /// <param name="primitives">图元对象</param>
        public void Add(List<AbstractPrimitive> primitives)
        {
            Add(primitives?.ToArray());
        }

        /// <summary>
        /// 根据图元ID获取对象
        /// </summary>
        /// <param name="id">图元ID</param>
        /// <returns>查找到的图元对象，如果没有找到返回null</returns>
        public AbstractPrimitive GetByID(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                return primitives.FirstOrDefault(s => s.UUID == id);
            }
            else
            {
                return null;
            }
        }

        public void Clear()
        {
            if (IsDisposed)
                return;
            if (__config.UseMultiThreaded)
            {
                __cmdList.Enqueue(_Clear);
            }
            else
            {
                _Clear();
            }
        }



        /// <summary>
        /// 从图层中去除一个图元对象
        /// </summary>
        /// <param name="primitive">需要去除的图元对象</param>
        /// <returns>如果去除成功返回true,反之返回false</returns>
        public bool Remove(AbstractPrimitive primitive)
        {
            var _s = primitives.FirstOrDefault(s => s.Equals(primitive));
            if (_s != null)
            {
                _s.Layer = null;
                _s.MessageListener -= Primitive_MessageListener;
                return primitives.Remove(_s);
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 从图层中根据图元ID去除一个图元对象
        /// </summary>
        /// <param name="id">需要去除的图元对象的ID</param>
        /// <returns>如果去除成功返回true,反之返回false</returns>
        public bool RemoveByID(string id)
        {
            var s = GetByID(id);
            if (s != null)
            {
                return Remove(s);
            }
            return false;
        }

        /// <summary>
        /// 所有本图层的所有图元
        /// </summary>
        /// <returns>所有图元列表</returns>
        public List<AbstractPrimitive> GetAll()
        {
            return new List<AbstractPrimitive>(primitives);
        }


        public RectangleF GetBounds()
        {
            float x = float.MaxValue, y = float.MaxValue, _x = float.MinValue, _y = float.MinValue;
            for (int i = 0; i < primitives.Count; i++)
            {
                var b = primitives[i].GetBounds();
                if (b.X < x) x = b.X;
                if (b.Y < y) y = b.Y;
                if (b.Right > _x) _x = b.Right;
                if (b.Bottom > _y) _y = b.Bottom;
            }

            return new RectangleF(x, y, _x - x, _y - y);
        }


        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

        #region 私有方法

        private void _Clear()
        {
            foreach (var s in primitives)
            {
                s.Dispose();
            }
            primitives.Clear();
        }

        private void Primitive_MessageListener(object sender, DrawingFrameworkMessage e)
        {
            __messagePipe.OnMessageSend(e);
        }

        #endregion

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    primitives.ForEach(s => s.Dispose());
                    primitives.Clear();
                    __messagePipe.Dispose();
                }
                __messagePipe = null;
                __cmdList = null;
                __config = null;
                disposedValue = true;
            }
        }

        #endregion

        #region Comparsion
        internal class LayerComparsion : IComparer<Layer>
        {
            internal static LayerComparsion Instance { get; } = new LayerComparsion();

            public int Compare(Layer x, Layer y)
            {
                return x.Index - y.Index;
            }

        }
        #endregion
    }
}
