using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericSingleton<T> : MonoBehaviour where T : Component
{
    private static T _instance;

    public static T Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = FindObjectOfType<T>();          //인스턴스 오브젝트를 찾는다.
                if (_instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = typeof(T).Name;
                    _instance = obj.AddComponent<T>();          //생성하고 컨포넌트 붙인다.
                }
            }
            return _instance;
        }

    }

    private void Awake()
    {
        if (Instance == null)                                   //instance가 null일떄
        {
            _instance = this as T;                              
            DontDestroyOnLoad(gameObject);                      //게임 오브젝트가 Scene이 전환되고 파괴되지 않음
        }
        else if(_instance != null)
        {
            Destroy(gameObject);                                //1개로 유지시키기 위해 생성된 객체를 파괴한다.
        }
    }
}
