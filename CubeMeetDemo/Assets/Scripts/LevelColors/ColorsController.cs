using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorsController : MonoBehaviour
{
    [Header("Background Image")]
    [SerializeField] private RawImage _backgroundImage;
    

    [Header("Cube materials")]
    [SerializeField] private Material _backBlue;
    [SerializeField] private Material _downYellow;
    [SerializeField] private Material _frontGreen;
    [SerializeField] private Material _leftOrange;
    [SerializeField] private Material _rightRed;
    [SerializeField] private Material _upWhite;

    [Header("Color Schemes")]
    [SerializeField] private List<ColorScheme> _colorSchemes;

    private void Start() 
    {
        SetSchemeColor();
    }

    private void OnValidate() 
    {
        SetSchemeColor();
    }

    private void SetSchemeColor()
    {
        if (_colorSchemes == null)
            return;

        ColorScheme currentColorScheme = _colorSchemes[Random.Range(0, _colorSchemes.Count)];

        _backgroundImage.color = currentColorScheme.backgroundImageColor;

        _backBlue.color = currentColorScheme.backBlueColor;
        _downYellow.color = currentColorScheme.downYellowColor;
        _frontGreen.color = currentColorScheme.frontGreenColor;
        _leftOrange.color = currentColorScheme.leftOrangeColor;
        _rightRed.color = currentColorScheme.rightRedColor;
        _upWhite.color = currentColorScheme.upWhiteColor;
    }
}
