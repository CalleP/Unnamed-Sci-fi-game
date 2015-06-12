using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Holoville.HOTween;

public class WaypointFollower : MonoBehaviour {

    public  List<Waypoint> currentPath = new List<Waypoint>();

    public Waypoint currentWaypoint;
    public Waypoint waypointTest;

    public Room TestSelectedRoom;

	// Use this for initialization
	void Start () {
        //MoveToWayPoint(waypointTest);

        MoveToRoom(TestSelectedRoom);

	}
	
	// Update is called once per frame
	void Update () {

        if (currentPath != null && currentPath.Count != 0)
        {
            

            if (currentPath.Count != 0 && Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z), new Vector3(currentPath[0].transform.position.x,0,currentPath[0].transform.position.z)) < 0.15f)
            {
                currentWaypoint = currentPath[0];
                currentPath.RemoveAt(0);

                
            }
            if (currentPath.Count != 0)
            {
               // var normalizedVector = (currentPath[0].transform.position - transform.position).normalized;
               // var vectorNoVertical = new Vector3(normalizedVector.x, 0, normalizedVector.z);
               // transform.Translate(vectorNoVertical/50);
                
                
                //Quaternion.FromToRotation(transform.rotation, (currentPath[0].transform.position);

                var vector = (new Vector3(currentPath[0].transform.position.x, transform.position.y, currentPath[0].transform.position.z) - transform.position);

                



                transform.Translate(vector.normalized/10, Space.World);


                var target = new Vector3(currentPath[0].transform.position.x, this.transform.position.y, currentPath[0].transform.position.z);

                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector), 0.1f);
                //transform.LookAt(target);
                //transform.Translate(new Vector3(vector.x, 0, vector.z).normalized/10);

                

                 //var velocity = GetComponent<Rigidbody>().velocity = new Vector3(0,0,0.001f);

                // rigidbody.
                //Vector3 worldLookDirection = currentPath[0].transform.position - transform.position;
                //Vector3 localLookDirection = transform.InverseTransformDirection(worldLookDirection);
                //localLookDirection.y = 0;
                //transform.forward = transform.rotation * localLookDirection;
                //HOTween.To(transform, 1.3f, "forward", transform.rotation * localLookDirection, false); 
             
                //transform.position = Vector3.MoveTowards(transform.position, currentPath[0].transform.position, 0.1f);
            }

            //Check if any of the waypoints have been closed off
            foreach (var path in currentPath)
            {
                if (path == null)
                {
                    Debug.Log("test");
                }
                if (path.blocked)
                {
                    Waypoint end = currentPath[currentPath.Count - 1];
                    MoveToWayPoint(end);
                    break;
                }
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

    public void MoveToRoom(Room targetRoom)
    {
        if (targetRoom == currentWaypoint.room.GetComponent<Room>())
        {
            return;
        }
        //Attempt to find shortest path into a room
        List<List<Waypoint>> potentialPaths = new List<List<Waypoint>>();

        foreach (var waypoint in targetRoom.ChildWaypoints)
        {
            if (waypoint.Type == Waypoint.WaypointType.EntryExit)
            {
                potentialPaths.Add(Waypoint.FindClosestPath(currentWaypoint, waypoint));
            }
            
        }



        float Best = Mathf.Infinity;
        List<Waypoint> BestPath = null;
        foreach (var path in potentialPaths)
        {
            float totalDistance = 0;
            Waypoint old = currentWaypoint;
            foreach (var waypoint in path)
            {
                totalDistance += Vector3.Distance(old.transform.position, waypoint.transform.position);
                old = waypoint;
            }
            if (totalDistance < Best)
            {
                BestPath = path;
                Best = totalDistance;
            }
        }
        
        float BestIdle = Mathf.Infinity;
        Waypoint BestIdlePoint = null;
        foreach (var idleSpot in targetRoom.IdleSpots)
        {
            var distance = Vector3.Distance(BestPath[BestPath.Count - 1].transform.position, idleSpot.transform.position);
            if (distance < BestIdle)
            {
                BestIdlePoint = idleSpot;
                BestIdlePoint.OccupiedIdle = true;
                BestIdle = distance;
                
            }
        }
        currentWaypoint.OccupiedIdle = false;
        BestPath.Add(BestIdlePoint);

        BestPath.RemoveAt(0);
        currentPath = BestPath;
        

    }


    


}
