using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

public class Ueda_GameManager : MonoBehaviourPunCallbacks {

    #region Public Field
    public static Ueda_GameManager Instance;
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab1;
    public GameObject playerPrefab2;
    public GameObject playerPrefab3;
    public GameObject playerPrefab4;
    public GameObject playerPrefab5;
    public GameObject playerPrefab6;
    public GameObject RaceControlObj;
    public int actorNumber;
    string Nickname;
    GameObject player;
    GameObject head;
    PlayerInstance playerinstance;
    BoatDataScript boatDataScript;
    RaceInfScript raceinfscript;
    [SerializeField] Transform[] m_spawnPositions;

    void Start() {
        PhotonNetwork.AutomaticallySyncScene = true;
        Instance = this;
        if (BoatPlayerController_Photon.LocalPlayerInstance == null) {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            actorNumber = PhotonNetwork.LocalPlayer.ActorNumber;
            Transform spawnPoint = m_spawnPositions[actorNumber - 1];
            boatDataScript = RaceControlObj.GetComponent<BoatDataScript>();
            playerinstance = this.GetComponent<PlayerInstance>();
            raceinfscript = RaceControlObj.GetComponent<RaceInfScript>();
            Nickname = PhotonNetwork.LocalPlayer.NickName;
            switch (actorNumber) {
                case 1:
                    player = PhotonNetwork.Instantiate(this.playerPrefab1.name, spawnPoint.position, Quaternion.identity);
                    break;
                case 2:
                    player = PhotonNetwork.Instantiate(this.playerPrefab2.name, spawnPoint.position, Quaternion.identity);
                    break;
                case 3:
                    player = PhotonNetwork.Instantiate(this.playerPrefab3.name, spawnPoint.position, Quaternion.identity);
                    break;
                case 4:
                    player = PhotonNetwork.Instantiate(this.playerPrefab4.name, spawnPoint.position, Quaternion.identity);
                    if (SceneManager.GetActiveScene().name == "Boat_Photon2") {
                        playerinstance.StartCountDown();
                    }
                    break;
                case 5:
                    player = PhotonNetwork.Instantiate(this.playerPrefab5.name, spawnPoint.position, Quaternion.identity);
                    break;
                case 6:
                    player = PhotonNetwork.Instantiate(this.playerPrefab6.name, spawnPoint.position, Quaternion.identity);
                    if (SceneManager.GetActiveScene().name == "Boat_Photon") {
                        playerinstance.StartCountDown();
                    }
                    break;
                default:
                    Debug.Log("Something is Wrong");
                    break;
            }
            raceinfscript.MyID = actorNumber;
            /*
            head = player.transform.Find("Sphere.003").gameObject;
            head.GetComponent<Renderer>().materials[1].color = Color.red;
            */
            playerinstance.SpawnPlayer(player, actorNumber);
            playerinstance.SetName(Nickname, actorNumber);
        } else {
            Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
        }
    }

    void Update() {
        playerinstance = this.GetComponent<PlayerInstance>();
        playerinstance.SpawnPlayer(player, actorNumber);
    }

    #endregion

    #region Photon Callbacks
    /*
    public override void OnLeftRoom() {
        SceneManager.LoadScene(0);
    }
    */

    /*
    public override void OnPlayerEnteredRoom(Player other) {
        Debug.LogFormat("OnPlayerEnteredRoom() {0}", other.NickName);
        if (PhotonNetwork.IsMasterClient) {
            Debug.LogFormat("OnPlayerEnteredRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
            LoadArena();
        }
    }

    public override void OnPlayerLeftRoom(Player other) {
        Debug.LogFormat("OnPlayerLeftRoom() {0}", other.NickName);
        if (PhotonNetwork.IsMasterClient) {
            Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);
            LoadArena();
        }
    }
    */

    #endregion

    #region Public Methods

    public void LeaveRoom() {
        PhotonNetwork.LoadLevel("Start_nishida");
    }

    public void ReTry() {
        PhotonView photonView = PhotonView.Get(this);
        photonView.RPC("Retry", RpcTarget.AllViaServer);
    }

    [PunRPC]
    void Retry() {
        Scene loadScene = SceneManager.GetActiveScene();
        PhotonNetwork.LoadLevel(loadScene.name);
    }

    #endregion

    #region Private Method

    void LoadArena() {
        if (!PhotonNetwork.IsMasterClient) {
            Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
        }
        Debug.LogFormat("PhotonNetwork : Loading Lvel : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
        PhotonNetwork.LoadLevel("Boat_Photon");
    }

    #endregion
}
