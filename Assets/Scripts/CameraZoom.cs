using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
    private Camera _cam;

    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    void Update()
    {
        var screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        var size = _cam.orthographicSize;
        var incVal = -Input.GetAxis("Mouse ScrollWheel") * size;
        var mousePos = Input.mousePosition - screenCenter;
        size += incVal;
        size = Mathf.Clamp(size, 1, 9999);
        _cam.orthographicSize = size;

        if (incVal != 0)
        {
        transform.position += mousePos * 0.001f * -Mathf.Sign(incVal);

        }
    }
}
