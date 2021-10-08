using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DLKJ
{
    public static class MathTool
    {
        private const float a = 0.02286f;
        public static float 随机频率 = 8.2f;
        public static float FA = 0;
        public static float FB = 0;
        public static float FC = 0;
        public static float FD = 0;
        public static float φA = 0;
        public static float φB = 0;
        public static float φC = 0;

        public static void Init()
        {

        }

        /// <summary>
        /// 三厘米测量线终端直接接短路板(不接二端口网络时)
        /// </summary>
        /// <param name="A">电压[10-1000]</param>
        /// <param name="δ">控制衰减器取值[0,1]</param>
        /// <param name="f">频率范围[8.2,12.5]Ghz</param>
        /// <param name="z">当前测量位置与三厘米测量线最右端距离</param>
        /// <returns></returns>
        public static double 等效截面(float A, float δ, float f, float z)
        {
            double i = Math.Pow(10, 8);
            double c = 3 * i;
            double ru = c / f;
            float ruc = 2 * a;
            double result1 = ru / ruc;
            // decimal di = (decimal)result1 * (decimal)result1;
            double 根号内的值 = Math.Pow(result1, 2);
            double result = Math.Sqrt(1 - 根号内的值);
            double β = 2 * Mathf.PI / ru * result;
            double U = δ * Math.Abs(A) * Math.Abs(Math.Sin(β * z));
            return U;
        }

        //public static double 二端口网络S参数测量()
        //{


        //}

        private static float Random(float value, float min, float max)
        {
            if (value < min)
                return value;
            if (value > max)
                return max;
            return value;
        }
    }
}