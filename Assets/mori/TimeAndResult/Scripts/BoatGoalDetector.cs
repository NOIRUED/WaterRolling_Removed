using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;

public class BoatGoalDetector : MonoBehaviour
{

    [SerializeField] ResultCanvasController resultCanvasController;
    [SerializeField] RaceTimeController raceTimeController;
    [SerializeField] RaceInfScript raceInfScript;

    [SerializeField] GameObject goalCelebrationEffect;
    public GameObject GameManager;
    Ueda_GameManager gamemanager;

    [SerializeField] AudioClip goalCheerSound;
    [SerializeField] AudioSource audioSource;

    private void Start()
    {
        //合計のboatの数を指定
        gamemanager = GameManager.GetComponent<Ueda_GameManager>();

    }

    private void OnTriggerEnter(Collider other)
    {
        //船の情報
        BoatDataScript.BoatData boatData = other.gameObject.GetComponent<BoatDataScript>().boatData;

        //どちらも0の時のみ「まだゴールしていない」と判断。これによりゴール後再度ゴールを潜る、という操作を無効にする
        if (boatData.goalTimeMinutes != 0 || boatData.goalTimeSeconds != 0.0f) return;


        //船の情報にゴール順位及び時間を追加し、結果の欄に追加する。ゴールしたとしてエフェクト表示
        resultCanvasController.createResultRow(boatData, raceTimeController.minutes, raceTimeController.seconds);
        ShowGoalCerebrateEffect(other.transform );
        audioSource.PlayOneShot(goalCheerSound);


        //自分がゴールした時にゴール表示
        if (boatData.player1id == gamemanager.actorNumber || boatData.player2id == gamemanager.actorNumber)
        {
            //raceTimeController.EndRace(); //タイマーをストップする
            StartCoroutine(resultCanvasController.ShowGoalText()); //goalの文字表示
            StartCoroutine(resultCanvasController.ShowResult()); //結果表示
        }
        
        
        //ゴールしたのが最後の１艘の時にゲーム終了としてボタン表示
        if (boatData.rank == raceInfScript.maxBoatNum)
        {
            if (PhotonNetwork.IsMasterClient) {
                StartCoroutine(resultCanvasController.ShowButton());
            }
        }
        
    }


    private void ShowGoalCerebrateEffect(Transform boatPos)
    {
        GameObject.Instantiate(goalCelebrationEffect, boatPos);
    }



    //ゴール後暇だろうから他の人の船を写すカメラから見ることができる様にしては？（仮）
    /*
    int NowTrackBoatNum = 0;
    Transform[] boatPos; //存在する全てのboatのtransformを格納する配列    
    Camera_FollowBoat camera_FollowBoat;
    public void CameraSwitch()
    {
        NowTrackBoatNum++;
        if(NowTrackBoatNum > raceInfScript.maxBoatNum) { NowTrackBoatNum = 0; }
        camera_FollowBoat.boatPos = boatPos[NowTrackBoatNum];
    }
    */
}
