using UnityEngine;

public class RagdollControler : MonoBehaviour
{
    [SerializeField] private float _explosiveForce = 20f;
    [SerializeField] private Transform _ragdollRootBone;
    public void Setup(Transform originalRootBone)
    {
        MatchAllChildTransforms(originalRootBone, _ragdollRootBone);
        ApplyForceToRagdol(_ragdollRootBone, _explosiveForce);
    }

    private void MatchAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);
            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;
                //Rerursive call, next root and next clone
                MatchAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyForceToRagdol(Transform root, float explosionForce)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidbody))
            {
                childRigidbody.AddForce(
                new Vector3(Random.Range(-explosionForce, explosionForce), explosionForce, Random.Range(explosionForce/2, explosionForce)),
                ForceMode.Impulse);
            }
            ApplyForceToRagdol(child, explosionForce);
        }
    }
}
