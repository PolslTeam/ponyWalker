using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraStabilizer : MonoBehaviour
{

    void Start()
    {
    }

    void Update()
    {
        // move camera with horse
        Vector3 torsoPosition = GameObject.Find("torso").GetComponent<Transform>().position;
        transform.position = new Vector3(torsoPosition.x, torsoPosition.y + 2, -69);

        // don't tilt camera with horse
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
