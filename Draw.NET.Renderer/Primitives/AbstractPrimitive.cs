using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.ComponentModel;
using Draw.NET.Renderer.Styles;

namespace Draw.NET.Renderer.Primitives
{
    /// <summary>
    /// 抽象图元基类
    /// </summary>
    public abstract class AbstractPrimitive : IMessagePipe, IStyle, INotifyPropertyChanged, IDisposable
    {
        protected MessagePipe __messagePipe;


        /// <summary>
        /// 消息管道监听
        /// </summary>
        public event EventHandler<DrawingFrameworkMessage> MessageListener
        {
            add { __messagePipe.MessageListener += value; }
            remove { __messagePipe.MessageListener -= value; }
        }


        public event PropertyChangedEventHandler PropertyChanged;



        #region 属性
        /// <summary>
        /// 图元的全局唯一ID
        /// </summary>
        public string UUID { get; protected set; }

        protected BorderPattern border;
        /// <summary>
        /// 边框样式
        /// </summary>
        public BorderPattern BorderPattern
        {
            get { return border; }
        }


        protected FillPattern fillPattern;
        /// <summary>
        /// 填充样式
        /// </summary>
        public FillPattern FillPattern
        {
            get { return fillPattern; }
            set
            {
                if (value != fillPattern && value != null)
                {
                    fillPattern.PropertyChanged -= PropertyChangedEvent;
                    fillPattern = value;
                    fillPattern.PropertyChanged += PropertyChangedEvent;
                    OnPropertyChanged(nameof(FillPattern));
                }
            }
        }



        protected Layer layer;
        /// <summary>
        /// 所属图层
        /// </summary>
        public Layer Layer
        {
            get { return layer; }
            set
            {
                if (value != layer)
                {
                    layer = value;
                    OnPropertyChanged(nameof(Layer));
                }
            }
        }


        /// <summary>
        /// 用于指示图元属性已更改，图元数据需要重新计算
        /// </summary>
        public bool IsPropertyChanged { get; set; }



        protected bool visible = false;
        /// <summary>
        /// 是否可见
        /// </summary>
        public virtual bool Visible
        {
            get { return visible; }
            set
            {
                if (value != visible)
                {
                    visible = value;
                    OnPropertyChanged(nameof(Visible));
                }
            }
        }


        protected string description;
        /// <summary>
        /// 图元描述
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                if (description != value)
                {
                    description = value;
                    OnPropertyChanged(nameof(Description));
                }
            }
        }


        public bool IsDisposed { get { return disposedValue; } }
        #endregion


        /// <summary>
        /// 抽象图元基类构造函数
        /// </summary>
        protected AbstractPrimitive()
        {
            UUID = Guid.NewGuid().ToString();

            border = new BorderPattern();
            border.PropertyChanged += PropertyChangedEvent;

            fillPattern = new FillPattern.None();
            fillPattern.PropertyChanged += PropertyChangedEvent;

            PropertyChanged += PropertyChangedEvent;


            IsPropertyChanged = true;
            Layer = null;
            __messagePipe = new MessagePipe(this);
        }

        /// <summary>
        /// 渲染本图元
        /// </summary>
        public abstract void Draw(object state);


        /// <summary>
        /// 计算本图元的数据
        /// </summary>
        /// <param name="state">计算参数</param>
        public abstract void Prepare(object state);


        public abstract RectangleF GetBounds();


        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


        #region IDisposable Support
        protected bool disposedValue = false; // 要检测冗余调用


        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    __messagePipe.Dispose();
                    fillPattern.Dispose();
                    border.Dispose();
                }

                Layer = null;
                __messagePipe = null;
                PropertyChanged = null;
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion



        protected void PropertyChangedEvent(object sender, PropertyChangedEventArgs e)
        {
            IsPropertyChanged = true;
        }
    }
}
