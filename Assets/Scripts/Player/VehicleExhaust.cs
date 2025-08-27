using UnityEngine;

public class VehicleExhaust : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;
    [Range(0.01f, 1f)]
    [SerializeField] float _maxParticleSize = 1f;
    [Range(0.01f, 1f)]
    [SerializeField] float _floatMinParticleSize = 0.1f;

    private void Start()
    {
        PlayExhaustEffect();
    }
    
    public void PlayExhaustEffect()
    {
        SetFlameSize(0f);
        _particleSystem.Play();
    }

    public void StopExhaustEffect()
    {
        Debug.Log("StopExhaustEffect");
        _particleSystem.Stop();
    }

    public void SetFlameSize(float speedNormalized)
    {
        var main = _particleSystem.main;
        main.startSize = Mathf.Lerp(_floatMinParticleSize, _maxParticleSize, speedNormalized);
    }
}
