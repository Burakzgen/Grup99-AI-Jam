using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button _startButton, _showTeamButton, _hideTeamButton, _quitButton;
    bool _isActive = false;
    [SerializeField] GameObject _teamPanel, _warningPanel, _loadingPanel;
    AudioSource _audioSource;
    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _startButton.onClick.AddListener(StartGame);
        _showTeamButton.onClick.AddListener(ShowTeamPanel);
        _hideTeamButton.onClick.AddListener(HideTeamPanel);
        _quitButton.onClick.AddListener(QuitGame);
    }

    void StartGame()
    {
        _audioSource.Play();
        _loadingPanel.GetComponent<Image>().DOFade(1f, 0.3f).SetEase(Ease.InQuart).OnComplete(() => UnityEngine.SceneManagement.SceneManager.LoadScene(1));
    }
    void ShowTeamPanel()
    {
        _teamPanel.SetActive(true);
        PanelEffect(_teamPanel, 1f, 0.3f, Ease.InCirc);
    }
    void HideTeamPanel() =>
        PanelEffect(_teamPanel, 0.2f, 0.2f, Ease.OutCirc, () => _teamPanel.SetActive(false));

    public void ShowHidePanel()
    {
        _isActive = !_isActive;
        if (_isActive)
        {
            _warningPanel.SetActive(true);
            PanelEffect(_warningPanel, 1f, 0.3f, Ease.InCirc);
        }
        else
            PanelEffect(_warningPanel, 0.2f, 0.2f, Ease.OutCirc, () => _warningPanel.SetActive(false));
    }
    void QuitGame() => Application.Quit();
    void PanelEffect(GameObject panel, float endValue, float durationTime, Ease ease, TweenCallback tweenCallback = null)
    {
        _audioSource.Play();
        panel.transform.GetChild(0).transform.DOScale(endValue, durationTime).SetEase(ease).OnComplete(() =>
        {
            tweenCallback?.Invoke();
        });
    }
}
