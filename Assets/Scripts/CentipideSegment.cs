using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CentipideSegment : MonoBehaviour
{
    public CentipideSegment next;
    public CentipideSegment previous;
    public Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

}
