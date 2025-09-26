// MainMenuSoundManager.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenuSoundManager : MonoBehaviour 
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Background Music")]
    [SerializeField] private AudioClip mainMenuBGM;
    [SerializeField] private float bgmVolume = 0.5f;
    [SerializeField] private bool fadeInBGM = true;
    [SerializeField] private float bgmFadeInDuration = 2f;

    [Header("UI Sound Effects")]
    [SerializeField] private AudioClip buttonHoverSound;
    [SerializeField] private AudioClip buttonClickSound;
    [SerializeField] private AudioClip menuTransitionSound;
    [SerializeField] private AudioClip errorSound;
    
    [Header("Volume Settings")]
    [SerializeField, Range(0f, 1f)] private float sfxVolume = 0.7f;
    [SerializeField, Range(0f, 1f)] private float masterVolume = 1f;

    [Header("Auto-Setup")]
    [SerializeField] private bool autoSetupButtons = true;
    [SerializeField] private MainMenuManager menuManager; // Reference to your menu manager

    // Singleton instance (optional, for easy access)
    public static MainMenuSoundManager Instance { get; private set; }

    void Awake() 
    {
        // Singleton setup
        if (Instance == null) 
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep across scenes if needed
        } 
        else 
        {
            Destroy(gameObject);
            return;
        }

        SetupAudioSources();
        
        if (autoSetupButtons && menuManager != null) 
        {
            SetupButtonSounds();
        }
    }

    void Start() 
    {
        PlayBackgroundMusic();
    }

    void SetupAudioSources() 
    {
        // Create BGM AudioSource if not assigned
        if (bgmSource == null) 
        {
            GameObject bgmGO = new GameObject("BGM AudioSource");
            bgmGO.transform.SetParent(transform);
            bgmSource = bgmGO.AddComponent<AudioSource>();
        }

        // Create SFX AudioSource if not assigned
        if (sfxSource == null) 
        {
            GameObject sfxGO = new GameObject("SFX AudioSource");
            sfxGO.transform.SetParent(transform);
            sfxSource = sfxGO.AddComponent<AudioSource>();
        }

        // Configure BGM source
        bgmSource.loop = true;
        bgmSource.playOnAwake = false;
        bgmSource.volume = fadeInBGM ? 0f : bgmVolume * masterVolume;

        // Configure SFX source
        sfxSource.loop = false;
        sfxSource.playOnAwake = false;
        sfxSource.volume = sfxVolume * masterVolume;
    }

    void SetupButtonSounds() 
    {
        // Get all buttons from the menu manager
        Button[] buttons = {
            menuManager.startButton,
            menuManager.highscoreButton,
            menuManager.settingsButton,
            menuManager.exitButton
        };

        // Add hover and click sounds to each button
        foreach (Button button in buttons) 
        {
            if (button != null) 
            {
                AddButtonSounds(button);
            }
        }
    }

    void AddButtonSounds(Button button) 
    {
        // Add EventTrigger component for hover effects
        UnityEngine.EventSystems.EventTrigger trigger = button.gameObject.GetComponent<UnityEngine.EventSystems.EventTrigger>();
        if (trigger == null) 
        {
            trigger = button.gameObject.AddComponent<UnityEngine.EventSystems.EventTrigger>();
        }

        // Create hover entry
        UnityEngine.EventSystems.EventTrigger.Entry hoverEntry = new UnityEngine.EventSystems.EventTrigger.Entry();
        hoverEntry.eventID = UnityEngine.EventSystems.EventTriggerType.PointerEnter;
        hoverEntry.callback.AddListener((eventData) => PlayButtonHover());
        trigger.triggers.Add(hoverEntry);

        // Add click sound to existing onClick events
        button.onClick.AddListener(PlayButtonClick);
    }

    #region Public Audio Methods

    public void PlayBackgroundMusic() 
    {
        if (bgmSource == null || mainMenuBGM == null) return;

        bgmSource.clip = mainMenuBGM;
        bgmSource.Play();

        if (fadeInBGM) 
        {
            StartCoroutine(FadeInBGM());
        }
    }

    public void PlayButtonHover() 
    {
        PlaySFX(buttonHoverSound);
    }

    public void PlayButtonClick() 
    {
        PlaySFX(buttonClickSound);
    }

    public void PlayMenuTransition() 
    {
        PlaySFX(menuTransitionSound);
    }

    public void PlayError() 
    {
        PlaySFX(errorSound);
    }

    public void PlaySFX(AudioClip clip) 
    {
        if (sfxSource == null || clip == null) return;
        
        sfxSource.volume = sfxVolume * masterVolume;
        sfxSource.PlayOneShot(clip);
    }

    public void PlaySFX(AudioClip clip, float volume) 
    {
        if (sfxSource == null || clip == null) return;
        
        sfxSource.volume = volume * masterVolume;
        sfxSource.PlayOneShot(clip);
    }

    #endregion

    #region Volume Controls

    public void SetMasterVolume(float volume) 
    {
        masterVolume = Mathf.Clamp01(volume);
        UpdateAllVolumes();
    }

    public void SetBGMVolume(float volume) 
    {
        bgmVolume = Mathf.Clamp01(volume);
        if (bgmSource != null) 
        {
            bgmSource.volume = bgmVolume * masterVolume;
        }
    }

    public void SetSFXVolume(float volume) 
    {
        sfxVolume = Mathf.Clamp01(volume);
        if (sfxSource != null) 
        {
            sfxSource.volume = sfxVolume * masterVolume;
        }
    }

    void UpdateAllVolumes() 
    {
        if (bgmSource != null) bgmSource.volume = bgmVolume * masterVolume;
        if (sfxSource != null) sfxSource.volume = sfxVolume * masterVolume;
    }

    #endregion

    #region BGM Controls

    public void StopBGM() 
    {
        if (bgmSource != null) 
        {
            bgmSource.Stop();
        }
    }

    public void PauseBGM() 
    {
        if (bgmSource != null) 
        {
            bgmSource.Pause();
        }
    }

    public void ResumeBGM() 
    {
        if (bgmSource != null) 
        {
            bgmSource.UnPause();
        }
    }

    public void FadeOutBGM(float duration = 1f) 
    {
        if (bgmSource != null) 
        {
            StartCoroutine(FadeOutBGMCoroutine(duration));
        }
    }

    #endregion

    #region Coroutines

    IEnumerator FadeInBGM() 
    {
        float currentTime = 0f;
        float targetVolume = bgmVolume * masterVolume;

        while (currentTime < bgmFadeInDuration) 
        {
            currentTime += Time.deltaTime;
            if (bgmSource != null) 
            {
                bgmSource.volume = Mathf.Lerp(0f, targetVolume, currentTime / bgmFadeInDuration);
            }
            yield return null;
        }

        if (bgmSource != null) 
        {
            bgmSource.volume = targetVolume;
        }
    }

    IEnumerator FadeOutBGMCoroutine(float duration) 
    {
        float startVolume = bgmSource.volume;
        float currentTime = 0f;

        while (currentTime < duration) 
        {
            currentTime += Time.deltaTime;
            if (bgmSource != null) 
            {
                bgmSource.volume = Mathf.Lerp(startVolume, 0f, currentTime / duration);
            }
            yield return null;
        }

        if (bgmSource != null) 
        {
            bgmSource.volume = 0f;
            bgmSource.Stop();
        }
    }

    #endregion

    #region Unity Editor Helper
#if UNITY_EDITOR
    [UnityEngine.ContextMenu("Test Button Hover Sound")]
    void TestHoverSound() 
    {
        PlayButtonHover();
    }

    [UnityEngine.ContextMenu("Test Button Click Sound")]
    void TestClickSound() 
    {
        PlayButtonClick();
    }
#endif
    #endregion
}