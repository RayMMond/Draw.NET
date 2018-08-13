/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       StateDefinations 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/27 11:08:24
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
    //enum MouseEditState
    //{
    //    Normal,
    //    Selection,
    //    MoveShape,
    //    Resize,
    //    Rotate,
    //    Connect,
    //    Texting,
    //    AddingShape
    //}


    /// <summary>
    /// 正常模式
    /// </summary>
    class NormalState : MouseState
    {
        private AbstractShape shape;

        public bool IsBindedShape(AbstractShape shape, bool isPressed, bool isTexting = false)
        {
            return false;
        }

        public AbstractShape MouseDown(MouseManager m, PointF pt)
        {
            shape = m.HitShape(pt);
#if PerfMon
            System.Diagnostics.Debug.WriteLine($"{StateManager.GetCurrentStateAndAction()} Hit {shape.ToDebugString()}");
#endif

            return shape;
        }


        public List<AbstractShape> MouseMove(MouseManager m, PointF pt)
        {
            if (m.IsPressed && !m.IsDoubleClicked)
            {
                if (!m.MultiSelect && shape?.Selected != true)
                {
                    m.SelectedShapes.ForEach(s => s.UnSelect());
                    m.SelectedShapes.Clear();
                }

                if (shape == null)
                {
                    m.State = StateManager.Selecting;
                    return m.State.MouseMove(m, pt);
                }
                else
                {
                    if (shape.CanSelect)
                    {
                        shape.Select();
                        m.SelectedShapes.Add(shape);
                        var state = StateManager.GetStateByShape(shape, m.IsPressed);
                        if (state != null)
                        {
                            m.State = state;
                            return m.State.MouseMove(m, pt);
                        }
                        else
                        {
                            throw new InvalidOperationException(StateManager.GetNotExistStateErrorMessage());
                        }
                    }
                    else
                    {
                        if (shape is AbstractHandle)
                        {
                            var state = StateManager.GetStateByShape(shape, m.IsPressed);
                            if (state != null)
                            {
                                m.State = state;
                                return m.State.MouseMove(m, pt);
                            }
                            else
                            {
                                throw new InvalidOperationException(StateManager.GetNotExistStateErrorMessage());
                            }
                        }
                        else
                        {
                            m.State = StateManager.Selecting;
                            return m.State.MouseMove(m, pt);
                        }
                    }

                }
            }
            else
            {
                List<AbstractShape> list = new List<AbstractShape>();
                var shape = m.HitShape(pt);
                if (shape != null)
                {
                    list.Add(shape);
                    var state = StateManager.GetStateByShape(shape, m.IsPressed, m.IsDoubleClicked);
                    if (state != null)
                    {
                        m.State = state;
#if PerfMon
                        System.Diagnostics.Debug.WriteLine(
                             $"{StateManager.GetCurrentStateAndAction()} Hit {shape} Enter State : {m.State}");
#endif

                    }
                }
                else
                {
                    m.State = StateManager.Normal;
                }
                return list;
            }
        }

        public void MouseUp(MouseManager m, PointF pt)
        {
            if (!m.MultiSelect)
            {
                m.IsDoubleClicked = false;
                m.SelectedShapes.ForEach(s => s.UnSelect());
                m.SelectedShapes.Clear();
            }

            if (shape != null)
            {
                shape.Select();
                m.SelectedShapes.Add(shape);
            }

        }

        public override string ToString()
        {
            return base.ToString().Split('.').Last();
        }
    }

    /// <summary>
    /// 选择模式
    /// </summary>
    class SelectingState : MouseState
    {

        public bool IsBindedShape(AbstractShape shape, bool isPressed, bool isTexting = false)
        {
            return false;
        }

        public AbstractShape MouseDown(MouseManager m, PointF pt)
        {
            throw new InvalidOperationException(StateManager.GetNotExistStateErrorMessage());
        }

        public List<AbstractShape> MouseMove(MouseManager m, PointF pt)
        {
            if (m.IsPressed)
            {
                m.OperationLayer.GetSelectionRectangle();

                var r = Compute.GetRectangleByPoints(pt, m.PressedPoint);


                m.OperationLayer.Selection.Location = r.Location;
                m.OperationLayer.Selection.RectHeight = r.Height;
                m.OperationLayer.Selection.RectWidth = r.Width;

                var shapes = m.UserLayers.IsVisible(r);
                //#if PerfMon
                //                System.Diagnostics.Debug.WriteLine(
                //                    $@"{StateManager.GetCurrentStateAndAction()} At ({pt}). Rect:{r}.
                //Selected:{string.Join(Environment.NewLine, shapes.Select(s => s.ToDebugString()))}");
                //#endif

                //                shapes.ForEach(s => s.Select());
                //                m.SelectedShapes.AddRange(shapes);
                return shapes;
            }
            else
            {
                throw new InvalidOperationException(StateManager.GetNotExistStateErrorMessage());
            }
        }

        public void MouseUp(MouseManager m, PointF pt)
        {
            var r = Compute.GetRectangleByPoints(pt, m.PressedPoint);
            var shapes = m.UserLayers.IsVisible(r);
#if PerfMon
            System.Diagnostics.Debug.WriteLine(
                $@"{StateManager.GetCurrentStateAndAction()} At ({pt}). Rect:{r}.
Selected:{string.Join(Environment.NewLine, shapes.Select(s => s.ToDebugString()))}");
#endif

            shapes.ForEach(s => s.Select());
            m.SelectedShapes.AddRange(shapes);

            if (m.OperationLayer.Selection.Visible)
            {
                m.OperationLayer.DeleteSelectionRectangle();
            }

            m.State = StateManager.Normal;
        }
    }
    /// <summary>
    /// 移动模式
    /// </summary>
    class MovingShapeState : MouseState
    {

        public bool IsBindedShape(AbstractShape shape, bool isPressed, bool isTexting = false)
        {
            if (isPressed && !isTexting)
            {
                switch (shape.TypeString)
                {
                    case "Rect":
                    case "Line":
                    case "Ellipse":
                    case "Text":
                        return true;
                }
            }
            return false;
        }

        public AbstractShape MouseDown(MouseManager m, PointF pt)
        {
            //throw new InvalidOperationException(StateManager.GetNotExistStateErrorMessage());
            return null;
        }

        public List<AbstractShape> MouseMove(MouseManager m, PointF pt)
        {
            if (m.IsPressed)
            {
                var shapes = m.SelectedShapes;

                var size = pt - m.LastPoint.ToSizeF();

#if PerfMon
                System.Diagnostics.Debug.WriteLine(
                    $@"{StateManager.GetCurrentStateAndAction()} At ({pt}) Move ({size}). 
Selected:{string.Join(Environment.NewLine, shapes)}");
#endif

                shapes.ForEach(s => s.Move(size));
            }
            else
            {
                throw new InvalidOperationException(StateManager.GetNotExistStateErrorMessage());
            }
            return null;
        }

        public void MouseUp(MouseManager m, PointF pt)
        {
            m.State = StateManager.Normal;
        }
    }

    /// <summary>
    /// 缩放状态
    /// </summary>
    class ResizeState : MouseState
    {
        private AbstractHandle handle;

        public bool IsBindedShape(AbstractShape shape, bool isPressed, bool isTexting = false)
        {
            if (isPressed)
            {
                if (shape.TypeString == "RectResizeHandle")
                {
                    handle = shape as RectResizeHandle;
                    return true;
                }
                else if (shape.TypeString == "LineResizeHandle")
                {
                    handle = shape as LineResizeHandle;
                    return true;
                }
            }
            return false;
        }

        public AbstractShape MouseDown(MouseManager m, PointF pt)
        {
            throw new InvalidOperationException(StateManager.GetNotExistStateErrorMessage());

        }

        /// <summary>
        /// 改变图像大小
        /// </summary>
        /// <param name="m"></param>
        /// <param name="pt"></param>
        /// <returns></returns>
        public List<AbstractShape> MouseMove(MouseManager m, PointF pt)
        {
            if (m.IsPressed)
            {
                if (handle is RectResizeHandle)
                {
                    var resizeHandle = handle as RectResizeHandle;
                    //m.SetcursorByCornerType(resizeHandle.Type);
                    var parent = handle.ParentShape;
                    parent.Select();
                    m.SelectedShapes.Add(parent);
                    var size = pt - m.LastPoint.ToSizeF();
                    parent.Resize(resizeHandle.Type, pt, m.ResizeFixRadio);
                }
                else if (handle is LineResizeHandle)
                {
                    var lineResizeHandle = handle as LineResizeHandle;
                    var parent = lineResizeHandle.ParentShape;
                    if (parent is Line)
                    {
                        parent.Select();
                        m.SelectedShapes.Add(parent);
                        var vector = pt - m.LastPoint.ToSizeF();
                        (parent as Line).LineResize(lineResizeHandle, vector.ToSizeF(), true);
                    }
                }
            }
            else
            {
                throw new InvalidOperationException(StateManager.GetNotExistStateErrorMessage());
            }
            return null;

        }


        /// <summary>
        /// 文字提示+鼠标样式变换
        /// </summary>
        /// <param name="m"></param>
        /// <param name="pt"></param>
        public void MouseUp(MouseManager m, PointF pt)
        {
            handle.UnSelect();
            m.SelectedShapes.Remove(handle);
            //m.SetCursor(System.Windows.Forms.Cursors.Default);
            m.State = StateManager.Normal;
        }
    }

    /// <summary>
    /// 文字编辑状态
    /// </summary>
    class TextingState : MouseState
    {
        Text __text;

        public bool IsBindedShape(AbstractShape shape, bool isPressed, bool isTexting = false)
        {
            if (isTexting)
            {
                if (shape.TypeString == "Text")
                {
                    __text = shape as Text;
                    return true;
                }
            }
            return false;
        }

        public AbstractShape MouseDown(MouseManager m, PointF pt)
        {
            if (__text != null)
            {
                __text.ResizeHandles.ForEach(p => p.Visible = false);
                var curShape = m.HitShape(pt);
                if (curShape?.UUID != __text.UUID)
                {
                    __text.UnSelect();
                    if (curShape != null)
                    {
                        if(!(curShape is Text))
                        {
                            m.IsDoubleClicked = false;
                            m.State = StateManager.Normal;
                            m.SelectedShapes.Clear();
                        }
                        var state = StateManager.GetStateByShape(curShape, m.IsPressed, m.IsDoubleClicked);
                        if (state != null && m.State != state)
                        {
                            m.State = state;
                            m.MouseDown(pt);
                        }
                    }
                    else
                    {
                        m.IsDoubleClicked = false;
                        m.State = StateManager.Normal;
                        m.SelectedShapes.Clear();
                    }
                }
            }
            return __text;
        }

        public List<AbstractShape> MouseMove(MouseManager m, PointF pt)
        {
            
            return null;
        }

        public void MouseUp(MouseManager m, PointF pt)
        {
        }
    }

}
