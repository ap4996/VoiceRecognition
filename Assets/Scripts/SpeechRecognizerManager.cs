using UnityEngine;
using UnityEngine.Android;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Windows.Speech;
using System;
using System.Linq;
using Zenject;

public class SpeechRecognizerManager : MonoBehaviour
{
    private AndroidJavaObject speechRecognizer;
    private AndroidJavaObject recognitionIntent;
    private AndroidJavaObject activity;
    private bool isListening = false;

    private KeywordRecognizer keywordRecognizer;
    private List<string> actions = new List<string>();

    private string wordToRecognize = "run";

    [Inject] private SignalBus _signalBus;

    [Inject]
    private void InjectDependencies(SignalBus signalBus)
    {
        _signalBus = signalBus;
        SubscribeToSignals();
    }

    void Start()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            CheckForMicrophonePermission();
        }
        else
        {
            SetKeywordActions();
        }
        InitializeSpeechRecognizer();
    }

    private void CheckForMicrophonePermission()
    {
        // Request microphone permission
        if (!Permission.HasUserAuthorizedPermission(Permission.Microphone))
        {
            Permission.RequestUserPermission(Permission.Microphone);
        }
    }

    private void SetKeywordActions()
    {
        actions.Add(wordToRecognize);
    }

    void InitializeSpeechRecognizer()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            speechRecognizer = new AndroidJavaObject("android.speech.SpeechRecognizer", activity);
            recognitionIntent = new AndroidJavaObject("android.content.Intent",
                "android.speech.action.RECOGNIZE_SPEECH");

            recognitionIntent.Call<AndroidJavaObject>("putExtra",
                "android.speech.extra.LANGUAGE_MODEL", "android.speech.extra.LANGUAGE_MODEL_FREE_FORM");
            recognitionIntent.Call<AndroidJavaObject>("putExtra",
                "android.speech.extra.PARTIAL_RESULTS", true);
        }
        else
        {
            keywordRecognizer = new KeywordRecognizer(actions.ToArray());
            keywordRecognizer.OnPhraseRecognized += OnPhraseRecognized;
        }
    }

    private void OnPhraseRecognized(PhraseRecognizedEventArgs args)
    {
        _signalBus.Fire(new VoiceCommandSignal { command = args.text });
    }

    public void StartListening()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            if (!isListening)
            {
                isListening = true;
                activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
                {
                    speechRecognizer.Call("startListening", recognitionIntent);
                }));
            }
        }
        else
        {
            keywordRecognizer.Start();
            _signalBus.Fire<StartGame>();
        }
    }

    public void StopListening()
    {
        if (Application.platform == RuntimePlatform.Android && isListening)
        {
            isListening = false;
            activity.Call("runOnUiThread", new AndroidJavaRunnable(() =>
            {
                speechRecognizer.Call("stopListening");
            }));
        }
        else if (isListening)
        {
            keywordRecognizer.Stop();
        }
    }

    // Callback when speech is recognized (Android)
    public void OnSpeechResult(string recognizedText)
    {
        Debug.Log("Speech Recognized: " + recognizedText);

        if (recognizedText.ToLower().Contains(wordToRecognize))
        {
            _signalBus.Fire(new VoiceCommandSignal { command = recognizedText });
        }
    }

    private void OnDestroy()
    {
        StopListening();
        UnsubscribeToSignals();
    }

    private void SubscribeToSignals()
    {
        _signalBus.Subscribe<FinishLineReached>(StopListening);
    }

    private void UnsubscribeToSignals()
    {
        _signalBus.TryUnsubscribe<FinishLineReached>(StopListening);
    }
}
