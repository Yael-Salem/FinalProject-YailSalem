using System;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerLook : MonoBehaviour
{
    [Header("Player control variables")]
    public Camera cam;
    
    private float xRotation = 0f;

    public float xSensitivity = 30f;
    public float ySensitivity = 30f;
    
    
    [Header("Cutscene variables")]
    public float cutsceneTrackSpeed = 5f;

    private bool isOverrideActive = false;
    private Transform lookTarget;

    private void Update()
    {
        if (isOverrideActive && lookTarget != null)
        {
            Vector3 bodyTargetPosition =
                new Vector3(lookTarget.position.x, transform.position.y, lookTarget.position.z);

            Vector3 bodyDirection = bodyTargetPosition - transform.position;

            if (bodyDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetBodyRotation = Quaternion.LookRotation(bodyDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetBodyRotation,
                    cutsceneTrackSpeed * Time.deltaTime);
            }

            Vector3 camDirection = lookTarget.position - cam.transform.position;

            if (camDirection.sqrMagnitude > 0.001f)
            {
                Quaternion targetCamRotation = Quaternion.LookRotation(camDirection);

                float targetXRotation = targetCamRotation.eulerAngles.x;

                if (targetXRotation > 180)
                    targetXRotation -= 360f;

                targetXRotation = Mathf.Clamp(targetXRotation, -80f, 80f);

                xRotation = Mathf.MoveTowardsAngle(xRotation, targetXRotation, cutsceneTrackSpeed * Time.deltaTime * 15f);

                cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);


            }
        }
    }

    public void ProcessLook(Vector2 input)
    {
        if (isOverrideActive)
            return;
        
        float mouseX = input.x;

        float mouseY = input.y;
        
        // Calculate camera rotation for looking up and down
        xRotation -= (mouseY * Time.deltaTime) * ySensitivity;

        xRotation = Mathf.Clamp(xRotation, -80, 80);
        
        // Apply to camera transform
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        
        // Rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * Time.deltaTime) * xSensitivity);

    }


    public void SetCutsceneTrigger(Transform target)
    {
        lookTarget = target;
        isOverrideActive = (target != null);
    }

    public void ClearCutsceneLookTarget()
    {
        lookTarget = null;
        isOverrideActive = false;
    }
    
}
