using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public Transform target;
    public Camera cam;
    public float initialRadius = 10f;
    public float initialFOV = 60f;

    private float currentRadius;
    private float currentFOV;

    private Tween radiusTween;
    private Tween fovTween;
    private Tween orbitTween;
    
    private CameraModel _camaraSettings;

    public void Run(CameraModel camaraSettings)
    {
        _camaraSettings = camaraSettings;
        currentRadius = _camaraSettings.roundRadius;
        currentFOV = Random.Range(_camaraSettings.fovMin, _camaraSettings.fovMax);

        if (cam == null)
            cam = Camera.main;
        
        cam.fieldOfView = currentFOV;
        Vector3 startPos = target.position + new Vector3(currentRadius, _camaraSettings.height, 0);
        transform.position = startPos;
        transform.LookAt(target);
        
        StartOrbit(_camaraSettings.roundDuration);
        
        ChangeRadiusAndFOV(_camaraSettings.roamingRadius, Random.Range(_camaraSettings.fovMin, _camaraSettings.fovMax),
            _camaraSettings.fovDelay);
    }

    void StartOrbit(float duration)
    {
        orbitTween = DOTween.To(() => 0f, x =>
        {
            float angle = x * 360f;
            float rad = Mathf.Deg2Rad * angle;
            Vector3 newPos = target.position + new Vector3(Mathf.Cos(rad) * currentRadius, 0, Mathf.Sin(rad) * currentRadius);
            transform.position = newPos;
            transform.LookAt(target);
        }, 1f, duration).SetEase(Ease.Linear).SetLoops(-1);
    }

    private void ChangeRadiusAndFOV(float newRadius, float newFOV, float duration)
    {
        radiusTween?.Kill();
        radiusTween = DOTween.To(() => currentRadius, x =>
        {
            currentRadius = x;
        }, newRadius, duration);
        
        fovTween?.Kill();
        fovTween = DOTween.To(() => currentFOV, x =>
        {
            currentFOV = x;
            cam.fieldOfView = currentFOV;
        }, newFOV, duration).OnComplete(() =>
        {
            ChangeRadiusAndFOV(_camaraSettings.roamingRadius, Random.Range(_camaraSettings.fovMin, _camaraSettings.fovMax),
                _camaraSettings.fovDelay);
        });
    }
}
