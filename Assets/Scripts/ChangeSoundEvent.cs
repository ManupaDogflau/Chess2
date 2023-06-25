using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ChangeSound", menuName = "Events/Level/ChangeSound")]
public class ChangeSoundEvent : GameEventScriptable
{

    public SoundEventType Type;
    public float newValue;
    public AudioClip clip;
    public bool enabled;
}
