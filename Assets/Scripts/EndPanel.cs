using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class EndPanel : MonoBehaviour
{
    [SerializeField] private Slider _music_slider;
    [SerializeField] private Slider _sfx_slider;
    [SerializeField] private AudioClip _button_sound;
    public void ChangeMusicVolume(float f)
    {
        SoundEmitter.Instance().ChangeMusicVolume(f);
    }

    public void ChangeSFXVolume(float f)
    {
        SoundEmitter.Instance().ChangeSFXVolume(f);
    }

    public void OnEnable()
    {
        SoundEmitter.Instance().CheckAudioManager();
        _music_slider.value = FindObjectOfType<SoundManager>().GetMusicVolume();
        _sfx_slider.value = FindObjectOfType<SoundManager>().GetSfxVolume();
    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
        SoundEmitter.Instance().PlaySFX(_button_sound);
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
        SoundEmitter.Instance().PlaySFX(_button_sound);
    }

    public void MoreGames()
    {
        Application.OpenURL("https://manupa-dogflau.itch.io/");
        SoundEmitter.Instance().PlaySFX(_button_sound);
    }

    public void HowToPlay()
    {
        Application.OpenURL("https://www.youtube.com/watch?v=mcivL8u176Y");
        SoundEmitter.Instance().PlaySFX(_button_sound);
    }

}
