using UnityEngine;

public class MenuPanel : MonoBehaviour
{
    // a component attatched to every menu object.
    // later custom functionality like animations can be added by extending this class
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
