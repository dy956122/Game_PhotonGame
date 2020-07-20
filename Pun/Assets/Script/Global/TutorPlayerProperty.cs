using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using System.Net.NetworkInformation;

public class TutorPlayerProperty : MonoBehaviour, IPunObservable // IPunObservable 讓對方也能接收到角色移動的動畫資訊，就不會造成對方角色在移動，卻沒有動畫的狀態，記得使用實作介面
{
    PhotonView pv;
    TutorPlayerController myController;


    public Animator animator;

    // 使用封裝：取得 動畫 - 移動狀態 的浮點數，設定值 (Parameter 的 字串名稱 必須要完全對應到)
    public float speed
    {
        get
        {
            return animator.GetFloat("Speed");
        }
        set
        {
            animator.SetFloat("Speed", value);
            animator.SetInteger("Index", 0);
            animator.SetBool("Pose", false);
        }
    }

    public GameObject playerName;
    Transform camPos;             // 因為是使用程式去指定，所以不用 public 也OK


    void Start()
    {
        pv = GetComponent<PhotonView>();
        // animator = GetComponentInChildren<Animator>(); 此段只會找到最上面那一個，之後就會停掉了，用欄位，拉東西進去會比較直觀一些
        if (pv.IsMine)
        {
            myController = FindObjectOfType<TutorPlayerController>();
            transform.parent = myController.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            myController.myAxis += Movemont;
            myController.myTrigger += Motion;
        }

        playerName.GetComponentInChildren<TextMesh>().text = pv.Owner.NickName;
        camPos = Camera.main.transform; // 等於下面那句話
        // GameObject.FindGameObjectWithTag("MainCamera"); // 此句話建議只執行一次，因為超耗效能

        // print(pv.Owner.UserId);     // 每個玩家的固有ID都不一樣，用來給程式碼辨識身分用的
        print(pv.Owner.NickName);      // print 出 每位玩家的暱稱
    }


    private void Update()
    {
        playerName.transform.LookAt(camPos);
    }

    public void Movemont(float x, float y)
    {
        // 速度 = 絕對值(y)
        // Mathf.Clamp(值, 最小值, 最大值);
        speed = Mathf.Clamp(Mathf.Abs(y) + Mathf.Abs(x), 0, 1);
    }

    // 註冊到 Controller 裡面
    public void Motion(int index)
    {
        // print("myIndex" + index);

        // RPC ( 執行功能名稱,房主,房號) ,順序很重要,不能搞錯
        pv.RPC("PoseChange",RpcTarget.All,index);
    }

    [PunRPC]        // 適合 一對一的事件(角色對道具)
    public void PoseChange(int index)
    {
        animator.SetInteger("Index", index);
        animator.SetBool("Pose", true);
    }

    // 此段 要搭配上方 IPunObservable 的實作介面，要讓其他連線玩家的動畫可以同步出現
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(speed); // 封包會以 obj 的型態發出
        }
        else
        {
            speed = (float)stream.ReceiveNext();    // 將 obj 檔案 強制轉換型別成 float
        }
    }
}
