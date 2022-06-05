using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpsideDownGimmick : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine( other.GetComponent<BoatMoveController>().UpsideDown());
    }

    private void OnCollisionEnter(Collision collision)
    {
        StartCoroutine(collision.gameObject.GetComponent<BoatMoveController>().UpsideDown());
    }
}
