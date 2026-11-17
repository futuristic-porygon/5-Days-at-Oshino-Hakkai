using System.Collections;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    public Transform waypointParent;
    public float moveSpeed = 2f;
    public float waitTime = 2f;
    public bool loopWaypoints = true;

    private Transform[] waypoints;
    private int currentPointIndex;
    private bool isWaiting;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        waypoints = new Transform[waypointParent.childCount];

        for (int i = 0; i< waypointParent.childCount; i++)
        {
            waypoints[i] = waypointParent.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseManager.isGamePaused || isWaiting)
        {
            return;
        }
        MoveToWayPoint();

    }

   void MoveToWayPoint()
    {
        Transform target =waypoints[currentPointIndex];

        transform.position = Vector2.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);

        if(Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            StartCoroutine(WaitAtWaypoint());
        }
    }

    IEnumerator WaitAtWaypoint()
    {
        isWaiting = true;
        yield return new WaitForSeconds(waitTime);

        //Si esta activado el looping se incrementa el currentwaypointIndex y se vuelve a empezar el ciclo
        //Si no esta activado el looping se incrementa currentWayPointIndex no sobrepasa el ultimo waypoint
        currentPointIndex = loopWaypoints ? (currentPointIndex + 1) % waypoints.Length : Mathf.Min(currentPointIndex + 1, waypoints.Length - 1);

        isWaiting = false;
    }
}
