using Zenject;

public class SceneInstaller : MonoInstaller
{
    public SpeechRecognizerManager speechManager;
    public TimeManager timeManager;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<VoiceCommandSignal>();
        Container.DeclareSignal<RestartGame>();
        Container.DeclareSignal<UpdateSpeedText>();
        Container.DeclareSignal<FinishLineReached>();
        Container.DeclareSignal<StartGame>();
        Container.BindInstance(speechManager).AsSingle();
        Container.BindInstance(timeManager).AsSingle();
    }
}