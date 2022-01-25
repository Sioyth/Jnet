using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExecuteOnMainThread : MonoBehaviour
{
    private List<Action> _currentActions;
    private static ExecuteOnMainThread _instance;

    public static ExecuteOnMainThread Instance { get => _instance; set => _instance = value; }
    public List<Action> CurrentActions { get => _currentActions; set => _currentActions = value; }

    public ExecuteOnMainThread()
    {
        if (_instance == null)
            _instance = this;
        else
            Debug.LogWarning("A ThreadManager is already instanced.");

        _currentActions = new List<Action>();
    }

    public void Execute(Action a)
    {
        _currentActions.Add(a);
    }

    // Bugged atm
    public void ExecuteCoroutine(IEnumerator co)
    {
        _currentActions.Add(() => _instance.StartCoroutine(co));
    }

    private void Update()
    {
        if (_currentActions.Count < 0)
            return;

        for (int i = 0; i < _currentActions.Count; i++)
        {
            _currentActions[i]?.Invoke();
        }

        _currentActions.Clear();
    }


}
