/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       StateManager 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/27 11:09:48
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Draw.NET.Core.Shapes;

namespace Draw.NET.Core.Mouse
{
    public static class StateManager
    {

        internal static readonly NormalState Normal = new NormalState();
        internal static readonly MouseState Selecting = new SelectingState();
        internal static readonly MouseState Moving = new MovingShapeState();
        internal static readonly MouseState Resizing = new ResizeState();
        internal static readonly MouseState Texting = new TextingState();

        public static List<MouseState> States { get; private set; }


        static StateManager()
        {
            States = new List<MouseState>();
            ResetStates();
        }

        public static void ResetStates()
        {
            States?.Clear();
            States.Add(Normal);
            States.Add(Selecting);
            States.Add(Moving);
            States.Add(Resizing);
            States.Add(Texting);
        }

        public static string GetNotExistStateErrorMessage()
        {
            var lines = Environment.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 5)
            {
                var str = lines[4].Substring(37);
                str = str.Substring(0, str.IndexOf('('));
                str = str.Replace(".", " - ");
                return $"{str} 错误，出现了不应存在的状态！";
            }
            return "出现了不应存在的状态！";
        }

        public static string GetCurrentStateAndAction()
        {
            var lines = Environment.StackTrace.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length > 5)
            {
                var str = lines[4].Substring(37);
                str = str.Substring(0, str.IndexOf('('));
                str = str.Replace(".", " - ");
                return str;
            }
            return "";
        }

        /// <summary>
        /// 根据形状、是否按下、是否编辑文字来确定鼠标的状态
        /// </summary>
        /// <param name="shape">形状</param>
        /// <param name="isPressed">是否按下</param>
        /// <param name="isTexting">是否编辑文字</param>
        /// <returns></returns>
        internal static MouseState GetStateByShape(AbstractShape shape, bool isPressed,bool isTexting = false)
        {
            return States.FirstOrDefault(s => s.IsBindedShape(shape, isPressed, isTexting));
        }
    }
}
