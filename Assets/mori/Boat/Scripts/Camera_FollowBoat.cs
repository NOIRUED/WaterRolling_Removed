using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_FollowBoat : MonoBehaviour
{
    private Vector3 relativePosVec;
    //[SerializeField] Vector3 rotationVec;
    [SerializeField] public Transform boatPos;

    private void Start()
    {
        relativePosVec = this.transform.position - boatPos.position;
    }

    //船との相対座標を一定に保つ
    private void Update()
    {
        this.transform.position = boatPos.position + relativePosVec;
        //this.transform.rotation = Quaternion.LookRotation(rotationVec);
    }
}
