using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamRotation : MonoBehaviour
{
    [SerializeField] [Range(1, 20)] private float rotationSpeed = 5;

    void Update() => transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
}
