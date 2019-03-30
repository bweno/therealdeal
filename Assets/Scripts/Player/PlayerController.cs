using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PlayerController : MonoBehaviourPunCallbacks {
    public float speed = 10;
    public GameObject model;
    public float shotCooldown = 0.3f;
    public Transform barrel;
    private float shotTimer = 0; 

    void Start() {
        if(!photonView.IsMine) {
            Destroy(transform.Find("Camera").gameObject);
        }
        else {
            photonView.RPC("SetBaseColor", RpcTarget.AllBuffered, Random.Range(0, 1f), Random.Range(0, 1f), Random.Range(0, 1f));
        }
    }

    void Update()
    {
        if (photonView.IsMine) {
            Move();
            Look();
            Shoot();
        }
    }

    private void Move() {
        Vector3 finalPos = Vector3.zero;
        finalPos += transform.right * Input.GetAxis("Horizontal") * Time.deltaTime * speed;
        finalPos += transform.forward * Input.GetAxis("Vertical") * Time.deltaTime * speed;
        GetComponent<CharacterController>().Move(finalPos);
        finalPos = transform.position;
        finalPos.y = 0;
        transform.position = finalPos;
    }

    private void Look() {
        Vector3 lookVector =  new Vector3(Screen.width / 2, 0, Screen.height / 2);
        lookVector = new Vector3(Input.mousePosition.x, 0, Input.mousePosition.y) - lookVector;
        model.transform.forward = lookVector;
    }

    private void Shoot() {
        if(Input.GetMouseButton(0) && shotTimer < 0) {
            shotTimer = shotCooldown;
            RaycastHit hit;
            if(Physics.Raycast(barrel.position, barrel.forward, out hit)) {
                Quaternion rot = Quaternion.FromToRotation(transform.up, hit.normal);
                PhotonNetwork.Instantiate("Prefabs/ShotHit", hit.point + hit.normal * 0.01f, rot);
            }
        }
        shotTimer -= Time.deltaTime;
    }

    [PunRPC]
    void SetBaseColor(float r, float g, float b)
    {
        model.transform.Find("Cube").GetComponent<Renderer>().material.color = new Color(r, g, b);
    }
}
