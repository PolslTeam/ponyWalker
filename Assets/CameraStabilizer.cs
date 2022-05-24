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
        // this.transform.rotation = my_rotation;
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
}
