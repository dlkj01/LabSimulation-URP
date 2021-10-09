using System;
using UnityEngine;
namespace DLKJ
{
    public static class MathTool
    {
        private const float j = 1;
        private const float a = 0.02286f;
        private static float A = 10;//电压10mv-1000mv 初始化后不变
        public static float F = 8.2f;//频率8.2-12.5  初始化后不变
        private static float δ = 0;//控制衰减器取值[0,1] 初始化后不变
        private static double β;//贝特
        private static float X = 0;//取值范围[-200,200] 初始化后不变
        private static float R = 0;//取值范围[0,200] 初始化后不变
        private static float ZL = 0;//ZL=R+jX

        public static void Init()
        {
            RandomDataInit();
        }
        public static void RandomDataInit()
        {
            F = UnityEngine.Random.Range(8.2f, 12.5f);
            A = UnityEngine.Random.Range(10f, 1000f);
            δ = UnityEngine.Random.Range(0f, 1f); //Random(0.00f, 1.00f);
            X = UnityEngine.Random.Range(-200f, 200f);
            R = UnityEngine.Random.Range(0f, 200f);
            ZL = R + X;
            β = Getβ();

            FA = UnityEngine.Random.Range(0f, 100f);
            FB = UnityEngine.Random.Range(0f, 100f);
            FC = UnityEngine.Random.Range(0f, 100f);
            FD = UnityEngine.Random.Range(0f, 1f);
            ShanA = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            ShanB = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            ShanC = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            ShanD = UnityEngine.Random.Range(0, 2 * Mathf.PI);
        }

        /// <summary>
        /// 公式一、(1)
        /// 三厘米测量线终端直接接短路板(不接二端口网络时)
        /// </summary>
        /// <param name="A">电压[10-1000]</param>
        /// <param name="δ">控制衰减器取值[0,1]</param>
        /// <param name="f">频率范围[8.2,12.5]Ghz</param>
        /// <param name="z">当前测量位置与三厘米测量线最右端距离</param>
        /// <returns></returns>
        public static double SLMCLXZDDLB(float z)
        {
            double U = δ * Math.Abs(A) * Math.Abs((Math.Sin(β * z)));
            return U;
        }

        /// <summary>
        /// 二端口网络S参数测量
        /// </summary>
        /// <returns></returns>
        //public static double EDKWLSCSCL(float z)
        //{

        //}


        #region 二端口网络S参数测量
        private static float FA;
        private static float FB;
        private static float FC;
        private static float FD;
        private static float ShanA;
        private static float ShanB;
        private static float ShanC;
        private static float ShanD;
        public static double 二端口网络S参数测量(float z)
        {
            double β = MathTool.β;
            return 0;
        }
        #endregion


        /// <summary>
        /// 公式一、(3)
        /// 接二端口和短路版
        /// </summary>
        /// <param name="z">旋转3厘米测量线的旋钮就是调节z的数值</param>
        /// <returns></returns>
        public static double EDKTODLB(float z)
        {
            //int FD = 1;
            //double 山D = Math.PI;
            //int T2 = -1;
            return FuZaiZuLiangKangHengFormula(GetTl(), CalculateShan(GetTl_a(), GetTl_b()), z);
        }


        /// <summary>
        /// 公式一、(4)
        /// 二端口和匹配负载
        /// </summary>
        /// <param name="z">右端开始为0,越向信号源越大</param>
        /// <returns></returns>
        public static double EDKPPFZ(float z)
        {
            double T1 = FA;
            double Shan1 = ShanA;
            double U = δ * Math.Abs(A) * Math.Sqrt(1 + Math.Abs(Math.Pow(T1, 2)) + 2 * Math.Abs(T1) * Math.Cos(2 * β * z - Shan1));
            return U;
        }

        /// <summary>
        /// 负载阻抗测量第一个公式
        /// </summary>
        /// <param name="z">旋转3厘米测量线的旋钮就是调节z的数值</param>
        /// <returns></returns>
        public static double FZZKCL_First(float z)
        {
            return FuZaiZuLiangKangHengFormula(GetTL(), CalculateShan(GetTL_a(), GetTL_b()), z);
        }

        /// <summary>
        /// 负载抗组匹配
        /// </summary>
        /// <param name="l"></param>
        /// <param name="d"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double FZKZPP(float l, float d, float z)
        {
            return FuZaiZuLiangKangHengFormula(GetFZZKPPTL(l, d), CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)), z);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultT">长得像T的符号的结果,作为参数传递进公式</param>
        /// <param name="resultShan">长得像横着的山的符号的结果,作为参数传递进公式</param>
        /// <param name="z">变量</param>
        /// <returns></returns>
        private static double FuZaiZuLiangKangHengFormula(double resultT, double resultShan, float z)
        {
            double U = δ * Math.Abs(A) * Math.Sqrt(1 + Math.Pow(Math.Abs(resultT), 2) + 2 * Math.Abs(resultT) * Math.Cos(2 * β * z - resultShan));
            return U;
        }

        /// <summary>
        /// 像横着的山的符号的算法
        /// </summary>
        /// <param name="TFirst"></param>
        /// <param name="TSecond"></param>
        /// <returns></returns>
        private static double CalculateShan(double TFirst, double TSecond)
        {
            if (TFirst >= 0 && TSecond >= 0)
                return Math.Atan(Math.Abs(TSecond) / Math.Abs(TFirst));

            if (TFirst < 0 && TSecond >= 0)
                return Math.Atan(Math.Abs(TSecond) / Math.Abs(TFirst)) + Math.PI / 2;

            if (TFirst < 0 && TSecond < 0)
                return Math.Atan(Math.Abs(TSecond) / Math.Abs(TFirst)) + Math.PI;

            if (TFirst >= 0 && TSecond < 0)
                return Math.Atan(Math.Abs(TSecond) / Math.Abs(TFirst)) + 3 * Math.PI / 2;
            return 0;
        }




        //private static float 山0;
        ///// <summary>
        ///// 二端口可变短路器
        ///// </summary>
        ///// <returns></returns>
        //public static double ErDuanKouKeBianDuanLuQi()
        //{

        //}







        /// <summary>
        /// 固定公式,计算贝特值
        /// </summary>
        /// <param name="f">频率范围[8.2,12.5]Ghz</param>
        /// <returns></returns>
        private static double Getβ()
        {
            double c = 3 * Math.Pow(10, 8);
            double ru = c / (F * Math.Pow(10, 9));
            double ruc = 2 * a;
            double squareOut = 2 * Math.PI / ru;
            double squareIn = 1 - Math.Pow(ru / ruc, 2);
            double 贝特 = squareOut * Math.Sqrt(squareIn);
            return 贝特;
        }
        private static double Yin_a(float l)
        {
            double topLeft = R * (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) + X * Math.Tan(β * l)));
            double topRight = R * (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) * Math.Tan(β * l - X) * Math.Tan(β * l)));
            double Top = topLeft + topRight;
            double down = Math.Pow((200 * (Math.Pow(R, 2) + Math.Pow(X, 2)) + X * Math.Tan(β * l)), 2) + Math.Pow(R, 2) * (Math.Pow(Math.Tan(β * l), 2));
            return Top / down;
        }

        private static double Yin_b(float l, float d)
        {
            double topLeft = (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) + X * Math.Tan(β * l)));
            double topRight = R * (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) * Math.Tan(β * l - X) * Math.Tan(β * l)));
            double Top = topLeft + topRight;
            double down = Math.Pow((200 * (Math.Pow(R, 2) + Math.Pow(X, 2)) + X * Math.Tan(β * l)), 2) + Math.Pow(R, 2) * (Math.Pow(Math.Tan(β * l), 2));
            return Top / down + Math.Tan(β * d);
        }

        /// <summary>
        /// 负载阻抗匹配的TL_a值
        /// </summary>
        private static double GetFZZKPPTL_a(float l, float d)
        {
            double yina = Yin_a(l);
            double yinb = Yin_b(l, d);
            double top = 1 - Math.Pow(yina, 2) - Math.Pow(yinb, 2);
            double down = Math.Pow((1 + yina), 2) + Math.Pow(yinb, 2);
            return top / down;
        }
        /// <summary>
        /// 负载阻抗匹配的TL_b值
        /// </summary>
        private static double GetFZZKPPTL_b(float l, float d)
        {
            double yina = Yin_a(l);
            double yinb = Yin_b(l, d);
            double top = -2 * yina * yinb;
            double down = Math.Pow((1 + yina), 2) + Math.Pow(yinb, 2);
            return top / down;
        }
        /// <summary>
        /// 负载阻抗匹配的TL值
        /// </summary>
        private static double GetFZZKPPTL(float l, float d)
        {
            return Math.Sqrt(Math.Pow(GetFZZKPPTL_a(l, d), 2) - Yin_b(l, d) * Math.Pow(GetFZZKPPTL_b(l, d), 2));
        }


        private static double GetTl_a()
        {
            double T1_aAddLeft = FA * Math.Cos(ShanA);
            double T1_aDivisionTop = Math.Pow(FB, 2) * (Math.Cos(2 * ShanB) * (1 + FC * Math.Cos(ShanC)) + (FC * Math.Sin(2 * ShanB) * Math.Sin(ShanC)));
            double T1_aDivisionDown = Math.Pow((1 + FC * Mathf.Cos(ShanC)), 2) + Math.Pow(FC * Math.Sin(ShanC), 2);
            double T1_a = T1_aAddLeft + T1_aDivisionTop / T1_aDivisionDown;
            return T1_a;
        }

        public static double GetTl_b()
        {
            double T1_bAddLeft = FA * Math.Sin(ShanA);
            double T1_bDivisionTop = Math.Pow(FB, 2) * (Math.Sin(2 * ShanB) * (1 + FC * Math.Cos(ShanC)) - FC * Math.Cos(2 * ShanB) * Math.Sin(ShanC));
            double T1_bDivisionDown = Math.Pow(1 + FC * Math.Cos(ShanC), 2) + Math.Pow(FC * Math.Sin(ShanC), 2);
            double T1_b = T1_bAddLeft + T1_bDivisionTop / T1_bDivisionDown;
            return T1_b;
        }

        private static double GetTl()
        {
            return Math.Sqrt(Math.Pow(GetTl_a(), 2) + Math.Pow(GetTl_b(), 2)); /*GetTL_a() + j*GetTL_b()*/
        }


        private static double GetTL_a()
        {
            double Tl_aDivisionTop = Math.Pow(R, 2) + (Math.Pow(X, 2) - 1);
            double T1_aDivisionDown = Math.Pow(R + 1, 2) + Math.Pow(X, 2);
            double T1_a = Tl_aDivisionTop / T1_aDivisionDown;
            return T1_a;
        }

        private static double GetTL_b()
        {
            double Tl_baDivisionTop = 2 * R * X;
            double T1_bDivisionDown = Math.Pow(R + 1, 2) + Math.Pow(X, 2);
            double T1_b = Tl_baDivisionTop / T1_bDivisionDown;
            return T1_b;
        }
        /// <summary>
        /// 计算负载阻抗测量的算法
        /// </summary>
        /// <returns></returns>
        private static double GetTL()
        {
            return Math.Sqrt(Math.Pow(GetTL_a(), 2) + Math.Pow(GetTL_b(), 2));
        }
    }
}