using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Ventilation : MonoBehaviour {

    public bool Enabled = true;
    public List<Room> ConnectedRooms;
    public float TransferRate = 10f;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (enabled)
        {
            foreach (var room in ConnectedRooms)
            {
                room.ChangeOxygen(TransferRate / ConnectedRooms.Count);
            }
        }

	}

    
}
