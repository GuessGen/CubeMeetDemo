using UnityEngine;

public class UIManager : SingletonGeneric<UIManager>
{
    [SerializeField] private StartScreenMenu _startScreen;
    [SerializeField] private EndScreenMenu _endScreen;
    [SerializeField] private GameObject _levelNumberText;

    public void ShowStartScreen()
    {
        _startScreen.Show();
    }

    public void HideStartScreen()
    {
        _startScreen.Hide();
    }

    public void ShowEndScreen()
    {
        _levelNumberText.SetActive(false);
        _endScreen.Show();
    }
}
