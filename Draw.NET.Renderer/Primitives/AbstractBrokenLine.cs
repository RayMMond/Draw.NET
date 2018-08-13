using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Draw.NET.Renderer.Primitives
{
    public abstract class AbstractBrokenLine : AbstractPrimitive, IList<PointF>, ICollection<PointF>, IEnumerable<PointF>
    {
        protected List<PointF> __points;

        protected AbstractBrokenLine()
            : base()
        {
            __points = new List<PointF>();
        }

        public AbstractBrokenLine(List<PointF> pts)
            : this()
        {
            if (pts == null)
            {
                throw new ArgumentNullException("pts");
            }

            __points = pts;
        }

        /// <summary>
        /// 断点集合的副本
        /// </summary>
        public List<PointF> Points { get { return new List<PointF>(__points); } }

        #region IList接口
        public PointF this[int index]
        {
            get
            {
                return ((IList<PointF>)__points)[index];
            }

            set
            {
                ((IList<PointF>)__points)[index] = value;
            }
        }

        public int Count
        {
            get
            {
                return ((IList<PointF>)__points).Count;
            }
        }

        public bool IsReadOnly
        {
            get
            {
                return ((IList<PointF>)__points).IsReadOnly;
            }
        }

        public void Add(PointF item)
        {
            ((IList<PointF>)__points).Add(item);
            OnPropertyChanged(nameof(Points));
        }

        public void AddRange(IEnumerable<PointF> pts)
        {
            __points.AddRange(pts);
            OnPropertyChanged(nameof(Points));
        }

        public void Clear()
        {
            ((IList<PointF>)__points).Clear();
            OnPropertyChanged(nameof(Points));
        }

        public bool Contains(PointF item)
        {
            return ((IList<PointF>)__points).Contains(item);
        }

        public void CopyTo(PointF[] array, int arrayIndex)
        {
            ((IList<PointF>)__points).CopyTo(array, arrayIndex);
        }

        public IEnumerator<PointF> GetEnumerator()
        {
            return ((IList<PointF>)__points).GetEnumerator();
        }

        public int IndexOf(PointF item)
        {
            return ((IList<PointF>)__points).IndexOf(item);
        }

        public void Insert(int index, PointF item)
        {
            ((IList<PointF>)__points).Insert(index, item);
            OnPropertyChanged(nameof(Points));
        }

        public bool Remove(PointF item)
        {
            bool b = ((IList<PointF>)__points).Remove(item);
            OnPropertyChanged(nameof(Points));
            return b;
        }

        public void RemoveAt(int index)
        {
            ((IList<PointF>)__points).RemoveAt(index);
            OnPropertyChanged(nameof(Points));
        }


        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IList<PointF>)__points).GetEnumerator();
        }
        #endregion



        public override RectangleF GetBounds()
        {
            return __points.GetBounds();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                __points.Clear();
            }
            base.Dispose(disposing);
        }
    }
}
