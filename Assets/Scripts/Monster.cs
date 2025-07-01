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
    [SerializeField] private float threshold;
    [SerializeField] private float sensitivity;
    [SerializeField] private CentipideSegment head;
    [SerializeField] private CentipideSegment tail;
    private int pointIdx;
    private int oldPointIdx;
    private float currentAngle;
    private bool isReversing;

    void Update()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameStarted)
            return;
        
        if (AudioLoudnessDetector.Instance != null && AudioLoudnessDetector.Instance.GetLoudnessFromMic() * sensitivity >= threshold && !isReversing)
        {
            ToggleReverse();
        }
        else if (AudioLoudnessDetector.Instance != null && AudioLoudnessDetector.Instance.GetLoudnessFromMic()*sensitivity < threshold && isReversing)
        {
            ToggleReverse();
        }
        CentipideSegment current = isReversing? tail.previous:head.next;
        
        while (current != null)
        {
            if(!isReversing && segmentDistance < Vector3.Distance(current.transform.localPosition, current.previous.transform.localPosition))
            {
                SetSegment(current, current.previous);
            }
            else if(isReversing && segmentDistance < Vector3.Distance(current.transform.localPosition, current.next.transform.localPosition))
            {
                SetSegment(current, current.next);
            }
            
            current = isReversing? current.previous : current.next;
        }
        if (!isReversing)
        {
            currentAngle += angularSpeed * Time.deltaTime;
            Vector3 offset = new Vector2(Mathf.Sin(currentAngle), Mathf.Cos(currentAngle)) * circleRad;
            head.transform.position = Vector3.MoveTowards(head.transform.position,this.transform.position + offset,Time.deltaTime*speed*2);
        }
        else
            tail.transform.position = this.transform.position;

        head.transform.LookAt(Camera.main.transform);
    }

    void FixedUpdate()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameStarted)
            return;

        Vector3 point = points[pointIdx].transform.position;
        rb.velocity = (point-this.transform.position).normalized * speed;
    }

    public void ToggleReverse()
    {
        isReversing = !isReversing;

        if (isReversing)
        {
            rb.position = tail.transform.position;
            oldPointIdx = pointIdx;
            pointIdx = 0;
        }
        else
        {
            rb.position = head.transform.position;
            pointIdx = oldPointIdx;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        Debug.Log(collision);
        if (!isReversing && ++pointIdx >= points.Count)
            pointIdx = points.Count-1;
        //endgame
        else if (isReversing && --pointIdx < 0)
            pointIdx = 0;
    }

    void SetSegment(CentipideSegment current, CentipideSegment other)
    {
        float dz = other.transform.localPosition.z - current.transform.localPosition.z;
        float dx = other.transform.localPosition.x - current.transform.localPosition.x;
        float angle = Mathf.Atan2(dx, dz);
        current.transform.localPosition = new Vector3(other.transform.localPosition.x - Mathf.Sin(angle) * segmentDistance, current.transform.localPosition.y, other.transform.localPosition.z - Mathf.Cos(angle) * segmentDistance);
        current.transform.LookAt(other.transform.position);
        Vector3 dir = current.transform.position - other.transform.position;
        dir = dir.normalized * segmentDistance;
        current.transform.position = other.transform.position + dir;
    }

    
}
