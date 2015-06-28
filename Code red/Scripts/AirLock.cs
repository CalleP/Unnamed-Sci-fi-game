using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AirLock : MonoBehaviour
{

    public DoorOpener PhysicalAirLock;

    // Use this for initialization
    void Start()
    {

    }
    public float drainRate = -5.55f;
    public List<Room> RoomsWithAirPassage;

    

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
