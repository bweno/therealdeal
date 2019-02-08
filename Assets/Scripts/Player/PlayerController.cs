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
        if(photonView.Owner.ActorNumber == 2)
        {
            transform.Find("Cube").GetComponent<Renderer>().material.color = Color.blue;
        }
    }

    void Update()
    {
        if (photonView.IsMine) {
            transform.position += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed;
            transform.position += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * speed;
            if(Input.GetMouseButtonDown(0))
            {
                photonView.RPC("Thing", RpcTarget.All, Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f));
            }
            if(Input.GetMouseButton(1))
            {
                photonView.RPC("ThingTwo", RpcTarget.All, Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f), photonView.Owner.ActorNumber);
            }
        }
    }

    [PunRPC]
    void Thing(float r, float g, float b)
    {
        transform.Find("Cube").GetComponent<Renderer>().material.color = new Color(r, g, b);
    }

    [PunRPC]
    void ThingTwo(float r, float g, float b, int actorId)
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player");
        foreach(GameObject player in players)
        {
            if(!(player.GetPhotonView().Owner.ActorNumber == actorId))
            {
                player.transform.Find("Cube").GetComponent<Renderer>().material.color = new Color(r, g, b);
            }
        }
    }
}
