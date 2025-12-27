using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    // a component attatched to every menu object.
    // later custom functionality like animations can be added by extending this class
    [Header("Write the full name of the panel, in camelCase, without the word 'Panel'")]
    [Header("Ex. GameOverPanel -> gameOver")]
    public string menuName;

    public void EnablePanel()
    {
        Debug.Log(menuName + " enabled");
        gameObject.SetActive(true);
    }

    public void DisablePanel()
    {
        Debug.Log(menuName + " disabled");
        gameObject.SetActive(false);
    }
}
