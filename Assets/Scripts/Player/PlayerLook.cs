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

    private const string SENSITIVITY_PREFS_KEY = "MouseSensitivity";
    
    [SerializeField] private float minSensitivity = 0.05f;
    [SerializeField] private float maxSensitivity = 0.3f;

    [SerializeField] private float maxLookDeltaPerFrame = 50f;

    private bool suppressNextFrame;

    private void Awake()
    {
        float savedSensitivity = PlayerPrefs.GetFloat(SENSITIVITY_PREFS_KEY, 50f);
        ApplySensitivity(savedSensitivity);
    }
    
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

        if (suppressNextFrame)
        {
            suppressNextFrame = false;
            return;
        }

        float mouseX = Mathf.Clamp(input.x, -maxLookDeltaPerFrame, maxLookDeltaPerFrame);
        float mouseY = Mathf.Clamp(input.y, -maxLookDeltaPerFrame, maxLookDeltaPerFrame);
        
        // Calculate camera rotation for looking up and down
        xRotation -= mouseY * ySensitivity;

        xRotation = Mathf.Clamp(xRotation, -80, 80);
        
        // Apply to camera transform
        cam.transform.localRotation = Quaternion.Euler(xRotation, 0, 0);
        
        // Rotate player to look left and right
        transform.Rotate(Vector3.up * (mouseX * xSensitivity));

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
    
    public void ApplySensitivity(float sliderValue)
    {
        float sensitivity = Mathf.Lerp(minSensitivity, maxSensitivity, (sliderValue - 1f) / 99f);
        
        ySensitivity = sensitivity;
        xSensitivity = sensitivity;
    }

    // Function to prevent mouse movement for the next frame
    public void SetSuppressNextFrame()
    {
        suppressNextFrame = true;
    }
    
}
