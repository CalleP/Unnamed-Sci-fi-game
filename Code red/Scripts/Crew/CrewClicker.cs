using UnityEngine;
using System.Collections;

public class CrewClicker : MonoBehaviour, IScreenClickReceiever {


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	    
	}




    public void Clicked()
    {
        Squad.Instance.SingleSelection(gameObject.transform.parent.gameObject.GetSafeComponent<BaseCrew>());
        
       
    }

    public void AltClicked()
    {
        return;

    }
}
