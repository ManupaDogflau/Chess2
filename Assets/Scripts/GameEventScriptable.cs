using System.Collections.Generic;
using UnityEngine;

public class GameEventScriptable : ScriptableObject
{
    private readonly List<GameEventListener> _listeners = new List<GameEventListener>();
    public virtual void Fire()
    {
        foreach (var eventListener in _listeners)
            eventListener.OnEventRaise();
    }

    public void Register(GameEventListener listener)
    {
        if (!_listeners.Contains(listener))
            _listeners.Add(listener);
    }

    public void Unregister(GameEventListener listener)
    {
        if (_listeners.Contains(listener))
            _listeners.Remove(listener);
    }
}