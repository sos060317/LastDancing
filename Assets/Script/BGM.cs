using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    private void Update()
    {
        if (GameManager.Instance.curPlayer == null)
            return;

        transform.position = GameManager.Instance.curPlayer.position;
    }
}