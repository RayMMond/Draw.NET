using System;
using System.Collections.Generic;
using System.Drawing;

namespace Draw.NET.Renderer
{
    public interface IAbstractRenderer : IDisposable, IMessagePipe
    {
        RenderConfig Configuration { get; }
        bool Disposed { get; }

        void ChangeSize(SizeF newSize);
        Layer GetLayerByID(string id);
        List<Layer> GetLayers();
        Layer GetNewLayer(int index = 0, string name = "");
        void RemoveLayer(Layer l);
        void RemoveLayerByID(string id);
        void Render(RectangleF clip = default(RectangleF));
    }
}