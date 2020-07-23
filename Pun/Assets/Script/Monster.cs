using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Monster : MonoBehaviour
{
    // 這是關於 怪物動作 的腳本

    public Transform monster;
    public Transform target;

    [Header("怪物的移動速度"),Range(0,10f),Tooltip("怪物的移動速度")]
    public float monsterSpeed = 0.1f;

    float scriptSpeed;

    private void Start()
    {
        scriptSpeed = monsterSpeed;
    }


    void Update()
    {
        Track();
    }

    [PunRPC]
    public void Track()
    {
        GameObject.Find("MonsterTarget");
        float dis = Vector3.Distance(monster.position, target.position);
        monster.position = Vector3.Lerp(monster.position, target.position, Time.deltaTime * monsterSpeed);
        // monster.transform.LookAt(target);
    }


    private void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().tag == "Wall")
        {
            scriptSpeed = 0;
           // GetComponent<Animator>().SetBool("Att", true);
        }

        if (hit.GetComponent<Collider>().tag == "Prop")
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerExit(Collider hit)
    {
        if (hit.GetComponent<Collider>().tag == "Wall")
        {
            scriptSpeed = monsterSpeed;
           // GetComponent<Animator>().SetBool("Att",false);
        }
    }

}

