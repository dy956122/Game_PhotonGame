using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class TutorPlayerController : MonoBehaviour
{
    // 委任：類似代辦事項(清單)，丟進去之後，會一個一個執行
    public delegate void Axis(float x, float y);
    public Axis myAxis;

    public delegate void Trigger(int index);
    public Trigger myTrigger;

    public float speed = 3;

    

    void Update()
    {
        float x, y;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        if (x != 0 || y != 0)
        {
            transform.position += new Vector3(x, 0, y) * Time.deltaTime * speed;
            myAxis(x, y);

            // 此段旋轉 較為 一般, 只會有上下左右旋轉，
            // 可設定 -1 ~ 1，這是比較漂亮的做法
            float RotY = 0;
            if (y < 0)
            {
                RotY = 180;
            }

            if (x > 0)
            {
                RotY = 90;
            }
            else if (x < 0)
            {
                RotY = -90;
            }
            transform.localEulerAngles = new Vector3(0, RotY, 0);
        }

        // 移動邊界範圍 限制
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -1.5f, 1.5f), transform.position.y, Mathf.Clamp(transform.position.z, -1.5f, 1.5f));

        if (Input.GetMouseButton(0))
        {
            myTrigger(1);
        }

        /*if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            myTrigger(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            myTrigger(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            myTrigger(3);
        }*/
    }
}
