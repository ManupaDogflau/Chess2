using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour
{
    [SerializeField] private GameEventScriptable gameEventScriptable;
    [SerializeField] private UnityEvent response;

    private void OnEnable() => gameEventScriptable.Register(this);

    private void OnDisable() => gameEventScriptable.Unregister(this);
		
    public void OnEventRaise() => response?.Invoke();
    
}