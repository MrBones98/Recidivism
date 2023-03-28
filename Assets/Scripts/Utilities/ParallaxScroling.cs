using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxScroling : MonoBehaviour
{
    [SerializeField] private float _parallaxMultiplier;

    private Transform _cameraTransform;
    private Vector3 _lastCameraPos;
    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _lastCameraPos = _cameraTransform.position;
    }

    private void FixedUpdate()
    {
        Vector3 deltaMovement = _cameraTransform.position - _lastCameraPos;
        transform.position += deltaMovement * _parallaxMultiplier;

        _lastCameraPos = _cameraTransform.position;
    }
}
