using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SwipeTrail : MonoBehaviour {

    public List<Line> AllLines = new List<Line>();

    public Material TrailMaterial;
    private bool touchedAlready;
    private TrailRenderer trailRenderer;
    public bool retraceLine; // currently set to true in the inspector
    public int rayPositionCounter = 1;
    private bool firstTouch = true;

    // Use this for initialization
    void Awake () {
        trailRenderer = GetComponent<TrailRenderer>();
        setTrailColour(Color.green, trailRenderer);
    }
	

	void Update () {
        if(trailRenderer.positionCount > 0 && firstTouch)
        {
            ClearTrail();
            firstTouch = false;
        }
        // Check, if the screen is touched and if the finger / mouse is moving
		if((Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved) || Input.GetMouseButton(0))
        {
            if (touchedAlready == false) touchedAlready = true;
            Plane objPlane = new Plane(Camera.main.transform.forward * -1, this.transform.position);
            Ray mRay = Camera.main.ScreenPointToRay(Input.mousePosition);
            float rayDistance;
            if(objPlane.Raycast(mRay, out rayDistance))
            {
                transform.position = mRay.GetPoint(rayDistance);
            }
        }



        // Check, if the user has released the screen to store the last drawn line
        if (!Input.GetMouseButton(0) && touchedAlready)
        {
            Debug.Log("Lifted");

            StoreRay();
            touchedAlready = false;
        }

        // Everything inside the following if statement is for the retracing of the line
        if (retraceLine) 
        {
            RetraceLine();
        }

        }

        void StoreRay(){
        int arrayLength = trailRenderer.positionCount;
        Color currentColour = trailRenderer.endColor;
        Vector3[] rayPositions = new Vector3[arrayLength];
        GetComponent<TrailRenderer>().GetPositions(rayPositions);

        AllLines.Add(new Line(currentColour, rayPositions));
     
        // TODO not to clear at this point in final version
        trailRenderer.Clear();
       
    }

    public void ClearTrail()
    {
        trailRenderer.Clear();
        Debug.Log("Trail cleared");
    }

    public void RedrawLine()
    {
        // TODO get not only the first line, but the required one
        Line currentLine = AllLines[0];
        if (rayPositionCounter == 1)
        {
            
           
            transform.position = currentLine.Positions[1];
            rayPositionCounter++;
            return;
        }
        if (rayPositionCounter == 2)
        {
            ClearTrail();
        }
        transform.position = currentLine.Positions[rayPositionCounter];

        rayPositionCounter++;
        if (rayPositionCounter == currentLine.Positions.Length)
        {
            retraceLine = false;
            rayPositionCounter = 1;
        }
    }

    public void RetraceLine()
    {
        // TODO get not only the first line, but the required one
        Line currentLine = AllLines[0];

        LineRenderer lineRenderer = gameObject.AddComponent<LineRenderer>();

        // Set the width of the Line Renderer
        lineRenderer.SetWidth(0.1F, 0.1F);
        // Set the number of vertex fo the Line Renderer
        lineRenderer.SetVertexCount(currentLine.Positions.Length);
        lineRenderer.material = TrailMaterial;
        setTrailColour(Color.green, null, lineRenderer);
        lineRenderer.SetPositions(currentLine.Positions);
        retraceLine = false;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="color"></param>
    /// <param name="tRenderer">Optional. Changes the colour of the passed trail renderer</param>
    /// <param name="lRenderer">Optional. Changes the colour of the passed line renderer</param>
    public void setTrailColour(Color color, TrailRenderer tRenderer = null, LineRenderer lRenderer = null)
    {
        if (tRenderer != null)
        {
            tRenderer.startColor = color;
            tRenderer.endColor = color;
            Debug.Log(tRenderer.endColor.ToString("F5"));
        }
        else if(lRenderer != null)
        {
            lRenderer.startColor = color;
            lRenderer.endColor = color;
        }
    }


  
}
