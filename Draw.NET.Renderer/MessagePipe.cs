﻿/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       MessagePipe 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/14 10:30:01
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer
{


    /// <summary>
    /// 消息管道的实现，目前功能为消息的向上传递
    /// </summary>
    public class MessagePipe : IMessagePipe, IDisposable
    {

        private object sender;


        /// <summary>
        /// 消息传递等级，更改此属性可以筛选需要传递的消息等级
        /// </summary>
        public static DrawingMessageLevel MessageLevel = DrawingMessageLevel.Info;


        /// <summary>
        /// 消息监听事件
        /// </summary>
        public event EventHandler<DrawingFrameworkMessage> MessageListener;

        /// <summary>
        /// 消息管道构造函数
        /// </summary>
        /// <param name="sender">发送消息的类</param>
        public MessagePipe(object sender)
        {
            this.sender = sender;
        }

        /// <summary>
        /// 传递一个消息
        /// </summary>
        /// <param name="msg">消息文本</param>
        /// <param name="level">消息等级</param>
        /// <param name="ex">可选参数，异常</param>
        public virtual void OnMessageSend(string msg, DrawingMessageLevel level, Exception ex = null)
        {
            if ((int)level >= (int)MessageLevel)
            {
                MessageListener?.Invoke(sender, new DrawingFrameworkMessage(msg, level, ex: ex));
            }
        }

        /// <summary>
        /// 传递一个消息
        /// </summary>
        /// <param name="arg">消息类</param>
        public virtual void OnMessageSend(DrawingFrameworkMessage arg)
        {
            if ((int)arg.Level >= (int)MessageLevel)
            {
                MessageListener?.Invoke(sender, arg);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // 要检测冗余调用

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {

                }
                MessageListener = null;
                sender = null;

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }

}

