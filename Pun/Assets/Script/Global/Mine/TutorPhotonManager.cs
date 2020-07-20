using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using Photon.Voice.Unity;

namespace Classlin
{
    public class TutorPhotonManager : MonoBehaviourPunCallbacks // 此段必須繼承到MonoBehaviourPunCallbacks
    {

        #region Varibles

        // 創建靜態欄位 TutorPhotonManager 的值
        public static TutorPhotonManager instance;

        public static TutorPhotonManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<TutorPhotonManager>();

                    if (instance == null)
                    {
                        GameObject tmp = new GameObject("TutorPhotonManager");
                        instance = tmp.AddComponent<TutorPhotonManager>();
                        PhotonVoiceNetwork voiceNetwork = tmp.AddComponent<PhotonVoiceNetwork>();

                        voiceNetwork.PrimaryRecorder = tmp.AddComponent<Recorder>();
                        voiceNetwork.PrimaryRecorder.TransmitEnabled = true;
                    }
                }

                return instance;
            }
        }

        Recorder recorder;

        // 使按鈕一開始 能否使用 的開關
        public bool init = false;

        #endregion Varibles

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        /// <summary>
        /// 連接到 ID 設定處
        /// </summary>
        public void ConnectedToMaster()
        {
            PhotonNetwork.ConnectUsingSettings();
        }

        /// <summary>
        /// 創建 開新房間 的方法
        /// </summary>
        /// <param name="roomName"></param>
        public void CreateRoom(string roomName)    // 可打成 public void CreateRoom(string roomName,int maxPlayers)，這樣的話，可以在最後端加入 人數限制，並且在另一邊程式被呼叫輸入值
        {
            // 進入房間 最多4人,是否會出現在大廳列表,是否可以被加入
            RoomOptions options = new RoomOptions { MaxPlayers = 4, IsVisible = true, IsOpen = true };


            PhotonNetwork.CreateRoom(roomName, options);     // 創建房間(房間名, 人數)
        }

        /// <summary>
        /// 創建 加入隨機房間 的方法
        /// </summary>
        public void JoinRandomRoom()
        {
            PhotonNetwork.JoinRandomRoom();         // 創建 隨機 房間            
        }

        /// <summary>
        /// 創建 加入指定房間 的方法
        /// </summary>
        /// <param name="roomName"></param>
        public void JoinRoom(string roomName)
        {
            // 預設創建 空 房間，也就是說，產生出來的房間，預設名字 叫做 " "(沒有名字的房間)
            if (string.IsNullOrEmpty(roomName))
            {
                roomName = " ";
            }
            PhotonNetwork.JoinRoom(roomName);
        }


        #region Pun Bahavior

        /// <summary>
        /// 創建到房間後的提示與作動
        /// </summary>
        public override void OnConnectedToMaster()
        {
            base.OnConnectedToMaster();

            print("connect to Master Success!!");
            init = true;
            PhotonVoiceNetwork.Instance.PrimaryRecorder.RestartRecording();
        }

        /// <summary>
        /// 加入房間後的提示
        /// </summary>
        public override void OnJoinedRoom()
        {
            base.OnJoinedRoom();

            print("Join Room Success!!");
            PhotonNetwork.LoadLevel("TutorLevel1");     // 開啟新場景房間，不需要使用到ScenesManager
        }
        #endregion Pun Bahavior
    }
}
