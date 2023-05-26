using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class WaypointMover : MonoBehaviour
{
    [SerializeField] Waypoints waypoint;

    [SerializeField] float rotateSpeed = 90f;
    [SerializeField] float moveSpeed;
    [SerializeField] float maxWaypointDistanceBuffer = 0.1f;
    [SerializeField] float maxDistanceToPlayer = 5f;

    GameObject player;
    Transform currentWaypoint;
    bool isFollowingWaypoints = true;
    public bool IsFollowingWaypoints
    {
        get { return isFollowingWaypoints; }
    }
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //Set initial position to the first wapoint
        currentWaypoint = waypoint.GetNextWayPoint(currentWaypoint);
        transform.position = currentWaypoint.position;

        //Set the next waypoint target
        currentWaypoint = waypoint.GetNextWayPoint(currentWaypoint);
        ProcessRotation();
    }

    // Update is called once per frame
    void Update()
    {
        if (isFollowingWaypoints)
        {
            FollowWaypoints();
        }
        if (Vector3.Distance(transform.position, player.transform.position) < maxDistanceToPlayer)
        {
            isFollowingWaypoints = false;
        }
    }

    void FollowWaypoints()
    {
        transform.position = Vector3.MoveTowards(transform.position, currentWaypoint.position, moveSpeed * Time.deltaTime);
        ProcessRotation();
        if (Vector3.Distance(transform.position, currentWaypoint.position) < maxWaypointDistanceBuffer)
        {
            currentWaypoint = waypoint.GetNextWayPoint(currentWaypoint);
        }
    }

    void ProcessRotation()
    {
        Vector3 targetLocation = currentWaypoint.position;
        targetLocation.z = transform.position.z;
        Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, currentWaypoint.position - transform.position);
        Quaternion newRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotateSpeed * Time.deltaTime);
        transform.rotation = newRotation;
    }
}
