using UnityEngine;
using System.Collections;

class DoorAction : Action, IScreenClickReceiever{

    public DoorOpener doorOpener = null;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        
	}



    void IScreenClickReceiever.Clicked()
    {
        doorOpener.open = !doorOpener.open;
        doorOpener.AutoClose = !doorOpener.AutoClose;
        
        
    }

    void IScreenClickReceiever.AltClicked()
    {
        doorOpener.locked = !doorOpener.locked;
        
    }
}
