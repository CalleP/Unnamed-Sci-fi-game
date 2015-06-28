using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


class ShipOxygenManager : MonoBehaviour
{
    private Ventilation[] vents;
    private LifeSupportStation[] stations;

    void Start()
    { 
        vents = GameObject.FindObjectsOfType<Ventilation>();
        stations = GameObject.FindObjectsOfType<LifeSupportStation>();
    }

    void Update()
    {
        ReCalculateAirPressure();
    }

    private void ReCalculateAirPressure()
    {
        if (vents.Length == 0)
        {
            return;
        }

        float totalOxygenGenerated = 0f;

        foreach (var station in stations)
        {
            if (!station.IsBroken())
            {
                totalOxygenGenerated += station.OxygenGeneration;
            }
        }

        float NewTransferRate = totalOxygenGenerated / vents.Length;
        foreach (var vent in vents)
        {
            vent.SetTransferRate(NewTransferRate);
        }
    }
}

