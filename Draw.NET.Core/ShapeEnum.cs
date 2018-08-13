/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       ShapeEnum 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/20 15:14:17
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draw.NET.Core
{

    /// <summary>
    /// 线段Handle类型
    /// </summary>
    public enum LineResizeType
    {
        /// <summary>
        /// 拐点
        /// </summary>
        BreakPoint,
        /// <summary>
        /// 中点
        /// </summary>
        MiddlePoint
    }


    /// <summary>
    /// 锚点位于线段的位置
    /// </summary>
    public enum AnchorLocation
    {
        /// <summary>
        /// 锚点位于线段起点
        /// </summary>
        Front,
        /// <summary>
        /// 锚点位于线段终点
        /// </summary>
        Back,
    }



}
