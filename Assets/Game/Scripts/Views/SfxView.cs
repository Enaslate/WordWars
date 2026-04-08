using UnityEngine;

public class SfxView : MonoBehaviour
{
    [field: SerializeField] public AudioSource AudioSource { get; private set; }
    private SfxPoolController _sfxPool;
    private bool _started;

    public void Setup(SfxPoolController sfxPool)
    {
        _sfxPool = sfxPool;
    }

    public void PlaySound(AudioClip audioSource)
    {
        AudioSource.clip = audioSource;
        var pitch = Random.Range(0.9f, 1.11f);
        AudioSource.pitch = pitch;
        AudioSource.Play();
        _started = true;
    }

    private void Update()
    {
        while (!_started || AudioSource.isPlaying)
            return;

        _sfxPool.Release(this);
    }

    public void Reset()
    {
        _started = false;
    }
}