using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Waypoint))]
abstract class BaseCrewStation : MonoBehaviour, ICrewInteractable, IPowerInteractable {

    
    private float MaxHealth = 100;
    protected bool IsAtStation;
    private float health;
    public float Health
    {
        get { 
            return health;
        }
        set {
            if (health - value <= 0)
                health = 0;
            else if (health + value >= MaxHealth)
                health = MaxHealth;
            else
                health = value;
        }
    }

    public Waypoint InteractionWaypoint;
    public virtual void Start()
    {
        InteractionWaypoint = GetComponent<Waypoint>();
	}

    
    public virtual void Update()
    {
	    
	}

    //Crew Interactions
    public virtual void Repair(float amount)
    {
        Health += amount;
    }

    public virtual void Damage(float amount)
    {
        Health -= amount;
    }

    public bool IsBroken()
    {
        if (Health > 0)
            return false;
        else
            return true;
    }

    //Power Interactions
    public bool Enabled
    {
        get;
        protected set;
    }
    public void Enable()
    {
        Enabled = true;
    }
    public void Disable()
    {
        Enabled = false;
    }

    

    private float enableCost = 5f;
    float IPowerInteractable.EnableCost
    {
        get { return enableCost; }
        set { enableCost = value; }
    }



}
