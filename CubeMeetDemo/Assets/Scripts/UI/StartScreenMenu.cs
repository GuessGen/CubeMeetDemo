using UnityEngine;

public class StartScreenMenu : MonoBehaviour, IOverlaybleMenu
{
    public void Show()
    {
        GameManager.Instance.gameplayInputAllowed = false;
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        GameManager.Instance.gameplayInputAllowed = true;
        gameObject.SetActive(false);
    }
}
