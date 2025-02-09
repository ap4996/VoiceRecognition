# VoiceRecognition
Overview

This is a Unity-based Android game where a car moves forward when the player speaks specific words (e.g., "Go", "Vroom"). The goal is to complete the race before the timer runs out.

The game initially used Unity’s built-in SpeechRecognizer API via AndroidJavaObject to process speech input. However, due to limitations in Unity Editor testing and continuous speech detection, improvements were required.
Approach

- Speech Recognition (Using Android Speech API)
        Used android.speech.SpeechRecognizer via Unity’s AndroidJavaObject to recognize speech input.
        The game listens for keywords and triggers movement when detected.

- Unity Integration
        Implemented a SpeechManager script to interact with the Android Speech API.
        Used AndroidJavaObject to call Android's speech recognition functions.

- Gameplay Mechanics
        The car moves forward when the player says "Run".
        If the player stops speaking, the car slows down and eventually stops.
        A speed meter UI updates dynamically based on voice input.
        A timer (30s limit) challenges players to reach the finish line in time.

Challenges & Limitations
1. Speech Recognition in Unity Editor

    Unity does not support native Android Speech API in the Editor.
    Current Workaround: Used KeywordRecognizer in Editor mode for testing speech commands.

2. Continuous Speech Detection Issue

    Android’s SpeechRecognizer stops listening after detecting speech.
    Potential Fix: Implement a loop to restart speech recognition after each result.

3. Delays in Speech Processing

    Speech processing introduces a slight delay in movement response.
    Future Plan: Optimize recognition settings to reduce latency.

Next Steps

🔹 Implement a custom Java plugin (SpeechRecognitionPlugin.java) for better control over speech recognition.
🔹 Improve continuous speech detection by restarting recognition automatically.
🔹 Add UI feedback to indicate when the game is actively listening.
How to Test

- Unity Editor (Testing Mode)

    Run the game in Play Mode.
    Type or say "Run" (using KeywordRecognizer).
    Observe the triangle car moving forward.

- Android Device (Real Speech Recognition)

    Build & Run the game on an Android device.
    Grant microphone permissions when prompted.
    Say "Run" to move the triangle car forward.
    Stop speaking to slow down and stop.
