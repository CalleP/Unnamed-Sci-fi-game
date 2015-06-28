using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;




class ShipPowerManager : MonoBehaviour
{
    public Waypoint PowerStart;
    public List<Waypoint> PowerPath;
    public List<Room> PoweredRooms;

    private float CurrentCurrent = 0f;
    public float BaseCurrent = 3f;

    public float EmergencyPower = 10f;
    public float EmergencyPowerGeneration = 0.1f;

    public GeneratorStation[] Generators;

    void Start()
    {
        Generators = GameObject.FindObjectsOfType<GeneratorStation>();
    }

    void Update()
    {
        ReCalculateCurrent();
        ReCalculateEmergencyPower();
        
        //PowerRoom()
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

    private void PowerRoom(Room room)
    {

    }

    private void DePowerRoom(Room room)
    { 
    
    }

    private void PowerInteractable(IPowerInteractable interactable)
    { 
    
    }

    private void DePowerInteractable(IPowerInteractable interactable)
    { 
    
    }
}

