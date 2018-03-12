using UnityEngine;
using System.Collections;

public class DetectClicksAndTouches : MonoBehaviour
{	
	public bool Debug = false;
	
	private Camera _camera;
	
	public LayerMask LeftClickLayerMask;
	
	void Start()
	{
		_camera = Camera.main;
	}
	
	void Update ()
	{
	    if (Input.GetMouseButtonDown(0))
        {
            var ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if(Physics.Raycast (ray, out hit, Mathf.Infinity,LeftClickLayerMask))
			{
				if(Debug)
					UnityEngine.Debug.Log("You clicked " + hit.collider.gameObject.name,hit.collider.gameObject);
					
				hit.transform.gameObject.SendMessage("OnClick", hit.point, SendMessageOptions.DontRequireReceiver);
			}
        }
	}
}
