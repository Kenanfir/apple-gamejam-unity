// MainMenuManager.cs (Updated with Audio Integration)
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using PrimeTween;

public class MainMenuManager : MonoBehaviour {
    [Header("Buttons (drag & drop)")]
    [SerializeField] public Button startButton;      // Made public for sound manager access
    [SerializeField] public Button highscoreButton; // Made public for sound manager access
    [SerializeField] public Button settingsButton;  // Made public for sound manager access
    [SerializeField] public Button exitButton;      // Made public for sound manager access

    [Header("Scene Config")]
    [SerializeField] private string gameSceneName = "Game"; // Name of your gameplay scene

    [Header("Targets to Tween")]
    [SerializeField] private Transform cameraTransform;       // Usually the active Camera's transform
    [SerializeField] private RectTransform uiRoot;            // Parent container for the menu UI

    [SerializeField] private CanvasGroup MainMenuButtons; 
    [SerializeField] private CanvasGroup MainMenuCanvas; 

    [Header("Tween Settings")]
    [SerializeField] private Vector3 targetCameraEuler = new Vector3(0f, 25f, 0f);
    [SerializeField] private Vector2 targetUIAnchoredPos = new Vector2(0f, -400f);
    [SerializeField] private float tweenDuration = 0.6f;
    [SerializeField] private Ease ease = Ease.InOutQuad;

    [Header("Keyboard")]
    [SerializeField] private bool enterStartsGame = true; // Press Enter to start
    bool _enterEnabled = true; // disabled after first Enter

    [SerializeField] private GameObject pressEnterText;

    [Header("Audio Integration")]
    [SerializeField] private MainMenuSoundManager soundManager; // Reference to sound manager

    // Internals (captured at runtime)
    Vector3 _initialCamEuler;
    Vector2 _initialUIPos;
    bool _isTransitioning;

    void Awake() {
        if (cameraTransform != null) _initialCamEuler = cameraTransform.localEulerAngles;
        if (uiRoot != null) _initialUIPos = uiRoot.anchoredPosition;

        // Find sound manager if not assigned
        if (soundManager == null) {
            soundManager = FindObjectOfType<MainMenuSoundManager>();
        }

        // Hook up button handlers (you can also wire these via Inspector if you prefer)
        if (startButton) startButton.onClick.AddListener(OnStartClicked);
        if (highscoreButton) highscoreButton.onClick.AddListener(OnHighscoreClicked);
        if (settingsButton) settingsButton.onClick.AddListener(OnSettingsClicked);
        if (exitButton) exitButton.onClick.AddListener(OnExitClicked);
    }
    
    void Update() {
        if (!enterStartsGame || !_enterEnabled || _isTransitioning) return;

        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter)) {
            _enterEnabled = false;      // turn off further Enter inputs
            OnEnterPressed();         // same flow as Start button
        }
    }

    public void OnEnterPressed()
    {
        if (_isTransitioning) return;
        
        // Play button click sound for Enter key
        if (soundManager != null) soundManager.PlayButtonClick();
        
        PlayTransition(() =>
        {
            pressEnterText.SetActive(false);
            // Fade from 0 to 1
            Tween.Alpha(MainMenuButtons, 1f, 0.8f, ease);
        });
    }

    // --- Public/Inspector-callable handlers (safe to wire via Inspector if you like) ---
    public void OnStartClicked()
    {
        if (_isTransitioning) return;
        
        // Play transition sound
        if (soundManager != null) soundManager.PlayMenuTransition();
        
        PlayTransition(() => {
            // Fade out BGM before scene change
            if (soundManager != null) soundManager.FadeOutBGM(0.5f);
            Tween.Alpha(MainMenuCanvas, 0f, 0.8f, ease);
            
            // Load game scene after a short delay to let audio fade
            StartCoroutine(LoadSceneAfterDelay(gameSceneName, 1f));
        });
    }

    public void OnHighscoreClicked() {
        if (_isTransitioning) return;
        
        // Play transition sound
        if (soundManager != null) soundManager.PlayMenuTransition();
        
        PlayTransition(() => {
            // TODO: Open Highscore scene here
            // Example:
            // SceneManager.LoadScene("Highscore");
            Debug.Log("Highscore menu would open here");
        });
    }

    public void OnSettingsClicked() {
        if (_isTransitioning) return;
        
        // Play transition sound
        if (soundManager != null) soundManager.PlayMenuTransition();
        
        PlayTransition(() => {
            // TODO: Open/Toggle Settings UI here
            // Example:
            // settingsPanel.SetActive(true);
            Debug.Log("Settings menu would open here");
        });
    }

    public void OnExitClicked() {
        if (_isTransitioning) return;
        
        // Play transition sound
        if (soundManager != null) soundManager.PlayMenuTransition();
        
        PlayTransition(() => {
            // Fade out BGM before exit
            if (soundManager != null) soundManager.FadeOutBGM(0.3f);
            
            // Exit application after audio fade
            StartCoroutine(ExitAfterDelay(0.3f));
        });
    }

    // --- Core tween logic using PrimeTween ---
    void PlayTransition(Action onComplete) {
        if (cameraTransform == null || uiRoot == null) {
            onComplete?.Invoke();
            return;
        }

        _isTransitioning = true;
        SetButtonsInteractable(false);

        int completedTweens = 0;
        int totalTweens = 2;

        // Tween camera rotation
        Tween.LocalRotation(cameraTransform, Quaternion.Euler(targetCameraEuler), tweenDuration, ease)
            .OnComplete(() => {
                completedTweens++;
                if (completedTweens >= totalTweens) FinishTransition(onComplete);
            });

        // Tween UI slide
        Tween.UIAnchoredPosition(uiRoot, targetUIAnchoredPos, tweenDuration, ease)
            .OnComplete(() => {
                completedTweens++;
                if (completedTweens >= totalTweens) FinishTransition(onComplete);
            });
    }

    void FinishTransition(Action onComplete) {
        onComplete?.Invoke();

        _isTransitioning = false;
        SetButtonsInteractable(true);
    }

    void SetButtonsInteractable(bool value) {
        if (startButton) startButton.interactable = value;
        if (highscoreButton) highscoreButton.interactable = value;
        if (settingsButton) settingsButton.interactable = value;
        if (exitButton) exitButton.interactable = value;
    }


    System.Collections.IEnumerator LoadSceneAfterDelay(string sceneName, float delay) {
        yield return new WaitForSeconds(delay);
        if (!string.IsNullOrEmpty(sceneName)) {
            SceneManager.LoadScene(sceneName);
        }
    }

    System.Collections.IEnumerator ExitAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}