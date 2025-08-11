using UnityEngine;

public class VehicleExhaust : MonoBehaviour
{
    [SerializeField] ParticleSystem _particleSystem;

    public void PlayExhaustEffect()
    {
        _particleSystem.Play();
    }

    public void StopExhaustEffect()
    {
        Debug.Log("StopExhaustEffect");
        _particleSystem.Stop();
    }
}
