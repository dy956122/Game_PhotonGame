﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using Photon.Voice.Unity;
using Photon.Voice.PUN;
public class PhotonManager : MonoBehaviourPunCallbacks
{
    #region Variables
    static PhotonManager instance;
    public static PhotonManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PhotonManager>();
                if (instance == null)
                {
                    GameObject tmp = new GameObject("Photonmanager");
                    instance = tmp.AddComponent<PhotonManager>();
                }
            }
            return instance;
        }
    }

    public static GameObject localPlayer;
    PhotonVoiceNetwork PVN;
    public bool init;
    #endregion
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        PVN = gameObject.AddComponent<PhotonVoiceNetwork>();
        PVN.PrimaryRecorder = gameObject.AddComponent<Recorder>();
        PVN.PrimaryRecorder.TransmitEnabled = true;
        PVN.PrimaryRecorder.VoiceDetection = true;
    }

    #region Function
    public void ConnetToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void CreateRoom(string RoomName)
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 4, IsOpen = true, IsVisible = true };

        PhotonNetwork.CreateRoom(RoomName, roomOptions);
    }

    public void JoinRoom(string roomName)
    {
        if ( string.IsNullOrEmpty(roomName))
        {
            roomName = " ";
        }
        PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinRandomRoom()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    #endregion
    
    #region PunBehaviour
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        print("Connect to Master Success!!");
        init = true;
        PVN.PrimaryRecorder.RestartRecording();
    }

    public override void OnCreatedRoom()
    {
        base.OnCreatedRoom();
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();

        PhotonNetwork.LoadLevel("Level1");
        PhotonNetwork.AutomaticallySyncScene = true;
        print("Join " + PhotonNetwork.CurrentRoom.Name + " Success!!");
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);

        print("Join random room Failed!!");
    }

    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        PhotonNetwork.LoadLevel("Main");
        init = false;
    }

    #endregion
}