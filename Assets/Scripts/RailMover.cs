using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RailMover : RoomBehavior, ITriggerable
{

    [Header("Waypoints")]
    public Vector3[] localWaypoints;
    public bool cyclic;
    public float waitTime;
    public bool triggerable;
    public bool keepsGoingAfterTriggered;

    [Header("Other settings")]
    [Range(0, 2)]
    public float easeAmount;
    public float speed;

    Vector3[] globalWaypoints;
    int fromWaypointIndex;
    float percentBetweenWaypoints;
    float nextMoveTime;
    bool triggered = false;

    void Awake()
    {
        base.Awake();

        globalWaypoints = new Vector3[localWaypoints.Length];

        for (int i = 0; i < localWaypoints.Length; i++)
        {
            globalWaypoints[i] = localWaypoints[i] + transform.position;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if ( ! triggerable || triggered)
        {
            Vector3 velocity = CalculatePlatformMovement();
            transform.Translate(velocity, Space.World);
        }
    }

    Vector3 CalculatePlatformMovement()
    {
        if (Time.time < nextMoveTime)
        {
            return Vector3.zero;
        }

        fromWaypointIndex %= globalWaypoints.Length;
        int toWayPointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
        float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex], globalWaypoints[toWayPointIndex]);
        percentBetweenWaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
        percentBetweenWaypoints = (Mathf.Clamp01(percentBetweenWaypoints));

        float easedPercentBetweenWaypoints = Ease(percentBetweenWaypoints);

        Vector3 newPosition = Vector3.Lerp(globalWaypoints[fromWaypointIndex], globalWaypoints[toWayPointIndex], easedPercentBetweenWaypoints);

        if (percentBetweenWaypoints >= 1)
        {
            percentBetweenWaypoints = 0;
            fromWaypointIndex++;
            Debug.Log(fromWaypointIndex);

            if (fromWaypointIndex == globalWaypoints.Length)
            {
                if (triggerable && !keepsGoingAfterTriggered)
                {
                    triggered = false;
                }
            }

            if (fromWaypointIndex >= globalWaypoints.Length - 1)
            {
                if (!cyclic)
                {
                    fromWaypointIndex = 0;
                    System.Array.Reverse(globalWaypoints);
                }
            }
            nextMoveTime = Time.time + waitTime;
        }
        return newPosition - transform.position;
    }

    float Ease(float x)
    {
        float a = easeAmount + 1;
        return Mathf.Pow(x, a) / (Mathf.Pow(x, a) + Mathf.Pow(1 - x, a));
    }

    public override void Renew()
    {
        base.Renew();
        transform.position = defaultPosition;
        triggered = false;
        fromWaypointIndex = 0;
        percentBetweenWaypoints = 0;
    }

    public override void SetDefaultValues()
    {
        base.SetDefaultValues();


    }

    public IEnumerator Activate(float delay)
    {
        yield return new WaitForSeconds(delay);
        triggered = true;
    }

    private void OnDrawGizmos()
    {
        if (localWaypoints != null)
        {
            Gizmos.color = Color.red;
            float size = 0.3f;

            for (int i = 0; i < localWaypoints.Length; i++)
            {
                Vector3 globalWaypointPosition = (Application.isPlaying) ? globalWaypoints[i] : localWaypoints[i] + transform.position;
                Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size, globalWaypointPosition + Vector3.up * size);
                Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size, globalWaypointPosition + Vector3.left * size);

            }
        }
    }


}
