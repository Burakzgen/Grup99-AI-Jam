using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    [SerializeField] GameObject[] _objects;
    [SerializeField] private Text countdownText;
    [SerializeField] private GameObject panelToClose;
    [SerializeField] private float countdownDuration = 1f;
    WaitForSeconds c_WaitForSeconds;
    AudioSource _audioSourceComp;
    private void Start()
    {
        _audioSourceComp = GetComponent<AudioSource>();

        c_WaitForSeconds = new WaitForSeconds(countdownDuration);
        StartCoroutine(CountdownSequence());
    }

    private IEnumerator CountdownSequence()
    {
        for (int i = 3; i > 0; i--)
        {
            countdownText.text = i.ToString();
            countdownText.transform.DOScale(1.5f, countdownDuration / 2).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad);
            _audioSourceComp.Play();
            yield return c_WaitForSeconds;
        }

        countdownText.text = "GO!";
        countdownText.transform.DOScale(1.5f, countdownDuration / 2).SetLoops(2, LoopType.Yoyo).SetEase(Ease.OutQuad);
        yield return c_WaitForSeconds;

        panelToClose.SetActive(false);
        for (int i = 0; i < _objects.Length; i++)
        {
            _objects[i].SetActive(true);
        }
    }
}
