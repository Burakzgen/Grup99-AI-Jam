using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotationSpeed = 500f;
    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Volume globalVolume; // Volume referansı
    [SerializeField] float maxChromaticAberration = 1f; // Maksimum chromatic aberration değeri

    bool isGrounded;
    float ySpeed;
    Quaternion targetRotation;
    CameraController cameraController;
    CharacterController characterController;
    Animator animator;
    ChromaticAberration chromaticAberration;
    bool isCutSceneActive = false;
    [SerializeField] ParticleSystem _stunPartical;
    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();

        // Volume içinden Chromatic Aberration bileşenini alın
        if (globalVolume != null)
        {
            globalVolume.profile.TryGet(out chromaticAberration);
        }
        else
        {
            Debug.LogError("Global Volume referansı atanmadı!");
        }
    }

    private void Update()
    {
        if (isCutSceneActive) return;

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        v = Mathf.Clamp(v, 0, 1);
        float moveAmount = Mathf.Abs(h) + Mathf.Abs(v);
        var moveInput = (new Vector3(h, 0, v)).normalized;
        var moveDir = cameraController.PlanarRotation * moveInput;

        GroundCheck();

        if (isGrounded)
        {
            ySpeed = -0.5f;
        }
        else
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;
        }

        var velocity = moveDir * moveSpeed;
        velocity.y = ySpeed;
        characterController.Move(velocity * Time.deltaTime);

        if (moveAmount > 0)
        {
            targetRotation = Quaternion.LookRotation(moveDir);
        }

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        animator.SetFloat("Speed", moveAmount);

        // Chromatic Aberration efektini hızla orantılı olarak ayarlayın
        if (chromaticAberration != null)
        {
            float chromaticAberrationAmount = Mathf.Lerp(0, maxChromaticAberration, moveAmount);
            chromaticAberration.intensity.value = chromaticAberrationAmount;
        }
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius, groundLayer);
    }

    public void StartCutScene(Transform lookAtPoint, float duration)
    {
        StartCoroutine(CutSceneCoroutine(lookAtPoint, duration));
    }

    private IEnumerator CutSceneCoroutine(Transform lookAtPoint, float duration)
    {
        isCutSceneActive = true;
        animator.SetFloat("Speed", 0);
        _stunPartical.gameObject.SetActive(true);
        yield return new WaitForSeconds(1.25f);
        _stunPartical.gameObject.SetActive(false);
        animator.enabled = false;
        cameraController.LookAtPoint(lookAtPoint);

        Vector3 direction = (lookAtPoint.position - transform.position).normalized;
        direction.y = 0; // Y eksenini sıfırla ki sadece yatay düzlemde döndürsün
        Quaternion lookRotation = Quaternion.LookRotation(direction);

        float timer = 0;
        while (timer < duration)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, timer / duration);
            timer += Time.deltaTime;
            yield return null;
        }

        yield return new WaitForSeconds(duration);

        animator.enabled = true;
        cameraController.ResetCamera();
        isCutSceneActive = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(0, 1, 0, 0.5f);
        Gizmos.DrawSphere(transform.TransformPoint(groundCheckOffset), groundCheckRadius);
    }
}
