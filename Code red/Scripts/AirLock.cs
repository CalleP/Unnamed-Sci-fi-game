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
    public float drainRate = -5.55f;
    public List<Room> RoomsWithAirPassage;
    private Dictionary<Waypoint, bool> hasBeenVisited = new Dictionary<Waypoint, bool>();
    private Stack<Waypoint> stack = new Stack<Waypoint>();
    

    void Update()
    {

        if (PhysicalAirLock.open)
        {
            gameObject.GetComponent<Room>().ChangeOxygen(-drainRate);
        }
        
       
        //if (prevState != PhysicalAirLock.open)
        //{
        //    RedistributeOxygen();
        //    prevState = PhysicalAirLock.open;
        //}



    }

    
}
