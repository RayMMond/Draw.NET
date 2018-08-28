/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       AbstractRender 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/13 17:23:30
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;

namespace Draw.NET.Renderer
{
    /// <summary>
    /// 抽象渲染器，负责对图层的渲染和管理
    /// </summary>
    public abstract class AbstractRenderer : IRenderer
    {
        protected MessagePipe __messagePipe;
        protected ConcurrentQueue<Action> __cmdList;
        protected bool disposedValue = false;
        protected bool disposing = false;


        private Thread __renderingThread;
        private ManualResetEventSlim __mres;
        private List<Layer> __layers;


        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="handle">绘制窗体句柄</param>
        protected AbstractRenderer()
        {
            __layers = new List<Layer>();
            __mres = new ManualResetEventSlim(false);
            __cmdList = new ConcurrentQueue<Action>();
            __messagePipe = new MessagePipe(this);
            __renderingThread = new Thread(new ThreadStart(RenderingLoop))
            {
                IsBackground = true
            };

            Configuration = new RenderConfig();
            Configuration.ClientSizeChanged += Configuration_ClientSizeChanged;
        }


        public virtual void Initialize(IntPtr handle, SizeF initialSize)
        {

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
        /// 渲染器配置
        /// </summary>
        public RenderConfig Configuration
        {
            get; protected set;
        }


        public bool Disposed { get { return disposedValue; } }


        #endregion


        #region 公共方法
        public void ChangeSize(SizeF newSize)
        {
            if (Configuration.UseMultiThreaded)
            {
                __cmdList.Enqueue(() => Configuration?.ChangeSize(newSize));
            }
            else
            {
                Configuration?.ChangeSize(newSize);
            }
        }


        public virtual void Render(RectangleF clip = default(RectangleF))
        {
            if (Configuration.UseMultiThreaded)
            {
                while ((__renderingThread.ThreadState & ThreadState.Unstarted) == ThreadState.Unstarted)
                {
                    __renderingThread.Start();
                }

                __cmdList.Enqueue(() => _RenderOnce(clip));
                __mres.Set();
            }
            else
            {
                _RenderOnce(clip);
            }
        }



        ///// <summary>
        ///// 加载渲染器的配置，之前的配置将被覆盖
        ///// </summary>
        ///// <param name="cfg"></param>
        //public virtual void LoadConfig(RenderConfig cfg)
        //{
        //    Configuration.ClientSizeChanged -= Configuration_ClientSizeChanged;
        //    if (cfg.ClientSize != Configuration.ClientSize)
        //    {
        //        if (cfg.ClientSize.Width != 0 && cfg.ClientSize.Height != 0)
        //        {
        //            OnRenderSizeChanged(cfg.ClientSize);
        //        }
        //    }

        //    Configuration = cfg;
        //    Configuration.ClientSizeChanged += Configuration_ClientSizeChanged;

        //    Render();
        //}

        #region Layer 管理

        /// <summary>
        /// 新建一个图层并返回
        /// </summary>
        /// <param name="index">图层索引</param>
        /// <param name="name">图层名称</param>
        /// <returns>新图层</returns>
        public ILayer<IPrimitive> GetNewLayer(int index = 0, string name = "")
        {
            var layer = new Layer(__cmdList, Configuration)
            {
                Index = index,
                Name = name
            };

            layer.MessageListener += Layer_MessageListener;

            __layers.Add(layer);

            return layer;
        }

        /// <summary>
        /// 删除图层，图层中的所有图元将被释放
        /// </summary>
        /// <param name="l">需要删除的图层</param>
        /// <returns>成功删除返回true，反之返回false</returns>
        public void RemoveLayer(ILayer<IPrimitive> l)
        {
            if (Configuration.UseMultiThreaded)
            {
                __cmdList.Enqueue(() => _RemoveLayer(l));
            }
            else
            {
                _RemoveLayer(l);
            }

        }

        private void _RemoveLayer(ILayer<IPrimitive> l)
        {
            var _l = __layers.FirstOrDefault(s => s.Equals(l));
            if (_l != null)
            {
                _l.MessageListener -= Layer_MessageListener;
                __layers.Remove(_l);
                _l.Dispose();
            }
        }

        /// <summary>
        /// 根据图层ID获取图层对象
        /// </summary>
        /// <param name="id">图层ID</param>
        /// <returns>需要获取的图层对象</returns>
        public ILayer<IPrimitive> GetLayerByID(string id)
        {
            if (!string.IsNullOrWhiteSpace(id))
            {
                return __layers.FirstOrDefault(s => s.UUID == id);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 根据图层ID删除图层，图层中的所有图元将被释放
        /// </summary>
        /// <param name="id">图层ID</param>
        /// <returns>成功删除返回true，反之返回false</returns>
        public void RemoveLayerByID(string id)
        {
            var l = GetLayerByID(id);
            if (l != null)
            {
                RemoveLayer(l);
            }
        }

        /// <summary>
        /// 获取所有Layer
        /// </summary>
        /// <returns>所有图层列表</returns>
        public List<ILayer<IPrimitive>> GetLayers()
        {
            return new List<ILayer<IPrimitive>>(__layers);
        }

        #endregion

        #endregion


        #region Protected
        /// <summary>
        /// 准备图元的数据
        /// </summary>
        protected virtual void Prepare(RectangleF clip)
        {
            OnPrepare(null);
        }

        /// <summary>
        /// 进行一次渲染
        /// </summary>
        protected abstract void OnRendering(RectangleF clip);

        protected virtual void OnPrepare(object state)
        {
#if PerfMon
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            __layers.Sort(Layer.LayerComparsion.Instance);
            foreach (var layer in __layers)
            {
                if (layer.IsDisposed)
                {
                    continue;
                }

                if (layer.Visible)
                {
                    var primitives = layer.GetAll();

                    for (int i = 0; i < primitives.Count; i++)
                    {
                        if (!primitives[i].IsDisposed)
                        {
                            primitives[i].Prepare(state);
                        }
                    }
                }
            }
#if PerfMon
            sw.Stop();
            System.Diagnostics.Debug.WriteLine($"Total prepare time: {sw.ElapsedTicks}");
#endif
        }


        protected virtual void OnLayerRendering(object state)
        {
#if PerfMon
            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
            sw.Start();
#endif
            foreach (var layer in __layers)
            {
                if (layer.Visible)
                {
                    foreach (var primitive in layer.GetAll())
                    {
                        if (primitive.Visible && !primitive.IsDisposed)
                        {
                            primitive.Draw(state);
                        }
                    }
                }
            }
#if PerfMon
            sw.Stop();
            System.Diagnostics.Debug.WriteLine($"Total render time: {sw.ElapsedTicks}");
#endif
        }

        protected virtual void OnRenderSizeChanged(SizeF newSize)
        {
            return;
        }
        #endregion

        #region 私有
        private void RenderingLoop()
        {
            while (!disposing)
            {
                try
                {
                    switch (Configuration.Mode)
                    {
                        case RenderMode.OnRenderCall:
                            ExecuteRenderCommand();

                            __mres.Reset();
                            __mres.Wait(Timeout.Infinite);
                            break;
                        case RenderMode.OnRenderCallDelay:
                            ExecuteRenderCommand();
                            Thread.Sleep(Configuration.RenderDelay);
                            __mres.Reset();
                            __mres.Wait(Timeout.Infinite);
                            break;
                        case RenderMode.FPS30:
                            ExecuteRenderCommand();
                            Thread.Sleep(30);
                            if (__cmdList.Count == 0)
                            {
                                __cmdList.Enqueue(() => _RenderOnce(default(RectangleF)));
                            }

                            break;
                        case RenderMode.FPS60:
                            ExecuteRenderCommand();
                            Thread.Sleep(15);
                            if (__cmdList.Count == 0)
                            {
                                __cmdList.Enqueue(() => _RenderOnce(default(RectangleF)));
                            }

                            break;
                        case RenderMode.FPSCustom:
                            ExecuteRenderCommand();
                            Thread.Sleep((ushort)(1000 / Configuration.FPS));
                            if (__cmdList.Count == 0)
                            {
                                __cmdList.Enqueue(() => _RenderOnce(default(RectangleF)));
                            }

                            break;
                        default:
                            break;
                    }

                }
                catch (ThreadInterruptedException) { }
                catch (ThreadAbortException) { }
                catch (Exception ex)
                {
                    __messagePipe.OnMessageSend("", DrawingMessageLevel.Fatal, ex);
                }

            }
        }

        private void ExecuteRenderCommand()
        {
            while (__cmdList.Count > 0)
            {
                if (__cmdList.TryDequeue(out Action cmd))
                {
                    cmd.Invoke();
                }
            }
        }

        private void _RenderOnce(RectangleF clip)
        {
            lock (this)
            {
                Prepare(clip);
                OnRendering(clip);
            }
        }

        private void Layer_MessageListener(object sender, DrawingFrameworkMessage e)
        {
            __messagePipe.OnMessageSend(e);
        }


        private void Configuration_ClientSizeChanged(object sender, SizeChangedEventArg e)
        {
            if (e.OldSize != e.NewSize)
            {
                if (e.NewSize.Width != 0 && e.NewSize.Height != 0)
                {
                    OnRenderSizeChanged(e.NewSize);
                }
            }
        }

        #endregion

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            this.disposing = true;

            while ((__renderingThread.ThreadState & ThreadState.Stopped) != ThreadState.Stopped)
            {
                if ((__renderingThread.ThreadState & ThreadState.Unstarted) == ThreadState.Unstarted)
                {
                    break;
                }

                __mres.Set();
                Thread.Sleep(5);
            }

            if (!disposedValue)
            {
                if (disposing)
                {
                    while (__cmdList.Count > 0)
                    {
                        __cmdList.TryDequeue(out Action a);
                    }

                    Configuration.Dispose();
                    __messagePipe.Dispose();
                    __layers.ForEach(l => l.Dispose());
                    __layers.Clear();
                    __mres.Dispose();
                }

                __messagePipe = null;
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
