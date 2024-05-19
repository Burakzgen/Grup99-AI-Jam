using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float waypointTolerance = 0.1f;
    [SerializeField] private Animator animator;

    private int currentWaypointIndex = 0;

    private void Start()
    {
        animator.Play("MediumRun");
        if (waypoints.Count > 0)
        {
            StartCoroutine(MoveToNextWaypoint());
        }
        else
        {
            Debug.LogWarning("Waypoints list is empty!");
        }
    }

    private IEnumerator MoveToNextWaypoint()
    {
        while (currentWaypointIndex < waypoints.Count)
        {
            Transform targetWaypoint = waypoints[currentWaypointIndex];

            while (Vector3.Distance(transform.position, targetWaypoint.position) > waypointTolerance)
            {
                Vector3 direction = (targetWaypoint.position - transform.position).normalized;
                transform.position += direction * moveSpeed * Time.deltaTime;

                transform.LookAt(targetWaypoint.position);

                yield return null;
            }

            currentWaypointIndex++;
        }
        if (animator != null)
        {
            animator.Play("Falling");
        }
    }

    //private void OnDrawGizmos()
    //{
    //    if (waypoints != null)
    //    {
    //        for (int i = 0; i < waypoints.Count; i++)
    //        {
    //            Gizmos.color = Color.red;
    //            Gizmos.DrawSphere(waypoints[i].position, 0.3f);

    //            if (i < waypoints.Count - 1)
    //            {
    //                Gizmos.color = Color.yellow;
    //                Gizmos.DrawLine(waypoints[i].position, waypoints[i + 1].position);
    //            }
    //        }
    //    }
    //}
}
