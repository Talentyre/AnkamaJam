using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class GameSceneUI : MonoBehaviour
{
    public Text ScoreText;
    public Text ComboText;
    public Text SoulText;
    public Image SoulGauge;
    public Slider AlertGauge;
    public Image AlertGaugeImage;
    public Transform SoulGainParent;
    public GameObject SoulGainFxPrefab;
    public CanvasGroup IntroCanvasGroup;

    private Sequence _alertTweener;
    private Vector2 _soulGaugeBaseSizeDelta;

    // Use this for initialization
    void Start()
    {
        _soulGaugeBaseSizeDelta = SoulGauge.rectTransform.sizeDelta;
        SoulGauge.rectTransform.sizeDelta = new Vector2(_soulGaugeBaseSizeDelta.x, 0);

        var doPunchScale = ComboText.transform.DOPunchScale(Vector3.one * 0.1f, 0.25f).Pause();
        GameSingleton.Instance.OnAlertUpdate += f =>
        {
            var alertPercent = (float) f / GameSingleton.MaxAlert;
            AlertGauge.value = alertPercent;
            _alertTweener = DOTween.Sequence();
            _alertTweener.Append(AlertGaugeImage.DOFade(0f, 0.2f));
            _alertTweener.Append(AlertGaugeImage.DOFade(1f, 0.2f));
            _alertTweener.Play();
        };
        GameSingleton.Instance.OnSoulGain += () => { StartCoroutine(SoulGainFeedback()); };

        GameSingleton.Instance.OnSoulUpdate += UpdateSoul;
        GameSingleton.Instance.OnComboUpdate += f =>
        {
            if (f > 1)
            {
                ComboText.text = "x " + f;
                doPunchScale.Restart();
            }
            else
            {
                ComboText.text = "";
            }
        };
        GameSingleton.Instance.OnScoreUpdate += f => { ScoreText.text = f.ToString(); };
    }

    private void UpdateSoul(long soul)
    {
        SoulText.text = soul.ToString();
        var percent = (float) soul / GameSingleton.MaxSouls;
        SoulGauge.rectTransform.sizeDelta = new Vector2(_soulGaugeBaseSizeDelta.x, _soulGaugeBaseSizeDelta.y * percent);
    }

    private IEnumerator SoulGainFeedback()
    {
        yield return new WaitForSeconds(1f);
        GameObject go = Instantiate(SoulGainFxPrefab);
        go.transform.SetParent(SoulGainParent);
        go.transform.localScale = new Vector3(Random.Range(0, 2) == 0 ? 1 : -1, 1, 1);
        go.transform.localPosition = Vector3.zero;
    }

    public void QuitGame()
    {
        var showPanels = SceneHandler.Instance.gameObject.GetComponent<ShowPanels>();
        SceneHandler.Instance.Load(SceneHandler.StartScene, () => showPanels.ShowMenu());
    }

    public void OnIntroClick()
    {
        StartCoroutine(IntroAnim());
    }

    private IEnumerator IntroAnim()
    {
        IntroCanvasGroup.DOFade(0f, 2f);
        yield return new WaitForSeconds(3f);
        GameSingleton.Instance.GameStarted = true;
    }
}