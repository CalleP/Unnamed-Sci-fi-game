using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Squad
{





    public static Squad Instance = new Squad();


    public bool SingleSelect;
    private List<BaseCrew> members = new List<BaseCrew>();
    private Squad()
    { 
            
    }

    public List<BaseCrew> GetMembers()
    {
        List<BaseCrew> newList = new List<BaseCrew>();      
        foreach (var crew in members)
        {
            newList.Add(crew);
        }
        return newList;
    }

    public void SingleSelection(BaseCrew crew)
    {
        ClearSquad();
        members.Add(crew);
        
    }

    public void AddMember(BaseCrew crew) 
    {
        if (!members.Contains(crew))
        {
            members.Add(crew);
            crew.UIRepresentation.GetSafeComponent<test>().HighLight();
        }

    }

    public void ClearSquad()
    {
        foreach (var crew in members)
        {
            crew.UIRepresentation.GetComponent<test>().Revert();
            

        }
        members.Clear();
    }

    public void RemoveMember(BaseCrew crew)
    {
        members.Remove(crew);
        crew.UIRepresentation.GetComponent<test>().lineColor = Color.yellow;
    }




    public void GotoRoom(Room room)
    {
        var a = new List<MonoBehaviour>();
        a.AddRange(members.Cast<MonoBehaviour>());
        var b = new List<MonoBehaviour>();
        b.AddRange(room.IdleSpots.Cast<MonoBehaviour>());
        foreach (var crew in Helper.GetClosestObjectsInOrder(a, b))
        {
            crew.GetComponent<WaypointFollower>().MoveToRoom(room);
        }

        
    }

    public void GotoWaypoint(Waypoint waypoint)
    {
        foreach (var crew in members)
        {
            crew.GetComponent<WaypointFollower>().MoveToWayPoint(waypoint);
        }
    }

        


}

