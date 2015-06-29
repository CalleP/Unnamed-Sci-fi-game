using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Action : MonoBehaviour, IScreenClickReceiever
{
    public float EnergyCost = 0f;
    public bool RequiresCrew = false;
    public bool Clickable = true;


    public void Clicked()
    {
        DoAction();
    }

    public void AltClicked()
    {
        DoAltAction();
    }

    //To be implemented
    //public List<Item> ItemRequirements;
    //public CrewType RequiredCrewType;
    //public Crew AssignedCrew;

    public virtual bool DoAction() 
    {
            
            
        //Finish Action
        //Remove energy
        //Succesful return true
        return false;


    }


    public virtual bool DoAltAction()
    {


        //Finish Action
        //Remove energy
        //Succesful return true
        return false;


    }

    public virtual bool DoActionWithCrew(/*Crew assignedCrew*/)
    {

        //SendCrew
        //DoAction
        //Finish Action
        //Remove energy
        //Succesful return true
        return false;


    }



    public void PowerClicked()
    {
        throw new NotImplementedException();
    }
}

