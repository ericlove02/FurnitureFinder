using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class SetVibe : MonoBehaviour
{
    public TMP_Dropdown vibeDropdown;

    public void Start()
    {
        if (SceneManager.GetActiveScene().name == "LandingScene")
        {
            PlayerPrefs.DeleteAll();
        }

        vibeDropdown.onValueChanged.AddListener(OnDropdownValueChanged);

        int selectedVibeIndex = PlayerPrefs.GetInt("SelectedVibeIndex", 0);
        vibeDropdown.value = selectedVibeIndex;
    }

    void OnDropdownValueChanged(int index)
    {
        // account for "select you vibe" placeholder
        if (SceneManager.GetActiveScene().name == "LandingScene")
        {
            PlayerPrefs.SetInt("SelectedVibeIndex", index - 1);
        }
        else
        {
            PlayerPrefs.SetInt("SelectedVibeIndex", index);
        }
        PlayerPrefs.SetString("Vibe", vibeDropdown.options[index].text);
        PlayerPrefs.Save();
    }
}
