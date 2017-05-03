using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicGUIElement : MonoBehaviour {

    public Vector3 startPos;
    public Vector3 onScreenPos;
    public float speed;

    private Vector3 targetPos;
    private bool moving;
    private float movementStartTime;

	// Use this for initialization
	protected virtual void Start () {
        transform.position = startPos;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        if (moving)
        {
            float distanceCovered = (Time.time - movementStartTime) * speed;
            if(targetPos != transform.position) { 
                transform.position = Vector3.Lerp(transform.position, targetPos, distanceCovered / Vector3.Distance(transform.position, targetPos));

            }
            if (transform.position == targetPos)
                    moving = false;
        }

	}

    public void MoveTo(Vector3 location)
    {
        targetPos = location;
        moving = true;
        movementStartTime = Time.time;
    }
}
