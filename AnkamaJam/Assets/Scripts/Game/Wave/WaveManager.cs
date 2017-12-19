using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

public class WaveManager : MonoBehaviour
{
    [SerializeField]
    private List<Wave> m_waves;
    [SerializeField]
    private Text m_text;

    private int m_currentWaveIndex;
    private int m_lastWaveIndex;

    private int m_runningWaves;
    

    void Awake()
    {
        foreach (var wave in m_waves)
            m_lastWaveIndex = Mathf.Max(m_lastWaveIndex, wave.Index);

        m_text.color = new Color(1f, 1f, 1f, 0f);
    }

    public bool IsRunningWaves { get { return m_runningWaves != 0; } }

    public bool HasRemainingWaves { get { return m_currentWaveIndex <= m_lastWaveIndex; } }

    public void DisplayCurrentWave()
    {
        m_text.text = "Wave " + m_currentWaveIndex;
        var m_textTween = DOTween.Sequence();
        m_textTween.Append(m_text.DOFade(1f, 0.5f));
        m_textTween.AppendInterval(2f);
        m_textTween.Append(m_text.DOFade(0f, 0.5f));
        m_textTween.Play();
    }

    public void StartNextWave()
    {
        if (m_runningWaves != 0)
        {
            Debug.LogWarning("Trying to start wave when some other wave is still running");
            return;
        }

        foreach (var w in m_waves)
        {
            if (w.Index == m_currentWaveIndex)
            {
                StartCoroutine(w.Action());
                m_runningWaves++;
            }
        }

        Debug.LogFormat("StartWave (index={0}, runningWaves={1})", m_currentWaveIndex, m_runningWaves);

        m_currentWaveIndex++;

        DisplayCurrentWave();
    }

    public void EndWave()
    {
        m_runningWaves--;
        Debug.LogFormat("EndWave (runningWaves={0})", m_runningWaves);
    }

}
