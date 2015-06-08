using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirLock : MonoBehaviour
{

    public DoorOpener PhysicalAirLock;
    private bool prevState;
    // Use this for initialization
    void Start()
    {
        prevState = PhysicalAirLock.open; 
    }

    public List<Room> RoomsWithAirPassage;
    private Dictionary<Waypoint, bool> hasBeenVisited = new Dictionary<Waypoint, bool>();
    private Stack<Waypoint> stack = new Stack<Waypoint>();
    void Update()
    {
        if (prevState != PhysicalAirLock.open)
        {
            RedistributeOxygen();
            prevState = PhysicalAirLock.open;
        }
    }

    public void RedistributeOxygen()
    {
        Room room = GetComponent<Room>();
        if (!(room != null && room.ChildWaypoints.Count != 0))
        {
            return;
        }
        stack.Push(room.ChildWaypoints[0]);

        while (stack.Count != 0)
        {
            
            var waypoint = stack.Pop();
            if (hasBeenVisited.ContainsKey(waypoint))
                continue;
            hasBeenVisited.Add(waypoint, true);


            var room1 = waypoint.room.GetComponent<Room>();
            if (room != null)
            {
                foreach (var wayP in waypoint.Adjecent)
                {
                    if (!wayP.AirTight)
                    {
                        stack.Push(wayP);
                    }
                    
                }
                if (!RoomsWithAirPassage.Contains(room1))
                {
                    RoomsWithAirPassage.Add(room1);
                }
            }
            else
            {
                Debug.Log(room1.gameObject + " Does not have a room script attached!");
            }
        }
    }
}
