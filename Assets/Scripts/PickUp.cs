using UnityEngine;

public class PickUp : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(StringConstants.PLAYER_TAG))
        {
            Debug.Log(other.gameObject.name);
        }
    }
}
