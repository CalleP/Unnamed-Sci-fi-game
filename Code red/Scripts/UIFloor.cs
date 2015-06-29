using UnityEngine;
using System.Collections;
using System.Collections.Generic;


class UIFloor : MonoBehaviour, IScreenClickReceiever {

    public Room ParentRoom;
    //public List<>
	// Use this for initialization
	void Start () {
       
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Clicked()
    {
        //Squad.Instance.GotoWaypoint(waypoint);
    }

    public void AltClicked()
    {
        Squad.Instance.GotoRoom(transform.parent.gameObject.GetSafeComponent<Room>());
        
    }


    public void PowerClicked()
    {
        ShipPowerManager.Instance.TogglePowerRoom(ParentRoom);
    }
}
