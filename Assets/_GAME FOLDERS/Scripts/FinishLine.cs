using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
public class FinishLine : MonoBehaviour
{
    [SerializeField] GameObject _finishPanel;
    [SerializeField] Button _homeButton;
    private void Start()
    {
        _homeButton.onClick.AddListener(HomeButton);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _finishPanel.SetActive(true);
            _finishPanel.transform.GetChild(0).DOScale(1, 0.75f).SetEase(Ease.OutQuad);
            other.gameObject.GetComponent<PlayerController>().enabled = false;
        }
    }
    void HomeButton()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
}
