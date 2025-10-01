using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public MenuPanel[] menuPanels;
    public MenuPanel curPanel;

    private void Start()
    {
        curPanel = menuPanels[0];
        curPanel.EnablePanel();
    }

    public void ChangePanel(string name)
    {
        curPanel.DisablePanel();

        foreach (MenuPanel panel in menuPanels)
        {
            if (panel.menuName == name)
            {
                curPanel = panel;
                curPanel.EnablePanel();
                break;
            }
        }
    }

    public void ChangeScene(Scene _scene)
    {
        SceneManager.LoadScene(nameof(_scene));
    }
}
