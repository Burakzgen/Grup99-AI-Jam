using UnityEngine;

public class TriggerObjects : MonoBehaviour
{
    public Transform cameraFocusPoint;
    float lookDuration = 1.2f;
    private void Start()
    {
        lookDuration = Random.Range(0.5f, 2);
    }
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
