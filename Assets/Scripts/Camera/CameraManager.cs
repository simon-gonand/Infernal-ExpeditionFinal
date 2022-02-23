using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[ExecuteInEditMode]
public class CameraManager : CinemachineExtension
{
    [Header("Position Movement")]
    [SerializeField]
    private float minYPosition = 25.0f;
    [SerializeField]
    private float maxYPosition = 35.0f;
    public float offsetPositionMovement;

    [Header("Rotation Movement")]
    [SerializeField]
    private float minXRotation = 50.0f;
    [SerializeField]
    private float maxXRotation = 60.0f;
    private float offsetRotationMovement;
    
    private float lockYPosition;
    private float lockXRotation;

    private bool _isUnzooming = false;
    public bool isUnzooming { set { _isUnzooming = value; } }

    private bool _isUnzoomMax = false;
    public bool isUnzoomMax { get { return _isUnzoomMax; } set { _isUnzoomMax = value; } }

    protected override void Awake()
    {
        base.Awake();
        lockYPosition = minYPosition;
        lockXRotation = minXRotation;

        CalculateOffsetRotationMovement();
    }

    public void CalculateOffsetRotationMovement()
    {
        offsetRotationMovement = offsetPositionMovement * (maxXRotation - minXRotation) / (maxYPosition - minYPosition);
    }

    protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            Vector3 position = state.RawPosition;
            position.y = lockYPosition;
            state.RawPosition = position;

            Vector3 euleurRot = state.RawOrientation.eulerAngles;
            euleurRot.x = lockXRotation;
            Quaternion rot = Quaternion.Euler(euleurRot);
            state.RawOrientation = rot;
        }
        if (_isUnzooming) UnzoomCamera();
        else ZoomCamera();
    }

    public void UnzoomCamera()
    {
        if (!_isUnzooming) return;
        if (lockYPosition >= maxYPosition && lockXRotation >= maxXRotation)
        {
            _isUnzooming = false;
            lockYPosition = maxYPosition;
            lockXRotation = maxXRotation;
            _isUnzoomMax = true;
            return;
        }
        _isUnzoomMax = false;
        lockYPosition += offsetPositionMovement;
        lockXRotation += offsetRotationMovement;
    }

    public void ZoomCamera()
    {
        if (_isUnzooming) return;
        if (lockYPosition <= minYPosition && lockXRotation <= minXRotation)
        {
            lockYPosition = minYPosition;
            lockXRotation = minXRotation;
            return;
        }

        lockYPosition -= offsetPositionMovement;
        lockXRotation -= offsetRotationMovement;
    }
}
