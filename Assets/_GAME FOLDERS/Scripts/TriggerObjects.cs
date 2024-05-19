using UnityEngine;

public class TriggerObjects : MonoBehaviour
{
    public Transform cameraFocusPoint;
    public float lookDuration = 3f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);

            PlayerController playerController = other.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.StartCutScene(cameraFocusPoint, lookDuration);
            }
        }
    }
}
