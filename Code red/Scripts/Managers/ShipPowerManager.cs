using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ShipPowerManager : MonoBehaviour
{
    public static ShipPowerManager Instance;

    public Room PowerStart;

    private float CurrentCurrent = 0f;
    public float BaseCurrent = 3f;

    public float EmergencyPower = 10f;
    public float EmergencyPowerGeneration = 0.1f;

    private Room[] allRooms;

    public GeneratorStation[] Generators;

    void Start()
    {
        Generators = GameObject.FindObjectsOfType<GeneratorStation>();
        Instance = this;
        allRooms = GameObject.FindObjectsOfType<Room>();
        PowerStart.Enable();
    }

    void Update()
    {
        ReCalculateCurrent();
        ReCalculateEmergencyPower();
        DePowerAllUnconnectedRooms();
    }

    private void ReCalculateCurrent()
    {
        float totalCurrent = BaseCurrent;
        foreach (var generator in Generators)
        {
            totalCurrent += generator.PowerGenerationAmount;
        }
        CurrentCurrent = totalCurrent;
    }

    private void ReCalculateEmergencyPower()
    {
        EmergencyPower += EmergencyPowerGeneration;
    }

    public void TogglePowerRoom(Room room)
    {
        if (!room.Powered)
        {
            bool roomCanBePowered = RoomHasPoweredNeighbour(room);
            if (roomCanBePowered || room == PowerStart)
                room.Enable();
            else
                room.Disable();
        }
        else
        {
            room.Disable();
            DePowerAllUnconnectedRooms();
        }
    }

    private void DePowerAllUnconnectedRooms()
    {
        var connectedPath = Waypoint.GetAllWaypointsConnected(PowerStart.ChildWaypoints[1], true, false, true);

        var connectedRooms = Waypoint.GetAllRoomsInPath(connectedPath);
        
        foreach (var room in allRooms)
            if (!connectedRooms.Contains(room))
                room.Disable();
    }



    private bool RoomHasPoweredNeighbour(Room room)
    {
        foreach (var waypoint in room.ChildWaypoints)
        {
            foreach (var adjecentWaypoint in waypoint.Adjecent)
            {
                var adjRoom = adjecentWaypoint.room.GetComponent<Room>();
                if (adjRoom != room && adjRoom.Powered)     
                    return true;
            }
        }
        return false;
    }

    private void PowerInteractable(IPowerInteractable interactable)
    { 
    
    }

    private void DePowerInteractable(IPowerInteractable interactable)
    { 
    
    }
}

