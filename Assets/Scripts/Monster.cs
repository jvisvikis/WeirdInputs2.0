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
    private bool canReverse;

    private void Start()
    {
        canReverse = true;
    }
    private void OnEnable()
    {
        CameraSwitchPriority.OnSwitch += SetCanReverse;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance != null && !GameManager.Instance.gameStarted)
            return;
        if(GameManager.Instance != null && GameManager.Instance.gameWon)
        {
            rb.useGravity = true;
            return;
        }

        //if (AudioLoudnessDetector.Instance != null && AudioLoudnessDetector.Instance.GetLoudnessFromMic() * sensitivity >= threshold && !isReversing && canReverse)
        //{
        //    ToggleReverse();
        //}
        //else if (AudioLoudnessDetector.Instance != null && AudioLoudnessDetector.Instance.GetLoudnessFromMic()*sensitivity < threshold && isReversing)
        //{
        //    ToggleReverse();
        //}
        CentipideSegment current = isReversing ? tail.previous : head.next;
        while (current != null)
        {

            if (!isReversing && segmentDistance < Vector3.Distance(current.rb.position, current.previous.rb.position))
            {
                SetSegment(current, current.previous);
            }
            else if (isReversing && segmentDistance < Vector3.Distance(current.rb.position, current.next.rb.position))
            {
                SetSegment(current, current.next);
            }

            current = isReversing ? current.previous : current.next;
        }
        if (GameManager.Instance == null || (GameManager.Instance != null && !GameManager.Instance.gameWon))
        {
            if (!isReversing)
            {
                currentAngle += angularSpeed * Time.deltaTime;
                Vector3 offset = new Vector2(Mathf.Sin(currentAngle), Mathf.Cos(currentAngle)) * circleRad;
                head.rb.position = Vector3.MoveTowards(head.rb.position, rb.position + offset, Time.deltaTime * speed * 2);
            }
            else
                tail.rb.position = rb.position;
            head.transform.LookAt(Camera.main.transform);
        }
        else
        {
            head.transform.position = rb.position;
        }
        Vector3 point = points[pointIdx].transform.position;
        rb.velocity = (point - rb.position).normalized * speed;

        if (GameManager.Instance != null && !GameManager.Instance.gameStarted)
            return;



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
        if (collision.gameObject.layer == 6 && !isReversing && ++pointIdx >= points.Count)
        {
            pointIdx = points.Count - 1;
            GameManager.Instance.GameLost();
        }
        //endgame
        else if (isReversing)
            pointIdx = 0;
    }

    private void SetSegment(CentipideSegment current, CentipideSegment other)
    {
        float dz = other.rb.position.z - current.rb.position.z;
        float dx = other.rb.position.x - current.rb.position.x;
        float angle = Mathf.Atan2(dx, dz);
        current.rb.position = new Vector3(other.rb.position.x - Mathf.Sin(angle) * segmentDistance, current.rb.position.y, other.rb.position.z - Mathf.Cos(angle) * segmentDistance);
        current.transform.LookAt(other.rb.position);
        Vector3 dir = current.rb.position - other.rb.position;
        dir = dir.normalized * segmentDistance;
        current.rb.position = other.rb.position + dir;
    }

    public void SetCanReverse(CameraSwitchPriority csp)
    {
        canReverse = !csp.vcam1Active;
    }

    
}
