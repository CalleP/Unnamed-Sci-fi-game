using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class Squad
{

    void Update()
    {
        foreach (var crew in GameObject.FindObjectsOfType<BaseCrew>())
        {
            if (!members.Contains(crew))
            {
                RemoveMember(crew);
                crew.UIRepresentation.GetComponent<test>().lineColor = Color.yellow;
            }
        }
    }

    public static Squad Instance = new Squad();

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
            crew.UIRepresentation.GetSafeComponent<test>().lineColor = Color.green;
        }

    }

    public void ClearSquad()
    {
        foreach (var crew in members)
        {
            crew.UIRepresentation.GetComponent<test>().Revert();
            crew.UIRepresentation.GetComponent<test>().lineColor = Color.yellow;

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
        foreach (var crew in members)
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

