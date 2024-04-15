using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê

    private static LevelManager instance = null;

    public static LevelManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

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
}