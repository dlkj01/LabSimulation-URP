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
        public bool init��;
        public bool initA;
    }
    /// <summary>
    /// ��ʽ����
    /// </summary>
    public struct FormulaData
    {
        public float A; /*= 10*///��ѹ10mv-1000mv ��ʼ���󲻱�
        public float F; /*= 8.2f*///Ƶ��8.2-12.5  ��ʼ���󲻱�
        public float ��;//����˥����ȡֵ[0,1] ��ʼ���󲻱�
        public float distanceZ;//z��λ�þ��������ײ������������
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
        public static float A { get; set; } /*= 10*///��ѹ10mv-1000mv ��ʼ���󲻱�
        public static float F { get; set; } /*= 8.2f*///Ƶ��8.2-12.5  ��ʼ���󲻱�
        public static float �� { get; set; }//����˥����ȡֵ[0,1] ��ʼ���󲻱�
        public static float distanceZ { get; set; }//z��λ�þ��������ײ������������
        private static double EDKKBDLQ��;//���˿�+�ɱ��·���ı���
        public static float X = 0;//�翹 ȡֵ��Χ[-200,200] ��ʼ���󲻱�
        public static float R = 0;//���� ȡֵ��Χ[0,200] ��ʼ���󲻱�
        public static double verify = 0;
        private static double ZL = 0;//ZL=R+jX
        private static float Z0 = 100;//100ŷķ
        public static float couplingFactorA;//��϶�����˵�ѹģA
        public static float couplingFactorC;//��϶�C

        public static void Init(/*FormulaData data*/)
        {
            Debug.Log("���ݳ�ʼ��:");

            //RandomDataInit();

            InitData();
        }

        static Complex S11Com;
        static Complex S12Com;
        static Complex S22Com;
        static Complex ZLCom;
        public static void RandomDataInit()
        {
            //F = UnityEngine.Random.Range(8.2f, 12.5f);
            //A = UnityEngine.Random.Range(2f, 2000f);
            // �� = 1; //Random(0.00f, 1.00f);


            // ����һ
            FB =/* 0.4f*/ UnityEngine.Random.Range(0f, 1f);
            FA =/* 0.2f*/ UnityEngine.Random.Range(0f, Mathf.Sqrt(1 - Mathf.Pow(float.Parse(FB.ToString()), 2)));
            FC =/* 0.1f*/ UnityEngine.Random.Range(0f, Mathf.Sqrt(1 - Mathf.Pow(float.Parse(FB.ToString()), 2)));
            // ������
            //FA = UnityEngine.Random.Range(0f, 1f);
            //FC = FA;
            //FB = Math.Sqrt(1 - Math.Pow(FA, 2));
            //do
            //{
            //    Shan0 = UnityEngine.Random.Range(0f, 0.25f * Mathf.PI);
            //} while (Shan0 <= 0 || Shan0 > 0.25f * Mathf.PI);
            Shan0 = 0;
            //FD = Math.Abs(Math.Cos(2 * GetEDKKBDLQ��() * zd - (Math.PI - Shan0)));
            RuDuanLuQi = /*0.03f*/ UnityEngine.Random.Range(0.024f, 0.0365f);
            //   RuDuanLuQi = 0.024f;
            //����һ
            ShanA =/* 0.5f*/ UnityEngine.Random.Range(0, 0.25f * Mathf.PI);
            ShanC = /*0.4f*/ UnityEngine.Random.Range(0, 0.25f * Mathf.PI);
            ShanB = /*0.3f*/ UnityEngine.Random.Range(0, 0.25f * Mathf.PI);

            // ������
            //ShanA = UnityEngine.Random.Range(0, 0.5f * Mathf.PI);
            //ShanC = UnityEngine.Random.Range(0, 0.5f * Mathf.PI);
            //ShanB = 0.5f * (ShanA + ShanC + Math.PI);

            EDKKBDLQ�� = GetEDKKBDLQ��();
            S11Com = new Complex(FA * Math.Cos(ShanA), FA * Math.Sin(ShanA));
            S12Com = new Complex(FB * Math.Cos(ShanB), FB * Math.Sin(ShanB));
            S22Com = new Complex(FC * Math.Cos(ShanC), FC * Math.Sin(ShanC));

            X = UnityEngine.Random.Range(-200f, 200f);
            R = UnityEngine.Random.Range(0f, 200f);

            List<double> lList = CalculateL();
            while (lList[1] < 2.1f || lList[0] > 14.8f)
            {
                X = UnityEngine.Random.Range(-200f, 200f);
                R = UnityEngine.Random.Range(0f, 200f);
                CalculateL();
            }

            double XRPow = Math.Pow(R, 2) + Math.Pow(X, 2);
            verify = GetVerify(XRPow);

            while (verify < 0)
            {
                verify = GetVerify(XRPow);
            }
            Debug.Log("R:" + R + "  X:" + X);
            ZLCom = new Complex(R, X);
            ZL = ZLCom.Real + ZLCom.Imaginary;
            couplingFactorC = UnityEngine.Random.Range(5, 20);
        }

       
       static void InitData()
        {
            // ����һ
            FB =/* 0.4f*/ UnityEngine.Random.Range(0f, 1f);
            FA =/* 0.2f*/ UnityEngine.Random.Range(0f, Mathf.Sqrt(1 - Mathf.Pow(float.Parse(FB.ToString()), 2)));
            FC =/* 0.1f*/ UnityEngine.Random.Range(0f, Mathf.Sqrt(1 - Mathf.Pow(float.Parse(FB.ToString()), 2)));
            Shan0 = 0;
            RuDuanLuQi = /*0.03f*/ UnityEngine.Random.Range(0.024f, 0.0365f);

            //����һ
            ShanA =/* 0.5f*/ UnityEngine.Random.Range(0, 0.25f * Mathf.PI);
            ShanC = /*0.4f*/ UnityEngine.Random.Range(0, 0.25f * Mathf.PI);
            ShanB = /*0.3f*/ UnityEngine.Random.Range(0, 0.25f * Mathf.PI);

            EDKKBDLQ�� = GetEDKKBDLQ��();
            S11Com = new Complex(FA * Math.Cos(ShanA), FA * Math.Sin(ShanA));
            S12Com = new Complex(FB * Math.Cos(ShanB), FB * Math.Sin(ShanB));
            S22Com = new Complex(FC * Math.Cos(ShanC), FC * Math.Sin(ShanC));

            X = UnityEngine.Random.Range(-200f, 200f);
            R = UnityEngine.Random.Range(0f, 200f);

            double XRPow = Math.Pow(R, 2) + Math.Pow(X, 2);
            verify = Math.Pow(2 * X * XRPow * Y0, 2) - 4 * XRPow * (1 - R * Y0) * (Math.Pow(Y0, 2) * Math.Pow(XRPow, 4) - Math.Pow(R, 3) * Y0 - R * Y0 * Math.Pow(X, 2));

            List<double> lList = CalculateL();
            while (lList[1] < 2.1f || lList[0] > 14.8f|| verify < 0)
            {
                X = UnityEngine.Random.Range(-200f, 200f);
                R = UnityEngine.Random.Range(0f, 200f);
                CalculateL();
                verify = GetVerify(XRPow);
            }

            ZLCom = new Complex(R, X);
            ZL = ZLCom.Real + ZLCom.Imaginary;
            couplingFactorC = UnityEngine.Random.Range(5, 20);
        }

        static double GetVerify(double XRPow)
        {
            List<double> lList = CalculateL();
            //List0�����ֵ������ΪС�� ƥ���ݶ�l�����ֵ
            while (R * Y0 == 1 || lList[1] < 2.1f || lList[0] > 14.8f)
            {

                X = UnityEngine.Random.Range(-200f, 200f);
                R = UnityEngine.Random.Range(0f, 200f);
            }

            double targetValue = Math.Pow(2 * X * XRPow * Y0, 2) - 4 * XRPow * (1 - R * Y0) * (Math.Pow(Y0, 2) * Math.Pow(XRPow, 4) - Math.Pow(R, 3) * Y0 - R * Y0 * Math.Pow(X, 2));
            return targetValue;
        }


        #region ��һ��ʵ����ȷ����Ĵ�
        public static void FixedCorrect1Calculate()
        {
            //���׻���ɺ���
            report1CorrectAnswer.SourceFrequency = F;//�ź�ԴƵ��
            report1CorrectAnswer.SourceVoltage = A;//��ѹ
            report1CorrectAnswer.Attenuator = ��;//˥����
            report1CorrectAnswer.EquivalentSectionPosition = GetDT(SLMCL_Start_Value, 0)*1000f;//��һ����Ч�����λ��
            report1CorrectAnswer.InputWavelength = Calculate��p1()*1000f;//����˲���
            report1CorrectAnswer.WaveNodePosShortCircuit = GetMinZUpperDTEDKDLB();//���ڵ�λ���ն˶�·

            report1CorrectAnswer.VariableShortCircuitFirstPos = GetFirstMinBoundKBDLQ();//�ɱ��·����һ���ڵ���Сֵλ��
            for (int i = 0; i < report1CorrectAnswer.VariableShortCircuitFirstPos.Count; i++)
            {
                double T1 = GetT1_EDKKBDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos[i].ToString()));
                double shan = CalculateShan(GetTl_a_EDKKBDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos[i].ToString())), GetTl_b_EDKKBDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos[i].ToString())));
                double readResult = �� * Math.Abs(A) * Math.Sqrt(1 + Math.Pow(T1, 2) + 2 * Math.Abs(T1) * Math.Cos(2 * Get��() * report1CorrectAnswer.WaveNodePosShortCircuit - shan));
                Debug.Log(readResult);
            }

            // �ɱ��·���ڶ����ڵ���Сֵλ��
            report1CorrectAnswer.VariableShortCircuitSecondPos = new List<double>();
            for (int i = 0; i < report1CorrectAnswer.VariableShortCircuitFirstPos.Count; i++)
            {
                report1CorrectAnswer.VariableShortCircuitSecondPos.Add((report1CorrectAnswer.VariableShortCircuitFirstPos[i] + 0.5f * RuDuanLuQi)*1000f);
            }

            report1CorrectAnswer.VariableWavelengthInShortCircuit = RuDuanLuQi*1000f;//�ɱ��·���в�����p2

            //��·����λ��
            report1CorrectAnswer.OpenLoadPosition = new List<double>();
            for (int i = 0; i < report1CorrectAnswer.VariableShortCircuitFirstPos.Count; i++)
            {
                report1CorrectAnswer.OpenLoadPosition.Add((report1CorrectAnswer.VariableShortCircuitFirstPos[i] + RuDuanLuQi * 0.25f)*1000);
                report1CorrectAnswer.OpenLoadPosition.Add((report1CorrectAnswer.VariableShortCircuitSecondPos[i] + RuDuanLuQi * 0.25f)*1000);
            }


            //���ڵ�λ���ն˿�·
            report1CorrectAnswer.WaveNodePosShortTerminal = GetMinReadUpperDTEDKKEDLQ(float.Parse(report1CorrectAnswer.OpenLoadPosition[0].ToString()))*1000f;
            report1CorrectAnswer.WaveNodePosShortMatching = GetMinZUpperDTEDKPPFZ()*1000f;//���ڵ�λ���ն�ƥ��
            report1CorrectAnswer.PhaseAngleCircuit = CalculateShan(GetTl_a(), GetTl_b());//����ն˶�·
            report1CorrectAnswer.PhaseAngleTerminal = CalculateShan(GetTl_a_EDKKBDLQ(float.Parse(report1CorrectAnswer.OpenLoadPosition[0].ToString()))
                , GetTl_b_EDKKBDLQ(float.Parse(report1CorrectAnswer.OpenLoadPosition[0].ToString())));//����ն˿�·
            report1CorrectAnswer.PhaseAngleMatching = ShanA;//����ն�ƥ��

            report1CorrectAnswer.StandingWaveRatioCircuit = SWREDKTODLB();//פ�����ն˶�·
            report1CorrectAnswer.StandingWaveRatioTerminal = SWREDKKBDLQ(float.Parse(report1CorrectAnswer.OpenLoadPosition[0].ToString("#0.0000000")));//פ�����ն˿�·
            report1CorrectAnswer.StandingWaveRatioMatching = SWREDKPPFZ();//פ�����ն�ƥ��

            report1CorrectAnswer.input��1S = GetTl();
            report1CorrectAnswer.input��10 = GetT1_EDKKBDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos[0].ToString()));
            report1CorrectAnswer.input��1L = FA;



            //report1CorrectAnswer.ReflectionCoefficient��1S = S11 - (Math.Pow(S12, 2) / (1 + S22));//����ϵ��T1S
            //report1CorrectAnswer.ReflectionCoefficient��10 = S11 + Math.Pow(S12, 2) / (1 - S22);//����ϵ��T10
            //report1CorrectAnswer.ReflectionCoefficient��1L = S11;//����ϵ��T1L

            //ѧ����ķ���ϵ��
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

            Complex ��1S = S11Com - Complex.Pow(S12Com, 2) / (1 + S22Com);//����ϵ��T1S
            Complex ��10 = S11Com + Complex.Pow(S12Com, 2) / (1 - S22Com);//����ϵ��T1S
            Complex ��1L = S11Com;//����ϵ��T1S
            report1CorrectAnswer.ReflectionCoefficient��1SReal = ��1S.Real;
            report1CorrectAnswer.ReflectionCoefficient��10Real = ��10.Real;
            report1CorrectAnswer.ReflectionCoefficient��1LReal = ��1L.Real;
            report1CorrectAnswer.ReflectionCoefficient��1SImaginary = ��1S.Imaginary;
            report1CorrectAnswer.ReflectionCoefficient��10Imaginary = ��10.Imaginary;
            report1CorrectAnswer.ReflectionCoefficient��1LImaginary = ��1L.Imaginary;

            //Debug.Log("ʵ����ֵ��" + S11Com.Real + "�鲿��ֵ��:" + S11Com.Imaginary);
            //Complex S11 = ��1L/*(��10 + ��1S) / (2 + ��10 - ��1S)*/;
            //Debug.Log("ʵ����ֵ��" + S22Com.Real + "�鲿��ֵ��:" + S22Com.Imaginary);
            //Complex S22 = (2 * ��1L - ��1S - ��10) / (��1S - ��10);
            //Complex S12ComPow = Complex.Pow(S12Com, 2);
            //Complex S12SQRT = Complex.Sqrt(S12ComPow);
            //Debug.Log("ʵ����ֵ��" + S12ComPow.Real + "�鲿��ֵ��:" + S12ComPow.Imaginary);
            //Complex S12Pow = (S22Com + 1) * (��1L - ��1S);
            //Complex S12PowThird = Complex.Sqrt((1 - S22Com) * (��10 - ��1L));
            report1CorrectAnswer.inputS11Real = S11Com.Real;
            report1CorrectAnswer.inputS11Imaginary = S11Com.Imaginary;
            report1CorrectAnswer.inputS12S21Real = S12Com.Real;
            report1CorrectAnswer.inputS12S21Imaginary = S12Com.Imaginary;
            report1CorrectAnswer.inputS22Real = S22Com.Real;
            report1CorrectAnswer.inputS22Imaginary = S22Com.Imaginary;

        }
        #endregion
        #region �ڶ���ʵ���һ����ȷ����Ĵ� �����迹����
        public static void FixedCorrect2FirstGroupCalculate()
        {  
            //��λͳһ����ɺ��ף�mm��
            report2CorrectAnswer.inputSourceFrequencyFirst = F;//�ź�ԴƵ��
            report2CorrectAnswer.inputSourceVoltageFirst = A;//��ѹ
            report2CorrectAnswer.inputAttenuatorSetupFirst = ��;//˥����
            report2CorrectAnswer.SWRFirst = SWRFZZKCL();
            report2CorrectAnswer.WaveguideWavelengthFirst = Calculate��p1();
            report2CorrectAnswer.EquivalentSectionPositionFirst = GetDT(SLMCL_Start_Value, 0)*1000f;//��һ����Ч�����λ��
            report2CorrectAnswer.MinimumVoltage = GetMinRead_FZZKCL();
            report2CorrectAnswer.MaximumVoltage = GetMaxRead_FZZKCL();
            report2CorrectAnswer.WaveNodePositionFirst = GetMinZUpperDTFZZKCL()*1000;
            //report2CorrectAnswer.NormalizedLoadImpedanceFirst = NormalizedLoadImpedance();
            Complex com = ZLCom / Z0;
            report2CorrectAnswer.NormalizedLoadImpedanceFirstReal = com.Real;//�����迹ʵ��
            report2CorrectAnswer.NormalizedLoadImpedanceFirstImaginary = com.Imaginary;//�����迹�鲿
            report2CorrectAnswer.LoadImpedanceFirstReal = ZLCom.Real;
            report2CorrectAnswer.LoadImpedanceFirstImaginary = ZLCom.Imaginary;
            report2CorrectAnswer.ScrewPositionFirst = CalculateL();
            report2CorrectAnswer.ScrewDepthFirst = CalculateD();
            report2CorrectAnswer.MinimumVoltageAfterMatchingFirst = GetMinReadFZKZPP(float.Parse(report2CorrectAnswer.ScrewPositionFirst[0].ToString()), float.Parse(report2CorrectAnswer.ScrewDepthFirst[0].ToString()));
            report2CorrectAnswer.MaximumVoltageAfterMatchingFirst = GetMaxReadFZKZPP(float.Parse(report2CorrectAnswer.ScrewPositionFirst[0].ToString()), float.Parse(report2CorrectAnswer.ScrewDepthFirst[0].ToString()));
            report2CorrectAnswer.SWRAfterMatchingFirst = report2CorrectAnswer.MaximumVoltageAfterMatchingFirst / report2CorrectAnswer.MinimumVoltageAfterMatchingFirst;
        }
        #endregion
        //#region �ڶ���ʵ��ڶ�����ȷ��
        //public static void FixedCorrect2SecondGroupCalculate()
        //{
        //    report2CorrectAnswer.inputSourceFrequencySecond = F;//�ź�ԴƵ��
        //    report2CorrectAnswer.inputSourceVoltageSecond = A;//��ѹ
        //    report2CorrectAnswer.inputAttenuatorSetupSecond = ��;//˥����
        //    report2CorrectAnswer.SWRSecond = SWRFZZKCL();
        //    report2CorrectAnswer.WaveguideWavelengthSecond = Calculate��p1();
        //    report2CorrectAnswer.EquivalentSectionPositionSecond = GetDT(SLMCL_Start_Value, 0);//��һ����Ч�����λ��
        //    report2CorrectAnswer.WaveNodePositionSecond = GetMinZUpperDTFZZKCL();//��һ�����ڵ�λ��
        //    report2CorrectAnswer.NormalizedLoadImpedanceSecond = NormalizedLoadImpedance();//�����迹��һ��
        //    report2CorrectAnswer.LoadImpedanceSecond = ZL;//�����迹
        //    report2CorrectAnswer.ScrewPositionSecond = CalculateL();//�ݶ�λ��
        //    report2CorrectAnswer.ScrewDepthSecond = CalculateD();//�ݶ����
        //    report2CorrectAnswer.MinimumVoltageAfterMatchingSecond = GetMinReadFZKZPP(float.Parse(report2CorrectAnswer.ScrewPositionFirst[0].ToString()), float.Parse(report2CorrectAnswer.ScrewDepthFirst[0].ToString()));
        //    report2CorrectAnswer.MinimumVoltageAfterMatchingSecond = GetMaxReadFZKZPP(float.Parse(report2CorrectAnswer.ScrewPositionFirst[0].ToString()), float.Parse(report2CorrectAnswer.ScrewDepthFirst[0].ToString()));
        //    report2CorrectAnswer.SWRAfterMatchingSecond = report2CorrectAnswer.MaximumVoltageAfterMatchingFirst / report2CorrectAnswer.MinimumVoltageAfterMatchingFirst;
        //}
        //#endregion

        #region ������ʵ����ȷ�𰸼���
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
            �� = 0;
            distanceZ = 0;
        }
        #region �𰸼���
        public static double Calculate��p1()
        {
            double c = 3 * Math.Pow(10, 8);
            double ru = c / (F * Math.Pow(10, 9));
            double ruc = 2 * a;
            double ��p1 = ru / Math.Sqrt((1 - Math.Pow(ru / ruc, 2)));
            return ��p1;
        }

        /// <summary>
        /// �@ȡ��Ч�������d��λ��
        /// </summary>
        /// <param name="endValue">����ֵ</param>
        /// <param name="step">��������</param>
        /// <param name="startValue">��ʼֵ</param>
        /// <param name="func">��ȡ���ֵ������Сֵ�ķ���</param>
        /// <returns></returns>
        public static double GetDT(float startValue, float offect /*float endValue, int step, float startValue, Func<List<double>, double> func*/)
        {
            double everyBandLength = Calculate��p1();//ÿһ�����εĳ���
            int index = 0;
            while (everyBandLength + (everyBandLength * 0.5f) * index < startValue)
            {
                index++;
            }
            // int startBandCount = (int)((startValue + offect) / everyBandLength) + 1;
            double result = everyBandLength + (everyBandLength * 0.5f) * index;  //���׻���ɺ���
            return result;
            // Debug.Log(SLMCLXZDDLB(float.Parse(result.ToString("#0.0000000"))));

            //List<double> allResult = new List<double>();
            //List<float> distanceList = new List<float>();
            //float everyStepValue = (endValue - startValue) / step;
            //for (float distance = startValue; distance <= endValue; distance += everyStepValue)
            //{
            //    allResult.Add(Math.Abs(Math.Sin(Get��() * distance)));
            //    distanceList.Add(distance);
            //}
            //double result = func(allResult);
            //int index = allResult.FindIndex((p) => { return p == result; });
            //Debug.Log("������:" + index + "---��Ӧ��ֵΪ:" + distanceList[index] + "---Sin��*z=" + result + "---����Ϊ" + SLMCLXZDDLB(distanceList[index]));
            //return distanceList[index];
        }



        /// <summary>
        /// ��Ч����λ��,�����ײ������ն˽Ӷ�·��ʱ����z=0��ʼ���󣬵�һ��ʹU�����ﵽ��Сֵ��λ��
        /// </summary>
        /// <returns></returns>
        public static double GetdT(int n)
        {
            double Shan1 = CalculateShan(GetTl_a(), GetTl_b());
            double z = Shan1 * Calculate��p1() / (4 * Math.PI) + (n + 1) * (Calculate��p1() / 4);
            return z;
        }
        /// <summary>
        /// �ɱ��·���ڵ�λ��
        /// </summary>
        /// <param name="n">��ȡ�ĵڼ�������</param>
        /// <returns></returns>
        public static double GetlT1(int n)
        {
            int index = 1;//�ӵ�һ�����ο�ʼ
            double Shan1 = CalculateShan(GetTl_a(), GetTl_b());
            double z = 0;
            for (int i = index; i < 10; i++)
            {
                z = Shan1 * Calculate��p1() / (4 * Math.PI) + (i) * (Calculate��p1() / 4);
                if (Math.Cos(2 * Get��() * z - Shan1) == -1)
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
        /// ���˿�����S��������
        /// </summary>
        /// <returns></returns>
        //public static double EDKWLSCSCL(float z)
        //{

        //}
        #region ����
        #region ��ʽһ&��ʽ�����ҵ�Ч����
        /// <summary>
        /// ��ʽһ��(1)
        /// �����ײ������ն�ֱ�ӽӶ�·��(���Ӷ��˿�����ʱ)
        /// </summary>
        /// <param name="z">��ǰ����λ���������ײ��������Ҷ˾���</param>
        /// <returns></returns>
        public static double SLMCLXZDDLB(float z)
        {
            double U = �� * Math.Abs(A) * Math.Abs((Math.Sin(Get��() * z)));
            return U;
        }
        #endregion

        #region ʵ��һ�����˿�����S��������()
        private static float FA;
        private static double FB;
        private static float FC;
        private static float FD;
        private static float ShanA;
        private static double ShanB;
        private static float ShanC;
        private static float ShanD;
        #endregion

        #region ʵ��һ���Ӷ��˿ںͶ�·��
        /// <summary>
        /// ��ʽһ��(3)
        /// �Ӷ��˿ںͶ�·��
        /// </summary>
        /// <param name="z">��ת3���ײ����ߵ���ť���ǵ���z����ֵ</param>
        /// <returns></returns>
        public static double EDKTODLB(float z)
        {
            //int FD = 1;
            //double ɽD = Math.PI;
            //int T2 = -1;
            return FuZaiZuLiangKangHengFormula(GetTl(), CalculateShan(GetTl_a(), GetTl_b()), z);
        }
        /// <summary>
        /// ��ȡ���˿ںͶ�·���������
        /// </summary>
        /// <returns></returns>
        public static double GetMaxReadEDKDLB()
        {
            double z = (CalculateShan(GetTl_a(), GetTl_b()) * Calculate��p1()) / (4 * Math.PI) + 2 * (Calculate��p1() / 4);
            double result = Math.Cos(2 * Get��() * z - CalculateShan(GetTl_a(), GetTl_b()));
            if (result == 1)
            {
                double Umax = �� * Math.Abs(A) * Math.Abs((1 + GetTl()));
                return Umax;
            }

            return 0;
        }
        /// <summary>
        /// ��ȡ���˿ںͶ�·�����С����
        /// </summary>
        /// <returns></returns>
        public static double GetMinReadEDKDLB()
        {
            double z = (CalculateShan(GetTl_a(), GetTl_b()) * Calculate��p1()) / (4 * Math.PI) + (2 + 1) * (Calculate��p1() / 4);
            double result = Math.Cos(2 * Get��() * z - CalculateShan(GetTl_a(), GetTl_b()));
            if (result == -1)
            {
                double min = �� * Math.Abs(A) * Math.Abs((1 - GetTl()));
                return min;
            }
            return 0;
        }
        /// <summary>
        /// פ����
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
            double min = �� * Math.Abs(A) * Math.Abs((1 - GetTl()));
            return z;
        }
        /// <summary>
        /// ��Ƕ��˿�����·��
        /// </summary>
        /// <returns></returns>
        public static double PhaseAngleEDKTODLB()
        {
            double result = CalculateShan(GetTl_a(), GetTl_b());
            return result;
        }
        public static double Get��1S()
        {
            double result = GetTl();
            return result;
        }
        #endregion

        #region ʵ��һ�����˿ں�ƥ�为��
        /// <summary>
        /// ��ʽһ��(4)
        /// ���˿ں�ƥ�为��
        /// </summary>
        /// <param name="z">�Ҷ˿�ʼΪ0,Խ���ź�ԴԽ��</param>
        /// <returns></returns>
        public static double EDKPPFZ(float z)
        {
            double T1 = FA;
            double Shan1 = ShanA;
            double U = �� * Math.Abs(A) * Math.Sqrt(1 + Math.Abs(Math.Pow(T1, 2)) + 2 * Math.Abs(T1) * Math.Cos(2 * Get��() * z - Shan1));
            return U;
        }
        public static double GetMaxZEDKPPFZ(int i)
        {
            double z = ShanA * Calculate��p1() / (4 * Math.PI) + 2 * i * (Calculate��p1() / 4);
            return z;
        }
        public static double GetMinZEDKPPFZ(int i)
        {
            double z = ShanA * Calculate��p1() / (4 * Math.PI) + (2 * i + 1) * (Calculate��p1() / 4);
            return z;
        }
        /// <summary>
        /// ���˿�ƥ����ڶ������ֵ
        /// </summary>
        /// <returns></returns>
        public static double GetMaxReadEDKPPFZ()
        {
            double z = GetMaxZEDKPPFZ(1);
            double result = Math.Cos(2 * Get��() * z - ShanA);
            double maxRead = �� * Math.Abs(A) * Math.Abs((1 + FA));
            return maxRead;
        }

        /// <summary>
        /// ���˿�ƥ����ڶ�����Сֵ
        /// </summary>
        /// <returns></returns>
        public static double GetMinReadEDKPPFZ()
        {
            double z = GetMinZEDKPPFZ(1);
            double result = Math.Cos(2 * Get��() * z - ShanA);
            double minRead = �� * Math.Abs(A) * Math.Abs((1 - FA));
            return minRead;
        }
        /// <summary>
        /// ���˿�ƥ����ڶ�����Сֵ
        /// </summary>
        /// <returns></returns>
        public static double GetMinZUpperDTEDKPPFZ()
        {
            double shan1 = ShanA;
            double z = GetMinRead(shan1);
            double min = �� * Math.Abs(A) * Math.Abs((1 - FA));
            return z;
            //double DT = GetDT(0.055f, 0);
            //int i = 0;
            //while (GetMinZEDKPPFZ(i) < DT)
            //{
            //    i++;
            //}
            //double z = GetMinZEDKPPFZ(i);
            //double result = Math.Cos(2 * Get��() * z - ShanA);
            //if (result == -1)
            //{
            //    double min = �� * Math.Abs(A) * Math.Abs((1 - FA));
            //    return min;
            //}
            //return 0;
        }
        /// <summary>
        /// פ����
        /// </summary>
        /// <returns></returns>
        public static double SWREDKPPFZ()
        {
            double result = GetMaxReadEDKPPFZ() / GetMinReadEDKPPFZ();
            return result;
        }
        /// <summary>
        /// ��Ƕ��˿ڽ�ƥ�为��
        /// </summary>
        /// <returns></returns>
        public static double PhaseAngleSWREDKPPFZ()
        {
            double result = ShanA;
            return result;
        }
        public static double Get��1L()
        {
            double result = FA;
            return result;
        }


        #endregion

        #region ʵ��һ�����˿ڿɱ��·��
        private static float Shan0;
        public static float RuDuanLuQi;
        /// <summary>
        /// ���˿ڿɱ��·��
        /// </summary>
        /// <param name="zd">�ɱ��·���в�������</param>
        /// <param name="z">��ͨ�ľ��β�������</param>
        /// <returns></returns>
        public static double ErDuanKouKeBianDuanLuQi(float zd, float z)
        {
            return FuZaiZuLiangKangHengFormula(GetT1_EDKKBDLQ(zd), CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)), z); ;
        }
        /// <summary>
        /// ���˿ڿɱ��·����������
        /// </summary>
        /// <param name="zd"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public static double ErDuanKouKeBianDuanLuQiNew(float zd, float z)
        {
            return KeBianDuanLuQiX(GetT1_EDKKBDLQ(zd), CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)), zd, z);
        }

        /// <summary>
        /// ��ȡ���˿ںͿɱ��·����������
        /// </summary>
        /// <returns></returns>
        public static double GetMaxReadEDKKBDLQ(float zd)
        {
            double z = CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)) * Calculate��p1() / (4 * Math.PI) + 2 * (Calculate��p1() / 4);
            double result = Math.Cos(2 * Get��() * z - CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)));
            if (result == 1)
            {
                double T1 = GetT1_EDKKBDLQ(zd);
                double Umax = �� * Math.Abs(A) * Math.Abs(1 + T1);
                return Umax;
            }
            return 0;
        }
        /// <summary>
        /// ��ȡ���˿ںͿɱ��·������С����
        /// </summary>
        /// <returns></returns>
        public static double GetMinReadEDKKBDLQ(float zd)
        {
            double z = CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)) * Calculate��p1() / (4 * Math.PI) + (2 + 1) * (Calculate��p1() / 4);
            double result = Math.Cos(2 * Get��() * z - CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)));
            if (result == -1)
            {
                double T1 = GetT1_EDKKBDLQ(zd);
                double Umin = �� * Math.Abs(A) * Math.Abs(1 - T1);
                return Umin;
            }
            return 0;
        }
        /// <summary>
        /// פ����
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
            double min = �� * Math.Abs(A) * Math.Abs(1 - GetT1_EDKKBDLQ(zd));
            return z;
        }

        /// <summary>
        /// ��ȡ�ɱ��·����N��������Сֵ
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
                    result.Add(value*1000); //���׻���ɺ���
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
        /// ��Ƕ��˿ںͿɱ��·��
        /// </summary>
        /// <param name="zd">�ɱ��·���ڿ�·lTλ��ʱ</param>
        /// <returns></returns>
        public static double PhaseAngle(float zd)
        {
            double result = CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd));
            return result;
        }
        public static double Get��1O(float zd)
        {
            double result = GetT1_EDKKBDLQ(zd);
            return result;
        }

        #endregion

        #region ʵ����������迹����
        /// <summary>
        /// �����迹������һ����ʽ
        /// </summary>
        /// <param name="z">��ת3���ײ����ߵ���ť���ǵ���z����ֵ</param>
        /// <returns></returns>
        public static double FZZKCL_First(float z)
        {
            return FuZaiZuLiangKangHengFormula(GetTL(), CalculateShan(GetTL_a(), GetTL_b()), z);
        }

        /// <summary>
        /// ��ȡ������
        /// </summary>
        /// <returns></returns>
        public static double GetMaxRead_FZZKCL()
        {
            double z = (CalculateShan(GetTL_a(), GetTL_b()) * Calculate��p1()) / (4 * Math.PI) + 2 * (Calculate��p1() * 0.25f);
            double result = Math.Cos(2 * Get��() * z - CalculateShan(GetTL_a(), GetTL_b()));
            double Umax = �� * Math.Abs(A) * Math.Abs((1 + GetTL()));
            return Umax;
        }
        /// <summary>
        /// ��ȡ��С����
        /// </summary>
        /// <returns></returns>
        public static double GetMinRead_FZZKCL()
        {
            double z = (CalculateShan(GetTL_a(), GetTL_b()) * Calculate��p1()) / (4 * Math.PI) + (2 + 1) * (Calculate��p1() * 0.25f);
            double result = Math.Cos(2 * Get��() * z - CalculateShan(GetTL_a(), GetTL_b()));
            double Umin = �� * Math.Abs(A) * Math.Abs((1 - GetTL()));
            return Umin;
        }
        /// <summary>
        /// �����迹����פ����
        /// </summary>
        /// <returns></returns>
        public static double SWRFZZKCL()
        {
            double result = GetMaxRead_FZZKCL() / GetMinRead_FZZKCL();
            return result;
        }

        /// <summary>
        /// ��һ���ڵ�λ��lmin�����迹����
        /// </summary>
        /// <returns></returns>
        public static double GetMinZUpperDTFZZKCL()
        {
            double shan1 = CalculateShan(GetTL_a(), GetTL_b());
            double z = GetMinRead(shan1);
            double min = �� * Math.Abs(A) * Math.Abs((1 - GetTl()));
            return z;
        }


        /// <summary>
        /// ��һ�������迹
        /// </summary>
        /// <returns></returns>
        public static double NormalizedLoadImpedance()
        {
            double result = ZL / Z0;
            return result;
        }
        #endregion

        #region ʵ����������迹ƥ��
        /// <summary>
        /// ���ؿ���ƥ��
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
        /// ����l��ֵ
        /// </summary>
        /// <returns></returns>
        public static List<double> CalculateL()
        {
            //X = UnityEngine.Random.Range(-200f, 200f);
            //R = UnityEngine.Random.Range(0f, 200f);

            double RX2 = Math.Pow(R, 2) + Math.Pow(X, 2);
            double topLeft = -2 * X * RX2 * Y0;
            double topRight = Math.Sqrt(Math.Pow(2 * X * RX2 * Y0, 2) - 4 * RX2 * (1 - R * Y0) * (Math.Pow(Y0, 2) * Math.Pow(RX2, 2) - Math.Pow(R, 3) * Y0 - R * Y0 * Math.Pow(X, 2)));
            double down = 2 * RX2 * (1 - R * Y0);
            double resultAdd = Math.Atan((topLeft + topRight) / down) / Get��();
            double resultSub = Math.Atan((topLeft - topRight) / down) / Get��();
            int k = 0;
            int p = 0;
            double c = 3 * Math.Pow(10, 8);
            double ru = c / (F * Math.Pow(10, 9));
            while (resultAdd < 0)
            {
                k++;
                resultAdd += 0.5f * k * Calculate��p1();
            }
            while (resultSub < 0)
            {
                p++;
                resultSub += 0.5f * p * Calculate��p1();
            }
            List<double> result = new List<double>();
            result.Add(resultAdd*1000); //���׻���ɺ���
            result.Add(resultSub*1000);
            return result;
        }

        /// <summary>
        /// ����d��ֵ
        /// </summary>
        /// <returns></returns>
        public static List<double> CalculateD()
        {
            List<double> L = CalculateL();
            double RXPOW = Math.Pow(R, 2) + Math.Pow(X, 2);
            List<double> result = new List<double>();
            for (int i = 0; i < L.Count; i++)
            {
                double topLeft = (Y0 * RXPOW * Math.Tan(Get��() * L[i]) - X);
                double topMiddle = (Y0 * RXPOW + X * Math.Tan(Get��() * L[i]));
                double topRight = Math.Pow(R, 2) * Math.Tan(Get��() * L[i]);
                double downLeft = Math.Pow(Y0 * RXPOW + X * Math.Tan(Get��() * L[i]), 2);
                double downRight = Math.Pow(R, 2) * Math.Pow(Math.Tan(Get��() * L[i]), 2);
                double leftResult = (topLeft * topMiddle - topRight) / (downLeft + downRight);
                double acot = 0.5 * Math.PI - Math.Atan(leftResult);
                result.Add(acot / Get��()*1000); //���׻���ɺ���
            }
            return result;
        }


        public static double GetMinReadFZKZPP(float l, float d)
        {
            int index = 0;
            double z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculate��p1() / (4 * Math.PI) + (2 * index + 1) * (Calculate��p1() / 4);
            while (z <= l)
            {
                index++;
                z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculate��p1() / (4 * Math.PI) + (2 * index + 1) * (Calculate��p1() / 4);
            }
            double result = Math.Cos(2 * Get��() * z - CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)));
            double min = �� * Math.Abs(A) * (1 - GetFZZKPPTL(l, d));
            return min;
        }
        public static double GetMaxReadFZKZPP(float l, float d)
        {
            int index = 0;
            double z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculate��p1() / (4 * Math.PI) + (2 * index) * (Calculate��p1() / 4);
            while (z <= l)
            {
                index++;
                z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculate��p1() / (4 * Math.PI) + (2 * index) * (Calculate��p1() / 4);
            }
            double result = Math.Cos(2 * Get��() * z - CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)));
            double max = �� * Math.Abs(A) * (1 + GetFZZKPPTL(l, d));
            return max;
        }
        /// <summary>
        /// פ���ȸ����迹ƥ��
        /// </summary>
        /// <returns></returns>
        public static double SWRFZKZPP(float l, float d)
        {
            double result = GetMaxReadFZKZPP(l, d) / GetMinReadFZKZPP(l, d);
            return result;
        }

        #endregion

        #region ʵ�������������������϶Ȳ���

        /// <summary>
        /// һ�˿ڵ�ѹ
        /// </summary>
        /// <returns></returns>
        public static double OnePortVoltage()
        {
            double result = Math.Abs(A);
            return result;
        }


        /// <summary>
        /// �����������϶Ȳ���
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

        #region ��ʽ��װ
        /// <summary>
        /// 
        /// </summary>
        /// <param name="resultT">������T�ķ��ŵĽ��,��Ϊ�������ݽ���ʽ</param>
        /// <param name="resultShan">��������ŵ�ɽ�ķ��ŵĽ��,��Ϊ�������ݽ���ʽ</param>
        /// <param name="z">����</param>
        /// <returns></returns>
        private static double FuZaiZuLiangKangHengFormula(double resultT, double resultShan, float z)
        {
            double U = �� * Math.Abs(A) * Math.Sqrt(1 + Math.Pow(Math.Abs(resultT), 2) + 2 * Math.Abs(resultT) * Math.Cos(2 * Get��() * z - resultShan));
            return U;
        }

        private static double KeBianDuanLuQiX(double resultT, double resultShan, float zd, float z)
        {
            double U = �� * Math.Abs(A) * Math.Sqrt(1 + Math.Pow(Math.Abs(resultT), 2) + 2 * Math.Abs(resultT) * Math.Cos(2 * Get��() * z - resultShan + (4 * Math.PI * zd) / RuDuanLuQi));
            return U;
        }

        /// <summary>
        /// ����ŵ�ɽ�ķ��ŵ��㷨
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
        /// ���˿ڿɱ��·��T1_a�㷨
        /// </summary>
        /// <returns></returns>
        private static double GetTl_a_EDKKBDLQ(float zd)
        {
            // double shanD = 2 * GetEDKKBDLQ��() * zd + Shan0;
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
        /// ���˿ڿɱ��·��T1_b�㷨
        /// </summary>
        /// <returns></returns>
        private static double GetTl_b_EDKKBDLQ(float zd)
        {
            //double shanD = 2 * GetEDKKBDLQ��() * zd + Shan0;
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

        private static double GetEDKKBDLQ��()
        {
            double ruc = 2 * a;
            double �� = 2 * Math.PI / RuDuanLuQi * Math.Sqrt(1 - RuDuanLuQi / ruc);
            //double �� = 2 * Math.PI / RuDuanLuQi;
            return ��;
        }


        /// <summary>
        /// �̶���ʽ,���㱴��ֵ
        /// </summary>
        /// <param name="f">Ƶ�ʷ�Χ[8.2,12.5]Ghz</param>
        /// <returns></returns>
        private static double Get��()
        {
            double c = 3 * Math.Pow(10, 8);
            double ru = c / (F * Math.Pow(10, 9));
            double ruc = 2 * a;
            double squareOut = 2 * Math.PI / ru;
            double squareIn = 1 - Math.Pow(ru / ruc, 2);
            double ���� = squareOut * Math.Sqrt(squareIn);
            return ����;
        }

        //private static double Get2��()
        //{ 


        //}
        private static double Yin_a(float l)
        {
            double Y0RX = Y0 * (Math.Pow(R, 2) + Math.Pow(X, 2));
            double topLeft = R * (Y0RX + X * Math.Tan(Get��() * l));
            double topRight = R * (Y0RX * Math.Tan(Get��() * l) - X) * Math.Tan(Get��() * l);
            double top = topLeft + topRight;
            double down = Math.Pow(Y0RX + X * Math.Tan(Get��() * l), 2) + Math.Pow(R, 2) * Math.Pow(Math.Tan(Get��() * l), 2);
            double result = Y0 * (top / down);
            return result;
        }

        private static double Yin_b(float l, float d)
        {
            double RXPow = Math.Pow(R, 2) + Math.Pow(X, 2);
            double ��l = Get��() * l;
            double top = (Y0 * RXPow * Math.Tan(��l) - X) * (Y0 * RXPow + X * Math.Tan(��l)) - Math.Pow(R, 2) * Math.Tan(��l);
            double down = Math.Pow(Y0 * RXPow + X * Math.Tan(��l), 2) + Math.Pow(R, 2) * Math.Pow(Math.Tan(��l), 2);
            double right = Y0 * (Math.Cos(Get��() * d) / Math.Sin(Get��() * d));
            double result = Y0 * (top / down) - right;
            return result;
        }

        /// <summary>
        /// �����迹ƥ���TL_aֵ
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
        /// �����迹ƥ���TL_bֵ
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
        /// �����迹ƥ���TLֵ
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
            //double Tl_aDivisionTop = Math.Pow(R, 2) + (Math.Pow(X, 2) - 1);  //Z0
            //double T1_aDivisionDown = Math.Pow(R + 1, 2) + Math.Pow(X, 2);
            //double T1_a = Tl_aDivisionTop / T1_aDivisionDown;

            double Tl_aDivisionTop = Math.Pow(R, 2) + (Math.Pow(X, 2) - Z0); 
            double T1_aDivisionDown = Math.Pow(R + Z0, 2) + Math.Pow(X, 2);
            double T1_a = Tl_aDivisionTop / T1_aDivisionDown;
            return T1_a;
        }

        private static double GetTL_b()
        {
            //double Tl_baDivisionTop = 2 * R * X;
            //double T1_bDivisionDown = Math.Pow(R + 1, 2) + Math.Pow(X, 2);
            //double T1_b = Tl_baDivisionTop / T1_bDivisionDown;

            double Tl_baDivisionTop = 2 * R * X;
            double T1_bDivisionDown = Math.Pow(R + Z0, 2) + Math.Pow(X, 2);
            double T1_b = Tl_baDivisionTop / T1_bDivisionDown;
            return T1_b;
        }
        /// <summary>
        /// ���㸺���迹�������㷨
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
            double z = shan * Calculate��p1() / (4 * Math.PI) + (2 * index + 1) * (Calculate��p1() / 4);
            double DT = GetDT(SLMCL_Start_Value, 0);
            while (z < DT)
            {
                index++;
                z = shan * Calculate��p1() / (4 * Math.PI) + (2 * index + 1) * (Calculate��p1() / 4);
            }
            double result = Math.Cos(2 * Get��() * z - shan);
            if (result == -1)
            {
                return z;
            }
            return 0;
        }
        /// <summary>
        /// ���˿ڿɱ��·����Сֵ��ȡ
        /// </summary>
        /// <param name="shan"></param>
        /// <returns></returns>
        public static double GetMinReadEDKKBDLQ(double shan, double zd)
        {
            int index = 0;
            double z = (shan - (4 * Math.PI * zd) / RuDuanLuQi) * Calculate��p1() / (4 * Math.PI) + (2 * index + 1) * (Calculate��p1() / 4);
            double DT = GetDT(SLMCL_Start_Value, 0);
            while (z < DT)
            {
                index++;
                z = (shan - (4 * Math.PI * zd) / RuDuanLuQi) * Calculate��p1() / (4 * Math.PI) + (2 * index + 1) * (Calculate��p1() / 4);
            }
            double result = Math.Cos(2 * Get��() * z - shan);
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