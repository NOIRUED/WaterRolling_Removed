using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.Demo.PunBasics;
using UnityEngine.SceneManagement;

public class PlayerInstance : MonoBehaviourPun
{
    public Transform boat1;
    public Transform boat2;
    public Transform boat3;
    public GameObject boat11;
    public GameObject boat22;
    public GameObject boat33;
    public float CountDownTime = 5.0f;
    bool CountDownBool = false;
    public Text TextCountDown;
    public GameObject CountDownPanel;
    public GameObject StartPanel;
    public GameObject RaceControlObj;
    RaceTimeController racetimecontroller;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(CountDownBool == true && CountDownTime>=0) {
            TextCountDown.text = Mathf.CeilToInt(CountDownTime).ToString();
            CountDownTime -= Time.deltaTime;
        } else if (CountDownTime<=0 && CountDownBool == true) {
            racetimecontroller = RaceControlObj.GetComponent<RaceTimeController>();
            racetimecontroller.StartRace();
            boat11.GetComponent<BoatMoveController_Photon>().RaceReady = true;
            boat22.GetComponent<BoatMoveController_Photon>().RaceReady = true;
            if (SceneManager.GetActiveScene().name == "Boat_Photon") {
                boat33.GetComponent<BoatMoveController_Photon>().RaceReady = true;
            }
            CountDownPanel.SetActive(false);
            StartPanel.SetActive(true);
            CountDownBool = false;
            StartCoroutine(ShowStart());
        } 
    }

    public void StartCountDown() {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("CountDown", RpcTarget.AllViaServer);
    }

    public void SpawnPlayer(GameObject player, int actorNumber) {
        PhotonView photonView = PhotonView.Get(this);
        string playername = player.name;
        photonView.RPC("SetPlayerParent", RpcTarget.All, playername, actorNumber);
    }
    public void SetName(string name, int actorNumber) {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("SetBoatData", RpcTarget.All, name, actorNumber);
    }
    
    [PunRPC] 
    void SetPlayerParent(string playername, int actorNumber) {
        GameObject player = GameObject.Find(playername);
        if (actorNumber == 1 || actorNumber == 2) {
            player.transform.SetParent(boat1);
        } else if (actorNumber == 3 || actorNumber == 4) {
            player.transform.SetParent(boat2);
        } else if (actorNumber == 5 || actorNumber == 6) {
            player.transform.SetParent(boat3);
        }
    }

    [PunRPC]
    void SetBoatData(string Nickname, int actorNumber) {
        BoatDataScript.BoatData boat;
        switch (actorNumber) {
            case 1:
                boat = boat11.GetComponent<BoatDataScript>().boatData;
                boat.player1name = Nickname;
                break;
            case 2:
                boat = boat11.GetComponent<BoatDataScript>().boatData;
                boat.player2name = Nickname;
                break;
            case 3:
                boat = boat22.GetComponent<BoatDataScript>().boatData;
                boat.player1name = Nickname;
                break;
            case 4:
                boat = boat22.GetComponent<BoatDataScript>().boatData;
                boat.player2name = Nickname;
                break;
            case 5:
                boat = boat33.GetComponent<BoatDataScript>().boatData;
                boat.player1name = Nickname;
                break;
            case 6:
                boat = boat33.GetComponent<BoatDataScript>().boatData;
                boat.player2name = Nickname;
                break;
            default:
                Debug.Log("Something is Wrong");
                break;
        }
    }

    [PunRPC]
    void CountDown() {
        CountDownBool = true;
        CountDownPanel.SetActive(true);

    }
    public IEnumerator ShowStart() {
        yield return new WaitForSeconds(0.5f);
        StartPanel.SetActive(false);
    }

}
