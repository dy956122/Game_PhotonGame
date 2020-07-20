using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class PlayerPropoty : MonoBehaviourPunCallbacks,IPunObservable
{
    PhotonView pv;
    PlayerController myController;
    public GameObject firework;
    public Animator animator;
    float speed
    {
        get
        {
            return animator.GetFloat("Speed");
        }
        set
        {
            animator.SetFloat("Speed", value);
            if (value < 0.1f)
            {
                animator.SetBool("Pose", true);
            }
            else if (value> 0.1f)
            {
                animator.SetBool("Pose", false);
                animator.SetInteger("Index", 0);
            }
        }
    }
    
    void Start()
    {
        pv = photonView;
        if (pv.IsMine)
        {
            myController = FindObjectOfType<PlayerController>();
            transform.parent = myController.transform;
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            myController.myTrigger += myRpc;
            myController.myMovement += Movment;
        }
    }

    private void myRpc(int index)
    {
        pv.RPC("DoSomthing", RpcTarget.All, index);
    }

    private void Movment(float x,float y)
    {
        speed = Mathf.Clamp(Mathf.Abs(y) + Mathf.Abs(x), 0, 1);
    }

    [PunRPC]
    void DoSomthing(int index)
    {
        print("Do Somthing");
        animator.SetInteger("Index", index);
    }


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(speed);
        }
        else
        {
            speed = (float)stream.ReceiveNext();
        }
    }

}