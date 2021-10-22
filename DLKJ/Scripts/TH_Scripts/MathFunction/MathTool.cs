using System;
using System.Collections.Generic;
using UnityEngine;
using System.Numerics;
using Common;
namespace DLKJ
{
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
    public class MathTool
    {
        public const float SLMCL_Start_Value = 0.055f;

        public static LabReportCorrect1Data report1CorrectAnswer;
        public static LabReport2Data report2CorrectAnswer;
        public static LabReport3Data report3CorrectAnswer;
        //   private const float j = 1;
        private const float a = 0.02286f;
        public const float Y0 = 0.01f;
        public static float A { get; set; } /*= 10*///��ѹ10mv-1000mv ��ʼ���󲻱�
        public static float F { get; set; } /*= 8.2f*///Ƶ��8.2-12.5  ��ʼ���󲻱�
        public static float �� { get; set; }//����˥����ȡֵ[0,1] ��ʼ���󲻱�
        public static float distanceZ { get; set; }//z��λ�þ��������ײ������������
        private static double EDKKBDLQ��;//���˿�+�ɱ��·���ı���
        private static float X = 0;//ȡֵ��Χ[-200,200] ��ʼ���󲻱�
        private static float R = 0;//ȡֵ��Χ[0,200] ��ʼ���󲻱�
        private static float ZL = 0;//ZL=R+jX
        private static float j = 1;
        private static float Z0 = 100;//100ŷķ
        private static double S11 = 0;
        private static double S12 = 0;
        private static double S22 = 0;
        public static float couplingFactorA;//��϶�����˵�ѹģA
        public static float couplingFactorC;//��϶�C

        public static void Init(/*FormulaData data*/)
        {
            //A = data.A;
            //F = data.F;
            //�� = data.��;
            //distanceZ = data.distanceZ;
            RandomDataInit();
        }
        public static void RandomDataInit()
        {
            F = UnityEngine.Random.Range(8.2f, 12.5f);
            A = UnityEngine.Random.Range(2f, 2000f);
            �� = UnityEngine.Random.Range(0f, 1f); //Random(0.00f, 1.00f);
            X = UnityEngine.Random.Range(-200f, 200f);
            R = UnityEngine.Random.Range(0f, 200f);
            ZL = R + j * X;
            //����һ
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
            //����һ
            //ShanA = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            //ShanC = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            //ShanB = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            //ShanD = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            ShanA = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            ShanC = ShanA;
            ShanB = 0.5f * (ShanA + ShanC + Math.PI);
            ShanD = UnityEngine.Random.Range(0, 2 * Mathf.PI);
            EDKKBDLQ�� = GetEDKKBDLQ��();

            S11 = new Complex(FA * Math.Cos(ShanA), FA * Math.Sin(ShanA)).Magnitude;
            S12 = new Complex(FB * Math.Cos(ShanB), FB * Math.Sin(ShanB)).Magnitude;
            S22 = new Complex(FC * Math.Cos(ShanC), FC * Math.Sin(ShanC)).Magnitude;
        }

        public static void FixedCorrectCalculate()
        {
            report1CorrectAnswer.SourceFrequency = F;//�ź�ԴƵ��
            report1CorrectAnswer.SourceVoltage = A;//��ѹ
            report1CorrectAnswer.Attenuator = ��;//˥����
            report1CorrectAnswer.EquivalentSectionPosition = GetDT(0.055f, 0);//��һ����Ч�����λ��
            report1CorrectAnswer.InputWavelength = Calculate��p1();//����˲���
            report1CorrectAnswer.VariableShortCircuitFirstPos = GetFirstMinBoundKBDLQ(1);//�ɱ��·����һ���ڵ���Сֵλ��
            report1CorrectAnswer.VariableShortCircuitSecondPos = report1CorrectAnswer.VariableShortCircuitFirstPos + 0.5f * RuDuanLuQi;//�ɱ��·���ڶ����ڵ���Сֵλ��
            report1CorrectAnswer.VariableWavelengthInShortCircuit = RuDuanLuQi;//�ɱ��·���в�����p2
            //��·����λ��
            report1CorrectAnswer.OpenLoadPosition = new List<double>();
            report1CorrectAnswer.OpenLoadPosition.Add(report1CorrectAnswer.VariableShortCircuitFirstPos + RuDuanLuQi * 0.25f);
            report1CorrectAnswer.OpenLoadPosition.Add(report1CorrectAnswer.VariableShortCircuitSecondPos + RuDuanLuQi * 0.25f);

            report1CorrectAnswer.WaveNodePosShortCircuit = GetMinZUpperDTEDKDLB();//���ڵ�λ���ն˶�·
            //���ڵ�λ���ն˿�·
            report1CorrectAnswer.WaveNodePosShortTerminal = GetMinReadUpperDTEDKKEDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos.ToString()));
            report1CorrectAnswer.WaveNodePosShortMatching = GetMinZUpperDTEDKPPFZ();//���ڵ�λ���ն�ƥ��


            report1CorrectAnswer.PhaseAngleCircuit = CalculateShan(GetTl_a(), GetTl_b());//����ն˶�·
            report1CorrectAnswer.PhaseAngleTerminal = CalculateShan(GetTl_a_EDKKBDLQ(float.Parse(GetFirstMinBoundKBDLQ(1).ToString())), GetTl_b_EDKKBDLQ(float.Parse(GetFirstMinBoundKBDLQ(1).ToString())));//����ն˿�·
            report1CorrectAnswer.PhaseAngleMatching = ShanA;//����ն�ƥ��

            report1CorrectAnswer.StandingWaveRatioCircuit = SWREDKTODLB();//פ�����ն˶�·
            report1CorrectAnswer.StandingWaveRatioTerminal = SWREDKKBDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos.ToString("#0.000000")));//פ�����ն˿�·
            report1CorrectAnswer.StandingWaveRatioCircuit = SWREDKPPFZ();//פ�����ն�ƥ��

            report1CorrectAnswer.input��1S = GetTl();
            report1CorrectAnswer.input��10 = GetT1_EDKKBDLQ(float.Parse(report1CorrectAnswer.VariableShortCircuitFirstPos.ToString()));
            report1CorrectAnswer.input��1L = FA;

            report1CorrectAnswer.ReflectionCoefficient��1S = S11 - (Math.Pow(S12, 2) / (1 + S22));//����ϵ��T1S
            report1CorrectAnswer.ReflectionCoefficient��10 = S11;//����ϵ��T10
            report1CorrectAnswer.ReflectionCoefficient��1L = S11;//����ϵ��T1L

            report1CorrectAnswer.inputS11 = S11;
            report1CorrectAnswer.inputS12S21 = S12;
            report1CorrectAnswer.inputS22 = S22;

        }
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
            int startBandCount = (int)((startValue + offect) / everyBandLength) + 1;
            double result = startBandCount * everyBandLength;
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
        /// ��ȡ���˿ںͿɱ��·����������
        /// </summary>
        /// <returns></returns>
        public static double GetMaxReadEDKKBDLQ(float zd)
        {
            double z = CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)) * Calculate��p1() / (4 * Math.PI) + 2 * (Calculate��p1() / 4);
            double result = Math.Cos(2 * Get��() * z - CalculateShan(GetTl_a_EDKKBDLQ(zd), GetTl_b_EDKKBDLQ(zd)));
            if (result == 1)
            {
                double Umax = �� * Math.Abs(A) * Math.Abs((1 + GetTl_a_EDKKBDLQ(zd)));
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
                double Umin = �� * Math.Abs(A) * Math.Abs((1 - GetT1_EDKKBDLQ(zd)));
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
            double z = GetMinRead(shan);
            double min = �� * Math.Abs(A) * Math.Abs((1 - GetT1_EDKKBDLQ(zd)));
            return z;
        }

        /// <summary>
        /// ��ȡ�ɱ��·����N��������Сֵ
        /// </summary>
        /// <returns></returns>
        public static double GetFirstMinBoundKBDLQ(int bound)
        {
            //float boundValue = RuDuanLuQi;//���ε�ֵ
            //int startBandCount = (int)(startValue / boundValue) + n;
            //float result = startBandCount * boundValue;

            //Debug.Log("������:" + ErDuanKouKeBianDuanLuQi(result, float.Parse(GetDT(SLMCL_Start_Value, 0).ToString())));
            //return result;
            int k = 0;
            while ((Math.PI - Shan0) / GetEDKKBDLQ��() + ((k * 0.5) * RuDuanLuQi) < 0)
            {
                k++;
            }
            for (int i = 0; i < bound; i++)
            {
                if (i == bound - 1)
                {
                    double result = (Math.PI - Shan0) / GetEDKKBDLQ��() + ((k * 0.5) * RuDuanLuQi);
                    return result;
                }
                k++;
            }
            return 0;

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
            for (int i = 0; i < 10; i++)
            {
                double z = (CalculateShan(GetTL_a(), GetTL_b()) * Calculate��p1()) / (4 * Math.PI) + 2 * i * (Calculate��p1() / 4);
                double result = Math.Cos(2 * Get��() * z - CalculateShan(GetTL_a(), GetTL_b()));
                if (result == 1)
                {
                    double Umax = �� * Math.Abs(A) * Math.Abs((1 + GetTL()));
                    return Umax;
                }
            }
            return 0;
        }
        /// <summary>
        /// ��ȡ��С����
        /// </summary>
        /// <returns></returns>
        public static double GetMinRead_FZZKCL()
        {
            for (int i = 0; i < 10; i++)
            {
                double z = (CalculateShan(GetTL_a(), GetTL_b()) * Calculate��p1()) / (4 * Math.PI) + (2 * i + 1) * (Calculate��p1() / 4);
                double result = Math.Cos(2 * Get��() * z - CalculateShan(GetTL_a(), GetTL_b()));
                if (result == -1)
                {
                    double Umin = �� * Math.Abs(A) * Math.Abs((1 - GetTL()));
                    return Umin;
                }
            }
            return 0;
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

        public static double GetMinReadFZKZPP(float l, float d)
        {
            for (int i = 0; i < 10; i++)
            {
                double z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculate��p1() / (4 * Math.PI) + (2 * i + 1) * (Calculate��p1() / 4);
                double result = Math.Cos(2 * Get��() * z - CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)));
                if (result == -1)
                {
                    double min = �� * Math.Abs(A) * (1 - GetFZZKPPTL(l, d));
                    return min;
                }
            }
            return 0;
        }
        public static double GetMaxReadFZKZPP(float l, float d)
        {
            for (int i = 0; i < 10; i++)
            {
                double z = CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)) * Calculate��p1() / (4 * Math.PI) + (2 * i) * (Calculate��p1() / 4);
                double result = Math.Cos(2 * Get��() * z - CalculateShan(GetFZZKPPTL_a(l, d), GetFZZKPPTL_b(l, d)));
                if (result == -1)
                {
                    double min = �� * Math.Abs(A) * (1 + GetFZZKPPTL(l, d));
                    return min;
                }
            }
            return 0;
        }


        #endregion

        #region ʵ�������������������϶Ȳ���
        /// <summary>
        /// �����������϶Ȳ���
        /// </summary>
        /// <returns></returns>
        public static double CouplingFactorObserved()
        {
            double U3 = Math.Abs(couplingFactorA) / Math.Sqrt(Math.Pow(10, couplingFactorC / 20));
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


        /// <summary>
        /// ����ŵ�ɽ�ķ��ŵ��㷨
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
        /// ���˿ڿɱ��·��T1_a�㷨
        /// </summary>
        /// <returns></returns>
        private static double GetTl_a_EDKKBDLQ(float zd)
        {
            double shanD = GetEDKKBDLQ��() * zd + Shan0;
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
            double shanD = GetEDKKBDLQ��() * zd + Shan0;
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
        private static double Yin_a(float l)
        {
            double topLeft = R * (Y0 * (Math.Pow(R, 2) + Math.Pow(X, 2)) + X * Math.Tan(Get��() * l));
            double topRight = R * (Y0 * (Math.Pow(R, 2) + Math.Pow(X, 2) * Math.Tan(Get��() * l) - X) * Math.Tan(Get��() * l));
            double Top = topLeft + topRight;
            double Down = Math.Pow((Y0 * (Math.Pow(R, 2) + Math.Pow(X, 2)) + X * Math.Tan(Get��() * l)), 2) + Math.Pow(R, 2) * (Math.Pow(Math.Tan(Get��() * l), 2));
            double result = Y0 * (Top / Down);
            return result;
        }

        private static double Yin_b(float l, float d)
        {
            double RPow = Math.Pow(R, 2);
            double Tan��l = Math.Tan(Get��() * l);
            double topLeft = Y0 * (RPow + Math.Pow(X, 2)) * Tan��l - X;
            double topRight = Y0 * (RPow + Math.Pow(X, 2)) + X * Tan��l - RPow * Tan��l;
            double Top = topLeft + topRight;
            double down = Math.Pow(Y0 * (RPow + X * Tan��l), 2) + RPow * Math.Pow(Tan��l, 2);
            double right = Math.Cos(Get��() * d) / Math.Sin(Get��() * d);
            double result = Y0 * (Top / down) - right;
            return result;
        }

        /// <summary>
        /// �����迹ƥ���TL_aֵ
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
        /// �����迹ƥ���TL_bֵ
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
        /// �����迹ƥ���TLֵ
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