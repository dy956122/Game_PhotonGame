using Classlin;

using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;

using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class TutorialCanvasManager : MonoBehaviourPunCallbacks // 此段必須繼承到MonoBehaviourPunCallbacks
{
    public InputField roomNameField;
    public InputField PlayerNameField;

    public Button createButton;

    public Button joinButton;

    // 大量取代下方同名物件,按下ctrl+R * 2
    public Button RandomButton;

    public Text state;

    // public int maxPlayers;

    IEnumerator Start()
    {
        TutorPhotonManager.Instance.ConnectedToMaster();

        yield return new WaitUntil(() => { return TutorPhotonManager.Instance.init; });

        // 一般的 button 點擊一下只能執行一個事件，但使用以下寫法，可使按鈕變成按一下執行多種事件
        // 創建房間
        createButton.onClick.AddListener(() =>
        {
            TutorPhotonManager.Instance.CreateRoom(roomNameField.text);
            PhotonNetwork.NickName = PlayerNameField.text;
            // TutorPhotonManager.Instance.CreateRoom(roomNameField.text, maxPlayers);
            ResetButton(false);
            state.text = "Creating...";
            state.color = Color.gray;
        });

        // 加入隨機房間
        RandomButton.onClick.AddListener(() =>
        {
            TutorPhotonManager.Instance.JoinRandomRoom();
            PhotonNetwork.NickName = PlayerNameField.text;
            ResetButton(false);
            state.text = "Joing...";
            state.color = Color.gray;
        });

        // 加入指定房間
        joinButton.onClick.AddListener(() =>
        {
            TutorPhotonManager.Instance.JoinRoom(roomNameField.text);
            PhotonNetwork.NickName = PlayerNameField.text;
            ResetButton(false);
            state.text = "Joing...";
            state.color = Color.gray;
        });
        ResetButton(true);
    }

    /// <summary>
    /// 設定初始按鈕
    /// </summary>
    /// <param name="value"></param>
    void ResetButton(bool value)
    {
        roomNameField.interactable = value;
        PlayerNameField.interactable = value;
        createButton.interactable = value;
        RandomButton.interactable = value;
        joinButton.interactable = value;
    }


    /// <summary>
    /// 連接到Photon伺服器，也提醒使用者需要等待時間連接
    /// </summary>
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        state.text = "Wait for Create or Join";
        state.color = Color.green;
    }

    /// <summary>
    /// 創建房間失敗的動作，這是一個防呆的措施，提醒使用者此階段無法執行，打OJRF會自行啟用
    /// </summary>
    /// <param name="returnCode"></param>
    /// <param name="message"></param>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        state.text = "There is no Room can join";
        state.color = Color.red;
        ResetButton(true);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        base.OnJoinRoomFailed(returnCode, message);
        state.text = "The Room is not exist.";
        state.color = Color.red;
        ResetButton(true);
    }
}
