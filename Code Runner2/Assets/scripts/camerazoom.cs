using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camerazoom : MonoBehaviour
{
    private Camera Cam;

    public float TargetZoom = 3; //The value of Zoom I want by manipulating the Camera Size

    private float ScrollData; //Float collected upon Mouse Scrolling

    public float ZoomSpeed = 3; //Speed of zooming process in or out

    // Start is called before the first frame update
    void Start()
    {

        Cam = GetComponent<Camera>();

        TargetZoom = Cam.orthographicSize; //The game will begin with TargetZoom being assigned the default given Camera size
    }

    // Update is called once per frame
    void Update()
    {
        //when the Mouse is still, this function returns 0. Forward scrolling returns a +ve value, Bakward scrolling return -ve value.
        ScrollData = Input.GetAxis("Mouse ScrollWheel");
        TargetZoom = TargetZoom - ScrollData;
        TargetZoom = Mathf.Clamp(TargetZoom, 3, 6);
        Cam.orthographicSize = Mathf.Lerp(Cam.orthographicSize, TargetZoom, Time.deltaTime * ZoomSpeed);
    }
}


