using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private SfxPoolController _sfxPool;

    [SerializeField] private AudioClip _explosion;
    [SerializeField] private AudioClip[] _clicks;

    public void PlayExplosion(Vector3 position)
    {
        var soundView = _sfxPool.Get();
        soundView.transform.position = position;
        soundView.PlaySound(_explosion);
    }

    public void PlayClick()
    {
        var soundView = _sfxPool.Get();
        var next = Random.Range(0, _clicks.Length);
        soundView.PlaySound(_clicks[next]);
    }
}