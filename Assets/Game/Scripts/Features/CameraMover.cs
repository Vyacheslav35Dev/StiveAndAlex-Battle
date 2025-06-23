using System.Collections;
using System.Collections.Generic;
using DG.Tweening; // Tweening library for smooth animations
using UnityEngine;

/// <summary>
/// Controls camera movement around a target with smooth radius and FOV transitions.
/// </summary>
public class CameraMover : MonoBehaviour
{
    public Transform target; // The target the camera orbits around
    public Camera cam; // The camera to control

    private float currentRadius; // Current orbit radius
    private float currentFOV; // Current field of view

    private Tween radiusTween; // Tween for radius transition
    private Tween fovTween; // Tween for FOV transition
    private Tween orbitTween; // Tween for orbit rotation

    private CameraModel _cameraSettings; // External settings object

    /// <summary>
    /// Initializes and starts camera movement based on provided settings.
    /// </summary>
    /// <param name="cameraSettings">Configuration object with movement parameters.</param>
    public void Run(CameraModel cameraSettings)
    {
        _cameraSettings = cameraSettings;

        // Set initial radius and FOV from settings
        currentRadius = _cameraSettings.roundRadius;
        currentFOV = Random.Range(_cameraSettings.fovMin, _cameraSettings.fovMax);

        if (cam == null)
            cam = Camera.main;

        cam.fieldOfView = currentFOV;

        // Set initial position based on radius and height offset
        Vector3 startPos = target.position + new Vector3(currentRadius, _cameraSettings.height, 0);
        transform.position = startPos;

        // Make the camera look at the target initially
        transform.LookAt(target);

        // Start continuous orbit around the target
        StartOrbit(_cameraSettings.roundDuration);

        // Begin changing radius and FOV with randomization and delay
        ChangeRadiusAndFOV(
            _cameraSettings.roamingRadius,
            Random.Range(_cameraSettings.fovMin, _cameraSettings.fovMax),
            _cameraSettings.fovDelay);
    }

    /// <summary>
    /// Starts an infinite orbit around the target over a specified duration.
    /// </summary>
    /// <param name="duration">Time to complete one full rotation.</param>
    void StartOrbit(float duration)
    {
        orbitTween?.Kill(); // Stop previous orbit if any

        orbitTween = DOTween.To(
            () => 0f, // Starting point (normalized angle)
            x =>
            {
                float angleDeg = x * 360f; // Convert normalized value to degrees
                float rad = Mathf.Deg2Rad * angleDeg;

                // Calculate new position on circle around target
                Vector3 newPos = target.position + new Vector3(Mathf.Cos(rad) * currentRadius, 0, Mathf.Sin(rad) * currentRadius);
                transform.position = newPos;

                // Keep the camera looking at the target during orbit
                transform.LookAt(target);
            },
            1f, // Animate from 0 to 1 (full circle)
            duration) 
            .SetEase(Ease.Linear) // Constant speed rotation
            .SetLoops(-1); // Loop infinitely
    }

    /// <summary>
    /// Smoothly transitions the camera's radius and FOV over a specified duration.
    /// After completion, recursively calls itself to create continuous variation.
    /// </summary>
    /// <param name="newRadius">Target radius value.</param>
    /// <param name="newFOV">Target field of view.</param>
    /// <param name="duration">Transition duration.</param>
    private void ChangeRadiusAndFOV(float newRadius, float newFOV, float duration)
    {
        // Kill previous tweens if they exist to prevent conflicts
        radiusTween?.Kill();
        fovTween?.Kill();

        // Animate radius change
        radiusTween = DOTween.To(
            () => currentRadius,
            x => { currentRadius = x; },
            newRadius,
            duration);

        // Animate FOV change with callback upon completion to continue variation cycle
        fovTween = DOTween.To(
            () => currentFOV,
            x =>
            {
                currentFOV = x;
                cam.fieldOfView = currentFOV;
            },
            newFOV,
            duration).OnComplete(() =>
            {
                // Recursively call to keep changing parameters with new random values after delay
                ChangeRadiusAndFOV(
                    _cameraSettings.roamingRadius,
                    Random.Range(_cameraSettings.fovMin, _cameraSettings.fovMax),
                    _cameraSettings.fovDelay);
            });
    }
}