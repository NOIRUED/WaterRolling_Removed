using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;

public class BoatMoveController_Photon : MonoBehaviourPun{
    //全てboat関係
    [SerializeField] private Vector3 boatVelocity;
    [SerializeField] private Rigidbody boatRb;
    [SerializeField] float moveSpeed=10.0f;
    //[SerializeField] float maxMoveSpeed=5.0f;
    [SerializeField] public float rotationAngle=0;
    [SerializeField] float addRotationAngleBase = 20.0f;

    //水関係
    [SerializeField] Vector3 WaterFlow = new Vector3(0.0f, 0.0f, 0.01f);
    [SerializeField] float WaterFriction = 0.05f;
    public bool RaceReady = false;
    public bool isUpsideDown = false;
    private int UpsideDownBouncePower = 16;

    [SerializeField] int boatMoveFluency=25;//0.5f/boatMoveFluency　時間毎に回転


    private Vector3 lastFrameVelocity;

    void Start() {


    }

    private void FixedUpdate()
    {
        boatRb.angularVelocity = Vector3.zero;

        //水の水流・抵抗
        if (RaceReady == true) {
            boatVelocity *= 1 - WaterFriction;
            boatRb.velocity = boatVelocity + WaterFlow; lastFrameVelocity = boatVelocity;
        } else if (RaceReady == false) {
            boatVelocity *= 0;
        }
    }

    float addRotationAngle;

    [PunRPC]
    public void RowDirection(string direction) {
        switch (direction) {
            case "RightForward":
                addRotationAngle = this.addRotationAngleBase;
                //rotationAngle += addRotationAngle;
                //this.transform.Rotate(new Vector3(0, addRotationAngle, 0));
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationAngle, 0), 5.0f);
                break;
            case "LeftForward":
                addRotationAngle = -this.addRotationAngleBase;
                //rotationAngle -= addRotationAngle;
                //this.transform.Rotate(new Vector3(0, -addRotationAngle, 0));
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationAngle, 0), 5.0f);
                break;
            case "RightBack":
                addRotationAngle = -this.addRotationAngleBase;
                break;
            case "LeftBack":
                addRotationAngle = this.addRotationAngleBase;
                break;
        }
        StartCoroutine(Row(direction));
    }

    public IEnumerator Row(string direction)
    {
        if (isUpsideDown) yield break;//転覆時は移動しない

        for (int i = 0; i < boatMoveFluency; i++) {
            rotationAngle -= addRotationAngle / boatMoveFluency;
            this.transform.Rotate(new Vector3(0, addRotationAngle / boatMoveFluency, 0));
            if (direction == "RightForward" || direction == "LeftForward") boatVelocity = this.transform.rotation * Vector3.forward * moveSpeed;
            else boatVelocity = this.transform.rotation * Vector3.forward * -moveSpeed * 0.5f;
            //boatRb.velocity = boatVelocity;
            yield return new WaitForSeconds(0.5f / boatMoveFluency);
        }
    }

    void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag=="Ball") {
            boatVelocity = Vector3.Reflect(lastFrameVelocity * 1.5f, other.contacts[0].normal);
        }
        if (other.gameObject.tag == "Cylinder") {
            StartCoroutine(UpsideDown());
        }
    }

    void OnCollisionStay(Collision other) {
        if (other.gameObject.tag == "Cylinder") {
            StartCoroutine(UpsideDown());
        }
    }

    public IEnumerator UpsideDown()//転覆する。一定時間行動禁止。
{
        if (isUpsideDown) yield break;
        isUpsideDown = true;

        this.transform.Rotate(0, 0, 180, Space.World);
        boatVelocity = Vector3.back * UpsideDownBouncePower;

        for (int i = 0; i < 60; i++)//転覆
        {
            this.transform.Rotate(0, 160 / (i + 1), 0); //1/(i+1)の和は4.679871,360*a/4.5=80a
            //this.transform.Rotate(0,0,3,Space.World);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(0.1f);

        for (int i = 0; i < 60; i++)//復帰
        {
            this.transform.Rotate(0, 0, 3, Space.World);
            yield return new WaitForSeconds(0.01f);
        }

        isUpsideDown = false;
        yield break;
    }





}
