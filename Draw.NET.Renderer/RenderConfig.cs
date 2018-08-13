/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       RenderConfig 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/14 14:43:47
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer
{
    /// <summary>
    /// 渲染器的配置
    /// </summary>
    public class RenderConfig : IDisposable
    {
        private bool disposedValue = false;


        internal event EventHandler<SizeChangedEventArg> ClientSizeChanged;


        public RenderConfig() { }

        public RenderConfig(Matrix m)
        {
            matrix = m;
        }


        /// <summary>
        /// 渲染区域
        /// </summary>
        public SizeF ClientSize { get; private set; }

        public RenderMode Mode { get; set; } = RenderMode.OnRenderCall;

        public bool UseMultiThreaded { get; set; } = false;

        public bool ScaleLineWidth { get; set; } = true;

        /// <summary>
        /// 1~120
        /// </summary>
        public ushort FPS { get; set; } = 30;

        public int RenderDelay { get; set; } = 20;



        private Matrix matrix = new Matrix();
        public Matrix TransformMatrix
        {
            get
            {
                if (matrix == null)
                {
                    matrix = new Matrix();
                }
                return matrix;
            }
            set
            {
                if (value == null)
                {
                    throw new InvalidOperationException("RenderConfig的转换矩阵不能为空！");
                }
                matrix = value;
            }
        }

        public Color BackgroundColor { get; set; }

        public int ParallelComputeMaxCount { get; set; } = 500;

        public void ChangeSize(SizeF newSize)
        {
            var oldSize = ClientSize;
            ClientSize = newSize;
            ClientSizeChanged?.Invoke(this, new SizeChangedEventArg(oldSize, newSize));
        }

        #region IDisposable Support

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    matrix.Dispose();
                }


                matrix = null;
                ClientSizeChanged = null;

                disposedValue = true;
            }
        }


        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

    public enum RenderMode
    {
        OnRenderCall,
        OnRenderCallDelay,
        FPS30,
        FPS60,
        FPSCustom
    }

    internal class SizeChangedEventArg : EventArgs
    {
        public readonly SizeF NewSize;
        public readonly SizeF OldSize;

        public SizeChangedEventArg(SizeF oldSize, SizeF newSize)
        {
            this.OldSize = oldSize;
            this.NewSize = newSize;
        }
    }
}
