/*-----------------------------------------------------------------------------------------------------------
//版权声明： 本文件由广州航新航空科技股份有限公司版权所有，未经授权，禁止第三方进行拷贝和使用
//
//文件名：       Line 
//文件功能描述： 
//
//创建标识：     孙云鹏 2018/6/20 15:50:00
//
//修改标识：    
 -----------------------------------------------------------------------------------------------------------*/
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Draw.NET.Renderer;
using Draw.NET.Renderer.Primitives;
using Draw.NET.Renderer.Styles;
using Draw.NET.Core.Shapes;

namespace Draw.NET.Core.Shapes
{
    public class Line : AbstractShape, IList<PointF>, ICollection<PointF>, IEnumerable<PointF>
    {
        #region 字段、事件声明、属性
        private IPrimitiveProvider provider;
        private AbstractBrokenLine __line { get { return PrimaryPrimitive as AbstractBrokenLine; } }

        protected event EventHandler<PointChangedEventArgs> PointChanged;


        public AnchorHandle FrontAnchor { get; set; }
        public AnchorHandle BackAnchor { get; set; }

        #endregion

        #region 公开方法
        public Line(IPrimitiveProvider primitiveProvider)
        {
            this.provider = primitiveProvider ?? throw new ArgumentNullException(nameof(primitiveProvider));
            var line = primitiveProvider.GetPrimitive<AbstractLine>();

            line.Visible = true;
            Styles.LineStyles.ApplyDefaultPatternTo(line);
            InitializePrimaryPrimitiveAndBound(line, null);
            PointChanged += Line_PointChanged;
            CanSelect = true;
        }

        #region 图形操作
        /// <summary>
        /// 移动图形
        /// </summary>
        /// <param name="vector"></param>
        protected override void MoveShape(SizeF vector)
        {
            base.MoveShape(vector);
            var allPoints = __line.Points;
            for (int i = 0; i < allPoints.Count; i++)
            {
                allPoints[i] = new PointF(allPoints[i].X + vector.Width, allPoints[i].Y + vector.Height);
            }
            __line.Clear();
            __line.AddRange(allPoints);
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="pt"></param>
        /// <param name="angle"></param>
        public override void RotateAt(PointF pt, float angle)
        {
            return;
        }

        /// <summary>
        /// 编辑文字操作
        /// </summary>
        public override void Edit()
        {
            //TODO:实现编辑逻辑
            throw new NotImplementedException();
        }

        /// <summary>
        /// 选中此图形
        /// </summary>
        public override void Select()
        {
            if (CanSelect)
            {
                __line.BorderPattern.Width = 2;
                base.Select();
            }

        }

        /// <summary>
        /// 去掉选中该图形
        /// </summary>
        public override void UnSelect()
        {
            __line.BorderPattern.Width = 1;
            base.UnSelect();
        }


        /// <summary>
        /// 线段Handle的移动
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="vector">移动大小</param>
        /// <param name="special">需要只水平垂直移动设置为true，否则为false</param>
        public void LineResize(LineResizeHandle handle, SizeF vector, bool special)
        {
            int handleIndex = ResizeHandles.IndexOf(handle);
            switch (handle.Type)
            {
                case LineResizeType.BreakPoint:
                    MoveBreakPointHandle(special, handleIndex, vector);
                    break;
                case LineResizeType.MiddlePoint:
                    MoveMiddlePointHandle(special, handleIndex, vector);
                    break;
                default:
                    break;
            }
            UpdateResizeHandle();
            IsGraphicsPathChanged = true;
        }

        /// <summary>
        /// 初始化Line中的Handle
        /// </summary>
        protected override void InitializeResizeHandle(IPrimitiveProvider provider)
        {
            PointF prePoint = PointF.Empty;
            List<PointF> allPoints = __line.Points;
            foreach (PointF p in allPoints)
            {
                if (prePoint != PointF.Empty)
                {
                    float x = (p.X + prePoint.X) / 2;
                    float y = (p.Y + prePoint.Y) / 2;
                    PointF newPoint = new PointF(x, y);
                    ResizeHandles.Add(new LineResizeHandle(newPoint, this, LineResizeType.MiddlePoint, provider));
                }
                ResizeHandles.Add(new LineResizeHandle(p, this, LineResizeType.BreakPoint, provider));
                prePoint = p;
            }
        }


        /// <summary>
        /// 获取图形Bound
        /// </summary>
        /// <returns></returns>
        public override RectangleF GetBounds()
        {
            return PrimaryPrimitive.GetBounds();
        }
        #endregion

        #region IList接口
        public int Count
        {
            get
            {
                return ((IList<PointF>)__line).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<PointF>)__line).IsReadOnly;
            }
        }

        public PointF this[int index]
        {
            get
            {
                return ((IList<PointF>)__line)[index];
            }

            set
            {
                ((IList<PointF>)__line)[index] = value;
            }
        }


        public int IndexOf(PointF item)
        {
            return ((IList<PointF>)__line).IndexOf(item);
        }

        public void Insert(int index, PointF item)
        {
            ((IList<PointF>)__line).Insert(index, item);
            OnPointChanged();
        }

        public void RemoveAt(int index)
        {
            ((IList<PointF>)__line).RemoveAt(index);
            OnPointChanged();
        }

        public void Add(PointF item)
        {
            ((IList<PointF>)__line).Add(item);
            OnPointChanged();
        }

        public void AddRange(IEnumerable<PointF> pts)
        {
            __line.AddRange(pts);
            OnPointChanged();
        }

        public void Clear()
        {
            ((IList<PointF>)__line).Clear();
            OnPointChanged();
        }

        public bool Contains(PointF item)
        {
            return ((IList<PointF>)__line).Contains(item);
        }

        public void CopyTo(PointF[] array, int arrayIndex)
        {
            ((IList<PointF>)__line).CopyTo(array, arrayIndex);
        }

        public bool Remove(PointF item)
        {
            var b = ((IList<PointF>)__line).Remove(item);
            OnPointChanged();
            return b;
        }

        public IEnumerator<PointF> GetEnumerator()
        {
            return ((IList<PointF>)__line).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<PointF>)__line).GetEnumerator();
        }

        public override void Resize(CornerType type, PointF targetPoint, bool resizeFixRadio)
        {
            IsGraphicsPathChanged = true;
        }

        #endregion

        #endregion

        #region Protected方法
        protected override Pen OnCreateWidenPen()
        {
            return new Pen(Color.Black, BorderPattern.Width + 1);
        }

        protected override Region OnRegionRecreate()
        {
            if (__line.Count < 2)
            {
                throw new ArgumentException("连线的端点数不能小于2个", "Points");
            }
            using (var gp = new GraphicsPath())
            {
                gp.AddLines(__line.ToArray());
                gp.Widen(WidenPen);
                return new Region(gp);
            }
        }


        /// <summary>
        /// 位置移动
        /// </summary>
        /// <param name="diff"></param>
        protected override void OnLocationChanged(SizeF diff)
        {
            for (int i = 0; i < __line.Points.Count; i++)
            {
                __line.Points[i] += diff;
            }
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {

            }
            PointChanged = null;
            FrontAnchor = null;
            BackAnchor = null;

            base.Dispose(disposing);
        }

        protected void OnPointChanged()
        {
            PointChanged?.Invoke(this, new PointChangedEventArgs());
        }

        protected virtual void Line_PointChanged(object sender, PointChangedEventArgs e)
        {
            ResizeHandles.ForEach(h => h.Dispose());
            ResizeHandles.Clear();
            InitializeResizeHandle(provider);
            IsGraphicsPathChanged = true;
        }

        #endregion

        #region 私有方法

        #region Handle缩放Line
        /// <summary>
        /// 移动端点Handle缩放处理
        /// </summary>
        /// <param name="special">表示端点或两端的关联端点进行水平或垂直移动</param>
        /// <param name="handleIndex">handle在Line中的索引</param>
        /// <param name="vector"></param>
        private void MoveBreakPointHandle(bool special, int handleIndex, SizeF vector)
        {
            int pIndex = handleIndex / 2;
            if (special)
            {
                int prePIndex = -1;
                int nextPIndex = -1;
                eAlignType alignType1 = eAlignType.NotAlign;
                eAlignType alignType2 = eAlignType.NotAlign;
                if (pIndex == 0)
                {
                    nextPIndex = 1;
                    alignType1 = GetAlignType(pIndex, nextPIndex);
                }
                else if (pIndex == __line.Points.Count - 1)
                {
                    prePIndex = __line.Points.Count - 2;
                    alignType2 = GetAlignType(pIndex, prePIndex);
                }
                else
                {
                    prePIndex = pIndex - 1;
                    nextPIndex = pIndex + 1;
                    alignType1 = GetAlignType(pIndex, nextPIndex);
                    alignType2 = GetAlignType(pIndex, prePIndex);
                }

                MovePointByAlignType(alignType1, nextPIndex, vector);
                MovePointByAlignType(alignType2, prePIndex, vector);
            }
            __line[pIndex] += vector;
        }

        /// <summary>
        /// 移动中间点Handle缩放处理
        /// </summary>
        /// <param name="special">表示端点或两端的关联端点进行水平或垂直移动</param>
        /// <param name="handleIndex">handle在Line中的索引</param>
        /// <param name="vector">鼠标移动该Handle移动的矩形大小</param>
        private void MoveMiddlePointHandle(bool special, int handleIndex, SizeF vector)
        {
            int prePointIndex = handleIndex / 2;
            int nextPointIndex = handleIndex / 2 + 1;
            eAlignType alignType = GetAlignType(prePointIndex, nextPointIndex);
            if (special)
            {
                MovePointByAlignType(alignType, prePointIndex, vector);
                MovePointByAlignType(alignType, nextPointIndex, vector);
            }
            else
            {
                __line[prePointIndex] += vector;
                __line[nextPointIndex] += vector;
            }
        }

        /// <summary>
        /// 获取两端点是对齐方式
        /// </summary>
        /// <param name="p1Index">端点1</param>
        /// <param name="p2Index">端点2</param>
        /// <returns>对齐方式</returns>
        private eAlignType GetAlignType(int p1Index, int p2Index)
        {
            PointF p1 = __line[p1Index];
            PointF p2 = __line[p2Index];
            if (p1.X == p2.X && p1.Y != p2.Y)
            {
                return eAlignType.Vertical;
            }
            else if (p1.X != p2.X && p1.Y == p2.Y)
            {
                return eAlignType.Horizontal;
            }
            else if (p1.X == p2.X && p1.Y == p2.Y)
            {
                return eAlignType.SamePoint;
            }
            else
            {
                return eAlignType.NotAlign;
            }
        }

        /// <summary>
        /// 根据对齐方式移动端点
        /// </summary>
        /// <param name="alignType">对齐方式</param>
        /// <param name="pointIndex">端点索引</param>
        /// <param name="vector">鼠标移动的矩形大小</param>
        private void MovePointByAlignType(eAlignType alignType, int pointIndex, SizeF vector)
        {
            if (pointIndex == -1) return;
            PointF p = __line[pointIndex];

            switch (alignType)
            {
                case eAlignType.Horizontal:
                    __line[pointIndex] = p + new SizeF(0, vector.Height);
                    break;
                case eAlignType.SamePoint:
                case eAlignType.Vertical:
                    __line[pointIndex] = p + new SizeF(vector.Width, 0);
                    break;
                case eAlignType.NotAlign:
                default:
                    __line[pointIndex] = p + vector;
                    break;
            }
        }
        #endregion

        /// <summary>
        /// 根据Line中所有端点重置所有Handle的位置
        /// </summary>
        protected override void UpdateResizeHandle()
        {
            PointF prePoint = PointF.Empty;
            List<PointF> allPoints = __line.Points;
            int handleIndex = 0;
            foreach (PointF p in allPoints)
            {
                if (prePoint != PointF.Empty)
                {
                    float x = (p.X + prePoint.X) / 2;
                    float y = (p.Y + prePoint.Y) / 2;
                    PointF newPoint = new PointF(x, y);
                    ResizeHandles[handleIndex].Location = newPoint;
                    handleIndex++;
                }
                ResizeHandles[handleIndex].Location = p;
                handleIndex++;
                prePoint = p;
            }
        }

        #endregion

        #region 类型定义
        protected class PointChangedEventArgs : EventArgs
        {
            public PointChangedEventArgs()
            {

            }
        }

        enum eAlignType
        {
            Horizontal,
            Vertical,
            SamePoint,
            NotAlign
        }
        #endregion
    }

}
