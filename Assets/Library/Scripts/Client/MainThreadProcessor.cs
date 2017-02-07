using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
 * Class that enqueues tasks to be executed in the main thread.
 * 
 * Credits: https://github.com/PimDeWitte/UnityMainThreadDispatcher/blob/master/UnityMainThreadDispatcher.cs
 * */
public class MainThreadProcessor : MonoBehaviour
{
    //This is the entity that must handle UI changes (if any) from the received message.
    private IUIHandler clientStrategy;

    private static readonly Queue<Action> _executionQueue = new Queue<Action>();
    private static MainThreadProcessor _instance = null;

    public static MainThreadProcessor Instance()
    {
        if (!Exists())
        {
            throw new Exception("MainThreadProcessor could not find the UnityMainThreadDispatcher object. Please ensure you have added the MainThreadExecutor Prefab to your scene.");
        }
        return _instance;
    }

    public static bool Exists()
    {
        return _instance != null;
    }

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            clientStrategy = GetComponent<IUIHandler>();
            if (clientStrategy == null) {
                Application.Quit();
                throw new Exception("Please add a client strategy that inherits from IUIHandler for UI updates references.");
            }
            //inputManager = GetComponent<InputManager>();
            DontDestroyOnLoad(this.gameObject);
        }
    }
    public void Update()
    {
        lock (_executionQueue)
        {
            while (_executionQueue.Count > 0)
            {
                _executionQueue.Dequeue().Invoke();
            }
        }
    }
    public void Enqueue(IEnumerator action)
    {
        lock (_executionQueue)
        {
            _executionQueue.Enqueue(() => {
                StartCoroutine(action);
            });
        }
    }

    public IEnumerator processMessage(string text)
    {
        if (clientStrategy != null) {
            clientStrategy.processMessage (text);
        }
        yield return null;
    }
}
