using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Slider volumeSlider;
    [SerializeField] TextMeshProUGUI volumeText;
    [SerializeField] string VOLUME_PREFS_KEY = "MASTER_VOLUME";

    void Start()
    {
        LoadVolumePrefs();
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void QuitGame()
    {
        // Preprocessor check, different behavior based on launch env
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void VolumeSliderChange()
    {
        volumeText.text = $"{volumeSlider.value}";

        SetGameVolume((int)volumeSlider.value);
        SaveVolumePrefs((int)volumeSlider.value);
    }

    void SaveVolumePrefs(int volume)
    {
        PlayerPrefs.SetInt(VOLUME_PREFS_KEY, volume);
    }

    void SetGameVolume(int volume)
    {
        AudioListener.volume = volume / 100.0f;
    }

    void LoadVolumePrefs()
    {
        int savedVolumePref = PlayerPrefs.GetInt(VOLUME_PREFS_KEY, 100);
        // SetGameVolume(savedVolumePref);

        volumeSlider.value = savedVolumePref;  // This will call VolumeSliderChange
        volumeText.text = $"{savedVolumePref}";
    }

}
