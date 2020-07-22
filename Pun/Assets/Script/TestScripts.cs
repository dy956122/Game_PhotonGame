using UnityEngine;

public class TestScripts : MonoBehaviour
{
    // 這是關於 產生怪物點 的腳本

    public Transform monster;
    public Transform target;
    public float speed = 0.5f;
    public byte createTime = 2;

    void Update()
    {
        Invoke("CreateMonster", createTime);
        Track();
    }

    public void Track()
    {
        float dis = Vector3.Distance(monster.position, target.position);
        monster.position = Vector3.Lerp(monster.position, target.position, Time.deltaTime * speed);
    }

    public void CreateMonster()
    {
        Instantiate(monster, Vector3.zero, Quaternion.identity);
    }
}

