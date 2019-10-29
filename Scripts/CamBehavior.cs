using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class CamBehavior : MonoBehaviour
{
    private void Update()
    {
        WebUpdate(); //temporary
    }

    //WEB

    public float speedH = 0.3f;
    public float speedV = 0.2f;

    public float yawRange = 3.8f;
    public float pitchRange = 2.5f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    private Quaternion initAngles;
    private Vector3 newAngles;

    private void WebUpdate()
    {
        yaw += speedH * Input.GetAxis("Mouse X");
        pitch -= speedV * Input.GetAxis("Mouse Y");

        yaw = Mathf.Clamp(yaw, -yawRange, yawRange);
        pitch = Mathf.Clamp(pitch, -pitchRange, pitchRange);

        newAngles = new Vector3(pitch, yaw, 0.0f);
        initAngles = transform.rotation;

        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(newAngles), Time.deltaTime * 4f);
    }
}