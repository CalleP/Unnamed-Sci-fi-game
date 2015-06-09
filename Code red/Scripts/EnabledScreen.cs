using UnityEngine;
using System.Collections;

public class EnabledScreen : MonoBehaviour {

    public bool enabled;

    public Mesh EnabledMesh;
    public Mesh EnabledMesh1;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SetState(bool state)
    {
        enabled = state;
        if (state)
        {
            GetComponent<MeshFilter>().mesh = EnabledMesh;
        }
        else
        {
            GetComponent<MeshFilter>().mesh = EnabledMesh1;
        }
    }
}
