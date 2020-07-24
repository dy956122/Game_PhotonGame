using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Prop : MonoBehaviour
{
    public float flySpeed, deleteTime;

    private void Start()
    {
        Destroy(gameObject, deleteTime);
    }

    private void Update()
    {
        transform.Translate(Vector3.forward * flySpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider hit)
    {
        if (hit.GetComponent<Collider>().tag == "Enemy")
        {
            Destroy(gameObject);
        }
    }
}
