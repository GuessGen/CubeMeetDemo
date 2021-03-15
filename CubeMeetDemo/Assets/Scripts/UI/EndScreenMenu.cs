using UnityEngine;

public class EndScreenMenu : MonoBehaviour, IOverlaybleMenu
{
    public void Show()
    {
        GameManager.Instance.gameplayInputAllowed = false;
        
        gameObject.SetActive(true);
        starFxController.myStarFxController.ea = GameManager.Instance.starsCollectedInThisLevel;
    }

    public void Hide()
    {
        GameManager.Instance.gameplayInputAllowed = true;
        gameObject.SetActive(false);
    }
}
