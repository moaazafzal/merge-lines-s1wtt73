using UnityEngine;

namespace AL_CustomAdmobAds.Scripts.ALAds
{
    public class SceneSingleton<T> : MonoBehaviour where T : SceneSingleton<T>
    {
        private static T m_Instance = null;
        public static T Instance
        {
            get
            {
                if (m_Instance == null)
                {
                    m_Instance = FindObjectOfType<T>();
                    // fallback, might not be necessary.
                    if (m_Instance == null)
                        m_Instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    DontDestroyOnLoad(m_Instance.gameObject);
                }
                return m_Instance;
            }
        }
    }
}
