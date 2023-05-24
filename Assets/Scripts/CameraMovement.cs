using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class CameraMovement : MonoBehaviour
{
    public Transform objectToFollow; // 타겟
    public float followSpeed = 10; //카메라가 따라가는 속도
    public float sensitivity = 100; //마우스 감도
    public float clampAngle = 70; // 제한 각도

    // 탄막 형식의 카메라 방식으로 이동
    public Transform topViewPos;
    public float smoothTime = 0.3f;
    private Vector3 velocity = Vector3.zero;
    public bool topviewMoveCheck = false;

    private float rotX;
    private float rotY;

    public Transform realCamera;
    public Vector3 dirNormalized;
    public Vector3 finalDir;
    public float minDistance; //벽에 닿았을때 카메라 위치를 위한 min max final
    public float maxDistance;
    public float finalDistance;
    public float smoothness = 10;

    private void Start()
    {
        topviewMoveCheck = true;
        rotX = transform.localRotation.eulerAngles.x;
        rotY = transform.localRotation.eulerAngles.y;

        dirNormalized = realCamera.localPosition.normalized;
        finalDistance = realCamera.localPosition.magnitude;

        //gameObject.transform.rotation = Quaternion.Euler(0, 0, 0);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (!topviewMoveCheck)
        {
            rotX += -(Input.GetAxis("Mouse Y")) * sensitivity * Time.deltaTime;
            rotY += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

            rotX = Mathf.Clamp(rotX, -clampAngle, clampAngle);
            Quaternion rot = Quaternion.Euler(rotX, rotY, 0);
            transform.rotation = rot;
        }

        TopViewMovePos();
    }

    private void LateUpdate()
    {
        if (!topviewMoveCheck)
        {
            transform.position = Vector3.MoveTowards(transform.position, objectToFollow.position, followSpeed * Time.deltaTime);

            finalDir = transform.TransformPoint(dirNormalized * maxDistance);

            RaycastHit hit;

            if (Physics.Linecast(transform.position, finalDir, out hit))
            {
                finalDistance = Mathf.Clamp(hit.distance, minDistance, maxDistance);
            }
            else
            {
                finalDistance = maxDistance;
            }
            realCamera.localPosition = Vector3.Lerp(realCamera.localPosition, dirNormalized * finalDistance, Time.deltaTime * smoothness);
        }
    }

    void TopViewMovePos()
    {
        if (topviewMoveCheck)
        {
            Camera.main.transform.position = Vector3.SmoothDamp(Camera.main.transform.position, topViewPos.position, ref velocity, smoothTime);
            Vector3 l_vector = objectToFollow.transform.position - Camera.main.transform.position;
            Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.LookRotation(l_vector).normalized, Time.deltaTime * 2f);

            /*
            if (Vector3.Distance(topViewPos.position, Camera.main.transform.position) < 0.1f)
            {
                topviewMoveCheck = false;
            }
            */
        }
        else
        {
            Camera.main.transform.rotation = Quaternion.Euler(rotX, rotY, 0);
        }
    }
}
