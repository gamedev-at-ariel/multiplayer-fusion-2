using System;
using UnityEngine;


/**
 * From Fusion tutorial https://doc.photonengine.com/fusion/current/tutorials/shared-mode-basics/3-movement-and-camera
 */
public class FirstPersonCamera: MonoBehaviour {
    //[SerializeField] float MouseSensitivity = 10f;
    //[SerializeField] float clampRotationAngle = 45f;

    //private float verticalRotation;
    //private float horizontalRotation;

    private Transform target;
    internal void SetTarget(Transform transform) {
        this.target = transform;
    }
    void LateUpdate() {
        if (target == null) return;

        transform.position = target.position;  // follow the player

        // Do not rotate with the player; this is handled by InputRotator
        //float mouseX = Input.GetAxis("Mouse X");
        //float mouseY = Input.GetAxis("Mouse Y");

        //verticalRotation -= mouseY * MouseSensitivity;
        //verticalRotation = Mathf.Clamp(verticalRotation, -clampRotationAngle, clampRotationAngle);

        //horizontalRotation += mouseX * MouseSensitivity;
        //transform.rotation = Quaternion.Euler(verticalRotation, horizontalRotation, 0);
    }
}
