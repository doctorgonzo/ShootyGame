using UnityEngine;
using Cinemachine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraZ : CinemachineExtension
{
    [Tooltip("Lock the camera's Z position to this value")]
    public float m_xPosition = 10;
    public float m_yPosition = 10; 
    public float m_zPosition = 10;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Aim)
        {
            var pos = state.RawOrientation;
            pos.x = m_xPosition;
            pos.y = m_yPosition;
            pos.z = m_zPosition; 
            state.RawOrientation = pos;
        }
    }
}
