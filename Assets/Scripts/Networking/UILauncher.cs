using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;

public class UILauncher : MonoBehaviourPunCallbacks {
    public GameObject roomInfoButton;
    private List<GameObject> buttons;

    private void Awake() {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    void Start() {
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.ConnectUsingSettings();
        buttons = new List<GameObject>();
    }

    void Update() {

    }

    public void CreateNewRoom() {
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public void ConnectToRoom(string roomName) {
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnConnectedToMaster() {
        PhotonNetwork.JoinLobby();
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList) {
        Debug.Log("Found " + roomList.Count + " rooms");
        foreach(GameObject button in buttons) {
            Destroy(button);
        }

        for(int i = 0; i < roomList.Count; i++) {
            GameObject button = Instantiate(roomInfoButton);
            button.transform.SetParent(transform);
            button.GetComponent<RectTransform>().localPosition = new Vector3(-100, 200 - i * 60, 1);
            string roomName = roomList[i].Name;
            button.transform.Find("Text").GetComponent<Text>().text = roomName + "\n" + roomList[i].PlayerCount + "/4";
            button.GetComponent<Button>().onClick.AddListener(() => ConnectToRoom(roomName));
        }
    }

    public override void OnJoinedRoom() {
        Debug.Log("I'm in a room! " + PhotonNetwork.CurrentRoom.Name);
        if(PhotonNetwork.IsMasterClient)
            PhotonNetwork.LoadLevel(1);
    }
}
