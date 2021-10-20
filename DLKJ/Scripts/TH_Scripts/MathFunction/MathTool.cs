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
        public static LabReportCorrect1Data report1CorrectAnswer;
        public static LabReport2Data report2CorrectAnswer;
        public static LabReport3Data report3CorrectAnswer;

        //   private const float j = 1;
        private const float a = 22.86f;
        public const float Y0 = 0.01f;
        public static float A { get; set; } /*= 10*///电压10mv-1000mv 初始化后不变
        public static float F { get; set; } /*= 8.2f*///频率8.2-12.5  初始化后不变
        public static float δ { get; set; }//控制衰减器取值[0,1] 初始化后不变
        public static float distanceZ { get; set; }//z的位置距离三厘米测量线最后侧距离
        private static double EDKKBDLQβ;//二端口+可变断路器的贝特
        private static float X = 0;//取值范围[-200,200] 初始化后不变
        private static float R = 0;//取值范围[0,200] 初始化后不变
        private static float ZL = 0;//ZL=R+jX
        private static float j = 1;
        private static float Z0 = 100;//100欧姆
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
            F = UnityEngine.Random.Range(8.2f, 12.5f);
            A = UnityEngine.Random.Range(2f, 2000f);
            δ = UnityEngine.Random.Range(0f, 1f); //Random(0.00f, 1.00f);
            X = UnityEngine.Random.Range(-200f, 200f);
            R = UnityEngine.Random.Range(0f, 200f);
            ZL = R + j * X;
            //方案一
            //FA = UnityEngine.Random.Range(0f, 1f);
            //FB = UnityEngine.Random.Range(0f, 1f);
            //FC = UnityEngine.Random.Range(0f, 1f);

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
            //方案一
            //ShanA = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            //ShanC = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            //ShanB = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            //ShanD = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            ShanA = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            ShanC = ShanA;
            ShanB = 0.5f * (ShanA + ShanC + Math.PI);
            ShanD = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            EDKKBDLQβ = GetEDKKBDLQβ();
        }

        private static void FixedCorrectCalculate()
        {
            report1CorrectAnswer.SourceFrequency = F;//信号源频率
            report1CorrectAnswer.SourceVoltage = A;//电压
            report1CorrectAnswer.Attenuator = δ;//衰减器
            report1CorrectAnswer.EquivalentSectionPosition = 0;//第一个等效截面的位置
            report1CorrectAnswer.InputWavelength = Calculateλp1();//输入端波长
            report1CorrectAnswer.VariableShortCircuitFirstPos = GetlT1(1);//可变短路器第一波节点最小值位置
            report1CorrectAnswer.VariableShortCircuitSecondPos = GetlT1(2);//可变短路器第一波节点最小值位置
            report1CorrectAnswer.VariableWavelengthInShortCircuit = RuDuanLuQi;//可变短路器中波长λp2
            //开路负载位置
            report1CorrectAnswer.OpenLoadPosition = new List<double>();
            report1CorrectAnswer.OpenLoadPosition.Add(report1CorrectAnswer.VariableShortCircuitFirstPos + RuDuanLuQi / 4);
            report1CorrectAnswer.OpenLoadPosition.Add(report1CorrectAnswer.VariableShortCircuitSecondPos + RuDuanLuQi / 4);


            report1CorrectAnswer.PhaseAngleCircuit = CalculateShan(GetTl_a(), GetTl_b());//相角终端短路

        }
        public static void Reset()
        {
            A = 0;
            F = 0;
            δ = 0;
            distanceZ = 0;
        }
        #region 答案计算
        private static double Calculateλp1()
        {
            double c = 3 * Math.Pow(10, 8);
            double ru = c / (F * Math.Pow(10, 9));
            double ruc = 2 * a;
            double λp1 = ru / Math.Sqrt((1 - Math.Pow(ru / ruc, 2)));
            return λp1;
        }

        /// <summary>
        /// 獲取等效截面读数d的位置
        /// </summary>
        /// <param name="endValue">最大的值</param>
        /// <param name="step">步骤数量</param>
        /// <param name="startValue">初始值</param>
        /// <param name="func">获取最大值或者最小值的方法</param>
        /// <returns></returns>
        public static double GetDT(float endValue, int step, float startValue, Func<List<double>, double> func)
        {
            List<double> allResult = new List<double>();
            List<float> distanceList = new List<float>();
            float everyStepValue = (endValue - startValue) / step;
            for (float distance = startValue; distance <= endValue; distance += everyStepValue)
            {
                allResult.Add(Math.Abs(Math.Sin(Getβ() * distance)));
                distanceList.Add(distance);
            }
            double result = func(allResult);
            int index = allResult.FindIndex((p) => { return p == result; });
            Debug.Log("步数是:" + index + "---对应的值为:" + distanceList[index] + "---Sinβ*z=" + result + "---读数为" + SLMCLXZDDLB(distanceList[index]));
            return distanceList[index];
        }



        /// <summary>
        /// 等效截面位置,三厘米测量线终端接短路板时，从z=0开始向左，第一个使U读数达到最小值的位置
        /// </summary>
        /// <returns></returns>
        public static double GetdT(int n)
        {
            double Shan1 = CalculateShan(GetTl_a(), GetTl_b());
            double z = Shan1 * Calculateλp1() / (4 * Math.PI) + (n + 1) * (Calculateλp1() / 4);
            return z;
        }
        /// <summary>
        /// 可变短路器节点位置
        /// </summary>
        /// <param name="n">获取的第几个波段</param>
        /// <returns></returns>
        public static double GetlT1(int n)
        {
            int index = 1;//从第一个波段开始
            double Shan1 = CalculateShan(GetTl_a(), GetTl_b());
            double z = 0;
            for (int i = index; i < 10; i++)
            {
                z = Shan1 * Calculateλp1() / (4 * Math.PI) + (i + 1) * (Calculateλp1() / 4);
                if (Math.Cos(2 * Getβ() * z - Shan1) == -1)
                {
                    if (index == n)
                        return z;
                    index++;
                }
            }
            return z;
        }

        #endregion
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
        /// <summary>
        /// 获取二端口和短路版的最大读数
        /// </summary>
        /// <returns></returns>
        public static double GetMaxReadEDKDLB()
        {
            for (int i = 0; i < 10; i++)
            {
                double z = (CalculateShan(GetTl_a(), GetTl_b()) * Calculateλp1()) / (4 * Math.PI) + 2 * i * (Calculateλp1() / 4);
                double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTl_a(), GetTl_b()));
                if (result == 1)
                {
                    double Umax = δ * Math.Abs(A) * Math.Abs((1 + GetTl()));
                    return Umax;
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取二端口和短路版的最小读数
        /// </summary>
        /// <returns></returns>
        public static double GetMinReadEDKDLB()
        {
            for (int i = 0; i < 10; i++)
            {
                double z = (CalculateShan(GetTl_a(), GetTl_b()) * Calculateλp1()) / (4 * Math.PI) + (2 * i + 1) * (Calculateλp1() / 4);
                double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTl_a(), GetTl_b()));
                if (result == 1)
                {
                    double Umax = δ * Math.Abs(A) * Math.Abs((1 - GetTl()));
                    return Umax;
                }
            }
            return 0;
        }
        /// <summary>
        /// 驻波比
        /// </summary>
        /// <returns></returns>
        public static double SWREDKTODLB()
        {
            double result = GetMaxReadEDKDLB() / GetMinReadEDKDLB();
            return result;
        }
        /// <summary>
        /// 相角二端口连短路版
        /// </summary>
        /// <returns></returns>
        public static double PhaseAngleEDKTODLB()
        {
            double result = CalculateShan(GetTl_a(), GetTl_b());
            return result;
        }
        public static double GetΓ1S()
        {
            double result = GetTl();
            return result;
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
        /// <summary>
        /// 二端口匹配符在读数最大值
        /// </summary>
        /// <returns></returns>
        public static double GetMaxReadEDKPPFZ()
        {
            for (int i = 0; i < 10; i++)
            {
                double z = ShanA * Calculateλp1() / (4 * Math.PI) + 2 * i * (Calculateλp1() / 4);
                double result = Math.Cos(2 * Getβ() * z - ShanA);
                if (result == 1)
                {
                    double Umax = δ * Math.Abs(A) * Math.Abs((1 + FA));
                    return Umax;
                }
            }
            return 0;
        }
        /// <summary>
        /// 二端口匹配符在读数最小值
        /// </summary>
        /// <returns></returns>
        public static double GetMinReadEDKPPFZ()
        {
            for (int i = 0; i < 10; i++)
            {
                double z = ShanA * Calculateλp1() / (4 * Math.PI) + (2 * i + 1) * (Calculateλp1() / 4);
                double result = Math.Cos(2 * Getβ() * z - ShanA);
                if (result == 1)
                {
                    double min = δ * Math.Abs(A) * Math.Abs((1 - FA));
                    return min;
                }
            }
            return 0;
        }
        /// <summary>
        /// 驻波比
        /// </summary>
        /// <returns></returns>
        public static double SWREDKPPFZ()
        {
            double result = GetMaxReadEDKPPFZ() / GetMinReadEDKPPFZ();
            return result;
        }
        /// <summary>
        /// 相角二端口接匹配负载
        /// </summary>
        /// <returns></returns>
        public static double PhaseAngleSWREDKPPFZ()
        {
            double result = ShanA;
            return result;
        }
        public static double GetΓ1L()
        {
            double result = FA;
            return result;
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
        /// <summary>
        /// 获取二端口和可变短路器的最大读数
        /// </summary>
        /// <returns></returns>
        public static double GetMaxReadEDKKBDLQ(float zd)
        {
            for (int i = 0; i < 10; i++)
            {
                double z = CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)) * Calculateλp1() / (4 * Math.PI) + 2 * i * (Calculateλp1() / 4);
                double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)));
                if (result == 1)
                {
                    double Umax = δ * Math.Abs(A) * Math.Abs((1 + GetTl_a_EDKKBDLQ(zd)));
                    return Umax;
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取二端口和可变短路器的最小读数
        /// </summary>
        /// <returns></returns>
        public static double GetMinReadEDKKBDLQ(float zd)
        {
            for (int i = 0; i < 10; i++)
            {
                double z = CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)) * Calculateλp1() / (4 * Math.PI) + (2 * i + 1) * (Calculateλp1() / 4);
                double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)));
                if (result == 1)
                {
                    double Umin = δ * Math.Abs(A) * Math.Abs((1 - GetTl_a_EDKKBDLQ(zd)));
                    return Umin;
                }
            }
            return 0;
        }
        /// <summary>
        /// 驻波比
        /// </summary>
        /// <param name="zd"></param>
        /// <returns></returns>
        public static double SWREDKKBDLQ(float zd)
        {
            double result = GetMaxReadEDKKBDLQ(zd) / GetMinReadEDKKBDLQ(zd);
            return result;
        }

        /// <summary>
        /// 相角二端口和可变短路器
        /// </summary>
        /// <param name="zd">可变短路器在开路lT位置时</param>
        /// <returns></returns>
        public static double PhaseAngle(float zd)
        {
            double result = CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd));
            return result;
        }
        public static double GetΓ1O(float zd)
        {
            double result = GetT1_EDKKBDLQ(zd);
            return result;
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

        /// <summary>
        /// 获取最大读数
        /// </summary>
        /// <returns></returns>
        public static double GetMaxRead_FZZKCL()
        {
            for (int i = 0; i < 10; i++)
            {
                double z = (CalculateShan(GetTL_a(), GetTL_b()) * Calculateλp1()) / (4 * Math.PI) + 2 * i * (Calculateλp1() / 4);
                double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTL_a(), GetTL_b()));
                if (result == 1)
                {
                    double Umax = δ * Math.Abs(A) * Math.Abs((1 + GetTL()));
                    return Umax;
                }
            }
            return 0;
        }
        /// <summary>
        /// 获取最小读数
        /// </summary>
        /// <returns></returns>
        public static double GetMinRead_FZZKCL()
        {
            for (int i = 0; i < 10; i++)
            {
                double z = (CalculateShan(GetTL_a(), GetTL_b()) * Calculateλp1()) / (4 * Math.PI) + (2 * i + 1) * (Calculateλp1() / 4);
                double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTL_a(), GetTL_b()));
                if (result == -1)
                {
                    double Umin = δ * Math.Abs(A) * Math.Abs((1 - GetTL()));
                    return Umin;
                }
            }
            return 0;
        }
        /// <summary>
        /// 归一化负载阻抗
        /// </summary>
        /// <returns></returns>
        public static double NormalizedLoadImpedance()
        {
            double result = ZL / Z0;
            return result;
        }
        #endregion

        #region 实验二、负载阻抗匹配
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

        public static double GetMinReadFZKZPP(float l, float d)
        {
            for (int i = 0; i < 10; i++)
            {
                double z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculateλp1() / (4 * Math.PI) + (2 * i + 1) * (Calculateλp1() / 4);
                double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)));
                if (result == -1)
                {
                    double min = δ * Math.Abs(A) * (1 - GetFZZKPPTL(l, d));
                    return min;
                }
            }
            return 0;
        }
        public static double GetMaxReadFZKZPP(float l, float d)
        {
            for (int i = 0; i < 10; i++)
            {
                double z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculateλp1() / (4 * Math.PI) + (2 * i) * (Calculateλp1() / 4);
                double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)));
                if (result == -1)
                {
                    double min = δ * Math.Abs(A) * (1 + GetFZZKPPTL(l, d));
                    return min;
                }
            }
            return 0;
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
            double topLeft = R * (Y0 * (Math.Pow(R, 2) + Math.Pow(X, 2)) + X * Math.Tan(Getβ() * l));
            double topRight = R * (Y0 * (Math.Pow(R, 2) + Math.Pow(X, 2) * Math.Tan(Getβ() * l) - X) * Math.Tan(Getβ() * l));
            double Top = topLeft + topRight;
            double Down = Math.Pow((Y0 * (Math.Pow(R, 2) + Math.Pow(X, 2)) + X * Math.Tan(Getβ() * l)), 2) + Math.Pow(R, 2) * (Math.Pow(Math.Tan(Getβ() * l), 2));
            double result = Y0 * (Top / Down);
            return result;
        }

        private static double Yin_b(float l, float d)
        {
            double RPow = Math.Pow(R, 2);
            double Tanβl = Math.Tan(Getβ() * l);
            double topLeft = Y0 * (RPow + Math.Pow(X, 2)) * Tanβl - X;
            double topRight = Y0 * (RPow + Math.Pow(X, 2)) + X * Tanβl - RPow * Tanβl;
            double Top = topLeft + topRight;
            double down = Math.Pow(Y0 * (RPow + X * Tanβl), 2) + RPow * Math.Pow(Tanβl, 2);
            double right = Math.Cos(Getβ() * d) / Math.Sin(Getβ() * d);
            double result = Y0 * (Top / down) - right;
            return result;
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

        public static double GetMin(List<double> result)
        {
            double min = result[0];
            for (int i = 1; i < result.Count; i++)
            {
                if (min - result[i] > 0)
                    min = result[i];
            }
            return min;
        }

        public static double GetMax(List<double> result)
        {
            double max = result[0];
            for (int i = 1; i < result.Count; i++)
            {
                if (result[i] - max > 0)
                    max = result[i];
            }
            return max;
        }
    }
}