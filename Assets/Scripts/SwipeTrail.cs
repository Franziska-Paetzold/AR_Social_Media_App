using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwipeTrail : MonoBehaviour {
    
    private Vector3[] rayPositions;

    private bool touchedAlready = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
        {
            if (touchedAlready == false) touchedAlready = true;
            Plane objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if(objPlane.Raycast(mRay, out rayDistance))
            {
                this.transform.position = mRay.GetPoint(rayDistance);
                
            }
        }
        if (!Input.GetMouseButton(0) && touchedAlready)
        {
            Debug.Log("Lifted");
            storeRay();
            
            touchedAlready = false;
        }
    }

    void storeRay()
    {
        int arrayLength = GetComponent<TrailRenderer>().positionCount;
        rayPositions = new Vector3[arrayLength];
        GetComponent<TrailRenderer>().GetPositions(rayPositions);
        for (int i = 0; i < rayPositions.Length; i++)
        {
            Debug.Log(rayPositions[i]);
        }
    }
}
