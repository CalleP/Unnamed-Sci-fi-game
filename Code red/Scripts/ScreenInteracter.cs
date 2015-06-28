using UnityEngine;
using System.Collections;

public class ScreenInteracter : MonoBehaviour
{

    // Use this for initialization
    public LayerMask layermask;


    public float viewportPointX;
    public float viewportPointY;
    public Camera UICamera;
    public float ActivationDistance = 4f;


    public LayerMask SelectionBoxLayerMask;


    void Start()
    {

    }




    // Update is called once per frame
    void Update()
    {
        CheckScreen();

    }


    private bool firstClick = true;
    public Vector3 firstClickPos;
    private GameObject SelectBox;
    void CheckScreen()
    {

        RaycastHit hit = new RaycastHit();
        if (Input.GetMouseButton(0) || Input.GetMouseButtonDown(1))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);



            if (Physics.Raycast(ray, out hit, ActivationDistance, layermask) && hit.collider.gameObject == this.gameObject)
            {
               
                ray = UICamera.ViewportPointToRay(new Vector3(hit.textureCoord.x, hit.textureCoord.y, UICamera.nearClipPlane));

                

                //Cast Ray through the selected point on the viewport
                if (Physics.Raycast(ray, out hit))
                {
                    var action = hit.collider.gameObject.GetComponent<IScreenClickReceiever>();

                 


                    if (action == null && Input.GetMouseButtonDown(0))
                    {
                        var neon = hit.collider.gameObject.GetComponent<test>();
                        if (neon ?? false)
                        {
                            neon.Fail();
                        }
                    }

                    if (Input.GetMouseButtonDown(0) && firstClick)
                    {
                        firstClickPos = hit.point;
                        if(action !=  null) action.Clicked();
                        firstClick = false;
                        Squad.Instance.SingleSelect = true;
                    }
                    else if (Input.GetMouseButton(0) && !firstClick)
                    {
                        if (SelectBox == null)
                        {
                            var obj = Resources.Load<GameObject>("PrefabsAux/SelectionBox");
                            SelectBox = Instantiate(obj);

                        }
                        Squad.Instance.SingleSelect = false;
                        SelectBox.transform.localScale = new Vector3(firstClickPos.x - hit.point.x, 5f, firstClickPos.z - hit.point.z);
                        SelectBox.transform.position = new Vector3(firstClickPos.x - ((firstClickPos.x - hit.point.x) / 2), -68, firstClickPos.z - ((firstClickPos.z - hit.point.z) / 2));

                    }
                    else if (Input.GetMouseButtonDown(1))
                    {
                        action.AltClicked();
                    }


                }


            }
        }
        


        if (Input.GetMouseButtonDown(0))
        { 
            firstClick = false;
            if (hit.collider != null)
            firstClickPos = hit.point;
        }

        if (!Input.GetMouseButton(0) && !Input.GetMouseButtonDown(0))
        {
            Destroy(SelectBox);
            SelectBox = null;
            firstClick = true;
            firstClickPos = new Vector3(5, 5, 5);
        }
    }
}
