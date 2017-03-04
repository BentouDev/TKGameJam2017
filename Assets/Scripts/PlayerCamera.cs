using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class PlayerCamera : MonoBehaviour
{
    ///	<summary>
    ///	Cel lub punkt odniesienia, na który chcemy patrzeć.
    /// </summary>
    public Transform Target;

    public GravityController Gravity;

    /// <summary>
    ///	Przesunięcie kamery względem CameraTarget.
    /// X przesuwa w poziomie (np. 1, jeżeli chcemy mieć kamerę znad ramienia)
    /// Z kontroluje odległość od celu (np. -3 dla kamery zza pleców)
    /// </summary>
    public Vector3 Offset;

    /// <summary>
    /// Prędkość z jaką chemy poruszać kamerą.
    /// </summary>
    public Vector3 Speed;

    /// <summary>
    /// Wysokość kamery nad postacią.
    /// </summary>
    public float Height;

    /// <summary>
    /// Minmalny kąt, który kamera może osiągnąć w pionie.
    /// </summary>
    public float MinAngleY;

    /// <summary>
    /// Maksymalny kąt, który kamera może osiągnąć w pionie.
    /// </summary>
    public float MaxAngleY;

    /// <summary>
    /// Obecny kąt obrotu kamery.
    /// </summary>
    private Vector3 Angle;

    private float LookX;
    private float LookY;

    public void OnStart()
    {
        //	Kopiujemy kąt, o jaki obrócony jest kamera w chwili uruchomienia
        Angle = new Vector3(
            transform.rotation.eulerAngles.x,
            transform.rotation.eulerAngles.y,
            transform.rotation.eulerAngles.z
        );
    }

    public void SetTarget(Transform target)
    {
        Target = target;
    }

    public void SetGravity(GravityController gravity)
    {
        Gravity = gravity;
    }

    public void LookAngles(float horizontal, float vertical)
    {
        LookX = horizontal;
        LookY = vertical;
    }

    public void OnUpdate()
    {
        if (Target)
        {
            Angle.x += LookX * Speed.x;
            Angle.y -= LookY * Speed.y;
            
            Angle.y = Mathf.Clamp(Angle.y, MinAngleY, MaxAngleY);
        }
    }

    void LateUpdate()
    {
        var gravityRot = Quaternion.FromToRotation(Vector3.up, Gravity.GravityDirection);
        
        var rot = gravityRot * Quaternion.Euler(Angle.y, Angle.x, 0);
        
        var pos = (rot * Offset) + (
                Target == null ? Vector3.zero : Target.position
            ) + gravityRot * new Vector3(0, Height, 0);
        
        transform.position = pos;
        transform.rotation = rot;
    }
}
