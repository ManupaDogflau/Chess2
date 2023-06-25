using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource _sfx;
    [SerializeField] private AudioSource _music;
    [SerializeField] private bool _soundNotActive;

    public void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void SelectChange(ChangeSoundEvent changeSoundEvent)
    {
        if (_soundNotActive) return;
        switch (changeSoundEvent.Type)
        {
            case SoundEventType.ToggleSound:
                SoundToggler();
                break;
            case SoundEventType.ChangeSfx:
                SetSfxVolume(changeSoundEvent.newValue);
                break;
            case SoundEventType.ChangeMusic:
                SetMusicVolume(changeSoundEvent.newValue);
                break;
            case SoundEventType.PlayMusic:
                PlayMusic(changeSoundEvent.clip);
                break;
            case SoundEventType.PlaySfx:
                PlaySfx(changeSoundEvent.clip);
                break;
            case SoundEventType.SetMusicMute:
                SetMusicMute(changeSoundEvent.enabled);
                break;
            case SoundEventType.SetSFXMute:
                SetSfxMute(changeSoundEvent.enabled);
                break;
        }
    }

    private void PlaySfx(AudioClip audioClip)
    {
        _sfx.PlayOneShot(audioClip);
    }

    private void PlayMusic(AudioClip audioClip)
    {
        _music.clip = audioClip;
        _music.Play();
    }

    private void SetSfxVolume(float value)
    {
        _sfx.volume = Mathf.Clamp01(value);
    }

    private void SetMusicVolume(float value)
    {
        _music.volume = Mathf.Clamp01(value);
    }

    public float GetSfxVolume()
    {
        return _sfx.volume;
    }

    public float GetMusicVolume()
    {
        return _music.volume;
    }

    public void SetMusicMute(bool enabled)
    {
        _music.enabled = enabled;
    }

    public void SetSfxMute(bool enabled)
    {
        _sfx.enabled = enabled;
    }

    private void SoundToggler()
    {
        SetSfxMute(!_sfx.enabled);
        SetMusicMute(!_music.enabled);
    }

}