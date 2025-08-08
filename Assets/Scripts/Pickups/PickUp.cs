using UnityEngine;

public abstract class PickUp : MonoBehaviour
{
    //[SerializeField] float _rotationSpeed = 180f;

    private void Update()
    {
        //transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.PLAYER_TAG))
        {
            OnPickUp();
            Destroy(gameObject);
        }
    }

    protected abstract void OnPickUp();
}
