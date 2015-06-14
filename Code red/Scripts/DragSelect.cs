using UnityEngine;
using System.Collections;
using System.Collections.Generic;

class DragSelect : MonoBehaviour
{

    

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
            Squad.Instance.AddMember(gObj.transform.parent.gameObject.GetComponent<BaseCrew>());
            Debug.Log(gObj.name);
        }
            

    }


    void OnTriggerExit(Collider other)
    {


        var gObj = other.GetComponent<Collider>().gameObject;
        if (gObj.GetComponent<CrewClicker>() != null)
        {
            Squad.Instance.RemoveMember(gObj.transform.parent.gameObject.GetComponent<BaseCrew>());
            Debug.Log(gObj.name);
        }


    }



}


