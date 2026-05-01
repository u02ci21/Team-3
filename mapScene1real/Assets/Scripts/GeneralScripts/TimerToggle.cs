using UnityEngine;
using UnityEngine.UI;

public class TimerToggle : MonoBehaviour
{
    public Toggle toggle;

    void Start()
    {
        bool isOn = PlayerPrefs.GetInt("TimerEnabled", 1) == 1;

        if (toggle != null)
            toggle.SetIsOnWithoutNotify(isOn);
    }

    public void OnToggleChanged(bool isOn)
    {
        PlayerPrefs.SetInt("TimerEnabled", isOn ? 1 : 0);
        PlayerPrefs.Save();
    }
}