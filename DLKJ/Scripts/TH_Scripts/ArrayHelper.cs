using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
namespace Common
{
    public static class ArrayHelper
    {
        /// <summary>
        /// 查找满足条件的单个元素:根据一个条件,从数组中返回符合条件的对象
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">查找条件</param>
        /// <returns></returns>
        public static T Find<T>(this T[] array, Func<T, bool> condition)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                    return array[i];
            }
            return default(T);
        }

        /// <summary>
        /// 查找所有符合条件的元素
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="condition"></param>
        /// <returns></returns>
        public static T[] FindAll<T>(this T[] array, Func<T, bool> condition)
        {
            //集合存储满足条件的元素
            List<T> list = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                if (condition(array[i]))
                    list.Add(array[i]);
            }
            return list.ToArray();
        }

        /// <summary>
        /// 从一个数组元素中获取最大值
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <typeparam name="Y">比较条件返回值类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static T GetMax<T, Y>(this T[] array, Func<T, Y> condition) where Y : IComparable
        {
            T max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (condition(max).CompareTo(condition(array[i])) < 0)
                    max = array[i];
            }
            return max;
        }

        /// <summary>
        /// 从一个数组元素中获取最小值
        /// </summary>
        /// <typeparam name="T">数组的类型</typeparam>
        /// <typeparam name="Y">比较条件返回值类型</typeparam>
        /// <param name="array">数组</param>
        /// <param name="condition">条件</param>
        /// <returns></returns>
        public static T GetMin<T, Y>(this T[] array, Func<T, Y> condition) where Y : IComparable
        {
            T max = array[0];
            for (int i = 1; i < array.Length; i++)
            {
                if (condition(max).CompareTo(condition(array[i])) > 0)
                    max = array[i];
            }
            return max;
        }

        /// <summary>
        /// 升序
        /// </summary>
        /// <typeparam name="T">数组</typeparam>
        /// <typeparam name="Y">比较条件返回值类型</typeparam>
        /// <param name="array"></param>
        /// <param name="condition"></param>
        public static T[] OrderAscending<T, Y>(this T[] array, Func<T, Y> condition) where Y : IComparable
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    //if (array[j] > array[j + 1])
                    if (condition(array[j]).CompareTo(condition(array[j + 1])) > 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            return array;
        }

        /// <summary>
        /// 降序
        /// </summary>
        /// <typeparam name="T">数组</typeparam>
        /// <typeparam name="Y">比较条件返回值类型</typeparam>
        /// <param name="array"></param>
        /// <param name="condition"></param>
        public static T[] OrderDescending<T, Y>(this T[] array, Func<T, Y> condition) where Y : IComparable
        {
            for (int i = 0; i < array.Length - 1; i++)
            {
                for (int j = 0; j < array.Length - 1 - i; j++)
                {
                    //if (array[j] > array[j + 1])
                    if (condition(array[j]).CompareTo(condition(array[j + 1])) < 0)
                    {
                        T temp = array[j];
                        array[j] = array[j + 1];
                        array[j + 1] = temp;
                    }
                }
            }
            return array;
        }



        public static Q[] Select<T, Q>(this T[] array, Func<T, Q> condition)
        {
            Q[] result = new Q[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                //筛选的条件,满足条件就赋值
                result[i] = condition(array[i]);
            }

            return result;
        }
    }
}