using System;
using System.Collections.Generic;

namespace Draw.NET
{
    public interface ILayer<T> : IDisposable
    {
        int Index { get; set; }

        string Name { get; set; }

        string UUID { get; }

        bool Visible { get; set; }


        /// <summary>
        /// 添加图元对象
        /// </summary>
        /// <param name="primitives">图元对象</param>
        void Add(params T[] primitives);
        /// <summary>
        /// 添加图元对象
        /// </summary>
        /// <param name="primitives">图元对象</param>
        void Add(List<T> primitives);
        /// <summary>
        /// 根据图元ID获取对象
        /// </summary>
        /// <param name="id">图元ID</param>
        /// <returns>查找到的图元对象，如果没有找到返回null</returns>
        T GetByID(string id);
        /// <summary>
        /// 所有本图层的所有图元
        /// </summary>
        /// <returns>所有图元列表</returns>
        List<T> GetAll();
        /// <summary>
        /// 从图层中去除一个图元对象
        /// </summary>
        /// <param name="primitive">需要去除的图元对象</param>
        /// <returns>如果去除成功返回true,反之返回false</returns>
        bool Remove(T primitive);

        /// <summary>
        /// 从图层中根据图元ID去除一个图元对象
        /// </summary>
        /// <param name="id">需要去除的图元对象的ID</param>
        /// <returns>如果去除成功返回true,反之返回false</returns>
        bool RemoveByID(string id);
        /// <summary>
        /// 清除并释放本图层所有图元
        /// </summary>
        void Clear();

        System.Drawing.RectangleF GetBounds();
    }
}