using UnityEngine;
namespace Common
{
    public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T>
    {
        private static T _instance;
        public static T GetInstance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();
                    if (_instance == null)
                        _instance = new GameObject("Singleton of" + typeof(T)).AddComponent<T>();
                }
                return _instance;
            }
        }

        private void Awake()
        {
            _instance = this as T;
            Init();
        }
        protected virtual void Init()
        {

        }
    }
}