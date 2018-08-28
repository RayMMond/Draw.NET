using System;
using System.Collections.Generic;
using System.Text;

namespace Draw.NET
{
    /// <summary>
    /// 消息管道接口
    /// </summary>
    public interface IMessagePipe
    {
        event EventHandler<DrawingFrameworkMessage> MessageListener;
    }


    /// <summary>
    /// 消息类型
    /// </summary>
    public enum DrawingMessageType
    {
        Message = 0,

    }

    /// <summary>
    /// 消息等级
    /// </summary>
    public enum DrawingMessageLevel
    {
        Info,
        Warning,
        Error,
        Fatal,
    }


    /// <summary>
    /// 消息类
    /// </summary>
    public class DrawingFrameworkMessage : EventArgs
    {
        /// <summary>
        /// 消息文本
        /// </summary>
        public readonly string Message;

        /// <summary>
        /// 消息附加对象
        /// </summary>
        public readonly object Tag;

        /// <summary>
        /// 本消息等级
        /// </summary>
        public readonly DrawingMessageLevel Level;


        /// <summary>
        /// 本消息的异常
        /// </summary>
        public readonly Exception Ex;

        /// <summary>
        /// 消息构造，能传递文本、等级、异常和附加对象
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="lv">消息等级</param>
        /// <param name="ex">异常</param>
        /// <param name="state">附加对象</param>
        public DrawingFrameworkMessage(string msg, DrawingMessageLevel lv, Exception ex = null, object state = null)
        {
            Message = msg;
            Level = lv;
            Ex = ex;
            Tag = state;
        }

    }
}
