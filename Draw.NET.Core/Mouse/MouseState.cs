/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       MouseState 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/27 11:06:52
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using Draw.NET.Core.Shapes;

namespace Draw.NET.Core.Mouse
{
    public interface MouseState
    {

        List<AbstractShape> MouseMove(MouseManager m, PointF pt);

        AbstractShape MouseDown(MouseManager m, PointF pt);

        void MouseUp(MouseManager m, PointF pt);
        
        /// <summary>
        /// 是否为本状态绑定的图形
        /// 若鼠标悬停在是本状态绑定的图形上，鼠标状态切换为本状态
        /// </summary>
        /// <param name="shape"></param>
        /// <param name="isPressed"></param>
        /// <returns></returns>
        bool IsBindedShape(AbstractShape shape,bool isPressed,bool isTexting = false);

    }
}
