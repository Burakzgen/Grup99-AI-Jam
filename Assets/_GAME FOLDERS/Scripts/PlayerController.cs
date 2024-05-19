using System.Collections;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float rotationSpeed = 500f;

    [SerializeField] float groundCheckRadius = 0.2f;
    [SerializeField] Vector3 groundCheckOffset;
    [SerializeField] LayerMask groundLayer;

    bool isGrounded;

    float ySpeed;

    Quaternion targetRotation;

    CameraController cameraController;
    CharacterController characterController;
    Animator animator;
    bool isCutSceneActive = false;
    AnimatorStateInfo previousAnimationState;

    private void Awake()
    {
        cameraController = Camera.main.GetComponent<CameraController>();
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
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
        previousAnimationState = animator.GetCurrentAnimatorStateInfo(0);
        animator.enabled = false;

        cameraController.LookAtPoint(lookAtPoint);
        Quaternion lookRotation = Quaternion.LookRotation(lookAtPoint.position - transform.position);
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
