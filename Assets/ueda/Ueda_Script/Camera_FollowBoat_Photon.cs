using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class Camera_FollowBoat_Photon : MonoBehaviourPun
{
    private Vector3 relativePosVec;
    //[SerializeField] Vector3 rotationVec;
    private bool isFollowing = false;

    private void Start()
    {

        if (photonView.IsMine) {
            relativePosVec.x = 0f;
            relativePosVec.y = Camera.main.transform.position.y - this.transform.position.y;
            relativePosVec.z = relativePosVec.z +10f;
        }
    }

    //船との相対座標を一定に保つ
    private void LateUpdate()
    {
        if (photonView.IsMine) {
            Camera.main.transform.position = this.transform.position + relativePosVec;
            //this.transform.rotation = Quaternion.LookRotation(rotationVec);
        }
    }
    /*
    public void OnStartFollowing() {
        isFollowing = true;
    }
    */
}
