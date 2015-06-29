using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Room : MonoBehaviour, IPowerInteractable {

    public List<Room> connectedViaAir;

    public float Oxygen = 50f;
    public float MaxOxygen = 100f;

    public float AirTransferRate = 2.1f;

    public float Heat;

   
    
    private bool powered; 
    public bool Powered
    {
        get { return powered; }
        private set 
        {

            
            powered = value; 
        }
    }

    public GameObject UIStatus;
    public List<test> UIWallsAndFloors;

    public List<Waypoint> IdleSpots = new List<Waypoint>();
    public List<Waypoint> ChildWaypoints = new List<Waypoint>();

	void Start () {
        
	}

	void Update () {
        test UI = null;
        if (UIStatus != null)
        {
            UI = UIStatus.GetComponent<test>();
        }
        


        if (UI ?? false)
        {
            Color newColor = Color.red;
            newColor.r = Oxygen / 100;

            UI.lineColor = newColor;
        }



        
	}



    public void DistributeAir()
    {
        Room roomWithLeastOxygen = null;

        float oxyRecord = Mathf.Infinity;
        foreach (var room in connectedViaAir)
	    {
            
            if (room.Oxygen < oxyRecord)
            {
                roomWithLeastOxygen = room;
                oxyRecord = roomWithLeastOxygen.Oxygen;
            }
                
            
	    }

        if (roomWithLeastOxygen != null && (roomWithLeastOxygen.Oxygen <= roomWithLeastOxygen.Oxygen +2.5f && roomWithLeastOxygen.Oxygen >= Oxygen - 2.5f))
        {
            //return;
        }

        
            
        if (roomWithLeastOxygen ?? false)
	    {
		 
            roomWithLeastOxygen.ChangeOxygen(AirTransferRate);
            ChangeOxygen(-(AirTransferRate));

        }

        
	{
		 
	}
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

    public void ChangeOxygen(float amount)
    {
        if (Oxygen + amount <= 0) 
            Oxygen = 0;
        else if (Oxygen + amount >= MaxOxygen)  Oxygen = MaxOxygen;
        else Oxygen += amount;

        
    }


    private class NodeWaypoint {
        
    
    }


    public void Enable()
    {
        Powered = true;
        foreach (var neon in UIWallsAndFloors)
                neon.Powered();



    }

    public void Disable()
    {
        Powered = false;
        foreach (var neon in UIWallsAndFloors)
            neon.Revert();
    }


    public float EnableCost
    {
        get;
        set;
    }
}
