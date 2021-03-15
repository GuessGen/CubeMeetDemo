using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : SingletonGeneric<GameManager>
{
    [SerializeField] public Character cornerCharacter;
    [SerializeField] public Character edgeCharacter;

    [HideInInspector] public bool gameplayInputAllowed = true;
    [HideInInspector] public int starsCollectedInThisLevel = 0;
    [HideInInspector] public readonly int numberOfLevelsWithTutorial = 6;

    public override void Awake()
    {
        base.Awake();
        
        if (!PlayerPrefs.HasKey("CurrentLevel"))
            PlayerPrefs.SetInt("CurrentLevel", 1);
    }

    private void Start() {
        UIManager.Instance.ShowStartScreen();
    }
    public int GetCurrentLevel()
    {
        return PlayerPrefs.GetInt("CurrentLevel");
    }

    public void OnCubeChangedCeckCharactersDistance()
    {
        if (cornerCharacter.DistanceTo(edgeCharacter) < 1.3f)
        {
            UIManager.Instance.ShowEndScreen();

            RotateCharactersTowardsEachOther();
            cornerCharacter.PlayWinAnimation();
            edgeCharacter.PlayWinAnimation();

            IncrementPlayerLevel();

            Invoke(nameof(NextLevel), 3f);

            return;
        }

        PlayRandomCharacterCasualAnimation();
    }

    private void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void RotateCharactersTowardsEachOther()
    {
        cornerCharacter.RotateTowards(edgeCharacter);
        edgeCharacter.RotateTowards(cornerCharacter);
    }

    private void PlayRandomCharacterCasualAnimation()
    {
        int playOrNot = Random.Range(0, 4);

        if (playOrNot == 0)
        {
            int playCornerCharacter = Random.Range(0, 2);

            Character characterToPlay = edgeCharacter;

            if (playCornerCharacter == 1)
                characterToPlay = cornerCharacter;

            characterToPlay.PlayRandomCasualAnimation();
        }
    }

    private void IncrementPlayerLevel()
    {
        PlayerPrefs.SetInt("CurrentLevel", GetCurrentLevel() + 1);
    }
}