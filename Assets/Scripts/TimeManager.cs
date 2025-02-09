using TMPro;
using UnityEngine;
using Zenject;

public class TimeManager : MonoBehaviour
{
    public TMP_Text timerText;

    private float timer;
    private bool isTimerRunning;

    private SignalBus _signalBus;

    public float Timer { get => timer; set => timer = value; }

    [Inject]
    private void InjectDependencies(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<StartGame>(StartTimer);
        _signalBus.Subscribe<FinishLineReached>(StopTimer);
    }

    private void StartTimer()
    {
        timer = 0f;
        isTimerRunning = true;
    }

    private void StopTimer()
    {
        isTimerRunning = false;
    }

    private void Update()
    {
        if(isTimerRunning)
        {
            timer += Time.deltaTime;
            UpdateTimerText();
        }
    }

    private void UpdateTimerText()
    {
        timerText.text = "TIME: " + ((int)timer).ToString();
    }
}
