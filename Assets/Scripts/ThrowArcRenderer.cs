using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class ThrowArcRenderer : MonoBehaviour
{
    LineRenderer line;

    private GameObject player;
    private CameraScript camera;

    public float v;
    public float angle;
    public int lineSegs;

    float g;
    float radAngle;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        camera = (CameraScript) GameObject.FindGameObjectWithTag("MainCamera").GetComponent(typeof(CameraScript));

        line = GetComponent<LineRenderer>();
        g = Mathf.Abs(Physics.gravity.y);
    }

    // Start is called before the first frame update
    void Start()
    {
        line.useWorldSpace = false;
        v = 10;
        angle = 45;
        lineSegs = 20;
    }

    // Update is called once per frame
    void Update()
    {
        if (line != null)
        {
            RenderArc();
        }

        //Currently with camHeight min:25 and max:50 this will go between 2.5 and 10
        v = (camera.getCamHeight() * camera.getCamHeight()) / 250;
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
        //return new Vector3(x + player.transform.position.x, y + player.transform.position.y, 0 + player.transform.position.z);
    }

    //How to apply a force to a rigid body to make it follow this parabola
    //GetComponent<Rigidbody>().velocity = transform.TransformDirection(0, velocity * Mathf.Sqrt(2) / 2, velocity * Mathf.Sqrt(2) / 2);

}
