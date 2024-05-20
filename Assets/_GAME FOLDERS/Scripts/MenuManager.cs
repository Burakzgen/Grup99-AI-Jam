using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    [SerializeField] Button _startButton, _showTeamButton, _hideTeamButton, _hideOptionsButton, _quitButton;
    bool _isActive = false;
    [SerializeField] GameObject _teamPanel, _warningPanel, _loadingPanel, _startPanel, _modePanel, _typePanel;
    AudioSource _audioSource;
    [SerializeField] Button _easyMode, _hardMode;
    [SerializeField] Button _womanType, _manType;
    private void Start()
    {

        PlayerPrefs.SetFloat("Coefficient", 1f);
        PlayerPrefs.SetString("CharacterType", "Man");

        _audioSource = GetComponent<AudioSource>();
        _startButton.onClick.AddListener(StartGame);
        _showTeamButton.onClick.AddListener(ShowTeamPanel);
        _hideTeamButton.onClick.AddListener(HideTeamPanel);
        _easyMode.onClick.AddListener(() => ModeSelected("Easy"));
        _hardMode.onClick.AddListener(() => ModeSelected("Hard"));

        _womanType.onClick.AddListener(() => CharacterTypeSelected("Woman"));
        _manType.onClick.AddListener(() => CharacterTypeSelected("Man"));
        _quitButton.onClick.AddListener(QuitGame);

    }

    void StartGame()
    {
        _startPanel.SetActive(true);
        PanelEffect(_startPanel, 1f, 0.3f, Ease.InCirc);
        _startPanel.transform.GetChild(1).transform.DOScale(1f, 0.3f).SetEase(Ease.InCirc);

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

    void ModeSelected(string mode)
    {
        if (mode == "Easy")
            PlayerPrefs.SetFloat("Coefficient", 1f);
        else
            PlayerPrefs.SetFloat("Coefficient", 2f);

        _typePanel.SetActive(true);
        _typePanel.transform.DOScale(1f, 0.2f).SetEase(Ease.InCirc).OnComplete(() =>
        {
            _modePanel.SetActive(false);
        });
    }
    void CharacterTypeSelected(string mode)
    {
        if (mode == "Woman")
            PlayerPrefs.SetString("CharacterType", "Woman");
        else
            PlayerPrefs.SetString("CharacterType", "Man");


        _loadingPanel.GetComponent<Image>().DOFade(1f, 0.3f).SetEase(Ease.InQuart).OnComplete(() => UnityEngine.SceneManagement.SceneManager.LoadScene(1));
    }
}
