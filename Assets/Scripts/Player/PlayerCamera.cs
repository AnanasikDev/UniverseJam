using Sirenix.OdinInspector;
using System;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Required] public new Camera camera;
    [SerializeField][OnValueChanged("SetCameraFollowingMode")] private CameraFollowingMode cameraFollowingMode = CameraFollowingMode.Following;
    [SerializeField][ShowIf("cameraFollowingMode", CameraFollowingMode.Following)] private Vector3 cameraFollowingSpeed = new Vector3(5.0f, 0.0f, 1.5f);
    private Func<Vector3> GetCameraPosition = null;
    private Vector3 cameraDefaultLocalPosition;

    public enum CameraFollowingMode
    {
        Static,
        Attached,
        Following
    }

    private void SetCameraFollowingMode()
    {
        if (cameraFollowingMode == CameraFollowingMode.Static)
        {
            camera.transform.SetParent(null);
            GetCameraPosition = () => PlayerController.instance.defaultPosition + cameraDefaultLocalPosition;
        }

        if (cameraFollowingMode == CameraFollowingMode.Following)
        {
            camera.transform.SetParent(null);
            GetCameraPosition = () =>
            {
                Vector3 result = new Vector3(
                    Mathf.Lerp(camera.transform.position.x, transform.position.x + cameraDefaultLocalPosition.x, Time.deltaTime * cameraFollowingSpeed.x),
                    Mathf.Lerp(camera.transform.position.y, transform.position.y + cameraDefaultLocalPosition.y, Time.deltaTime * cameraFollowingSpeed.y),
                    Mathf.Lerp(camera.transform.position.z, transform.position.z + cameraDefaultLocalPosition.z, Time.deltaTime * cameraFollowingSpeed.z)
                    );

                return result;
            };
        }

        if (cameraFollowingMode == CameraFollowingMode.Attached)
        {
            camera.transform.SetParent(transform);
            GetCameraPosition = () => transform.position + cameraDefaultLocalPosition;
        }
    }

    public void Init()
    {
        Transform parent = camera.transform.parent;
        camera.transform.SetParent(PlayerController.instance.transform);
        cameraDefaultLocalPosition = camera.transform.localPosition; // get default position relative to the player
        camera.transform.SetParent(parent);

        SetCameraFollowingMode();
    }

    public void UpdateCamera()
    {
        camera.transform.position = GetCameraPosition();
    }
}