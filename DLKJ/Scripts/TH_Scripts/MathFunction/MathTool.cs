using System;
using UnityEngine;
namespace DLKJ
{
    public static class MathTool
    {
        private const float j = 1;
        private const float a = 0.02286f;
        private static float A = 10;//��ѹ10mv-1000mv ��ʼ���󲻱�
        public static float F = 8.2f;//Ƶ��8.2-12.5  ��ʼ���󲻱�
        private static float �� = 0;//����˥����ȡֵ[0,1] ��ʼ���󲻱�
        private static double ��;//����
        private static float X = 0;//ȡֵ��Χ[-200,200] ��ʼ���󲻱�
        private static float R = 0;//ȡֵ��Χ[0,200] ��ʼ���󲻱�
        private static float ZL = 0;//ZL=R+jX

        public static void Init()
        {
            RandomDataInit();
        }
        public static void RandomDataInit()
        {
            F = UnityEngine.Random.Range(8.2f, 12.5f);
            A = UnityEngine.Random.Range(10f, 1000f);
            �� = UnityEngine.Random.Range(0f, 1f); //Random(0.00f, 1.00f);
            X = UnityEngine.Random.Range(-200f, 200f);
            R = UnityEngine.Random.Range(0f, 200f);
            ZL = R + X;
            �� = Get��();

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
        /// ��ʽһ��(1)
        /// �����ײ������ն�ֱ�ӽӶ�·��(���Ӷ��˿�����ʱ)
        /// </summary>
        /// <param name="A">��ѹ[10-1000]</param>
        /// <param name="��">����˥����ȡֵ[0,1]</param>
        /// <param name="f">Ƶ�ʷ�Χ[8.2,12.5]Ghz</param>
        /// <param name="z">��ǰ����λ���������ײ��������Ҷ˾���</param>
        /// <returns></returns>
        public static double SLMCLXZDDLB(float z)
        {
            double U = �� * Math.Abs(A) * Math.Abs((Math.Sin(�� * z)));
            return U;
        }

        /// <summary>
        /// ���˿�����S��������
        /// </summary>
        /// <returns></returns>
        //public static double EDKWLSCSCL(float z)
        //{

        //}


        #region ���˿�����S��������
        private static float FA;
        private static float FB;
        private static float FC;
        private static float FD;
        private static float ShanA;
        private static float ShanB;
        private static float ShanC;
        private static float ShanD;
        public static double ���˿�����S��������(float z)
        {
            double �� = MathTool.��;
            return 0;
        }
        #endregion


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
        /// ��ʽһ��(4)
        /// ���˿ں�ƥ�为��
        /// </summary>
        /// <param name="z">�Ҷ˿�ʼΪ0,Խ���ź�ԴԽ��</param>
        /// <returns></returns>
        public static double EDKPPFZ(float z)
        {
            double T1 = FA;
            double Shan1 = ShanA;
            double U = �� * Math.Abs(A) * Math.Sqrt(1 + Math.Abs(Math.Pow(T1, 2)) + 2 * Math.Abs(T1) * Math.Cos(2 * �� * z - Shan1));
            return U;
        }

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
        /// 
        /// </summary>
        /// <param name="resultT">������T�ķ��ŵĽ��,��Ϊ�������ݽ���ʽ</param>
        /// <param name="resultShan">��������ŵ�ɽ�ķ��ŵĽ��,��Ϊ�������ݽ���ʽ</param>
        /// <param name="z">����</param>
        /// <returns></returns>
        private static double FuZaiZuLiangKangHengFormula(double resultT, double resultShan, float z)
        {
            double U = �� * Math.Abs(A) * Math.Sqrt(1 + Math.Pow(Math.Abs(resultT), 2) + 2 * Math.Abs(resultT) * Math.Cos(2 * �� * z - resultShan));
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




        //private static float ɽ0;
        ///// <summary>
        ///// ���˿ڿɱ��·��
        ///// </summary>
        ///// <returns></returns>
        //public static double ErDuanKouKeBianDuanLuQi()
        //{

        //}







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
            double topLeft = R * (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) + X * Math.Tan(�� * l)));
            double topRight = R * (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) * Math.Tan(�� * l - X) * Math.Tan(�� * l)));
            double Top = topLeft + topRight;
            double down = Math.Pow((200 * (Math.Pow(R, 2) + Math.Pow(X, 2)) + X * Math.Tan(�� * l)), 2) + Math.Pow(R, 2) * (Math.Pow(Math.Tan(�� * l), 2));
            return Top / down;
        }

        private static double Yin_b(float l, float d)
        {
            double topLeft = (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) + X * Math.Tan(�� * l)));
            double topRight = R * (200 * (Math.Pow(R, 2) + Math.Pow(X, 2) * Math.Tan(�� * l - X) * Math.Tan(�� * l)));
            double Top = topLeft + topRight;
            double down = Math.Pow((200 * (Math.Pow(R, 2) + Math.Pow(X, 2)) + X * Math.Tan(�� * l)), 2) + Math.Pow(R, 2) * (Math.Pow(Math.Tan(�� * l), 2));
            return Top / down + Math.Tan(�� * d);
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
        /// ���㸺���迹�������㷨
        /// </summary>
        /// <returns></returns>
        private static double GetTL()
        {
            return Math.Sqrt(Math.Pow(GetTL_a(), 2) + Math.Pow(GetTL_b(), 2));
        }
    }
}