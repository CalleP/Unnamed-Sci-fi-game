using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;
//using UnityEditor;

public class Waypoint : MonoBehaviour {

    public static int AmountOfWaypoints;

    public bool AutoConnect;
    public List<Waypoint> Adjecent = new List<Waypoint>();

    public GameObject room;

    public static List<Waypoint> MostRecentPath;

    public bool slowDownBeforeApproaching;

    public bool blocked;
    public bool AirTight = false;

    public bool IdlePoint;
    public bool OccupiedIdle;
    public bool PowerWaypoint = false;


    public enum WaypointDirections{
        None,
        West,
        East,
        North,
        South
        

    }

    public enum WaypointType { 
        EntryExit,
        Transition
        
    }

    public WaypointType Type;



	// Use this for initialization
	void Start () {


        

        if (AutoConnect)
        {
            var nearestDistanceSqr = Mathf.Infinity;
            var taggedGameObjects = GameObject.FindGameObjectsWithTag("Waypoint"); 
            Transform nearestObj = null;
 
            // loop through each tagged object, remembering nearest one found
            foreach (var obj in taggedGameObjects)
	        {
		        var objectPos = obj.transform.position;
                var distanceSqr = (objectPos - transform.position).sqrMagnitude;

                if (distanceSqr <= 1f && distanceSqr < nearestDistanceSqr && (obj.GetComponent<Waypoint>().room != room && obj.GetComponent<Waypoint>().Type == WaypointType.EntryExit))
                {
                    nearestObj = obj.transform;
                    nearestDistanceSqr = distanceSqr;
                }
	        }

            if (nearestObj != null)
            {
                Adjecent.Add(nearestObj.GetComponent<Waypoint>());
            }
            

        }
        
	}
	

    private class DijkstraSet : IComparable<DijkstraSet>{

       
        public float Distance;
        public bool Visited;
        public List<Waypoint> path = new List<Waypoint>();
        public Waypoint Waypoint;

        public DijkstraSet(float distance, bool visited, Waypoint waypoint)
        {
            this.Distance = distance;
            this.Visited = visited;
            this.Waypoint = waypoint;
        }



        public int CompareTo(DijkstraSet obj)
        {
            if (obj.Distance == this.Distance)
            {
                return 0;
            }
            else if (this.Distance > obj.Distance)
            {
                return -1;
            }
            else
            {
                return 1;
            }
        }

    }

    //Finds the closest path through waypoints using Dijkstras algorithm
    public static List<Waypoint> FindClosestPath(Waypoint start, Waypoint end)
    {
        Dictionary<GameObject, DijkstraSet> waypoints = new Dictionary<GameObject, DijkstraSet>();
        foreach (var gameObject in GameObject.FindGameObjectsWithTag("Waypoint"))
	    {
		    waypoints.Add(gameObject, new DijkstraSet(-1, false, gameObject.GetComponent<Waypoint>()));
	    }

        waypoints[start.gameObject].Distance = 0;

        Waypoint current = start;

        Stack<Waypoint> stack = new Stack<Waypoint>();
        stack.Push(current);

        while (stack.Count != 0)
        {
            current = stack.Pop();

            waypoints[current.gameObject].path.Add(current);

            //Goes through each adjecent node and checks if a faster path is found
            foreach (var adjecent in current.Adjecent)
            {
                float distanceAdjCurr = Vector3.Distance(adjecent.gameObject.transform.position, current.gameObject.transform.position);
                float totalDistance = waypoints[current.gameObject].Distance + distanceAdjCurr;
                DijkstraSet adjValues = waypoints[adjecent.gameObject];
                

                if (adjValues.Visited || adjecent.blocked)
	            {
		            continue;
	            }
                //If infinity
                if (adjValues.Distance == -1)
	            {
		            adjValues.Distance = totalDistance;
                    waypoints[adjecent.gameObject].path.InsertRange(0, waypoints[current.gameObject].path);
	            }
                //if the current path is faster than the previously fastest one to this node
                else if (totalDistance < adjValues.Distance)
                {
                    adjValues.Distance = totalDistance;
                    waypoints[adjecent.gameObject].path.InsertRange(0, waypoints[current.gameObject].path);
                }
            }

            List<DijkstraSet> list = new List<DijkstraSet>();
            foreach (var adjecent in current.Adjecent)
            {
                if (!waypoints[adjecent.gameObject].Visited && !adjecent.blocked)
                {
                    list.Add(waypoints[adjecent.gameObject]);
                }
                
            }

            list.Sort();
            //Pushes the adjecent nodes to the stack in order of how fast they are
            foreach (var item in list)
            {
                stack.Push(item.Waypoint);
            }

            waypoints[current.gameObject].Visited = true;
            if (waypoints[end.gameObject].Visited)
            {

                MostRecentPath = waypoints[end.gameObject].path;
                return waypoints[end.gameObject].path;
            }
        }

        Debug.Log("No Path Found");
        return null;
    }

    //void OnDrawGizmos()
    //{
    //    if (MostRecentPath.Count != 0)
    //    {
    //        Waypoint prev = null;
    //        foreach (var node in MostRecentPath)
    //        {
    //            if (prev != null)
    //            {
    //                Gizmos.DrawLine(prev.transform.position + new Vector3(0, 1), node.transform.position + new Vector3(0, 1));
    //            }
    //            prev = node;
    //        }
    //    }
    //}

    void OnPreRender()
    {
        GL.wireframe = true;
    }




    void OnDrawGizmos()
    {
        //Gizmos.DrawIcon(transform.position, "test");
        name = transform.parent.gameObject.name + " " + "WayPoint" + transform.position;
        if (blocked)
        {
            Gizmos.color = Color.red;
        }
        if (IdlePoint)
        {
            Gizmos.color = Color.green;
            name += " IDLE";
        }
        
        Gizmos.DrawCube(transform.position, transform.localScale/5);
        

        foreach (var adj in Adjecent)
        {
            if (blocked || adj.blocked) Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, adj.transform.position);  
            
        }

    }

    public static List<Waypoint> GetAllWaypointsConnected(Waypoint start, bool IgnoreBlocked, bool IgnoreUnPowered, bool IgnoreAirTight)
    {

        var output = new List<Waypoint>();

        Dictionary<Waypoint, bool> visited = new Dictionary<Waypoint, bool>();
        Stack<Waypoint> stack = new Stack<Waypoint>();

        output.Add(start);
        stack.Push(start);

        while (stack.Count != 0)
        {
            var currentWP = stack.Pop();
            if (visited.ContainsKey(currentWP))
                continue;
            visited.Add(currentWP, true);
            foreach (var item in currentWP.Adjecent)
            {
                var room = item.room.GetComponent<Room>();
                if (!((!IgnoreBlocked && item.blocked) || (!IgnoreUnPowered && !room.Powered) || (!IgnoreAirTight && item.AirTight)))
                {
                    if (!output.Contains(item))
                    {
                        output.Add(item);
                    }
                    
                    stack.Push(item);
                }
            }
        }

        return output;
    }

    public static List<Room> GetAllRoomsInPath(List<Waypoint> path)
    {
        var output = new List<Room>();

        foreach (var waypoint in path)
        {
            var room = waypoint.room.GetComponent<Room>();
            if (!output.Contains(room))
                output.Add(room);
                
        }
        return output;
    }


}
