using UnityEngine;
using System.Collections;

public class ScreenInteracter : MonoBehaviour {

	// Use this for initialization
    public LayerMask layermask;


    public float viewportPointX;
    public float viewportPointY;
    public Camera UICamera;
    public float ActivationDistance = 4f;

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        //Debug.DrawLine(new Vector3(6.5f, -19.5f, 70.1f), new Vector3(0.3f, -0.9f, -0.1f), Color.yellow);

        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
	        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;


            if (Physics.Raycast(ray, out hit, ActivationDistance, layermask) && hit.collider.gameObject == this.gameObject)
            {
                var vector = Quaternion.AngleAxis(-90, Vector3.up) * new Vector3(hit.textureCoord.x, 0, hit.textureCoord.y);
                ray = UICamera.ViewportPointToRay(new Vector3(hit.textureCoord.x, hit.textureCoord.y, UICamera.nearClipPlane));
            
                if (Physics.Raycast(ray, out hit))
                {
                        var action = hit.collider.gameObject.GetComponent<IScreenClickReceiever>();
                        if (action != null)
                        {
                            Debug.Log(hit.collider.gameObject.name);
                            if (Input.GetMouseButtonDown(0))
                            {
                                action.Clicked();
                            }
                            else if(Input.GetMouseButtonDown(1))
                            {
                                 action.AltClicked();
                            }

                           
                        }
                        else
                        {
                            hit.collider.gameObject.GetComponent<test>().Fail();
                        }
                        
	                }
                }
        }
	}
}
