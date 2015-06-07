using UnityEngine;
using System.Collections;

using System;
using System.Collections.Generic;

public class Waypoint : MonoBehaviour {

    public static int AmountOfWaypoints;

    public bool AutoConnect;
    public List<Waypoint> Adjecent = new List<Waypoint>();

    public GameObject room;

    public static List<Waypoint> MostRecentPath;

    public bool slowDownBeforeApproaching;

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
        

        name = "Waypoint_" + AmountOfWaypoints;
        AmountOfWaypoints++;

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
	
	// Update is called once per frame
	void Update () {

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
                

                if (adjValues.Visited)
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
                if (!waypoints[adjecent.gameObject].Visited)
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
                foreach (var item in waypoints[end.gameObject].path)
                {
                    //Debug.Log("Path: " + item.name);
                }

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
}
