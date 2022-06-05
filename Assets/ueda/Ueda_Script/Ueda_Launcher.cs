using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class Ueda_Launcher : MonoBehaviourPunCallbacks {

    #region Private Serializable Fields

    [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so new room will be created")]
    [SerializeField]
    private byte maxPlayersPerRoom = 6;
    bool isConnecting;

    #endregion

    #region Private Fields

    //client's version number
    string gameVersion = "1";

    #endregion

    #region MonoBehaviour CallBacks

    //MonoBehaviour method called on GameObject by Unity during early initialization phase.
    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    // Start is called before the first frame update
    void Start() {
        //Connect();
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
    }

    #endregion

    #region MonoBehaviourPunCallbacks Callbacks

    public override void OnConnectedToMaster() {
        if (isConnecting) {
            PhotonNetwork.JoinRandomRoom();
            isConnecting = false;
        }
        Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");
    }

    public override void OnDisconnected(DisconnectCause cause) {
        progressLabel.SetActive(false);
        controlPanel.SetActive(true);
        Debug.LogWarningFormat("PUN Basics Tutorial/Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
    }

    public override void OnJoinRandomFailed(short returnCode, string message) {
        Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRAndomFailed() was called by PUN.No random room available, so we create one.\nCaling: PhotonNetwork.CreateRoom");
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
    }

    public override void OnJoinedRoom() {
        Debug.Log("We load the 'Boat_Photon' ");
        PhotonNetwork.LoadLevel("Boat_Photon");
        Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");
    }

    #endregion

    #region Public Fields

    [Tooltip("The UI Panel to let the user enter name, connect and play")]
    [SerializeField]
    private GameObject controlPanel;
    [Tooltip("The UI Label to inform the user that the connection is in progress")]
    [SerializeField]
    private GameObject progressLabel;

    #endregion

    #region Public Methods

    public void Connect() {
        progressLabel.SetActive(true);
        controlPanel.SetActive(false);
        isConnecting = PhotonNetwork.ConnectUsingSettings();
        if (PhotonNetwork.IsConnected) {
            PhotonNetwork.JoinRandomRoom();
        } else {
            PhotonNetwork.GameVersion = gameVersion;
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    #endregion


}