using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class DoorOpener : MonoBehaviour {

    public GameObject UIRepresentation;
    public bool open = false;
    public float MaxOpenDistance;
    public float MaxClosedDistance;
    public float Speed = 0.1f;
    public GameObject autoOpen;

    public Color ClosedColor;
    public Color OpenColor;
    public Color BrokenColor;
    public Color LockedColor;

    private float originalPos;
    private Vector3 firstPos;

    public bool locked;
    public bool broken;

	// Use this for initialization
	void Start () {
        originalPos = transform.position.y;
        firstPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (Mathf.Abs(originalPos-transform.position.y) <= MaxOpenDistance -0.5f)
        {
            UIRepresentation.GetComponent<test>().lineColor = ClosedColor; 
        }
        else
        {
            UIRepresentation.GetComponent<test>().lineColor = OpenColor;
        }

        if (locked)
	    {
		    UIRepresentation.GetComponent<test>().lineColor = LockedColor;
	    }
        else if (broken)
        {
            UIRepresentation.GetComponent<test>().lineColor = BrokenColor;
        }
        

        if (autoOpen != null)
        {
            if (Vector3.Distance(transform.position, autoOpen.transform.position) < 4)
            {
                open = true;
            }
            else
            {
                open = false;
            }
        }

        if ((!locked&&!broken&&open) && Mathf.Abs(originalPos-transform.position.y) < MaxOpenDistance)
        {
            HOTween.To(transform, Speed, "position", firstPos + new Vector3(0, MaxOpenDistance, 0));
            //transform.position = Vector3.Lerp(transform.position, firstPos + new Vector3(0, MaxOpenDistance, 0), Speed);
           //transform.Translate(new Vector3(0,Speed,0));//Vector3.MoveTowards(transform.position, transform.position + new Vector3(0, MaxOpenDistance, 0), Speed);
            
        }
        if ((!broken && !open) && Mathf.Abs(originalPos - transform.position.y) > MaxClosedDistance)
        {
            HOTween.To(transform, Speed, "position", firstPos + new Vector3(0, MaxClosedDistance, 0));
            //transform.position = Vector3.Lerp(transform.position, firstPos + new Vector3(0, MaxClosedDistance, 0), Speed);
        }
	}
}
