using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [SerializeField] private GameObject followObject;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;

    // Start is called before the first frame update
    void Start()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z - 3);
    }

    void FixedUpdate()
    {
        Vector3 point = followObject.transform.position;
        rb.velocity = (point - this.transform.position).normalized * speed;
    }
}
