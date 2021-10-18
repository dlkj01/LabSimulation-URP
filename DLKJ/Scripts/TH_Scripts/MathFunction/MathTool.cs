using System;
using System.Collections.Generic;
using UnityEngine;
using Common;
namespace DLKJ
{
    /// <summary>
    /// 公式数据
    /// </summary>
    public struct FormulaData
    {
        public float A; /*= 10*///电压10mv-1000mv 初始化后不变
        public float F; /*= 8.2f*///频率8.2-12.5  初始化后不变
        public float δ;//控制衰减器取值[0,1] 初始化后不变
        public float distanceZ;//z的位置距离三厘米测量线最后侧距离
    }
    public class MathTool
    {
        public static LabReport1Data report1CorrectAnswer;
        public static LabReport2Data report2CorrectAnswer;
        public static LabReport3Data report3CorrectAnswer;
        private static double Calculateλp1()
        {
            double c = 3 * Math.Pow(10, 8);
            double ru = c / (F * Math.Pow(10, 9));
            double ruc = 2 * a;
            double λp1 = ru / Math.Sqrt((1 - Math.Pow(ru / ruc, 2)));
            return λp1;
        }
        /// <summary>
        /// 实验一可变短路器第一波节点位置lT1
        /// </summary>
        /// <returns></returns>
        public static double CorrectLT1()
        {
            double Shan1 = CalculateShan(GetTl_a(), GetTl_b());
            for (int i = 0; i < 20; i++)
            {
                double z = Shan1 * Calculateλp1() / (4 * Math.PI) + (i + 1) * (Calculateλp1() / 4);
                if (Math.Cos(2 * Getβ() * z - Shan1) == -1)
                {
                    double result = δ * Math.Abs(A) * (1 - Math.Abs(GetTl()));
                    Debug.Log(result);
                }
            }
            return 0;
        }

        //   private const float j = 1;
        private const float a = 0.02286f;
        public static float A { get; set; } /*= 10*///电压10mv-1000mv 初始化后不变
        public static float F { get; set; } /*= 8.2f*///频率8.2-12.5  初始化后不变
        public static float δ { get; set; }//控制衰减器取值[0,1] 初始化后不变
        public static float distanceZ { get; set; }//z的位置距离三厘米测量线最后侧距离
        private static double EDKKBDLQβ;//二端口+可变断路器的贝特
        private static float X = 0;//取值范围[-200,200] 初始化后不变
        private static float R = 0;//取值范围[0,200] 初始化后不变
        private static float ZL = 0;//ZL=R+jX
        public static float couplingFactorA;//耦合度输入端电压模A
        public static float couplingFactorC;//耦合度C
        public static void Init(/*FormulaData data*/)
        {
            //A = data.A;
            //F = data.F;
            //δ = data.δ;
            //distanceZ = data.distanceZ;
            RandomDataInit();
        }
        public static void RandomDataInit()
        {
            //F = UnityEngine.Random.Range(8.2f, 12.5f);
            //A = UnityEngine.Random.Range(2f, 2000f);
            //δ = UnityEngine.Random.Range(0f, 1f); //Random(0.00f, 1.00f);
            X = UnityEngine.Random.Range(-200f, 200f);
            R = UnityEngine.Random.Range(0f, 200f);
            ZL = R + X;
            FA = UnityEngine.Random.Range(0f, 1f);
            FC = FA;
            FB = Math.Sqrt(1 - Math.Pow(FA, 2));
            FD = UnityEngine.Random.Range(0f, 1f);
            do
            {
                Shan0 = UnityEngine.Random.Range(0f, 2f * Mathf.PI);
            } while (Shan0 <= 0 || Shan0 > 2 * Mathf.PI);
            Shan0 = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            RuDuanLuQi = UnityEngine.Random.Range(0.0024f, 0.0365f);
            ShanA = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            ShanC = ShanA;
            ShanB = 0.5f * (ShanA + ShanC + Math.PI);
            ShanD = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            EDKKBDLQβ = GetEDKKBDLQβ();
        }
        public static void Reset()
        {
            A = 0;
            F = 0;
            δ = 0;
            distanceZ = 0;
        }


        /// <summary>
        /// 二端口网络S参数测量
        /// </summary>
        /// <returns></returns>
        //public static double EDKWLSCSCL(float z)
        //{

        //}
        #region 方法
        #region 公式一&公式二、找等效截面
        /// <summary>
        /// 公式一、(1)
        /// 三厘米测量线终端直接接短路板(不接二端口网络时)
        /// </summary>
        /// <param name="z">当前测量位置与三厘米测量线最右端距离</param>
        /// <returns></returns>
        public static double SLMCLXZDDLB(float z)
        {
            double U = δ * Math.Abs(A) * Math.Abs((Math.Sin(Getβ() * z)));
            return U;
        }
        #endregion

        #region 实验一、二端口网络S参数测量()
        private static float FA;
        private static double FB;
        private static float FC;
        private static float FD;
        private static float ShanA;
        private static double ShanB;
        private static float ShanC;
        private static float ShanD;
        #endregion

        #region 实验一、接二端口和短路版
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
        #endregion

        #region 实验一、二端口和匹配负载
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
            double U = δ * Math.Abs(A) * Math.Sqrt(1 + Math.Abs(Math.Pow(T1, 2)) + 2 * Math.Abs(T1) * Math.Cos(2 * Getβ() * z - Shan1));
            return U;
        }
        #endregion

        #region 实验一、二端口可变短路器
        private static float Shan0;
        public static float RuDuanLuQi;
        /// <summary>
        /// 二端口可变短路器
        /// </summary>
        /// <param name="zd">可变断路器中波导长度</param>
        /// <param name="z">连通的矩形波导长度</param>
        /// <returns></returns>
        public static double ErDuanKouKeBianDuanLuQi(float zd, float z)
        {
            return FuZaiZuLiangKangHengFormula(GetT1_EDKKBDLQ(zd), CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)), z); ;
        }
        #endregion

        #region 实验二、负载阻抗测量
        /// <summary>
        /// 负载阻抗测量第一个公式
        /// </summary>
        /// <param name="z">旋转3厘米测量线的旋钮就是调节z的数值</param>
        /// <returns></returns>
        public static double FZZKCL_First(float z)
        {
            return FuZaiZuLiangKangHengFormula(GetTL(), CalculateShan(GetTL_a(), GetTL_b()), z);
        }
        #endregion

        #region 实验二、负载阻抗搭配
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
        #endregion

        #region 实验三、定向耦合器的耦合度测量
        /// <summary>
        /// 定向耦合器耦合度测量
        /// </summary>
        /// <returns></returns>
        public static double CouplingFactorObserved()
        {
            double U3 = Math.Abs(couplingFactorA) / Math.Sqrt(Math.Pow(10, couplingFactorC / 20));
            return U3;
        }
        #endregion

        #endregion

        #region 公式封装
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultT">长得像T的符号的结果,作为参数传递进公式</param>
        /// <param name="resultShan">长得像横着的山的符号的结果,作为参数传递进公式</param>
        /// <param name="z">变量</param>
        /// <returns></returns>
        private static double FuZaiZuLiangKangHengFormula(double resultT, double resultShan, float z)
        {
            double U = δ * Math.Abs(A) * Math.Sqrt(1 + Math.Pow(Math.Abs(resultT), 2) + 2 * Math.Abs(resultT) * Math.Cos(2 * Getβ() * z - resultShan));
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







        /// <summary>
        /// 二端口可变断路器T1_a算法
        /// </summary>
        /// <returns></returns>
        private static double GetTl_a_EDKKBDLQ(float zd)
        {
            double shanD = GetEDKKBDLQβ() * zd + Shan0;
            double addLeft = FA * Math.Cos(ShanA);
            double topLeft = Math.Pow(FB, 2) * Math.Cos(2 * ShanB + shanD) * (1 - FC * Math.Cos(ShanC + shanD));
            double topRight = Math.Pow(FB, 2) * FC * Math.Sin(2 * ShanB + shanD) * Math.Sin(ShanC + shanD);
            double downLeft = Math.Pow((1 - FC * Math.Cos(ShanC + shanD)), 2);
            double downRight = Math.Pow(FC, 2) * Math.Pow(Math.Sin(ShanC + shanD), 2);
            return addLeft + (topLeft - topRight) / (downLeft + downRight);
        }
        /// <summary>
        /// 二端口可变断路器T1_b算法
        /// </summary>
        /// <returns></returns>
        private static double GetTl_b_EDKKBDLQ(float zd)
        {
            double shanD = GetEDKKBDLQβ() * zd + Shan0;
            double addLeft = FA * Math.Sin(ShanA);
            double topLeft = Math.Pow(FB, 2) * Math.Sin(2 * ShanB + shanD) * (1 - FC * Math.Cos(ShanC + shanD));
            double topRight = Math.Pow(FB, 2) * FC * Math.Cos(2 * ShanB + shanD) * Math.Sin(ShanC + shanD);
            double downLeft = Math.Pow((1 - FC * Math.Cos(ShanC + shanD)), 2);
            double downRight = Math.Pow(FC, 2) * Math.Pow(Math.Sin(ShanC + shanD), 2);
            return addLeft + (topLeft + topRight) / (downLeft + downRight);
        }

        private static double GetT1_EDKKBDLQ(float zd)
        {
            return Math.Sqrt(Math.Pow(GetTl_a_EDKKBDLQ(zd), 2) + Math.Pow(GetTl_b_EDKKBDLQ(zd), 2));
        }

        private static double GetEDKKBDLQβ()
        {
            double ruc = 2 * a;
            double β = 2 * Math.PI / RuDuanLuQi * Math.Sqrt(1 - RuDuanLuQi / ruc);
            return β;
        }


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
            double topLeft = R * (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) + X * Math.Tan(Getβ() * l)));
            double topRight = R * (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) * Math.Tan(Getβ() * l - X) * Math.Tan(Getβ() * l)));
            double Top = topLeft + topRight;
            double down = Math.Pow((200 * (Math.Pow(R, 2) + Math.Pow(X, 2)) + X * Math.Tan(Getβ() * l)), 2) + Math.Pow(R, 2) * (Math.Pow(Math.Tan(Getβ() * l), 2));
            return Top / down;
        }

        private static double Yin_b(float l, float d)
        {
            double topLeft = (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) + X * Math.Tan(Getβ() * l)));
            double topRight = R * (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) * Math.Tan(Getβ() * l - X) * Math.Tan(Getβ() * l)));
            double Top = topLeft + topRight;
            double down = Math.Pow((200 * (Math.Pow(R, 2) + Math.Pow(X, 2)) + X * Math.Tan(Getβ() * l)), 2) + Math.Pow(R, 2) * (Math.Pow(Math.Tan(Getβ() * l), 2));
            return Top / down + Math.Tan(Getβ() * d);
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
            return Math.Sqrt(Math.Pow(GetTl_a(), 2) + Math.Pow(GetTl_b(), 2));
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
        #endregion
    }
}