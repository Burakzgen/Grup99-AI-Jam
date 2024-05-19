using DG.Tweening;
using System.Collections;
using UnityEngine;

public class CameraCutscene : MonoBehaviour
{
    [SerializeField] private Transform initialCameraPosition;
    [SerializeField] private Transform targetCameraPosition;
    [SerializeField] private float moveDuration = 3f;
    [SerializeField] CameraController m_CameraController;
    private void Start()
    {
        StartCoroutine(CutsceneAndCountdown());
    }

    private IEnumerator CutsceneAndCountdown()
    {
        Camera.main.transform.position = initialCameraPosition.position;
        Camera.main.transform.rotation = initialCameraPosition.rotation;

        Camera.main.transform.DOMove(targetCameraPosition.position, moveDuration).SetEase(Ease.InOutQuad);
        Camera.main.transform.DORotateQuaternion(targetCameraPosition.rotation, moveDuration).SetEase(Ease.InOutQuad);

        yield return new WaitForSeconds(moveDuration);
        m_CameraController.enabled = true;
    }

}
