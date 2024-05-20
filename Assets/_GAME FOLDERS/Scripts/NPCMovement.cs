using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ASI model için el ile takip sistemi
public class NPCMovement : MonoBehaviour
{
    [SerializeField] private List<Transform> waypoints;
    [SerializeField] private float moveSpeed = 3f;
    [SerializeField] private float waypointTolerance = 0.1f;
    [SerializeField] private Animator animator;

    private int currentWaypointIndex = 0;
    private void Start()
    {
        Initalize();
    }
    void Initalize()
    {
        float value = PlayerPrefs.GetFloat("Coefficient");
        moveSpeed *= value;

        animator.Play("MediumRun");
        if (waypoints.Count > 0)
        {
            StartCoroutine(MoveToNextWaypoint());
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
}
