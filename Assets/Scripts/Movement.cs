using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.EventSystems;

public class Movement : MonoBehaviour
{
    public Animator anim;
    public GameObject duckHead;

    public delegate void Squawk();
    public static Squawk onSquawk;
    Camera cam;
    void Start()
    {
        cam = Camera.main;
    }
    void OnGUI() //from unity docs here https://docs.unity3d.com/ScriptReference/Camera.ScreenToWorldPoint.html
    {
        Vector3 point = new Vector3();
        Event currentEvent = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = currentEvent.mousePosition.x;
        mousePos.y = cam.pixelHeight - currentEvent.mousePosition.y;

        point = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.nearClipPlane));
        point.z = duckHead.transform.position.z;
        duckHead.transform.transform.position = point;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            onSquawk();
            anim.Play("BeakOpen");
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anim.Play("BeakClosed");
        }
    }
}
