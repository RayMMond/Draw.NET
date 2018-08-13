/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       StyleEnum 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/20 11:05:21
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer.Styles
{
    /// <summary>
    /// 边框线条类型
    /// </summary>
    public enum LineType
    {
        /// <summary>
        /// 无线条
        /// </summary>
        None,

        /// <summary>
        /// 实线
        /// </summary>
        Solid,

        /// <summary>
        /// 渐变线
        /// </summary>
        LinearGradient
    }


    public enum FillType
    {
        /// <summary>
        /// 无填充
        /// </summary>
        None,

        /// <summary>
        /// 纯色填充
        /// </summary>
        Solid,

        /// <summary>
        /// 线性渐变填充
        /// </summary>
        LinearGradient,

        /// <summary>
        /// 材质填充
        /// </summary>
        Texture
    }


    public enum ArrowType
    {
        /// <summary>
        /// 无箭头
        /// </summary>
        None,

        /// <summary>
        /// 空心
        /// </summary>
        Hollow,


        /// <summary>
        /// 实心
        /// </summary>
        Solid,
    }


    /// <summary>
    /// 箭头大小
    /// </summary>
    public enum ArrowSize
    {
        /// <summary>
        /// 较小
        /// </summary>
        Smaller,

        /// <summary>
        /// 小
        /// </summary>
        Small,

        /// <summary>
        /// 中等
        /// </summary>
        Normal,

        /// <summary>
        /// 大
        /// </summary>
        Big,

        /// <summary>
        /// 特大
        /// </summary>
        Bigger,

        /// <summary>
        /// 最大
        /// </summary>
        Biggest
    }

}
