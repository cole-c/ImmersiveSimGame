using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ThrowArcRenderer : MonoBehaviour
{
    LineRenderer line;

    private CameraScript mainCamera;

    public float v;
    public float angle;
    public int lineSegs;

    float g;
    float radAngle;

    GameObject hand;
    GameObject heldObject;

    //Colors for linerenderer to fade out
    Color clear = new Color(1, 1, 1, 0);

    private void Awake()
    {
        //Find needed components
        mainCamera = (CameraScript) GameObject.FindGameObjectWithTag("MainCamera").GetComponent(typeof(CameraScript));
        hand = GameObject.Find("Hand");
        line = GetComponent<LineRenderer>();

        //Physics Constants
        g = Mathf.Abs(Physics.gravity.y);
    }

    // Start is called before the first frame update
    void Start()
    {
      //Set up the line-renderer
        //treats parent object's position as (0,0,0)
        line.useWorldSpace = false;
        //default values for throwing, velocity, angle, and linesegments for linerenderer
        v = 10;
        angle = 45;
        lineSegs = 20;

        //fade linerenderer at the end
        line.startColor = Color.white;
        line.endColor = clear;
        line.material.color = Color.white;
    }

    // Update is called once per frame
    void Update()
    {
        //check that we have a line-renderer and something to throw
        if (line != null && heldObject != null)
        {
            line.startColor = Color.white;
        } else
        {
            //Hide the line renderer
            line.startColor = clear;
        }
        RenderArc();

        //Adjust throw based on camera height - currently with camHeight min:25 and max:50 this will go between 2.5 and 10
        v = (mainCamera.getCamHeight() * mainCamera.getCamHeight()) / 250;

        //Logic for picking up and throwing objects
        Collider[] objects = Physics.OverlapSphere(hand.transform.position, 2);

        float distance = 10;
        GameObject closestThing = null;

        for(int i=0; i<objects.Length; i++)
        {
            if(objects[i].tag == "throwable")
            {

                float tempDist = Vector3.Distance(objects[i].gameObject.transform.position, hand.transform.position);
                //Find the closest object TODO later highlight or indicate this object over others
                if (tempDist < distance)
                {
                    distance = tempDist;
                    closestThing = objects[i].gameObject;
                }
                //Pick up the object
                if(Input.GetKeyDown(KeyCode.Space))
                {
                    heldObject = closestThing;
                }
            }
        }

        if(heldObject != null)
        {
            //"hold" the object in hand, arm must be not solid TODO sometimes spins?
            heldObject.transform.position = hand.transform.position;

            if(Input.GetMouseButtonDown(0))
            {
                //Throw the object along the preview line
                heldObject.GetComponent<Rigidbody>().velocity = transform.TransformDirection(0, v * Mathf.Sqrt(2) / 2, v * Mathf.Sqrt(2) / 2);
                dropObject();
            }
            else if(Input.GetKeyDown(KeyCode.F))
            {
                dropObject();
            }
        }
        
    }

    void dropObject()
    {
        heldObject = null;
    }

    void RenderArc()
    {
        line.positionCount = lineSegs + 1;
        line.SetPositions(CalculateArcArray());
    }

    Vector3[] CalculateArcArray()
    {
        Vector3[] arcArray = new Vector3[lineSegs + 1];

        radAngle = Mathf.Deg2Rad * angle;
        float maxDistance = (v * v * Mathf.Sin(2 * radAngle)) / g;

        for(int i = 0; i <= lineSegs; i++)
        {
            float t = (float)i / (float)lineSegs;
            arcArray[i] = CalculateArcPoint(t, maxDistance);
        }

        return arcArray;
    }

    Vector3 CalculateArcPoint(float t, float maxDistance)
    {
        float z = t * maxDistance;
        float y = z * Mathf.Tan(radAngle) - ((g * z * z) / (2 * v * v * Mathf.Cos(radAngle) * Mathf.Cos(radAngle)));
        return new Vector3(0, y, z);
    }

}
