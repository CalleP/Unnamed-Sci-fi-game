using UnityEngine;
using System.Collections;

public class ScreenInteracter : MonoBehaviour {

	// Use this for initialization
    public LayerMask layermask;

    public Camera UICamera;
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //Debug.DrawLine(new Vector3(6.5f, -19.5f, 70.1f), new Vector3(0.3f, -0.9f, -0.1f), Color.yellow);
	    var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, layermask) && hit.collider.gameObject == this.gameObject)
        {
            Debug.Log("HIT SCREEN");
            
            ray = UICamera.ViewportPointToRay(new Vector3(hit.textureCoord.x, hit.textureCoord.y, 0));

            //Debug.DrawLine(ray.origin, ray.direction, Color.yellow);

            var vector = Quaternion.AngleAxis(-45, Vector3.up) * new Vector3(hit.textureCoord.x, -90, hit.textureCoord.y);

            Debug.DrawLine(ray.origin, vector = Quaternion.Euler(0, -90, 0) * new Vector3(hit.textureCoord.x, -90, hit.textureCoord.y), Color.red);


            Debug.Log(vector);
            
            Debug.DrawLine(ray.origin, vector, Color.green);

            

            if (Physics.Raycast(ray, out hit, layermask))
            {
                hit.collider.gameObject.name = "Poop";
                // hit should now contain information about what was hit through secondCamera
            }
        }
	}
}
