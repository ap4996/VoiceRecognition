using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class MenuUIManager : MonoBehaviour
{
    public Button startButton;
    
    private SpeechRecognizerManager _speechManager;
    private SignalBus _signalBus;

    [Inject]
    private void InjectDependencies(SpeechRecognizerManager speechManager, SignalBus signalBus)
    {
        _speechManager = speechManager;
        _signalBus = signalBus;
        _signalBus.Subscribe<RestartGame>(RestartGame);
    }

    private void Start()
    {
        SetButtons();
    }

    private void SetButtons()
    {
        startButton.onClick.RemoveAllListeners();
        startButton.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        gameObject.SetActive(false);
        _speechManager.StartListening();
    }

    private void RestartGame()
    {
        gameObject.SetActive(true);
    }
}
