using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Movement : MonoBehaviour
{
    public Transform player;
    public Vector3 offset = new Vector3(0f,2f,-5f);
    public float followSpeed = 10f;
    public float rotationSpeed = 5f;
    public float fixedXRotation = 10f;


    // Start is called before the first frame update
    private void LateUpdate()
    {
        Vector3 desiredPosition = player.position + offset;

        transform.position = Vector3.Lerp(transform.position, desiredPosition, followSpeed * Time.deltaTime);

        Quaternion targetRotation = Quaternion.LookRotation(player.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        Vector3 eulerRotation = transform.rotation.eulerAngles;
        transform.rotation = Quaternion.Euler(fixedXRotation, eulerRotation.y, eulerRotation.z);
    }
}
