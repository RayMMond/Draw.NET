/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       FillPattern 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/20 10:38:04
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer.Styles
{
    public abstract class FillPattern : StyleBase
    {
        public virtual FillType FillType
        {
            get
            {
                if (this is Solid)
                {
                    return FillType.Solid;
                }
                else if (this is None)
                {
                    return FillType.None;
                }
                else if (this is LinearGradient)
                {
                    return FillType.LinearGradient;
                }
                else if (this is Texture)
                {
                    return FillType.Texture;
                }
                else
                {
                    return FillType.None;
                }
            }
        }

        public abstract void Load(FillPattern pattern);

        #region 几种填充类型
        /// <summary>
        /// 纯色填充
        /// </summary>
        public class Solid : FillPattern
        {
            public override void Load(FillPattern pattern)
            {
                var s = pattern as Solid;
                if (s != null)
                {
                    Color = s.Color;
                }
            }
        }


        /// <summary>
        /// 无填充
        /// </summary>
        public class None : FillPattern
        {
            public override Color Color
            {
                get
                {
                    return Color.Transparent;
                }

                set { }
            }

            public override void Load(FillPattern pattern)
            {
                return;
            }
        }

        /// <summary>
        /// 渐变填充
        /// </summary>
        public class LinearGradient : None
        {
            //TODO:待完成


            public override void Load(FillPattern pattern)
            {
                var lg = pattern as LinearGradient;
                //TODO:待完成
            }
        }

        /// <summary>
        /// 材质填充
        /// </summary>
        public class Texture : None
        {
            private Image image;

            public Image Image
            {
                get { return image; }
                set
                {
                    if (value != image)
                    {
                        image = value;
                        OnPropertyChanged(nameof(Image));
                    }
                }
            }


            public override void Load(FillPattern pattern)
            {
                image?.Dispose();
                var t = pattern as Texture;
                if (t != null)
                {
                    image = t.image;
                }
            }


            protected override void Dispose(bool disposing)
            {
                if (disposing)
                {
                    image?.Dispose();
                }
                image = null;
                base.Dispose(disposing);
            }

        }
        #endregion

    }
}
