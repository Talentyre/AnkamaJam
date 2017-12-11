using System;
using System.Collections;
using UnityEngine;

public class ScreenFader : MonoBehaviour
{

    public const float DefaultDuration = 1.5f;

    public Texture2D BlackFadeTexture;
    public Texture2D WhiteFadeTexture;

    private Texture2D _fadeTexture;

    private float _timer;
    private float _duration;
    private float _direction;
    private Action _callback;
    private float _alpha;

    public void FadeIn(float duration = 1f, Action callback = null, bool black = true)
    {
        Fade(1f, duration, callback, black);
    }

    public void FadeOut(float duration = 1f, Action callback = null, bool black = true)
    {
        Fade(-1f, duration, callback, black);
    }

    private void Update()
    {
        if (_timer <= 0)
        {
            if (_callback != null) StartCoroutine(End(_callback));
            _callback = null;
            return;
        }

        _timer -= Time.deltaTime;
        _alpha = _direction >= 0
            ? Mathf.Lerp(1, 0, _timer / _duration)
            : Mathf.Lerp(0, 1, _timer / _duration);
    }

    private void OnGUI()
    {
        if (_alpha <= 0) return;
        GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, _alpha);
        GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), _fadeTexture);
    }

    private static IEnumerator End(Action callback)
    {
        yield return null;
        callback.Invoke();
    }

    private void Fade(float direction, float duration, Action callback, bool black)
    {
        _duration = _timer = duration;
        _direction = direction;
        _callback = callback;
        _fadeTexture = black ? BlackFadeTexture : WhiteFadeTexture;
    }
}