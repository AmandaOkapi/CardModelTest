using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundController : MonoBehaviour
{
    public Material radialStripesMaterial; // Assign the material in the Inspector
    public float rotationSpeed = 10f; // Speed at which to rotate the stripes

    void Update()
    {
        // Get the current rotation value
        float currentRotation = radialStripesMaterial.GetFloat("_Rotation");

        // Increase the rotation value over time
        currentRotation += rotationSpeed * Time.deltaTime;

        // Apply the new rotation value to the material
        radialStripesMaterial.SetFloat("_Rotation", currentRotation);
    }
}
