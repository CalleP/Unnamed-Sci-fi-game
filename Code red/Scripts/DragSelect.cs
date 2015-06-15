using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class DragSelect : MonoBehaviour
{


    public List<BaseCrew> currentCrew; 
    void FixedUpdate()
    {
        if (Input.GetMouseButtonDown(0))
        {
            
            Squad.Instance.ClearSquad();
        }
        
    }


    void OnTriggerEnter(Collider other)
    {
        

        var gObj = other.GetComponent<Collider>().gameObject;
        if (gObj.GetComponent<CrewClicker>() != null)
        {
            var crew = gObj.transform.parent.gameObject.GetComponent<BaseCrew>();
            gObj.GetComponent<test>().HighLight();
            currentCrew.Add(crew);
            
        }
            

    }


    void OnTriggerExit(Collider other)
    {


        var gObj = other.GetComponent<Collider>().gameObject;
        if (gObj.GetComponent<CrewClicker>() != null)
        {
            var crew = gObj.transform.parent.gameObject.GetComponent<BaseCrew>();
            gObj.GetComponent<test>().Revert();
            currentCrew.Add(crew);
            
        }


    }


    void OnDestroy()
    {
        Squad.Instance.ClearSquad();
        foreach (var crew in currentCrew)
        {
            Squad.Instance.AddMember(crew);
        }

    }

}


