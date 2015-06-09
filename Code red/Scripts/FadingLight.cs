using UnityEngine;
using System.Collections;

public class FadingLight : MonoBehaviour {

	public KeyCode plusintensity;
	public KeyCode negintensitivity;

	public KeyCode bouncen;
	public KeyCode bouncep;

	// Use this for initialization
	void Start () {
	
	}

	// Update is called once per frame
	void Update(){
		if(Input.GetKey(plusintensity)){
			gameObject.GetComponent<Light>().intensity += 0.01f;
		}
		if(Input.GetKey(negintensitivity)){
			gameObject.GetComponent<Light>().intensity -= 0.01f;
		}


	}
}
