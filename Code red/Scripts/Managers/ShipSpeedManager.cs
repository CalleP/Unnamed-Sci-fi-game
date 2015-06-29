using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class ShipSpeedManager : MonoBehaviour
{
    
    
    private float currentPosition = 0;
    public float CurrentPosition
    {
        get;
        protected set;
    }
    public float CurrentSpeed
    {
        get;
        protected set;
    }
    
    private float endGoal = 10000;
    public float EndGoal
    {
        get;
        protected set;
    }
    private EngineStation[] Engines;    
    public static ShipSpeedManager Instance;


    void Start()
    {
        Instance = this;
        Engines = FindObjectsOfType<EngineStation>();

    }

    void Update()
    {
        MoveForward();
    }

    private void MoveForward()
    {
        float TotalSpeed = 0f;
        foreach (var engine in Engines)
        {
            if (!engine.IsBroken())
            {
                TotalSpeed += engine.Speed;
            }
        }
        currentPosition += TotalSpeed;
    }

    public bool IsDestinationReached()
    {
        if (currentPosition >= endGoal)
        {
            return true;
        }
        return false;
    }
    
}

