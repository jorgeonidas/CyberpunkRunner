using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform _originalRootBone;
    [SerializeField] private Transform _ragdollPrefab;

    public void SpawnRagdoll()
    {
        Transform ragdollTransform = Instantiate(_ragdollPrefab, transform.position, transform.rotation);
        RagdollControler unitRagdoll = ragdollTransform.GetComponent<RagdollControler>();
        unitRagdoll.Setup(_originalRootBone);
    }
}
