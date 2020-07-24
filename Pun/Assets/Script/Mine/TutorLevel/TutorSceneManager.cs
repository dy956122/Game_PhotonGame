using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using ExitGames.Client.Photon;

public class TutorSceneManager : MonoBehaviour, IOnEventCallback
{
    // 創建倒數計時器的文字
    public Text timeCounter;

    public int timer = 60;
    bool startTime = false;

    public GameObject[] monster;
    public GameObject[] createMonsterCube;

    public GameObject WinImage;


    [Range(1, 10)]
    public float createTime = 1f;
    [Range(1, 10)]
    public float RepeatTime = 1f;
    // public float maxNum;

    public Transform target;

    public delegate void MonsterEvent();
    public MonsterEvent monsterEvent;


    void Start()
    {
        PhotonNetwork.Instantiate("NoMouthIdle", Vector3.up * 3, Quaternion.identity);
        InvokeRepeating("CreateMonster", createTime, RepeatTime);
    }


    void Update()
    {
        if (PhotonNetwork.IsMasterClient && !startTime)
        {
            StartCoroutine(Timer());
        }


        /*if (Input.GetKeyDown(KeyCode.F1))
        {
            // 只有房主可以倒數計時
            if (PhotonNetwork.IsMasterClient && !startTime)
            {
                StartCoroutine(Timer());
            }
        }*/
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }

    IEnumerator Timer()
    {
        startTime = true;

        // Time.time 是 Unity 遊戲內部的時間
        // Time.timeScale = 0.1f; 可以加速也可以減速
        // realtime 則是以現實世界的時間為基準，不會受到 速度影響
        // 如果是用 for 迴圈，然後在迴圈內放入 new 相關的單字，會超級耗效能
        WaitForSecondsRealtime ww = new WaitForSecondsRealtime(1f);
        while (timer > 0)
        {
            yield return ww;
            timer--;
            timeCounter.text = timer.ToString();

            // 預設狀態 1 : 開啟時間
            byte eventCode = 1;
            object[] obj = new object[] { timer };      // obj類型的陣列可以包任何東西,但是取出來要強制型別轉換

            RaiseEventOptions eventOption = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(eventCode, obj, eventOption, SendOptions.SendReliable);


        }

        if (timer == 0)
        {
            WinImage.SetActive(true);
        }

        // 預設狀態 2 : 失敗
        if (timer == 0)
        {
            byte eventCode2 = 2;
            object[] obj2 = new object[] { "GameOver" };      // obj類型的陣列可以包任何東西,但是取出來要強制型別轉換

            RaiseEventOptions eventOption2 = new RaiseEventOptions { Receivers = ReceiverGroup.All };

            PhotonNetwork.RaiseEvent(eventCode2, obj2, eventOption2, SendOptions.SendReliable);
        }
        

        // 預設狀態 3 : 勝利
        byte eventCode3 = 3;
        object[] obj3 = new object[] { "Win" };

        RaiseEventOptions eventOption3 = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        PhotonNetwork.RaiseEvent(eventCode3, obj3, eventOption3, SendOptions.SendReliable);
    }


    // 實作介面 Event, 底下此段為接收方
    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case 1:
                {       // print("yes"); // 測試每秒是否有收到訊號
                    object[] data = (object[])photonEvent.CustomData;
                    timeCounter.text = data[0].ToString();
                    break;
                }
            case 2:
                {       // print("yes"); // 測試每秒是否有收到訊號
                    object[] data = (object[])photonEvent.CustomData;
                    timeCounter.text = data[0].ToString();
                    break;
                }
            case 3:
                {       // print("yes"); // 測試每秒是否有收到訊號
                    object[] data = (object[])photonEvent.CustomData;
                    timeCounter.text = data[0].ToString();
                    break;
                }
        }
    }

    [PunRPC]
    public void CreateMonster()
    {
        Vector3 MaxValue = createMonsterCube[Random.Range(0, createMonsterCube.Length)].GetComponent<Collider>().bounds.max;
        Vector3 MinValue = createMonsterCube[Random.Range(0, createMonsterCube.Length)].GetComponent<Collider>().bounds.min;
        Vector3 RandomPos = new Vector3(Random.Range(MinValue.x, MaxValue.x), MinValue.y, Random.Range(MinValue.z, MaxValue.z));

        // 抓取怪物產生的腳本，然後讓怪物往 指定座標點 移動
        Monster monsterA = Instantiate(monster[Random.Range(0, monster.Length)], RandomPos, Quaternion.identity).GetComponent<Monster>();
        monsterA.target = target;
        // monster[Random.Range(0, monster.Length)].transform.LookAt(monsterA.target); // 之後再來處理
    }


}
