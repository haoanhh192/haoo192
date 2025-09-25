using D2D.Gameplay;
using UnityEngine;

public class CharacterCanvas : MonoBehaviour
{
    public HealthBar HealthBar;

    private Camera currentCamera;
    private void Awake()
    {
        currentCamera = Camera.main;
    }

    private void LateUpdate()
    {
        transform.forward = currentCamera.transform.forward;
    }
}