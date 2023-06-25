using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleMusicPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip clip;
    // Start is called before the first frame update
    void Start()
    {
        SoundEmitter.Instance().PlayMusic(clip);
    }

}
