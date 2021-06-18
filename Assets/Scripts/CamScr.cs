using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamScr : MonoBehaviour
{

    public Transform cam;
    public Transform camPivot;  //takip edilmesi istenen obje
    public float camSpeed = 4f;
    public Vector2 clampVertical = new Vector2(-70f, 70f);

    public Vector3 camOffset;
    float HorizontalRot;    //rot=rotation
    float VerticalRot;

    public float speed = 5;     //kameranýn hareket hýzý

    void Start()
    {

        HorizontalRot = cam.eulerAngles.x;
        VerticalRot = cam.eulerAngles.y;

    }

    void Update()
    {
        //Klavye Kontrolü
        float xAxisValue = Input.GetAxis("Horizontal");
        float yAxisValue = Input.GetAxis("Vertical");

        if (Camera.current != null)
        {
            Camera.current.transform.Translate(new Vector3(xAxisValue * 0.5f, yAxisValue * 0.5f, 0));
        }

        //Yakýnlaþtýrma-Uzaklaþtýrma
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {

            transform.LookAt(camPivot);
            transform.Translate(0, 0, scroll * speed, Space.Self);

        }

        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {

            transform.LookAt(camPivot);
            transform.Translate(0, 0, scroll * speed, Space.Self);
        }

        camOffset = cam.InverseTransformDirection(cam.position - camPivot.position);

        if (Input.GetMouseButton(2))    //Tekerlek ile kontrol
        {

            float mouseX = Input.GetAxis("Mouse X");
            float mouseY = Input.GetAxis("Mouse Y");

            HorizontalRot += mouseX * camSpeed;
            VerticalRot -= mouseY * camSpeed;

            VerticalRot = Mathf.Clamp(VerticalRot, clampVertical.x, clampVertical.y);

            Quaternion camRot = Quaternion.Euler(VerticalRot, HorizontalRot, 0f);
            cam.rotation = camRot;

            Vector3 camPos = camPivot.position + camRot * camOffset;
            cam.position = camPos;

        }

    }

}

