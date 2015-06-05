using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Room : MonoBehaviour {



    public List<Waypoint> ChildWaypoints = new List<Waypoint>();

	void Start () {
        
	}

	void Update () {
	    
	}

    public void connectRooms(int childWaypointIndex, Waypoint newWaypoint) 
    {
        if (childWaypointIndex >= 0 && childWaypointIndex < ChildWaypoints.Count)
        {
            ChildWaypoints[childWaypointIndex].Adjecent.Add(newWaypoint);
            newWaypoint.Adjecent.Add(ChildWaypoints[childWaypointIndex]);
        }
    }

    //Connects the inputted waypoint with the closes one in the room
    public void connectRooms(Waypoint newWaypoint)
    {
        if (ChildWaypoints.Count <= 0)
        {
            return;    
        }

        //finds closest waypoints and connects them
        Waypoint currentClosestWaypoint = ChildWaypoints[0];
        foreach (var waypoint in ChildWaypoints)
	    {
		    if (Vector3.Distance(newWaypoint.transform.position, waypoint.transform.position) < Vector3.Distance(newWaypoint.transform.position, currentClosestWaypoint.transform.position))
	        {
		        currentClosestWaypoint = waypoint;
	        }
	    }

        currentClosestWaypoint.Adjecent.Add(newWaypoint);
        newWaypoint.Adjecent.Add(currentClosestWaypoint);


    }

    private class DijkstraSet : IComparable<DijkstraSet> {

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
    
    

    public List<Waypoint> findClosestPathToRoom() {
        return null;
    }



    private class NodeWaypoint {
        
    
    }

}
