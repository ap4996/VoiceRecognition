using System.Collections;
using UnityEngine;
using Zenject;

public class CarController : MonoBehaviour
{
    public float speed = 0f;
    public float acceleration = 2f;
    public float deceleration = 1f;

    private bool isMoving = false;
    private SignalBus _signalBus;

    [Inject]
    private void InjectDependencies(SignalBus signalBus)
    {
        _signalBus = signalBus;
        _signalBus.Subscribe<VoiceCommandSignal>(OnVoiceCommand);
        _signalBus.Subscribe<RestartGame>(ResetCar);
        _signalBus.Subscribe<StartGame>(ResetCar);
    }

    void Update()
    {
        if (isMoving)
        {
            speed += acceleration * Time.deltaTime;
        }
        else
        {
            speed -= deceleration * Time.deltaTime;
        }

        speed = Mathf.Clamp(speed, 0, 10); // Max speed limit
        transform.Translate(Vector3.up * speed * Time.deltaTime);
        _signalBus.Fire(new UpdateSpeedText { speed = speed });
    }

    public void OnVoiceCommand(VoiceCommandSignal signal)
    {
        Debug.Log($"OnVoiceCommand {signal.command}");
        isMoving = true;
        StartCoroutine(StopAfterDelay());
    }

    IEnumerator StopAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        isMoving = false;
    }

    private void ResetCar()
    {
        transform.position = new Vector3(0, -4, 0);
        isMoving = false;
        speed = 0;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision");
        Debug.Log(collision.collider.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Triggering");
        Debug.Log(collision.gameObject.name);
        if(collision.CompareTag("Finish"))
        {
            _signalBus.Fire<FinishLineReached>();
        }
    }
}
