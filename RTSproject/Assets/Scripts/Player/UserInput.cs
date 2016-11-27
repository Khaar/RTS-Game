using UnityEngine;
using System.Collections;
using RTS;


public class UserInput : MonoBehaviour {

    private Player player;

    private void MoveCamera()
    {
        float xpos = Input.mousePosition.x;
        float ypos = Input.mousePosition.y;
        Vector3 movment = new Vector3(0, 0, 0); 

        if(xpos >= 0 && xpos < ResourceManager.ScrollWidth)
        {
            movment.x -= ResourceManager.ScrollSpeed;
        }
        else if(xpos <= Screen.width && xpos > Screen.width - ResourceManager.ScrollWidth)
        {
            movment.x += ResourceManager.ScrollSpeed;
        }
        if(ypos >= 0 && ypos < ResourceManager.ScrollWidth)
        {
            movment.z -= ResourceManager.ScrollSpeed;
        }
        else if(ypos <= Screen.height && ypos > Screen.height - ResourceManager.ScrollWidth)
        {
            movment.z += ResourceManager.ScrollSpeed;
        }

        movment = Camera.main.transform.TransformDirection(movment); // Chujowa kamera <- Bad comment
        movment.y = 0;
        movment.y -= ResourceManager.ScrollSpeed * Input.GetAxis("Mouse ScrollWheel");

        Vector3 origin = Camera.main.transform.position;
        Vector3 destination = origin;
        destination.x += movment.x;
        destination.y += movment.y;
        destination.z += movment.z;

        if(destination.y > ResourceManager.MaxCameraHeight)
        {
            destination.y = ResourceManager.MaxCameraHeight;
        }
        else if(destination.y < ResourceManager.MinCameraHeight)
        {
            destination.y = ResourceManager.MinCameraHeight;
        }
        if(destination != origin)
        {
            Camera.main.transform.position = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.ScrollSpeed);
        }
    }
    private void RotateCamera()
    {
        Vector3 origin = Camera.main.transform.eulerAngles;
        Vector3 destination = origin;

        if ((Input.GetKey(KeyCode.LeftAlt) || Input.GetKey(KeyCode.RightAlt)) && Input.GetMouseButton(1))
        {
            destination.x -= Input.GetAxis("Mouse Y") * ResourceManager.RotateAmount;
            destination.y += Input.GetAxis("Mouse X") * ResourceManager.RotateAmount;
        }

        if(destination != origin)
        {
            Camera.main.transform.eulerAngles = Vector3.MoveTowards(origin, destination, Time.deltaTime * ResourceManager.RotateSpeed);
        }
    }

    private void MouseActivity()
    {
        if (Input.GetMouseButtonDown(0)) LeftMouseClick();
        else if (Input.GetMouseButtonDown(1)) RightMouseClick();
    }

    private void LeftMouseClick()
    {
        if (player.hud.MouseInBounds())
        {
            GameObject hitObject = FindHitObject();
            Vector3 hitPoint = FindHitPoint();

            if(hitObject && hitPoint != ResourceManager.InvalidPosition)
            {
                if (player.SelectedObject)
                {
                    player.SelectedObject.MouseClick(hitObject, hitPoint, player);
                } 
                else if(hitObject.name != "Ground")
                {
                    WorldObject worldObject = hitObject.transform.root.GetComponent<WorldObject>();
                   
                    if(worldObject)
                    {
                        player.SelectedObject = worldObject;
                        worldObject.SetSelection(true);
                    }
                }
            }
        }
    }

    private void RightMouseClick()
    {
        if(player.hud.MouseInBounds() && !Input.GetKey(KeyCode.LeftAlt) && player.SelectedObject)
        {
            player.SelectedObject.SetSelection(false);
            player.SelectedObject = null;
        }
    }

    private GameObject FindHitObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit)) return hit.collider.gameObject;

        return null;
    }

    private Vector3 FindHitPoint()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) return hit.point;
        return ResourceManager.InvalidPosition;
    }
    void Start () {
        player = transform.root.GetComponent<Player>();
	}
	
	void Update () {
        if (player.human)
        {
            MoveCamera();
            RotateCamera();
            MouseActivity();
        }
	}
}
