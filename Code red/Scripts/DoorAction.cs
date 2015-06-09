using UnityEngine;
using System.Collections;

class DoorAction : Action {

    public DoorOpener doorOpener;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}

    public override bool DoAction()
    {
        doorOpener.open = !doorOpener.open;
        doorOpener.AutoClose = !doorOpener.AutoClose;
        return base.DoAction();
    }

    public override bool DoAltAction()
    {
        
        doorOpener.locked = !doorOpener.locked;
        
        return base.DoAltAction();
    }
}
