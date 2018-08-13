/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       ShapeExtensions 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/2 14:46:16
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draw.NET.Core.Shapes
{
    public static class ShapeExtensions
    {
        public static string ToDebugString(this AbstractShape shape)
        {
            if (shape == null)
            {
                return "None";
            }
            else
            {
                return shape.ToString();
            }
        }
    }

}
