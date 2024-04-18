using System;
using UnityEditor;
using UnityEngine;
public class Singleton<T>: MonoBehaviour
where T: MonoBehaviour
{
    public bool global = true;
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindAnyObjectByType<T>();
            }

            return _instance;
        }
    }

    private void Awake()
    {
        if (global)
        {
            DontDestroyOnLoad(gameObject);
        }
        OnStart();
    }

    protected virtual void OnStart()
    {
        
    }
}