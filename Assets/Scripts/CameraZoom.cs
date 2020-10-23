using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraZoom : MonoBehaviour
{
    private Camera _cam;

    // Start is called before the first frame update
    void Start()
    {
        _cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        var size = _cam.orthographicSize;
        size += -Input.GetAxis("Mouse ScrollWheel") * 2;
        size = Mathf.Clamp(size, 1, 9999);
        _cam.orthographicSize = size;
    }
}
