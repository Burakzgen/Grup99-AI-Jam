using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class FinishLine : MonoBehaviour
{
    [SerializeField] GameObject _finishPanel;
    [SerializeField] Button _homeButton;
    [SerializeField] Transform cameraTargetPosition; // Kameranın bakacağı pozisyon
    [SerializeField] float cameraMoveDuration = 2f; // Kameranın hareket süresi
    [SerializeField] float victoryDuration = 3f; // Victory animasyonu süresi
    [SerializeField] CameraController camareController;
    private Animator _animator;
    [SerializeField] GameObject _particalSystems;
    AudioSource _audioSource;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _homeButton.onClick.AddListener(HomeButton);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _animator = other.GetComponent<Animator>();
            StartCoroutine(EndSequence(other));
        }
    }

    private IEnumerator EndSequence(Collider player)
    {
        _audioSource.Play();
        // Karakterin kontrolünü devre dışı bırak
        player.gameObject.GetComponent<PlayerController>().enabled = false;
        camareController.enabled = false;
        _animator.SetFloat("Speed", 0);
        _particalSystems.gameObject.SetActive(true);
        // Kamerayı hedef pozisyona taşı
        Vector3 initialPosition = Camera.main.transform.position;
        Quaternion initialRotation = Camera.main.transform.rotation;
        Vector3 targetPosition = cameraTargetPosition.position;
        Quaternion targetRotation = cameraTargetPosition.rotation;

        float timer = 0f;
        while (timer < cameraMoveDuration)
        {
            Camera.main.transform.position = Vector3.Lerp(initialPosition, targetPosition, timer / cameraMoveDuration);
            Camera.main.transform.rotation = Quaternion.Lerp(initialRotation, targetRotation, timer / cameraMoveDuration);
            timer += Time.deltaTime;
            yield return null;
        }

        Camera.main.transform.position = targetPosition;
        Camera.main.transform.rotation = targetRotation;

        // Karakterin Y ekseninde 180 derece dönmesini sağla
        player.transform.DORotate(new Vector3(0, -135.69f, 0), victoryDuration);
        // Victory animasyonunu oynat
        _animator.Play("Victory"); // "Victory" animasyon klip ismi olmalı

        // Victory animasyon süresini bekle
        yield return new WaitForSeconds(victoryDuration);

        // Paneli aktif et ve ölçeklendirme animasyonunu çalıştır
        _finishPanel.SetActive(true);
        _finishPanel.transform.GetChild(0).DOScale(1, 0.75f).SetEase(Ease.OutQuad);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    void HomeButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
