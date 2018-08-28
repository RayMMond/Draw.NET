using System;
using System.Collections.Generic;
using System.Drawing;
using Draw.NET.Core.Layers;
using Draw.NET.Core.Shapes;

namespace Draw.NET.Core
{
    public interface ICanvas : IDisposable, IMessagePipe
    {
        bool IsDisposed { get; }
        IRenderer Renderer { get; }

        event EventHandler<BeginTextingEventArgs> BeginEditingEvent;

        void AddShape(AbstractShape shape, string layerID = null, string layerName = null);
        void AddShapes(string layerID = null, string layerName = null, params AbstractShape[] shapes);
        void AlignBottom();
        void AlignCenter();
        void AlignLeft();
        void AlignMiddle();
        void AlignRight();
        void AlignTop();
        void AutoAlign();
        void AutoSpace();
        void BeginDrag(PointF pt);
        void ChangeSize(SizeF newSize);
        void Clear();
        void DistributeHorizonally();
        void DistributeVertically();
        void Dragging(PointF pt);
        void EndDrag();
        void Fit(string layerID = null);
        UserLayer GetNewUserLayer(string name = null);
        AbstractShape GetShapeByID(string shapeID, string layerID = null, string layerName = null);
        void Load(IntPtr handle, SizeF initialSize);
        void Load(IntPtr handle, Size initialSize);
        AbstractShape MouseDoubleClick(PointF pt, out PointF actualPoint);
        AbstractShape MouseDown(PointF pt, out PointF actualPoint);
        List<AbstractShape> MouseMove(PointF pt, out PointF actualPoint);
        void MouseUp(PointF pt, out PointF actualPoint);
        void RemoveShape(AbstractShape shape);
        void ResetMouse();
        void Scale(PointF pt, bool zoomOut);
        void Scale(PointF pt, float scale);
        void SelectShape(string shapeID);
        void UnselectAll();
    }
}