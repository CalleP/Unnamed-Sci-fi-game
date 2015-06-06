using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class WaypointFollower : MonoBehaviour {

    public  List<Waypoint> currentPath = new List<Waypoint>();

    public Waypoint currentWaypoint;
    public Waypoint waypointTest;

	// Use this for initialization
	void Start () {
        MoveToWayPoint(waypointTest);

	}
	
	// Update is called once per frame
	void Update () {

        if (currentPath != null)
        {
            if (currentPath.Count != 0 && Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z), new Vector3(currentPath[0].transform.position.x,0,currentPath[0].transform.position.z)) < 1f)
            {
                currentPath.RemoveAt(0);
            }
            if (currentPath.Count != 0)
            {
               // var normalizedVector = (currentPath[0].transform.position - transform.position).normalized;
               // var vectorNoVertical = new Vector3(normalizedVector.x, 0, normalizedVector.z);
               // transform.Translate(vectorNoVertical/50);
                
                
                //Quaternion.FromToRotation(transform.rotation, (currentPath[0].transform.position);
                GetComponent<CharacterController>().Move((currentPath[0].transform.position - transform.position).normalized / 50);



                
                Vector3 worldLookDirection = currentPath[0].transform.position - transform.position;
                Vector3 localLookDirection = transform.InverseTransformDirection(worldLookDirection);
                localLookDirection.y = 0;
                //transform.forward = transform.rotation * localLookDirection;
                HOTween.To(transform, 1.3f, "forward", transform.rotation * localLookDirection, false); 
             
                //transform.position = Vector3.MoveTowards(transform.position, currentPath[0].transform.position, 0.1f);
            }
        }

        
	}

    public void TeleportToWayPoint(Waypoint waypoint)
    {
        
    }

    public void MoveToWayPoint(Waypoint waypoint)
    {
        currentPath = Waypoint.FindClosestPath(currentWaypoint, waypoint);

    }


    


}
