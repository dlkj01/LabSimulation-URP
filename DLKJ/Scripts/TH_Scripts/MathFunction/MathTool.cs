using System;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Common;
using Excel.Log;
using UnityEditor;

namespace DLKJ
{
    public struct InitValue
    {
        public bool initF;
        public bool initδ;
        public bool initA;
    }
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
    public struct AnswerCheck
    {
        public string answer;
        public bool isRight;
    }
    public class MathTool
    {
        public const float SLMCL_Start_Value = 0.055f;
        public static LabReportCorrect1Data report1CorrectAnswer = new LabReportCorrect1Data();
        public static LabReportCorrect2Data report2CorrectAnswer = new LabReportCorrect2Data();
        public static LabReport3Data report3CorrectAnswer = new LabReport3Data();
        //   private const float j = 1;
        private const float a = 0.02286f;
        public const float Y0 = 0.01f;
        public static float A { get; set; } /*= 10*///电压10mv-1000mv 初始化后不变
        public static float F { get; set; } /*= 8.2f*///频率8.2-12.5  初始化后不变
        public static float δ { get; set; }//控制衰减器取值[0,1] 初始化后不变
        public static float distanceZ { get; set; }//z的位置距离三厘米测量线最后侧距离
        private static double EDKKBDLQβ;//二端口+可变断路器的贝特
        public static float X = 0;//电抗 取值范围[-200,200] 初始化后不变
        public static float R = 0;//电阻 取值范围[0,200] 初始化后不变
        public static double verify = 0;
        private static double ZL = 0;//ZL=R+jX
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

        static Complex S11Com;
        static Complex S12Com;
        static Complex S22Com;
        static Complex ZLCom;
        public static void RandomDataInit()
        {
            //F = UnityEngine.Random.Range(8.2f, 12.5f);
            //A = UnityEngine.Random.Range(2f, 2000f);
            // δ = 1; //Random(0.00f, 1.00f);


            // 方案一
            FB =/* 0.4f*/ UnityEngine.Random.Range(0f, 1f);
            FA =/* 0.2f*/ UnityEngine.Random.Range(0f, Mathf.Sqrt(1 - Mathf.Pow(float.Parse(FB.ToString()), 2)));
            FC =/* 0.1f*/ UnityEngine.Random.Range(0f, Mathf.Sqrt(1 - Mathf.Pow(float.Parse(FB.ToString()), 2)));
            // 方案二
            //FA = UnityEngine.Random.Range(0f, 1f);
            //FC = FA;
            //FB = Math.Sqrt(1 - Math.Pow(FA, 2));
            //do
            //{
            //    Shan0 = UnityEngine.Random.Range(0f, 0.25f * Mathf.PI);
            //} while (Shan0 <= 0 || Shan0 > 0.25f * Mathf.PI);
            Shan0 = 0;
            //FD = Math.Abs(Math.Cos(2 * GetEDKKBDLQβ() * zd - (Math.PI - Shan0)));
            RuDuanLuQi = /*0.03f*/ UnityEngine.Random.Range(0.024f, 0.0365f);
            //   RuDuanLuQi = 0.024f;
            //方案一
            ShanA =/* 0.5f*/ UnityEngine.Random.Range(0, 0.25f * Mathf.PI);
            ShanC = /*0.4f*/ UnityEngine.Random.Range(0, 0.25f * Mathf.PI);
            ShanB = /*0.3f*/ UnityEngine.Random.Range(0, 0.25f * Mathf.PI);

            // 方案二
            //ShanA = UnityEngine.Random.Range(0, 0.5f * Mathf.PI);
            //ShanC = UnityEngine.Random.Range(0, 0.5f * Mathf.PI);
            //ShanB = 0.5f * (ShanA + ShanC + Math.PI);

            EDKKBDLQβ = GetEDKKBDLQβ();
            S11Com = new Complex(FA * Math.Cos(ShanA), FA * Math.Sin(ShanA));
            S12Com = new Complex(FB * Math.Cos(ShanB), FB * Math.Sin(ShanB));
            S22Com = new Complex(FC * Math.Cos(ShanC), FC * Math.Sin(ShanC));

            List<double> lList = CalculateL();
            while (lList[1] < 2.1f || lList[0] > 14.8f)
            {
                CalculateL();
            }
            Debug.Log("R:" + R + "  X:" + X);
            double XRPow = Math.Pow(R, 2) + Math.Pow(X, 2);
            verify = GetVerify(XRPow);

            while (verify < 0)
            {
                verify = GetVerify(XRPow);
            }

            ZLCom = new Complex(R, X);
            ZL = ZLCom.Real + ZLCom.Imaginary;
            couplingFactorC = UnityEngine.Random.Range(5, 20);
        }

        static double GetVerify(double XRPow)
        {
            double targetValue = Math.Pow(2 * X * XRPow * Y0, 2) - 4 * XRPow * (1 - R * Y0) * (Math.Pow(Y0, 2) * Math.Pow(XRPow, 4) - Math.Pow(R, 3) * Y0 - R * Y0 * Math.Pow(X, 2));

            List<double> lList = CalculateL();
            //List0是最大值，条件为小于 匹配螺钉l的最大值
            while (R * Y0 == 1 || lList[1] < 2.1f || lList[0] > 14.8f)
            {

                X = UnityEngine.Random.Range(-200f, 200f);
                R = UnityEngine.Random.Range(0f, 200f);
            }
            return targetValue;
        }

        #region 第一个实验正确计算的答案
        public static void FixedCorrect1Calculate()
        {
            report1CorrectAnswer.SourceFrequency = F;//信号源频率
            report1CorrectAnswer.SourceVoltage = A;//电压
            report1CorrectAnswer.Attenuator = δ;//衰减器
            report1CorrectAnswer.EquivalentSectionPosition = GetDT(SLMCL_Start_Value, 0);//第一个等效截面的位置
            report1CorrectAnswer.InputWavelength = Calculateλp1();//输入端波长
            report1CorrectAnswer.WaveNodePosShortCircuit = GetMinZUpperDTEDKDLB();//波节点位置终端短路

            report1CorrectAnswer.VariableShortCircuitFirstPos = GetFirstMinBoundKBDLQ();//可变短路器第一波节点最小值位置
            for (int i = 0; i < report1CorrectAnswer.VariableShortCircuitFirstPos.Count; i++)
            {
                double T1 = GetT1_EDKKBDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos[i].ToString()));
                double shan = CalculateShan(GetTl_a_EDKKBDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos[i].ToString())), GetTl_b_EDKKBDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos[i].ToString())));
                double readResult = δ * Math.Abs(A) * Math.Sqrt(1 + Math.Pow(T1, 2) + 2 * Math.Abs(T1) * Math.Cos(2 * Getβ() * report1CorrectAnswer.WaveNodePosShortCircuit - shan));
                Debug.Log(readResult);
            }

            // 可变短路器第二波节点最小值位置
            report1CorrectAnswer.VariableShortCircuitSecondPos = new List<double>();
            for (int i = 0; i < report1CorrectAnswer.VariableShortCircuitFirstPos.Count; i++)
            {
                report1CorrectAnswer.VariableShortCircuitSecondPos.Add(report1CorrectAnswer.VariableShortCircuitFirstPos[i] + 0.5f * RuDuanLuQi);
            }

            report1CorrectAnswer.VariableWavelengthInShortCircuit = RuDuanLuQi;//可变短路器中波长λp2

            //开路负载位置
            report1CorrectAnswer.OpenLoadPosition = new List<double>();
            for (int i = 0; i < report1CorrectAnswer.VariableShortCircuitFirstPos.Count; i++)
            {
                report1CorrectAnswer.OpenLoadPosition.Add(report1CorrectAnswer.VariableShortCircuitFirstPos[i] + RuDuanLuQi * 0.25f);
                report1CorrectAnswer.OpenLoadPosition.Add(report1CorrectAnswer.VariableShortCircuitSecondPos[i] + RuDuanLuQi * 0.25f);
            }


            //波节点位置终端开路
            report1CorrectAnswer.WaveNodePosShortTerminal = GetMinReadUpperDTEDKKEDLQ(float.Parse(report1CorrectAnswer.OpenLoadPosition[0].ToString()));
            report1CorrectAnswer.WaveNodePosShortMatching = GetMinZUpperDTEDKPPFZ();//波节点位置终端匹配
            report1CorrectAnswer.PhaseAngleCircuit = CalculateShan(GetTl_a(), GetTl_b());//相角终端短路
            report1CorrectAnswer.PhaseAngleTerminal = CalculateShan(GetTl_a_EDKKBDLQ(float.Parse(report1CorrectAnswer.OpenLoadPosition[0].ToString()))
                , GetTl_b_EDKKBDLQ(float.Parse(report1CorrectAnswer.OpenLoadPosition[0].ToString())));//相角终端开路
            report1CorrectAnswer.PhaseAngleMatching = ShanA;//相角终端匹配

            report1CorrectAnswer.StandingWaveRatioCircuit = SWREDKTODLB();//驻波比终端短路
            report1CorrectAnswer.StandingWaveRatioTerminal = SWREDKKBDLQ(float.Parse(report1CorrectAnswer.OpenLoadPosition[0].ToString("#0.0000000")));//驻波比终端开路
            report1CorrectAnswer.StandingWaveRatioMatching = SWREDKPPFZ();//驻波比终端匹配

            report1CorrectAnswer.inputΓ1S = GetTl();
            report1CorrectAnswer.inputΓ10 = GetT1_EDKKBDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos[0].ToString()));
            report1CorrectAnswer.inputΓ1L = FA;



            //report1CorrectAnswer.ReflectionCoefficientΓ1S = S11 - (Math.Pow(S12, 2) / (1 + S22));//反射系数T1S
            //report1CorrectAnswer.ReflectionCoefficientΓ10 = S11 + Math.Pow(S12, 2) / (1 - S22);//反射系数T10
            //report1CorrectAnswer.ReflectionCoefficientΓ1L = S11;//反射系数T1L

            //学生算的反射系数
            //double shanD = 0;
            //double addLeft = FA * Math.Cos(ShanA);
            //double topLeft = Math.Pow(FB, 2) * Math.Cos(2 * ShanB + shanD) * (1 - FC * Math.Cos(ShanC + shanD));
            //double topRight = Math.Pow(FB, 2) * FC * Math.Sin(2 * ShanB + shanD) * Math.Sin(ShanC + shanD);
            //double downLeft = Math.Pow((1 - FC * Math.Cos(ShanC + shanD)), 2);
            //double downRight = Math.Pow(FC, 2) * Math.Pow(Math.Sin(ShanC + shanD), 2);
            //double result = addLeft + (topLeft - topRight) / (downLeft + downRight);

            //double shanD2 = 0;
            //double addLeft2 = FA * Math.Sin(ShanA);
            //double topLeft2 = Math.Pow(FB, 2) * Math.Sin(2 * ShanB + shanD) * (1 - FC * Math.Cos(ShanC + shanD));
            //double topRight2 = Math.Pow(FB, 2) * FC * Math.Cos(2 * ShanB + shanD) * Math.Sin(ShanC + shanD);
            //double downLeft2 = Math.Pow((1 - FC * Math.Cos(ShanC + shanD)), 2);
            //double downRight2 = Math.Pow(FC, 2) * Math.Pow(Math.Sin(ShanC + shanD), 2);
            //double result2 = addLeft2 + (topLeft2 + topRight2) / (downLeft2 + downRight2);

            //Complex Student1S = new Complex(GetTl_a(), GetTl_b());
            //Complex Student10 = new Complex(result, result2);
            //Complex Student1L = S11Com;

            Complex Γ1S = S11Com - Complex.Pow(S12Com, 2) / (1 + S22Com);//反射系数T1S
            Complex Γ10 = S11Com + Complex.Pow(S12Com, 2) / (1 - S22Com);//反射系数T1S
            Complex Γ1L = S11Com;//反射系数T1S
            report1CorrectAnswer.ReflectionCoefficientΓ1SReal = Γ1S.Real;
            report1CorrectAnswer.ReflectionCoefficientΓ10Real = Γ10.Real;
            report1CorrectAnswer.ReflectionCoefficientΓ1LReal = Γ1L.Real;
            report1CorrectAnswer.ReflectionCoefficientΓ1SImaginary = Γ1S.Imaginary;
            report1CorrectAnswer.ReflectionCoefficientΓ10Imaginary = Γ10.Imaginary;
            report1CorrectAnswer.ReflectionCoefficientΓ1LImaginary = Γ1L.Imaginary;

            //Debug.Log("实部的值是" + S11Com.Real + "虚部的值是:" + S11Com.Imaginary);
            //Complex S11 = Γ1L/*(Γ10 + Γ1S) / (2 + Γ10 - Γ1S)*/;
            //Debug.Log("实部的值是" + S22Com.Real + "虚部的值是:" + S22Com.Imaginary);
            //Complex S22 = (2 * Γ1L - Γ1S - Γ10) / (Γ1S - Γ10);
            //Complex S12ComPow = Complex.Pow(S12Com, 2);
            //Complex S12SQRT = Complex.Sqrt(S12ComPow);
            //Debug.Log("实部的值是" + S12ComPow.Real + "虚部的值是:" + S12ComPow.Imaginary);
            //Complex S12Pow = (S22Com + 1) * (Γ1L - Γ1S);
            //Complex S12PowThird = Complex.Sqrt((1 - S22Com) * (Γ10 - Γ1L));
            report1CorrectAnswer.inputS11Real = S11Com.Real;
            report1CorrectAnswer.inputS11Imaginary = S11Com.Imaginary;
            report1CorrectAnswer.inputS12S21Real = S12Com.Real;
            report1CorrectAnswer.inputS12S21Imaginary = S12Com.Imaginary;
            report1CorrectAnswer.inputS22Real = S22Com.Real;
            report1CorrectAnswer.inputS22Imaginary = S22Com.Imaginary;

        }
        #endregion
        #region 第二个实验第一组正确计算的答案 负载阻抗测量
        public static void FixedCorrect2FirstGroupCalculate()
        {
            report2CorrectAnswer.inputSourceFrequencyFirst = F;//信号源频率
            report2CorrectAnswer.inputSourceVoltageFirst = A;//电压
            report2CorrectAnswer.inputAttenuatorSetupFirst = δ;//衰减器
            report2CorrectAnswer.SWRFirst = SWRFZZKCL();
            report2CorrectAnswer.WaveguideWavelengthFirst = Calculateλp1();
            report2CorrectAnswer.EquivalentSectionPositionFirst = GetDT(SLMCL_Start_Value, 0);//第一个等效截面的位置
            report2CorrectAnswer.MinimumVoltage = GetMinRead_FZZKCL();
            report2CorrectAnswer.MaximumVoltage = GetMaxRead_FZZKCL();
            report2CorrectAnswer.WaveNodePositionFirst = GetMinZUpperDTFZZKCL();
            //report2CorrectAnswer.NormalizedLoadImpedanceFirst = NormalizedLoadImpedance();
            Complex com = ZLCom / Z0;
            report2CorrectAnswer.NormalizedLoadImpedanceFirstReal = com.Real;//负载阻抗实部
            report2CorrectAnswer.NormalizedLoadImpedanceFirstImaginary = com.Imaginary;//负载阻抗虚部
            report2CorrectAnswer.LoadImpedanceFirstReal = ZLCom.Real;
            report2CorrectAnswer.LoadImpedanceFirstImaginary = ZLCom.Imaginary;
            report2CorrectAnswer.ScrewPositionFirst = CalculateL();
            report2CorrectAnswer.ScrewDepthFirst = CalculateD();
            report2CorrectAnswer.MinimumVoltageAfterMatchingFirst = GetMinReadFZKZPP(float.Parse(report2CorrectAnswer.ScrewPositionFirst[0].ToString()), float.Parse(report2CorrectAnswer.ScrewDepthFirst[0].ToString()));
            report2CorrectAnswer.MaximumVoltageAfterMatchingFirst = GetMaxReadFZKZPP(float.Parse(report2CorrectAnswer.ScrewPositionFirst[0].ToString()), float.Parse(report2CorrectAnswer.ScrewDepthFirst[0].ToString()));
            report2CorrectAnswer.SWRAfterMatchingFirst = report2CorrectAnswer.MaximumVoltageAfterMatchingFirst / report2CorrectAnswer.MinimumVoltageAfterMatchingFirst;
        }
        #endregion
        //#region 第二个实验第二组正确答案
        //public static void FixedCorrect2SecondGroupCalculate()
        //{
        //    report2CorrectAnswer.inputSourceFrequencySecond = F;//信号源频率
        //    report2CorrectAnswer.inputSourceVoltageSecond = A;//电压
        //    report2CorrectAnswer.inputAttenuatorSetupSecond = δ;//衰减器
        //    report2CorrectAnswer.SWRSecond = SWRFZZKCL();
        //    report2CorrectAnswer.WaveguideWavelengthSecond = Calculateλp1();
        //    report2CorrectAnswer.EquivalentSectionPositionSecond = GetDT(SLMCL_Start_Value, 0);//第一个等效截面的位置
        //    report2CorrectAnswer.WaveNodePositionSecond = GetMinZUpperDTFZZKCL();//第一个波节点位置
        //    report2CorrectAnswer.NormalizedLoadImpedanceSecond = NormalizedLoadImpedance();//负载阻抗归一化
        //    report2CorrectAnswer.LoadImpedanceSecond = ZL;//负载阻抗
        //    report2CorrectAnswer.ScrewPositionSecond = CalculateL();//螺钉位置
        //    report2CorrectAnswer.ScrewDepthSecond = CalculateD();//螺钉深度
        //    report2CorrectAnswer.MinimumVoltageAfterMatchingSecond = GetMinReadFZKZPP(float.Parse(report2CorrectAnswer.ScrewPositionFirst[0].ToString()), float.Parse(report2CorrectAnswer.ScrewDepthFirst[0].ToString()));
        //    report2CorrectAnswer.MinimumVoltageAfterMatchingSecond = GetMaxReadFZKZPP(float.Parse(report2CorrectAnswer.ScrewPositionFirst[0].ToString()), float.Parse(report2CorrectAnswer.ScrewDepthFirst[0].ToString()));
        //    report2CorrectAnswer.SWRAfterMatchingSecond = report2CorrectAnswer.MaximumVoltageAfterMatchingFirst / report2CorrectAnswer.MinimumVoltageAfterMatchingFirst;
        //}
        //#endregion

        #region 第三个实验正确答案计算
        public static void FixedCorrect3Calculate()
        {
            report3CorrectAnswer.inputSourceFrequency = F;
            report3CorrectAnswer.inputSourceVoltage = A;
            report3CorrectAnswer.OnePortVoltage = OnePortVoltage();
            report3CorrectAnswer.ThreePortVoltage = CouplingFactorObserved();
            report3CorrectAnswer.CouplingFactor = couplingFactorC;
        }
        #endregion
        public static void Reset()
        {
            A = 0;
            F = 0;
            δ = 0;
            distanceZ = 0;
        }
        #region 答案计算
        public static double Calculateλp1()
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
        public static double GetDT(float startValue, float offect /*float endValue, int step, float startValue, Func<List<double>, double> func*/)
        {
            double everyBandLength = Calculateλp1();//每一个波段的长度
            int index = 0;
            while (everyBandLength + (everyBandLength * 0.5f) * index < startValue)
            {
                index++;
            }
            // int startBandCount = (int)((startValue + offect) / everyBandLength) + 1;
            double result = everyBandLength + (everyBandLength * 0.5f) * index;
            return result;
            // Debug.Log(SLMCLXZDDLB(float.Parse(result.ToString("#0.0000000"))));

            //List<double> allResult = new List<double>();
            //List<float> distanceList = new List<float>();
            //float everyStepValue = (endValue - startValue) / step;
            //for (float distance = startValue; distance <= endValue; distance += everyStepValue)
            //{
            //    allResult.Add(Math.Abs(Math.Sin(Getβ() * distance)));
            //    distanceList.Add(distance);
            //}
            //double result = func(allResult);
            //int index = allResult.FindIndex((p) => { return p == result; });
            //Debug.Log("步数是:" + index + "---对应的值为:" + distanceList[index] + "---Sinβ*z=" + result + "---读数为" + SLMCLXZDDLB(distanceList[index]));
            //return distanceList[index];
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
                z = Shan1 * Calculateλp1() / (4 * Math.PI) + (i) * (Calculateλp1() / 4);
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
            double z = (CalculateShan(GetTl_a(), GetTl_b()) * Calculateλp1()) / (4 * Math.PI) + 2 * (Calculateλp1() / 4);
            double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTl_a(), GetTl_b()));
            if (result == 1)
            {
                double Umax = δ * Math.Abs(A) * Math.Abs((1 + GetTl()));
                return Umax;
            }

            return 0;
        }
        /// <summary>
        /// 获取二端口和短路版的最小读数
        /// </summary>
        /// <returns></returns>
        public static double GetMinReadEDKDLB()
        {
            double z = (CalculateShan(GetTl_a(), GetTl_b()) * Calculateλp1()) / (4 * Math.PI) + (2 + 1) * (Calculateλp1() / 4);
            double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTl_a(), GetTl_b()));
            if (result == -1)
            {
                double min = δ * Math.Abs(A) * Math.Abs((1 - GetTl()));
                return min;
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
        public static double GetMinZUpperDTEDKDLB()
        {
            double shan1 = CalculateShan(GetTl_a(), GetTl_b());
            double z = GetMinRead(shan1);
            double min = δ * Math.Abs(A) * Math.Abs((1 - GetTl()));
            return z;
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
        public static double GetMaxZEDKPPFZ(int i)
        {
            double z = ShanA * Calculateλp1() / (4 * Math.PI) + 2 * i * (Calculateλp1() / 4);
            return z;
        }
        public static double GetMinZEDKPPFZ(int i)
        {
            double z = ShanA * Calculateλp1() / (4 * Math.PI) + (2 * i + 1) * (Calculateλp1() / 4);
            return z;
        }
        /// <summary>
        /// 二端口匹配符在读数最大值
        /// </summary>
        /// <returns></returns>
        public static double GetMaxReadEDKPPFZ()
        {
            double z = GetMaxZEDKPPFZ(1);
            double result = Math.Cos(2 * Getβ() * z - ShanA);
            double maxRead = δ * Math.Abs(A) * Math.Abs((1 + FA));
            return maxRead;
        }

        /// <summary>
        /// 二端口匹配符在读数最小值
        /// </summary>
        /// <returns></returns>
        public static double GetMinReadEDKPPFZ()
        {
            double z = GetMinZEDKPPFZ(1);
            double result = Math.Cos(2 * Getβ() * z - ShanA);
            double minRead = δ * Math.Abs(A) * Math.Abs((1 - FA));
            return minRead;
        }
        /// <summary>
        /// 二端口匹配符在读数最小值
        /// </summary>
        /// <returns></returns>
        public static double GetMinZUpperDTEDKPPFZ()
        {
            double shan1 = ShanA;
            double z = GetMinRead(shan1);
            double min = δ * Math.Abs(A) * Math.Abs((1 - FA));
            return z;
            //double DT = GetDT(0.055f, 0);
            //int i = 0;
            //while (GetMinZEDKPPFZ(i) < DT)
            //{
            //    i++;
            //}
            //double z = GetMinZEDKPPFZ(i);
            //double result = Math.Cos(2 * Getβ() * z - ShanA);
            //if (result == -1)
            //{
            //    double min = δ * Math.Abs(A) * Math.Abs((1 - FA));
            //    return min;
            //}
            //return 0;
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
        /// 二端口可变短路器读数测试
        /// </summary>
        /// <param name="zd"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double ErDuanKouKeBianDuanLuQiNew(float zd, float z)
        {
            return KeBianDuanLuQiX(GetT1_EDKKBDLQ(zd), CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)), zd, z);
        }

        /// <summary>
        /// 获取二端口和可变短路器的最大读数
        /// </summary>
        /// <returns></returns>
        public static double GetMaxReadEDKKBDLQ(float zd)
        {
            double z = CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)) * Calculateλp1() / (4 * Math.PI) + 2 * (Calculateλp1() / 4);
            double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)));
            if (result == 1)
            {
                double T1 = GetT1_EDKKBDLQ(zd);
                double Umax = δ * Math.Abs(A) * Math.Abs(1 + T1);
                return Umax;
            }
            return 0;
        }
        /// <summary>
        /// 获取二端口和可变短路器的最小读数
        /// </summary>
        /// <returns></returns>
        public static double GetMinReadEDKKBDLQ(float zd)
        {
            double z = CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)) * Calculateλp1() / (4 * Math.PI) + (2 + 1) * (Calculateλp1() / 4);
            double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)));
            if (result == -1)
            {
                double T1 = GetT1_EDKKBDLQ(zd);
                double Umin = δ * Math.Abs(A) * Math.Abs(1 - T1);
                return Umin;
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
        public static double GetMinReadUpperDTEDKKEDLQ(float zd)
        {
            double shan = CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd));
            double z = GetMinReadEDKKBDLQ(shan, zd);
            //double z = GetMinRead(shan);
            double min = δ * Math.Abs(A) * Math.Abs(1 - GetT1_EDKKBDLQ(zd));
            return z;
        }

        /// <summary>
        /// 获取可变短路器第N个波段最小值
        /// </summary>
        /// <returns></returns>
        public static List<double> GetFirstMinBoundKBDLQ()
        {
            List<double> result = new List<double>();
            //int k = 0;
            //while (((2 * k + 1) * Math.PI - Shan0) * (RuDuanLuQi / (4 * Math.PI)) <= 0.1f)
            //{
            //    double value = ((2 * k + 1) * Math.PI - Shan0) * (RuDuanLuQi / (4 * Math.PI));
            //    if (value >= 0f)
            //        result.Add(value);
            //    k++;
            //}

            int k = 0;
            while (k * RuDuanLuQi / 2 <= 0.1f)
            {
                double value = k * RuDuanLuQi / 2;
                if (value > 0f)
                    result.Add(value);
                k++;
            }

            //while ((Math.PI - Shan0) * RuDuanLuQi + k * RuDuanLuQi * 0.5f <= 0.1f)
            //{
            //    double value = (Math.PI - Shan0) * RuDuanLuQi + k * RuDuanLuQi * 0.5f;
            //    if (value >= 0f)
            //        result.Add(value);
            //    k++;
            //}
            //int k = -2;
            //while (RuDuanLuQi * ((Math.PI - Shan0) / (4 * Math.PI)) + k * 0.5f * RuDuanLuQi <= 0.1f)
            //{
            //    float value = float.Parse((RuDuanLuQi * ((Math.PI - Shan0) / (4 * Math.PI)) + k * 0.5f * RuDuanLuQi).ToString());
            //    if (value > 0)
            //    {
            //        result.Add(value);
            //    }
            //    k++;
            //}
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
            double z = (CalculateShan(GetTL_a(), GetTL_b()) * Calculateλp1()) / (4 * Math.PI) + 2 * (Calculateλp1() * 0.25f);
            double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTL_a(), GetTL_b()));
            double Umax = δ * Math.Abs(A) * Math.Abs((1 + GetTL()));
            return Umax;
        }
        /// <summary>
        /// 获取最小读数
        /// </summary>
        /// <returns></returns>
        public static double GetMinRead_FZZKCL()
        {
            double z = (CalculateShan(GetTL_a(), GetTL_b()) * Calculateλp1()) / (4 * Math.PI) + (2 + 1) * (Calculateλp1() * 0.25f);
            double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetTL_a(), GetTL_b()));
            double Umin = δ * Math.Abs(A) * Math.Abs((1 - GetTL()));
            return Umin;
        }
        /// <summary>
        /// 负载阻抗测量驻波比
        /// </summary>
        /// <returns></returns>
        public static double SWRFZZKCL()
        {
            double result = GetMaxRead_FZZKCL() / GetMinRead_FZZKCL();
            return result;
        }

        /// <summary>
        /// 第一波节点位置lmin负载阻抗测量
        /// </summary>
        /// <returns></returns>
        public static double GetMinZUpperDTFZZKCL()
        {
            double shan1 = CalculateShan(GetTL_a(), GetTL_b());
            double z = GetMinRead(shan1);
            double min = δ * Math.Abs(A) * Math.Abs((1 - GetTl()));
            return z;
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

        /// <summary>
        /// 计算l的值
        /// </summary>
        /// <returns></returns>
        public static List<double> CalculateL()
        {
            X = UnityEngine.Random.Range(-200f, 200f);
            R = UnityEngine.Random.Range(0f, 200f);

            double RX2 = Math.Pow(R, 2) + Math.Pow(X, 2);
            double topLeft = -2 * X * RX2 * Y0;
            double topRight = Math.Sqrt(Math.Pow(2 * X * RX2 * Y0, 2) - 4 * RX2 * (1 - R * Y0) * (Math.Pow(Y0, 2) * Math.Pow(RX2, 2) - Math.Pow(R, 3) * Y0 - R * Y0 * Math.Pow(X, 2)));
            double down = 2 * RX2 * (1 - R * Y0);
            double resultAdd = Math.Atan((topLeft + topRight) / down) / Getβ();
            double resultSub = Math.Atan((topLeft - topRight) / down) / Getβ();
            int k = 0;
            int p = 0;
            double c = 3 * Math.Pow(10, 8);
            double ru = c / (F * Math.Pow(10, 9));
            while (resultAdd < 0)
            {
                k++;
                resultAdd += 0.5f * k * Calculateλp1();
            }
            while (resultSub < 0)
            {
                p++;
                resultSub += 0.5f * p * Calculateλp1();
            }
            List<double> result = new List<double>();
            result.Add(resultAdd);
            result.Add(resultSub);
            return result;
        }

        /// <summary>
        /// 计算d的值
        /// </summary>
        /// <returns></returns>
        public static List<double> CalculateD()
        {
            List<double> L = CalculateL();
            double RXPOW = Math.Pow(R, 2) + Math.Pow(X, 2);
            List<double> result = new List<double>();
            for (int i = 0; i < L.Count; i++)
            {
                double topLeft = (Y0 * RXPOW * Math.Tan(Getβ() * L[i]) - X);
                double topMiddle = (Y0 * RXPOW + X * Math.Tan(Getβ() * L[i]));
                double topRight = Math.Pow(R, 2) * Math.Tan(Getβ() * L[i]);
                double downLeft = Math.Pow(Y0 * RXPOW + X * Math.Tan(Getβ() * L[i]), 2);
                double downRight = Math.Pow(R, 2) * Math.Pow(Math.Tan(Getβ() * L[i]), 2);
                double leftResult = (topLeft * topMiddle - topRight) / (downLeft + downRight);
                double acot = 0.5 * Math.PI - Math.Atan(leftResult);
                result.Add(acot / Getβ());
            }
            return result;
        }


        public static double GetMinReadFZKZPP(float l, float d)
        {
            int index = 0;
            double z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculateλp1() / (4 * Math.PI) + (2 * index + 1) * (Calculateλp1() / 4);
            while (z <= l)
            {
                index++;
                z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculateλp1() / (4 * Math.PI) + (2 * index + 1) * (Calculateλp1() / 4);
            }
            double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)));
            double min = δ * Math.Abs(A) * (1 - GetFZZKPPTL(l, d));
            return min;
        }
        public static double GetMaxReadFZKZPP(float l, float d)
        {
            int index = 0;
            double z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculateλp1() / (4 * Math.PI) + (2 * index) * (Calculateλp1() / 4);
            while (z <= l)
            {
                index++;
                z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculateλp1() / (4 * Math.PI) + (2 * index) * (Calculateλp1() / 4);
            }
            double result = Math.Cos(2 * Getβ() * z - CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)));
            double max = δ * Math.Abs(A) * (1 + GetFZZKPPTL(l, d));
            return max;
        }
        /// <summary>
        /// 驻波比负载阻抗匹配
        /// </summary>
        /// <returns></returns>
        public static double SWRFZKZPP(float l, float d)
        {
            double result = GetMaxReadFZKZPP(l, d) / GetMinReadFZKZPP(l, d);
            return result;
        }

        #endregion

        #region 实验三、定向耦合器的耦合度测量

        /// <summary>
        /// 一端口电压
        /// </summary>
        /// <returns></returns>
        public static double OnePortVoltage()
        {
            double result = Math.Abs(A);
            return result;
        }


        /// <summary>
        /// 定向耦合器耦合度测量
        /// </summary>
        /// <returns></returns>
        public static double CouplingFactorObserved()
        {
            double U3 = Math.Abs(A) / Math.Pow(10, couplingFactorC / 20);
            //  double U3 = Math.Abs(couplingFactorA) / Math.Sqrt(Math.Pow(10, couplingFactorC / 20));
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

        private static double KeBianDuanLuQiX(double resultT, double resultShan, float zd, float z)
        {
            double U = δ * Math.Abs(A) * Math.Sqrt(1 + Math.Pow(Math.Abs(resultT), 2) + 2 * Math.Abs(resultT) * Math.Cos(2 * Getβ() * z - resultShan + (4 * Math.PI * zd) / RuDuanLuQi));
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
            return Math.Atan(TSecond / TFirst);

            //if (TFirst >= 0 && TSecond >= 0)
            //    return Math.Atan(Math.Abs(TSecond) / Math.Abs(TFirst));

            //if (TFirst < 0 && TSecond >= 0)
            //    return Math.Atan(Math.Abs(TSecond) / Math.Abs(TFirst)) + Math.PI / 2;

            //if (TFirst < 0 && TSecond < 0)
            //    return Math.Atan(Math.Abs(TSecond) / Math.Abs(TFirst)) + Math.PI;

            //if (TFirst >= 0 && TSecond < 0)
            //    return Math.Atan(Math.Abs(TSecond) / Math.Abs(TFirst)) + 3 * Math.PI / 2;
            //return 0;
        }







        /// <summary>
        /// 二端口可变断路器T1_a算法
        /// </summary>
        /// <returns></returns>
        private static double GetTl_a_EDKKBDLQ(float zd)
        {
            // double shanD = 2 * GetEDKKBDLQβ() * zd + Shan0;
            //float index = zd / (RuDuanLuQi * 0.25f);
            //if (index > 1)
            //{
            //    zd = zd % (RuDuanLuQi * 0.25f);
            //}
            //double shanD = 4 * Math.PI * (zd / RuDuanLuQi) + Shan0;

            double shanD = Math.PI;
            double addLeft = FA * Math.Cos(ShanA);
            double topLeft = Math.Pow(FB, 2) * Math.Cos(2 * ShanB + shanD) * (1 - FC * Math.Cos(ShanC + shanD));
            double topRight = Math.Pow(FB, 2) * FC * Math.Sin(2 * ShanB + shanD) * Math.Sin(ShanC + shanD);
            double downLeft = Math.Pow((1 - FC * Math.Cos(ShanC + shanD)), 2);
            double downRight = Math.Pow(FC, 2) * Math.Pow(Math.Sin(ShanC + shanD), 2);
            double result = addLeft + (topLeft - topRight) / (downLeft + downRight);

            return result;
        }
        /// <summary>
        /// 二端口可变短路器T1_b算法
        /// </summary>
        /// <returns></returns>
        private static double GetTl_b_EDKKBDLQ(float zd)
        {
            //double shanD = 2 * GetEDKKBDLQβ() * zd + Shan0;
            //float index = zd / (RuDuanLuQi * 0.25f);
            //if (index > 1)
            //{
            //    zd = zd % (RuDuanLuQi * 0.25f);
            //}
            //double shanD = 4 * Math.PI * (zd / RuDuanLuQi) + Shan0;

            double shanD = Math.PI;

            double addLeft = FA * Math.Sin(ShanA);
            double topLeft = Math.Pow(FB, 2) * Math.Sin(2 * ShanB + shanD) * (1 - FC * Math.Cos(ShanC + shanD));
            double topRight = Math.Pow(FB, 2) * FC * Math.Cos(2 * ShanB + shanD) * Math.Sin(ShanC + shanD);
            double downLeft = Math.Pow((1 - FC * Math.Cos(ShanC + shanD)), 2);
            double downRight = Math.Pow(FC, 2) * Math.Pow(Math.Sin(ShanC + shanD), 2);
            double result = addLeft + (topLeft + topRight) / (downLeft + downRight);
            return result;
        }

        private static double GetT1_EDKKBDLQ(float zd)
        {
            double result = Math.Sqrt(Math.Pow(GetTl_a_EDKKBDLQ(zd), 2) + Math.Pow(GetTl_b_EDKKBDLQ(zd), 2));
            return result;
        }

        private static double GetEDKKBDLQβ()
        {
            double ruc = 2 * a;
            double β = 2 * Math.PI / RuDuanLuQi * Math.Sqrt(1 - RuDuanLuQi / ruc);
            //double β = 2 * Math.PI / RuDuanLuQi;
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

        //private static double Get2β()
        //{ 


        //}
        private static double Yin_a(float l)
        {
            double Y0RX = Y0 * (Math.Pow(R, 2) + Math.Pow(X, 2));
            double topLeft = R * (Y0RX + X * Math.Tan(Getβ() * l));
            double topRight = R * (Y0RX * Math.Tan(Getβ() * l) - X) * Math.Tan(Getβ() * l);
            double top = topLeft + topRight;
            double down = Math.Pow(Y0RX + X * Math.Tan(Getβ() * l), 2) + Math.Pow(R, 2) * Math.Pow(Math.Tan(Getβ() * l), 2);
            double result = Y0 * (top / down);
            return result;
        }

        private static double Yin_b(float l, float d)
        {
            double RXPow = Math.Pow(R, 2) + Math.Pow(X, 2);
            double βl = Getβ() * l;
            double top = (Y0 * RXPow * Math.Tan(βl) - X) * (Y0 * RXPow + X * Math.Tan(βl)) - Math.Pow(R, 2) * Math.Tan(βl);
            double down = Math.Pow(Y0 * RXPow + X * Math.Tan(βl), 2) + Math.Pow(R, 2) * Math.Pow(Math.Tan(βl), 2);
            double right = Y0 * (Math.Cos(Getβ() * d) / Math.Sin(Getβ() * d));
            double result = Y0 * (top / down) - right;
            return result;
        }

        /// <summary>
        /// 负载阻抗匹配的TL_a值
        /// </summary>
        private static double GetFZZKPPTL_a(float l, float d)
        {
            double yina = Yin_a(l);
            double yinb = Yin_b(l, d);
            double top = Math.Pow(Y0, 2) - Math.Pow(yina, 2) - Math.Pow(yinb, 2);
            double down = Math.Pow((Y0 + yina), 2) + Math.Pow(yinb, 2);
            double result = top / down;
            return result;
        }
        /// <summary>
        /// 负载阻抗匹配的TL_b值
        /// </summary>
        private static double GetFZZKPPTL_b(float l, float d)
        {
            double yina = Yin_a(l);
            double yinb = Yin_b(l, d);
            double top = -2 * Y0 * yinb;
            double down = Math.Pow((Y0 + yina), 2) + Math.Pow(yinb, 2);
            double result = top / down;
            return result;
        }
        /// <summary>
        /// 负载阻抗匹配的TL值
        /// </summary>
        private static double GetFZZKPPTL(float l, float d)
        {
            double result = Math.Sqrt(Math.Pow(GetFZZKPPTL_a(l, d), 2) + Math.Pow(GetFZZKPPTL_b(l, d), 2));
            return result;
        }


        private static double GetTl_a()
        {
            double T1_aAddLeft = FA * Math.Cos(ShanA);
            double T1_aDivisionTop = Math.Pow(FB, 2) * Math.Cos(2 * ShanB + Math.PI) * (1 - FC * Math.Cos(ShanC + Math.PI)) - (Math.Pow(FB, 2) * FC * Math.Sin(2 * ShanB + Math.PI) * Math.Sin(ShanC + Math.PI));
            double T1_aDivisionDown = Math.Pow((1 - FC * Math.Cos(ShanC + Math.PI)), 2) + Math.Pow(FC, 2) * Math.Pow(Math.Sin(ShanC + Math.PI), 2);
            double T1_a = T1_aAddLeft + T1_aDivisionTop / T1_aDivisionDown;
            return T1_a;
        }

        public static double GetTl_b()
        {
            double T1_bAddLeft = FA * Math.Sin(ShanA);
            double T1_bDivisionTop = Math.Pow(FB, 2) * Math.Sin(2 * ShanB + Math.PI) * (1 - FC * Math.Cos(ShanC + Math.PI)) + (Math.Pow(FB, 2) * FC * Math.Cos(2 * ShanB + Math.PI) * Math.Sin(ShanC + Math.PI));
            double T1_bDivisionDown = Math.Pow((1 - FC * Math.Cos(ShanC + Math.PI)), 2) + Math.Pow(FC, 2) * Math.Pow(Math.Sin(ShanC + Math.PI), 2);
            double T1_b = T1_bAddLeft + T1_bDivisionTop / T1_bDivisionDown;
            return T1_b;
        }

        private static double GetTl()
        {
            double result = Math.Sqrt(Math.Pow(GetTl_a(), 2) + Math.Pow(GetTl_b(), 2));
            return result;
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

        public static double GetMinRead(double shan)
        {
            int index = 0;
            double z = shan * Calculateλp1() / (4 * Math.PI) + (2 * index + 1) * (Calculateλp1() / 4);
            double DT = GetDT(SLMCL_Start_Value, 0);
            while (z < DT)
            {
                index++;
                z = shan * Calculateλp1() / (4 * Math.PI) + (2 * index + 1) * (Calculateλp1() / 4);
            }
            double result = Math.Cos(2 * Getβ() * z - shan);
            if (result == -1)
            {
                return z;
            }
            return 0;
        }
        /// <summary>
        /// 二端口可变短路器最小值获取
        /// </summary>
        /// <param name="shan"></param>
        /// <returns></returns>
        public static double GetMinReadEDKKBDLQ(double shan, double zd)
        {
            int index = 0;
            double z = (shan - (4 * Math.PI * zd) / RuDuanLuQi) * Calculateλp1() / (4 * Math.PI) + (2 * index + 1) * (Calculateλp1() / 4);
            double DT = GetDT(SLMCL_Start_Value, 0);
            while (z < DT)
            {
                index++;
                z = (shan - (4 * Math.PI * zd) / RuDuanLuQi) * Calculateλp1() / (4 * Math.PI) + (2 * index + 1) * (Calculateλp1() / 4);
            }
            double result = Math.Cos(2 * Getβ() * z - shan);
            if (result == -1)
            {
                return z;
            }
            return 0;
        }


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