using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameController : MonoBehaviourPunCallbacks {
    public GameObject player;
    void Start()
    {
        GameObject itsAMe = PhotonNetwork.Instantiate("Prefabs/" + player.name, Vector3.zero, Quaternion.identity, 0);
    }
}
