using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpGetter : MonoBehaviour
{
    [SerializeField] private GameObject particle;

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.CompareTag("EXP"))
        {
            Instantiate(particle, other.transform.position, Quaternion.identity);

            Destroy(other.gameObject);
        }
    }
}