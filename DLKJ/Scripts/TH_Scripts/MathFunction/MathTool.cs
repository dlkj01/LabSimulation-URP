using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DLKJ
{
    public static class MathTool
    {
        private const float a = 0.02286f;
        public static float ���Ƶ�� = 8.2f;
        public static float FA = 0;
        public static float FB = 0;
        public static float FC = 0;
        public static float FD = 0;
        public static float ��A = 0;
        public static float ��B = 0;
        public static float ��C = 0;

        public static void Init()
        {

        }

        /// <summary>
        /// �����ײ������ն�ֱ�ӽӶ�·��(���Ӷ��˿�����ʱ)
        /// </summary>
        /// <param name="A">��ѹ[10-1000]</param>
        /// <param name="��">����˥����ȡֵ[0,1]</param>
        /// <param name="f">Ƶ�ʷ�Χ[8.2,12.5]Ghz</param>
        /// <param name="z">��ǰ����λ���������ײ��������Ҷ˾���</param>
        /// <returns></returns>
        public static double ��Ч����(float A, float ��, float f, float z)
        {
            double i = Math.Pow(10, 8);
            double c = 3 * i;
            double ru = c / f;
            float ruc = 2 * a;
            double result1 = ru / ruc;
            // decimal di = (decimal)result1 * (decimal)result1;
            double �����ڵ�ֵ = Math.Pow(result1, 2);
            double result = Math.Sqrt(1 - �����ڵ�ֵ);
            double �� = 2 * Mathf.PI / ru * result;
            double U = �� * Math.Abs(A) * Math.Abs(Math.Sin(�� * z));
            return U;
        }

        //public static double ���˿�����S��������()
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