using System;
using UnityEngine;

public abstract class EventChannel<T> : ScriptableObject
{
    public event Action<T> OnEventRaised;
    public void Raise(T payload) => OnEventRaised?.Invoke(payload);
}
