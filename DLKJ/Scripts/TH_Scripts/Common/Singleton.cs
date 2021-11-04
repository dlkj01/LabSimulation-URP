

using System;
namespace Common
{
    public class Singleton<T> where T : class, new()
    {
        private static T _instance;

        protected Singleton()
        {
        }

        public static T GetInstance
        {
            get
            {
                if (Singleton<T>._instance == null)
                {
                    Singleton<T>.CreateInstance();
                }
                return Singleton<T>._instance;
            }
        }

        #region 对外辅助接口
        public static void CreateInstance()
        {
            if (Singleton<T>._instance == null)
            {
                Singleton<T>._instance = Activator.CreateInstance<T>();
            }
        }

        public static void DestroyInstance()
        {
            if (Singleton<T>._instance != null)
            {
                Singleton<T>._instance = null;
            }
        }

        public static bool HasInstance()
        {
            return (Singleton<T>._instance != null);
        }
        #endregion
    }
}