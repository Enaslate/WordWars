using UnityEngine;

public class VfxView : MonoBehaviour
{
    [field: SerializeField] public ParticleSystem ParticleSystem { get; private set; }
    private VfxPoolController _vfxPool;

    public void Setup(VfxPoolController vfxPool)
    {
        _vfxPool = vfxPool;
    }

    private void Update()
    {
        while (ParticleSystem.isPlaying)
            return;

        _vfxPool.Release(this);
    }
}