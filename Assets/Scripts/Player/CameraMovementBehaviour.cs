using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementBehaviour : MonoBehaviour
{
    [SerializeField] private Transform upDownTransform;
    [SerializeField] private Transform leftRightTransform;
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float minYRot, maxYRot;

    private float mouseX;
    private float mouseY;
    private float rotation = 0f;

    void LateUpdate()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        rotation -= mouseY;
        rotation = Mathf.Clamp(rotation, minYRot, maxYRot);
        upDownTransform.localRotation = Quaternion.Euler(rotation, 0, 0);
        leftRightTransform.Rotate(Vector3.up * mouseX);
    }
}
