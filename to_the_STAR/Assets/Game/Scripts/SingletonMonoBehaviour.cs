using UnityEngine;

namespace Game
{
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = FindAnyObjectByType<T>();
                if (_instance != null)
                {
                    return _instance;
                }

                _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                return _instance;
            }
        }

        protected void Awake()
        {
            if (_instance == null)
            {
                _instance = this as T;
                DontDestroyOnLoad(gameObject);

                OnAwake();

                return;
            }

            Destroy(gameObject);
        }

        public virtual void Init()
        {
        }

        protected virtual void OnAwake()
        {
        }
    }
}