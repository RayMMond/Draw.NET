using System;
using System.ComponentModel;
using System.Drawing;

namespace Draw.NET
{
    public interface IPrimitive : IMessagePipe, IStyle, INotifyPropertyChanged, IDisposable
    {
        string Description { get; set; }
        bool IsDisposed { get; }
        bool IsPropertyChanged { get; set; }
        ILayer<IPrimitive> Layer { get; set; }
        string UUID { get; }
        bool Visible { get; set; }
        void Draw(object state);
        RectangleF GetBounds();
        void Prepare(object state);
    }
}