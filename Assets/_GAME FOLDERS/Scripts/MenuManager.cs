using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    #region VARIABLES
    // Genel butonlar 
    [SerializeField] Button _startButton, _showTeamButton, _hideTeamButton, _hideStartButton, _quitButton, _showOptionsButton, _hideOptionsButton;
    [SerializeField] GameObject _teamPanel, _warningPanel, _loadingPanel, _startPanel, _optionsPanel, _modePanel, _typePanel;
    bool _isActive = false;
    AudioSource _audioSource;
    // Oyun modu
    [SerializeField] Button _easyMode, _hardMode;
    [SerializeField] Button _womanType, _manType;
    // Ayarlar menu
    [SerializeField] Slider _volumeSlider;
    [SerializeField] Button _dynamicCamera, _staticCamera;
    #endregion

    #region  UNITY
    private void Awake()
    {
        if (!PlayerPrefs.HasKey("Coefficient"))
            PlayerPrefs.SetFloat("Coefficient", 1f);
        if (!PlayerPrefs.HasKey("Volume"))
            PlayerPrefs.SetFloat("Volume", 1f);
        if (!PlayerPrefs.HasKey("CharacterType"))
            PlayerPrefs.SetString("CharacterType", "Man");
        if (!PlayerPrefs.HasKey("CameraMode"))
            PlayerPrefs.SetInt("CameraMode", 0);
    }
    private void Start()
    {
        float volume = PlayerPrefs.GetFloat("Volume"); ;
        _volumeSlider.value = volume;
        AudioListener.volume = volume;

        _audioSource = GetComponent<AudioSource>();
        AddListeners();

    }
    private void OnDestroy()
    {
        RemoveListeners();
    }
    void AddListeners()
    {
        _startButton.onClick.AddListener(StartGame);
        _hideStartButton.onClick.AddListener(HideStartPanel);
        _showTeamButton.onClick.AddListener(ShowTeamPanel);
        _hideTeamButton.onClick.AddListener(HideTeamPanel);
        _easyMode.onClick.AddListener(() => ModeSelected("Easy"));
        _hardMode.onClick.AddListener(() => ModeSelected("Hard"));

        _womanType.onClick.AddListener(() => CharacterTypeSelected("Woman"));
        _manType.onClick.AddListener(() => CharacterTypeSelected("Man"));
        _quitButton.onClick.AddListener(QuitGame);

        _showOptionsButton.onClick.AddListener(ShowOptionsPanel);
        _hideOptionsButton.onClick.AddListener(HideOptionsPanel);

        _dynamicCamera.onClick.AddListener(() => SelectedCameraMode(0));
        _staticCamera.onClick.AddListener(() => SelectedCameraMode(1));
    }
    void RemoveListeners()
    {
        _startButton.onClick.RemoveListener(StartGame);
        _hideStartButton.onClick.RemoveListener(HideStartPanel);
        _showTeamButton.onClick.RemoveListener(ShowTeamPanel);
        _hideTeamButton.onClick.RemoveListener(HideTeamPanel);
        _easyMode.onClick.RemoveListener(() => ModeSelected("Easy"));
        _hardMode.onClick.RemoveListener(() => ModeSelected("Hard"));

        _womanType.onClick.RemoveListener(() => CharacterTypeSelected("Woman"));
        _manType.onClick.RemoveListener(() => CharacterTypeSelected("Man"));
        _quitButton.onClick.RemoveListener(QuitGame);

        _showOptionsButton.onClick.RemoveListener(ShowOptionsPanel);
        _hideOptionsButton.onClick.RemoveListener(HideOptionsPanel);

        _dynamicCamera.onClick.RemoveListener(() => SelectedCameraMode(0));
        _staticCamera.onClick.RemoveListener(() => SelectedCameraMode(1));
    }
    #endregion

    #region TEAM WINDOW
    void ShowTeamPanel()
    {
        _teamPanel.SetActive(true);
        PanelEffect(_teamPanel, 0, 1f, 0.3f, Ease.InCirc);
    }
    void HideTeamPanel() =>
        PanelEffect(_teamPanel, 0, 0.2f, 0.2f, Ease.OutCirc, () => _teamPanel.SetActive(false));
    #endregion

    #region OPTIONS WINDOW
    void ShowOptionsPanel()
    {
        _optionsPanel.SetActive(true);
        _volumeSlider.value = PlayerPrefs.GetFloat("Volume");
        PanelEffect(_optionsPanel, 1, 1f, 0.3f, Ease.InCirc);
    }
    void HideOptionsPanel()
    {
        PanelEffect(_optionsPanel, 1, 0.2f, 0.2f, Ease.OutCirc, () => _optionsPanel.SetActive(false));
    }

    public void SetVolume(float value)
    {
        AudioListener.volume = value;
        _volumeSlider.value = value;
        PlayerPrefs.SetFloat("Volume", value);
    }
    void SelectedCameraMode(int modeType)
    {
        if (modeType == 0)
            PlayerPrefs.SetInt("CameraMode", 0);
        else
            PlayerPrefs.SetInt("CameraMode", 1);

        HideOptionsPanel();
    }
    #endregion

    #region UTILITY
    public void ShowHidePanel()
    {
        _isActive = !_isActive;
        if (_isActive)
        {
            _warningPanel.SetActive(true);
            PanelEffect(_warningPanel, 0, 1f, 0.3f, Ease.InCirc);
        }
        else
            PanelEffect(_warningPanel, 0, 0.2f, 0.2f, Ease.OutCirc, () => _warningPanel.SetActive(false));
    }
    void QuitGame() => Application.Quit();
    void PanelEffect(GameObject panel, int index, float endValue, float durationTime, Ease ease, TweenCallback tweenCallback = null)
    {
        _audioSource.Play();
        panel.transform.GetChild(index).transform.DOScale(endValue, durationTime).SetEase(ease).OnComplete(() =>
        {
            tweenCallback?.Invoke();
        });
    }
    #endregion

    #region START WINDOW
    void StartGame()
    {
        _startPanel.SetActive(true);
        PanelEffect(_startPanel, 1, 1f, 0.3f, Ease.InCirc);
    }
    void HideStartPanel()
    {
        PanelEffect(_startPanel, 1, 0.2f, 0.2f, Ease.OutCirc, () => _startPanel.SetActive(false));
    }
    void ModeSelected(string mode)
    {
        _audioSource.Play();
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
        _audioSource.Play();
        if (mode == "Woman")
            PlayerPrefs.SetString("CharacterType", "Woman");
        else
            PlayerPrefs.SetString("CharacterType", "Man");


        _loadingPanel.GetComponent<Image>().DOFade(1f, 1f).SetEase(Ease.InQuart).OnComplete(() => UnityEngine.SceneManagement.SceneManager.LoadScene(1));
    }
    #endregion
}
