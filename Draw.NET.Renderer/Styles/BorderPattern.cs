/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       BorderLine 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/20 10:37:45
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;

namespace Draw.NET.Renderer.Styles
{
    /// <summary>
    /// 边框样式类
    /// </summary>
    public class BorderPattern : StyleBase
    {

        public BorderPattern()
        {

        }

        #region 属性
        protected DashStyle dashStyle = DashStyle.Solid;

        public DashStyle DashStyle
        {
            get { return dashStyle; }
            set
            {
                if (value != dashStyle)
                {
                    dashStyle = value;
                    OnPropertyChanged(nameof(DashStyle));
                }
            }
        }


        private bool dashCapVisible = false;

        public bool DashCapVisible
        {
            get { return dashCapVisible; }
            set
            {
                if (value != dashCapVisible)
                {
                    dashCapVisible = value;
                    OnPropertyChanged(nameof(DashCapVisible));
                }
            }
        }


        protected DashCap dashCap = DashCap.Flat;

        public DashCap DashCap
        {
            get { return dashCap; }
            set
            {
                if (value != dashCap)
                {
                    dashCap = value;
                    OnPropertyChanged(nameof(DashCap));
                }
            }
        }

        

        protected int width = 1;
        /// <summary>
        /// 线宽
        /// </summary>
        public virtual int Width
        {
            get { return width; }
            set
            {
                if (value != width)
                {
                    width = value;
                    OnPropertyChanged(nameof(Width));
                }
            }
        }


        protected ArrowType frontArrowType = ArrowType.None;

        public ArrowType FrontArrowType
        {
            get { return frontArrowType; }
            set
            {
                if (value != frontArrowType)
                {
                    frontArrowType = value;
                    OnPropertyChanged(nameof(FrontArrowType));
                }
            }
        }

        protected ArrowType backArrowType = ArrowType.None;

        public ArrowType BackArrowType
        {
            get { return backArrowType; }
            set
            {
                if (value != backArrowType)
                {
                    backArrowType = value;
                    OnPropertyChanged(nameof(BackArrowType));
                }
            }
        }

        protected ArrowSize frontArrowSize = ArrowSize.Small;

        public ArrowSize FrontArrowSize
        {
            get { return frontArrowSize; }
            set
            {
                if (value != frontArrowSize)
                {
                    frontArrowSize = value;
                    OnPropertyChanged(nameof(FrontArrowSize));
                }
            }
        }

        protected ArrowSize backArrowSize = ArrowSize.Small;

        public ArrowSize BackArrowSize
        {
            get { return backArrowSize; }
            set
            {
                if (value != backArrowSize)
                {
                    backArrowSize = value;
                    OnPropertyChanged(nameof(BackArrowSize));
                }
            }
        }

        #endregion



        public void Load(BorderPattern b)
        {
            this.BackArrowSize = b.BackArrowSize;
            this.BackArrowType = b.BackArrowType;
            this.Color = b.Color;
            this.DashCap = b.DashCap;
            this.DashCapVisible = b.DashCapVisible;
            this.DashStyle = b.DashStyle;
            this.FrontArrowSize = b.FrontArrowSize;
            this.FrontArrowType = b.FrontArrowType;
            this.Width = b.Width;
        }
    }


}
