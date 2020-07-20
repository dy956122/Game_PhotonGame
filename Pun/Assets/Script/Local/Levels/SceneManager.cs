using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class SceneManager : MonoBehaviourPunCallbacks
{
    public PlayerController myPlayer;

    void Start()
    {
        myPlayer = FindObjectOfType<PlayerController>();
        PhotonNetwork.Instantiate("Player", Vector3.up * 3f, Quaternion.identity);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            PhotonNetwork.LeaveRoom();
        }
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        Destroy(myPlayer.gameObject);
    }
}
