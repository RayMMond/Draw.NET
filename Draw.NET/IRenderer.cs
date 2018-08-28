using System;
using System.Collections.Generic;
using System.Drawing;

namespace Draw.NET
{
    public interface IRenderer : IDisposable, IMessagePipe
    {
        void Initialize(IntPtr handle, SizeF initialSize);
        RenderConfig Configuration { get; }
        bool Disposed { get; }

        void ChangeSize(SizeF newSize);
        ILayer<IPrimitive> GetLayerByID(string id);
        List<ILayer<IPrimitive>> GetLayers();
        ILayer<IPrimitive> GetNewLayer(int index = 0, string name = "");
        void RemoveLayer(ILayer<IPrimitive> l);
        void RemoveLayerByID(string id);
        void Render(RectangleF clip = default(RectangleF));
    }
}