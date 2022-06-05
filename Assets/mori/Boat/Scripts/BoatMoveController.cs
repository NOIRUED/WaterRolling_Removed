using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoatMoveController : MonoBehaviour
{
    //全てboat関係
    [SerializeField] private Vector3 boatVelocity;
    [SerializeField] private Rigidbody boatRb;
    [SerializeField] float moveSpeed=10.0f;
    //[SerializeField] float maxMoveSpeed=5.0f;
    [SerializeField] public float rotationAngle=0;
    [SerializeField] float addRotationAngleBase = 20.0f;


    //水関係
    [SerializeField] public Vector3 WaterFlow = new Vector3(0.0f, 0.0f, 0.01f);
    [SerializeField] float WaterFriction = 0.05f;


    [SerializeField] int boatMoveFluency=25;//0.5f/boatMoveFluency　時間毎に回転

    //転覆関係
    public bool isUpsideDown = false;
    private int UpsideDownBouncePower = 16;

    public enum Direction
    {
        RightForward,
        LeftForward
    }



    private void FixedUpdate()
    {
        boatRb.angularVelocity = Vector3.zero;

        //水の水流・抵抗
        boatVelocity *= 1-WaterFriction;
        boatRb.velocity = boatVelocity + WaterFlow;
       
    }


    float addRotationAngle;
    public IEnumerator Row(Direction direction)//船をこいで進む。
    {

        //方向directionに応じて船を動かす角度を決定
        switch (direction)
        {
            case Direction.RightForward:
                addRotationAngle = this.addRotationAngleBase;
                //rotationAngle += addRotationAngle;
                //this.transform.Rotate(new Vector3(0, addRotationAngle, 0));
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationAngle, 0), 5.0f);
                break;
            case Direction.LeftForward:
                addRotationAngle = -this.addRotationAngleBase;
                //rotationAngle -= addRotationAngle;
                //this.transform.Rotate(new Vector3(0, -addRotationAngle, 0));
                //transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.Euler(0, rotationAngle, 0), 5.0f);
                break;
        }

        for(int i = 0; i < boatMoveFluency; i++)
        {
            if(isUpsideDown) yield break;//転覆時は移動しない

            rotationAngle -= addRotationAngle / boatMoveFluency;
            this.transform.Rotate(new Vector3(0, addRotationAngle/boatMoveFluency, 0));
            boatVelocity = this.transform.rotation * Vector3.forward * moveSpeed;
            //boatRb.velocity = boatVelocity;
            yield return new WaitForSeconds(0.5f/ boatMoveFluency);
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
            this.transform.Rotate(0, 160/(i+1), 0); //1/(i+1)の和は4.679871,360*a/4.5=80a
            //this.transform.Rotate(0,0,3,Space.World);
            yield return new WaitForSeconds(0.01f);
        }

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < 60; i++)//復帰
        {
            this.transform.Rotate(0, 0, 3, Space.World);
            yield return new WaitForSeconds(0.01f);
        }

        isUpsideDown = false;
        yield break;
    }





    
}
