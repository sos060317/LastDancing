using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TimeManager : MonoBehaviour
{
    public Action TimeStopAction;
    public Action TimePlayAction;

    private static TimeManager instance = null;

    public static TimeManager Instance
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void TimeStop()
    {
        TimeStopAction?.Invoke();
    }

    public void TimePlay()
    {
        TimePlayAction?.Invoke();
    }
}