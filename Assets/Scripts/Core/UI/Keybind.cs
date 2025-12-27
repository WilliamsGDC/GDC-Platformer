using UnityEngine;
using TMPro;
using System;

public class Keybind : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI label;
    [SerializeField] string keybindName;

    //This code has not been finished at all yet. It will be fixed in the future
    /*
    private void Start()
    {
        if (PlayerPrefs.HasKey(keybindName))
        {
            label.text = PlayerPrefs.GetString(keybindName);
        } else
        {
            PlayerPrefs.SetString(keybindName, label.text);
        }
    }

    private void Update()
    {
        if (label.text == "Awaiting Input")
        {
            foreach (KeyCode keycode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKey(keycode))
                {
                    label.text = keycode.ToString();
                    PlayerPrefs.SetString(keybindName, keycode.ToString());
                    PlayerPrefs.Save();
                }
            }
        }
    }

    public void ChangeKey()
    {
        label.text = "Awaiting Input";
    }
    */
}
