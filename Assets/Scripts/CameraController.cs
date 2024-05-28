using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float cameraRotationSpeed;
    public Transform pivot;
    public float maxViewAngle;
    public float minViewAngle;

    // Start is called before the first frame update
    void Start()
    {
        maxViewAngle = 45f;
        minViewAngle = -45f;
        cameraRotationSpeed = 3;
        offset = target.position - transform.position;

        pivot.transform.position = target.transform.position;
        pivot.transform.parent = target.transform;

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void LateUpdate() // LateUpdate to fix the player frame rate problem
    {
        // Get the X position of the mouse and rotate the target
        float horizontal = Input.GetAxis("Mouse X") * cameraRotationSpeed;
        target.Rotate(0, horizontal, 0);

        // Get the Y position of the mouse and rotate the pivot
        float vertical = Input.GetAxis("Mouse Y") * cameraRotationSpeed;
        pivot.Rotate(vertical, 0, 0); // use -vvertical for inverted rotation

        // Limit up and down camera rotation
        if (pivot.rotation.eulerAngles.x > maxViewAngle && pivot.rotation.eulerAngles.x < 180f)
        {
            pivot.rotation = Quaternion.Euler(maxViewAngle, 0, 0);
        }

        if (pivot.rotation.eulerAngles.x > 180f && pivot.rotation.eulerAngles.x < 360 + minViewAngle)
        {
            pivot.rotation = Quaternion.Euler(360 + minViewAngle, 0, 0);
        }


        // Move camera based on the current rotation of the target and the original offset
        float desiredXAngle = pivot.eulerAngles.x;
        float desiredYAngle = target.eulerAngles.y;

        Quaternion rotation = Quaternion.Euler(desiredXAngle, desiredYAngle, 0);
        transform.position = target.position - (rotation * offset);

        //transform.position = target.position - offset;

        if(transform.position.y < target.position.y)
        {
            transform.position = new Vector3(transform.position.x, target.position.y - 0.5f, transform.position.z);
        }

        transform.LookAt(target);
    }
}
