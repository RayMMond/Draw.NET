/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       Command 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/7/4 16:09:10
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer
{
    struct Command<T>
    {
        public readonly Action<T> CommandDelegate;

        public readonly T Parameter;


        public Command(Action<T> a, T parameter)
        {
            Parameter = parameter;
            CommandDelegate = a;
        }

        public void Run()
        {
            CommandDelegate?.Invoke(Parameter);
        }
    }
}
