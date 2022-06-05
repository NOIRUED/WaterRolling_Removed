using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWaterFlow : MonoBehaviour
{
    [SerializeField] private Vector3 BasicWaterFlow=Vector3.forward;
    [SerializeField] public Vector3 NewWaterFlow;

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<BoatMoveController>().WaterFlow = this.NewWaterFlow;
    }

    private void OnTriggerExit(Collider other)
    {
        other.GetComponent<BoatMoveController>().WaterFlow = this.BasicWaterFlow;
    }
}
