using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class LevelNumberText : MonoBehaviour
{
    private void Start()
    {
        TextMeshProUGUI textMesh = GetComponent<TextMeshProUGUI>();

        textMesh.text = "Level " + GameManager.Instance.GetCurrentLevel();       
    }
}
