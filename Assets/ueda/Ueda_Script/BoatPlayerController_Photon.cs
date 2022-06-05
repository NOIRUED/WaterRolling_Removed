using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using System;

public class BoatPlayerController_Photon : MonoBehaviourPun
{
    //boat
    [SerializeField] GameObject boatObj;
    [SerializeField] BoatMoveController_Photon boatMoveController_Photon;
    [SerializeField] BoatDataScript boatdatascript;
    [SerializeField] GameObject Player1Name;
    [SerializeField] GameObject Player2Name;
    [SerializeField] GameObject BoatUI;

    //人の動き
    [SerializeField] private Animator playerAnimator;

    //フリック関連
    [SerializeField] Vector3 TouchStartPos;
    [SerializeField] Vector3 TouchEndPos;
    [SerializeField] float SwipeDistance = 0.1f;//スワイプを判定する距離

    public static GameObject LocalPlayerInstance;
    public int actorNumber;
    [SerializeField] AudioClip watersound1;
    [SerializeField] AudioClip watersound2;
    [SerializeField] AudioSource audioSource;
    bool randval = true;


    private void Awake() {
        if (photonView.IsMine) {
            BoatPlayerController_Photon.LocalPlayerInstance = this.gameObject;
            actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
        }
        BoatUI = GameObject.Find("/Canvas/BoatUI");
        Player1Name = BoatUI.transform.transform.Find("Player1Name").gameObject;
        Player2Name = BoatUI.transform.transform.Find("Player2Name").gameObject;

    }

    private void Update()
    {
        if (photonView.IsMine == false && PhotonNetwork.IsConnected == true) {
            return;
        }

        boatObj = transform.parent.gameObject;
        boatMoveController_Photon = boatObj.GetComponent<BoatMoveController_Photon>();
        BoatDataScript.BoatData boatData = boatObj.GetComponent<BoatDataScript>().boatData;
        boatdatascript = boatObj.GetComponent<BoatDataScript>();
        if (boatMoveController_Photon.RaceReady == false) {
            Player1Name.GetComponent<Text>().text = boatData.player1name;
            Player2Name.GetComponent<Text>().text = boatData.player2name;
        } else if (boatMoveController_Photon.RaceReady == true) {
            BoatUI.SetActive(false);
        }



        /*
        if (Input.GetKey(KeyCode.RightArrow)) StartCoroutine(Row("RightForward"));
        else if (Input.GetKey(KeyCode.LeftArrow)) StartCoroutine(Row("LeftForward"));
        if (Input.GetKey(KeyCode.UpArrow)) StartCoroutine(Row("RightBack"));
        else if (Input.GetKey(KeyCode.DownArrow)) StartCoroutine(Row("LeftBack"));
        */
        SwipeCheck();


    }


    //船を漕ぐ
    bool rowing = false;
    private IEnumerator Row(string direction)
    {
        if (rowing) yield break;
        rowing = true;

        if (actorNumber % 2 == 0) {
            switch (direction) {
                /*
                case "RightForward":
                    playerAnimator.SetBool("RightForward", true);
                    break;
                */
                case "LeftForward":
                    playerAnimator.SetBool("LeftForward", true);
                    break;
                /*
                case "RightBack":
                    playerAnimator.SetBool("RightBack", true);
                    break;
                */
                case "LeftBack":
                    playerAnimator.SetBool("LeftBack", true);
                    break;
            }
        } else if (actorNumber % 2 == 1) {
            switch (direction) {
                case "RightForward":
                    playerAnimator.SetBool("RightForward", true);
                    break;
                /*
                case "LeftForward":
                    playerAnimator.SetBool("LeftForward", true);
                    break;
                */
                case "RightBack":
                    playerAnimator.SetBool("RightBack", true);
                    break;
                /*
                case "LeftBack":
                    playerAnimator.SetBool("LeftBack", true);
                    break;
                */
            }
        }
        if (randval == true) {
            audioSource.PlayOneShot(watersound1);
            randval = false;
        } else {
            audioSource.PlayOneShot(watersound2);
            randval = true;
        }

        yield return new WaitForSeconds(0.5f);
        PhotonView photonView = PhotonView.Get(this);
        boatMoveController_Photon.photonView.RPC("RowDirection", RpcTarget.All, direction);
        //StartCoroutine(boatMoveController_Photon.Row(direction));

        yield return new WaitForSeconds(0.5f);

        playerAnimator.SetBool("RightForward", false);
        playerAnimator.SetBool("LeftForward", false);
        playerAnimator.SetBool("RightBack", false);
        playerAnimator.SetBool("LeftBack", false);
        rowing = false;
        yield break;

    }



    //スワイプで船を漕ぐ関数発火
    private void SwipeCheck()
    {

            if (Input.GetMouseButtonDown(0))
            {
                TouchStartPos = Input.mousePosition;
                //Debug.Log((TouchStartPos.x-Screen.width/2)+","+(TouchStartPos.y-Screen.height/2));
            }
            /*
            else if (Input.GetMouseButton(0))
            {
                TouchEndPos = Input.mousePosition;
                if ((TouchStartPos - TouchEndPos).magnitude > SwipeDistance)//一定以上でスワイプ
                {
                    Swiped();
                }
            }
            */
            
            else if (Input.GetMouseButtonUp(0))
            {
                TouchEndPos = Input.mousePosition;
                if ((TouchStartPos - TouchEndPos).magnitude > SwipeDistance)//一定以上でスワイプ
                {
                    Swiped();
                }
            }

    }


    //スワイプした時、船を漕ぐ
    private void Swiped()
    {

        float PosX = (TouchStartPos.x + TouchEndPos.x)/2 - Screen.width/2;
        float PosY = (TouchStartPos.y + TouchEndPos.y)/2 - Screen.height/2;

        //船の角度だけマイナス方向に回転した時のx座標が右の時は右のオールを漕ぐ
        float Angle = boatObj.transform.rotation.eulerAngles.y;
      
        float JudgePosX = PosX * Mathf.Cos(Mathf.PI*Angle/180) - PosY * Mathf.Sin(Mathf.PI*Angle/180);
        //Debug.Log(JudgePosX);
        if (actorNumber % 2 == 1) {
            if (TouchStartPos.y - TouchEndPos.y > 0) StartCoroutine(Row("RightForward"));
            else if (TouchStartPos.y - TouchEndPos.y < 0) StartCoroutine(Row("RightBack"));
        } else if (actorNumber % 2 == 0) {
            if (TouchStartPos.y - TouchEndPos.y > 0) StartCoroutine(Row("LeftForward"));//マウスダウンした場所で左右判定
            else if (TouchStartPos.y - TouchEndPos.y < 0) StartCoroutine(Row("LeftBack"));
        }
        /*
        if (JudgePosX > 0 && TouchStartPos.y - TouchEndPos.y > 0) StartCoroutine(Row("LeftForward"));//マウスダウンした場所で左右判定
        else if (JudgePosX > 0 && TouchStartPos.y - TouchEndPos.y < 0) StartCoroutine(Row("LeftBack"));
        else if (JudgePosX < 0 && TouchStartPos.y - TouchEndPos.y > 0) StartCoroutine(Row("RightForward"));
        else if (JudgePosX < 0 && TouchStartPos.y - TouchEndPos.y < 0) StartCoroutine(Row("RightBack"));
        */
        TouchStartPos = Vector3.zero;
        TouchEndPos = Vector3.zero;

    }





}