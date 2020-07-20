using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 3f;

    public delegate void trigger(int index);
    public trigger myTrigger;

    public delegate void Movement(float x, float y);
    public Movement myMovement;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        float x, y;
        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");
        if (x != 0 || y != 0)
        {
            myMovement(x, y);
            transform.position += new Vector3(x, 0, y) * Time.deltaTime * speed;

            float rotY = 0;
            if (y < 0)
            {
                rotY += 180;
            }
            if (x > 0)
            {
                rotY = 90;
            }
            else if (x < 0)
            {
                rotY = -90;
            }

            transform.localEulerAngles = new Vector3(0, rotY, 0);
        }


        if (transform.position.y < -3f)
        {
            transform.position = Vector3.up * 3f;
        }

        PoseCheck();
    }

    void PoseCheck()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            print("1 Has pressed!!");
            myTrigger(1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            print("1 Has pressed!!");
            myTrigger(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            print("1 Has pressed!!");
            myTrigger(3);
        }
    }

}
