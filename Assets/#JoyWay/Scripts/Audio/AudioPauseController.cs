using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioPauseController : MonoBehaviour
{
    [SerializeField] private AudioSource source;

    void OnEnable()
    {
        PauseManager.instance.onPause += PauseState;
    }

    private void PauseState(bool state)
    {
        if (state) source.Pause();
        else source.Play();
    }
    
    void OnDisable()
    {
        PauseManager.instance.onPause -= PauseState;
    }
}
