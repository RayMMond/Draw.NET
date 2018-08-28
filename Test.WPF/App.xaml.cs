using Draw.NET;
using Draw.NET.Core;
using Draw.NET.Renderer.GDIPlus.Primitive;
using Draw.NET.Renderer.Primitives;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Test.WPF
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        public static Container Container { get; }

        static App()
        {
            Container = new Container();
            Container.Register<IPrimitiveProvider, GDIPrimitiveProvider>(Lifestyle.Singleton);
            Container.Register<IRenderer, GDIPlusRenderer>(Lifestyle.Singleton);
            Container.Register<ICanvas, Canvas>(Lifestyle.Singleton);
            Container.Verify();


        }
    }
}
