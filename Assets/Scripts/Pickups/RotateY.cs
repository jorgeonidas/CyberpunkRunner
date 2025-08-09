using UnityEngine;

public class RotateY : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 180f;

    void OnEnable()
    {
        transform.rotation = Quaternion.Euler(Vector3.zero);
    }
    
    void Update()
    {
        transform.Rotate(Vector3.up * _rotationSpeed * Time.deltaTime);
    }
}
