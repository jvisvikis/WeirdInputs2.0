using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private List<GameObject> points = new List<GameObject>();
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed;
    [SerializeField] private float angularSpeed;
    [SerializeField] private float circleRad;
    [SerializeField] private float segmentDistance;
    [SerializeField] private CentipideSegment head;
    private int pointIdx;
    private float currentAngle;

    void Update()
    {
        CentipideSegment current = head.next;
        while (current != null)
        {
            if(segmentDistance < Vector3.Distance(current.transform.localPosition, current.previous.transform.localPosition))
            {
                SetSegment(current, current.previous.transform.localPosition.z, current.previous.transform.localPosition.x, current.previous.transform.localPosition.y);
            }
            
            current = current.next;
        }
        currentAngle += angularSpeed * Time.deltaTime;
        Vector3 offset = new Vector2(Mathf.Sin(currentAngle), Mathf.Cos(currentAngle)) * circleRad;
        head.transform.position = this.transform.position + offset;
    }

    void FixedUpdate()
    {
        Vector3 point = points[pointIdx].transform.position;
        
        rb.velocity = (point-this.transform.position).normalized * speed;
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision);
        pointIdx = (pointIdx + 1) % points.Count;
    }

    void SetSegment(CentipideSegment current, float zin, float xin, float yin)
    {
        float dz = current.previous.transform.localPosition.z - current.transform.localPosition.z;
        float dx = current.previous.transform.localPosition.x - current.transform.localPosition.x;
        float dy = current.previous.transform.localPosition.y - current.transform.localPosition.y;
        float angle = Mathf.Atan2(dx, dz);
        float yAngle = Mathf.Atan2(dy, dz);
        current.transform.LookAt(current.previous.transform.position);
    }

    
}
