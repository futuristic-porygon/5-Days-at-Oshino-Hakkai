
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class MicrogameTimer : MonoBehaviour
{
    [Header("Timer")]
    [SerializeField] private float timeLimit = 30f;

    [Header("Instructions")]
    [SerializeField] private GameObject instructionPanel;
    [SerializeField] private TMP_Text instructionText;

    [Header("Time Up")]
    [SerializeField] private GameObject timesUpPanel;
    [SerializeField] private TMP_Text timesUpText;

    [Header("Vignette")]
    [SerializeField] private Volume globalVolume;
    [SerializeField, Range(0f, 1f)] private float maxVignetteIntensity = 0.45f;
    [SerializeField] private float vignetteStartTime = 10f;

    [Header("Events")]
    [SerializeField] private UnityEvent onTimeUp;

    private float timeRemaining;
    private bool isRunning;
    private Vignette vignette;

    private void Awake()
    {
        timeRemaining = timeLimit;

        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
        }

        if (timesUpPanel != null)
        {
            timesUpPanel.SetActive(false);
        }

        if (globalVolume != null && globalVolume.profile.TryGet(out vignette))
        {
            vignette.active = true;
            vignette.intensity.value = 0f;
        }
        else
        {
            Debug.LogWarning("No se encontro el efecto Vignette en el Global Volume.");
        }

    }

    private void Update()
    {
        if (!isRunning)
        {
            return;
        }

        timeRemaining -= Time.deltaTime;
        timeRemaining = Mathf.Max(0f, timeRemaining);
       
        UpdateVignette();

        if (timeRemaining <= 0f)
        {
            FinishTimer();
        }
    }

    public void StartTimer()
    {
        if (isRunning)
        {
            return;
        }

        if (instructionPanel != null)
        {
            instructionPanel.SetActive(false);
        }

        if (timesUpPanel != null)
        {
            timesUpPanel.SetActive(false);
        }

        timeRemaining = timeLimit;
        isRunning = true;
    }

    public void PauseTimer()
    {
        isRunning = false;
    }

    public void ResumeTimer()
    {
        if (timeRemaining > 0f)
        {
            isRunning = true;
        }
    }

    public void ResetTimer()
    {
        isRunning = false;
        timeRemaining = timeLimit;

        if (instructionPanel != null)
        {
            instructionPanel.SetActive(true);
        }

        if (timesUpPanel != null)
        {
            timesUpPanel.SetActive(false);
        }

        if (vignette != null)
        {
            vignette.intensity.value = 0f;
        }

    }

    private void FinishTimer()
    {
        isRunning = false;

        if (timesUpPanel != null)
        {
            timesUpPanel.SetActive(true);
        }

        onTimeUp?.Invoke();
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    private void UpdateVignette()
    {
        if (vignette == null)
        {
            return;
        }

        float progress = 1f - Mathf.Clamp01(timeRemaining / vignetteStartTime);

        vignette.intensity.value = progress * maxVignetteIntensity;
    }
}
