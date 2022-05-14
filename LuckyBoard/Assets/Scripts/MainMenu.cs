using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{   
    [SerializeField]
    private Slider volumeSlider;
    public void PlayGame()
    {
        SaveSliderValue();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void SaveSliderValue()
    {
        PlayerPrefs.SetFloat("SliderVolumeLevel", volumeSlider.value);
        PlayerPrefs.Save();
    }
}
