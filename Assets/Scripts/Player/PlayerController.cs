using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks {
    public float speed = 10;

    void Start() {
        if(!photonView.IsMine) {
            Destroy(transform.Find("Camera").gameObject);
        }
    }

    void Update()
    {
        if (photonView.IsMine) {
            transform.position += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            transform.position += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * speed;
        }
    }
}
