using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class GamePlayUIManager : MonoBehaviour
{
    public TMP_Text speedText, endScreenMessage;

    public Button restartButton;

    private SignalBus _signalBus;
    private TimeManager _timeManager;

    [Inject]
    private void InjectDependencies(SignalBus signalBus, TimeManager timeManager)
    {
        _timeManager = timeManager;
        _signalBus = signalBus;
        _signalBus.Subscribe<UpdateSpeedText>(UpdateSpeedText);
        _signalBus.Subscribe<FinishLineReached>(SetEndGameScreenMessage);
    }

    private void UpdateSpeedText(UpdateSpeedText signal)
    {
        speedText.text = $"Speed: {(int)(signal.speed * 100)}";
    }

    private void Start()
    {
        SetButton();
    }

    private void SetButton()
    {
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(RestartButtonDelegate);
    }

    private void RestartButtonDelegate()
    {
        _signalBus.Fire<RestartGame>();
        restartButton.gameObject.SetActive(false);
        endScreenMessage.text = "";
    }

    private void SetEndGameScreenMessage()
    {
        string isFinishLineReachedInTime = _timeManager.Timer <= 30f ? "Yes" : "No";
        endScreenMessage.text = $"Finish line reached!\nTime taken: {(int)_timeManager.Timer} seconds\nRace won: {isFinishLineReachedInTime}";
        restartButton.gameObject.SetActive(true);
    }
}
