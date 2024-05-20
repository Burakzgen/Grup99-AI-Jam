using UnityEngine;

public class CameraController : MonoBehaviour
{
    public enum CameraMode
    {
        FreeControl,
        FollowPlayer
    }

    Transform followTarget;
    [SerializeField] Transform followWomanTarget;
    [SerializeField] Transform followManTarget;
    [SerializeField] CameraMode cameraMode = CameraMode.FreeControl;

    [SerializeField] bool mouseControl = true;

    [SerializeField] float rotationSpeed = 2f;
    [SerializeField] float distance = 5f;
    [SerializeField] float followDelay = 0.1f; // Takip gecikmesi

    //[SerializeField] float minVerticalAngle = -45f;
    //[SerializeField] float maxVerticalAngle = 45f;

    Vector2 framingOffset;
    [SerializeField] float followHeightOffset = 2f; // Y ekseni offset
    float followZOffset = -2f; // Z ekseni offset

    [SerializeField] bool invertCameraX;
    [SerializeField] bool invertCameraY;

    float rotationY;

    float invertXVal;
    float invertYVal;
    bool isLookingAtPoint = false;
    Transform lookAtPoint;
    float originalFOV;
    private void Awake()
    {
        if (PlayerPrefs.GetString("CharacterType") == "Woman")
        {
            followTarget = followWomanTarget;
        }
        else
            followTarget = followManTarget;

        if (PlayerPrefs.GetInt("CameraMode") == 0)
            cameraMode = CameraMode.FreeControl;
        else
            cameraMode = CameraMode.FollowPlayer;

    }
    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        originalFOV = Camera.main.fieldOfView;

        if (cameraMode == CameraMode.FreeControl)
        {
            followZOffset = -2;
            framingOffset = new Vector2(0, 2.15f);
        }
        else
        {
            followZOffset = 0;
            framingOffset = new Vector2(0, -1);
        }
    }

    private void Update()
    {
        if (isLookingAtPoint)
        {
            Vector3 direction = lookAtPoint.position - transform.position;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, followDelay);
            return;
        }

        invertXVal = (invertCameraX) ? -1 : 1;
        invertYVal = (invertCameraY) ? -1 : 1;

        if (cameraMode == CameraMode.FreeControl && mouseControl)
        {
            rotationY += Input.GetAxis("Mouse X") * invertXVal * rotationSpeed;
        }
        else if (cameraMode == CameraMode.FollowPlayer)
        {
            rotationY = Mathf.LerpAngle(rotationY, followTarget.eulerAngles.y, followDelay);
        }

        var targetRotation = Quaternion.Euler(0, rotationY, 0);
        var focusPosition = followTarget.position + new Vector3(framingOffset.x, framingOffset.y, 0);

        if (cameraMode == CameraMode.FollowPlayer)
        {
            focusPosition.z += followZOffset;
            focusPosition.y += followHeightOffset;
        }

        transform.position = focusPosition - targetRotation * new Vector3(0, 0, distance);
        transform.rotation = targetRotation;
    }

    public Quaternion PlanarRotation => Quaternion.Euler(0, rotationY, 0);

    public void LookAtPoint(Transform point)
    {
        lookAtPoint = point;
        isLookingAtPoint = true;
        Camera.main.fieldOfView = 21f;
    }

    public void ResetCamera()
    {
        isLookingAtPoint = false;
        Camera.main.fieldOfView = originalFOV;
    }
}
