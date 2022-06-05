using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun;
using Photon.Pun.Demo.Asteroids;

namespace mixi.rolling.nishida {
    public class Panel_management : MonoBehaviourPunCallbacks {

        [Header("ConnectPanel")]
        public GameObject ConnectPanel;
        public InputField PlayerNameInput;
        [Header("SelectPanel")]
        public GameObject SelectPanel;
        [Header("RoomCreatePanel")]
        public GameObject RoomCreatePanel;
        public InputField RoomNameInput;
        [Header("InsideRoomPanel")]
        public GameObject InsideRoomPanel;
        public GameObject plist;
        public GameObject Start;
        public Text roomname;
        public GameObject PlayerListPrefab;

        private string roomName;

        private Dictionary<int, GameObject> playerListEntries;


        private void Awake() {
            PhotonNetwork.AutomaticallySyncScene = true;
            PlayerNameInput.text = "Player" + Random.Range(1000, 10000);
            Screen.SetResolution(1600, 900, false, 60);
            if (PhotonNetwork.IsConnected) {
                PhotonNetwork.Disconnect();
            }
        }

        #region about photon

        public override void OnConnectedToMaster() {
            this.SetActivePanel(RoomCreatePanel.name);
        }

        public override void OnJoinedRoom() {
            this.SetActivePanel(InsideRoomPanel.name);
            
            
            if(playerListEntries == null) {
                playerListEntries = new Dictionary<int, GameObject>();
            }
            

            foreach(Player p in PhotonNetwork.PlayerList) {
                GameObject entry = Instantiate(PlayerListPrefab);
                entry.transform.SetParent(plist.transform);
                GameObject cldname = entry.transform.GetChild(0).gameObject;
                Text name_text = cldname.GetComponent<Text>();
                name_text.text = p.NickName;


                playerListEntries.Add(p.ActorNumber, entry);

            }

            if (PhotonNetwork.IsMasterClient) {
                Start.SetActive(true);
            } else {
                Start.SetActive(false);
            }

            roomname.text = "RoomName:"+roomName;

        }

        public override void OnPlayerEnteredRoom(Player newPlayer) {
            GameObject entry = Instantiate(PlayerListPrefab);
            entry.transform.SetParent(plist.transform);
            GameObject cldname = entry.transform.GetChild(0).gameObject;
            Text name_text = cldname.GetComponent<Text>();
            name_text.text = newPlayer.NickName;



            playerListEntries.Add(newPlayer.ActorNumber, entry);

        }

        public override void OnPlayerLeftRoom(Player otherPlayer) {
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);

            if (PhotonNetwork.IsMasterClient) {
                Start.SetActive(true);
            } else {
                Start.SetActive(false);
            }


        }


        #endregion




        #region about Button
        public void OnConnectButtonClicked() {
            string playerName = PlayerNameInput.text;

            if (!playerName.Equals("")) {
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.ConnectUsingSettings();
            } else {
                Debug.LogError("Player Name is invalid.");
            }

        }

        public void OnRoomCreateButtonClicked() {
            roomName = RoomNameInput.text;
            //roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;
            if (roomName.Equals(string.Empty)) {
                return;
            }

            RoomOptions options = new RoomOptions { MaxPlayers = 6 };


            PhotonNetwork.CreateRoom(roomName,options);
        }

        public void OnRoomEnterButtonClicked() {
            string roomName = RoomNameInput.text;
            if (roomName.Equals(string.Empty)) {
                return;
            }


            PhotonNetwork.JoinRoom(roomName);

        }

        public void OnBackButtonClicked() {
            if (PhotonNetwork.InRoom) {
                PhotonNetwork.LeaveRoom();
            }

            foreach (Transform n in plist.transform) {
                GameObject.Destroy(n.gameObject);
            }

            SetActivePanel(SelectPanel.name);
        }

        public void OnStartButtonClicked() {
            if (PhotonNetwork.CurrentRoom.PlayerCount == 6) {
                PhotonNetwork.LoadLevel("Boat_Photon");
            } else if (PhotonNetwork.CurrentRoom.PlayerCount == 4) {
                PhotonNetwork.LoadLevel("Boat_Photon2");
            }
        }

        #endregion


        public void SetActivePanel(string activePanel) {
            ConnectPanel.SetActive(activePanel.Equals(ConnectPanel.name));
            SelectPanel.SetActive(activePanel.Equals(SelectPanel.name));
            RoomCreatePanel.SetActive(activePanel.Equals(RoomCreatePanel.name));
            InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
        }
    }
}
