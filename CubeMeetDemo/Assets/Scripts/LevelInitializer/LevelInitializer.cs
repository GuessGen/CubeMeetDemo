using System;
using UnityEngine;
using UnityEngine.Events;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class LevelInitializer : MonoBehaviour
{
    [Header ("Prefabs")]
    [SerializeField] private GameObject _characterCornerPrefab;
    [SerializeField] private GameObject _characterEdgePrefab;
    [SerializeField] private GameObject _collectableStarPrefab;
    [SerializeField] private GameObject _bubbleSpeechPrefab;
    [SerializeField] private RubikCube _rubikCubePrefab;
    [SerializeField] private GameObject _cubletPrefab;

    [Header ("General")]
    [Range(0, 3)]
    [SerializeField] private int _starsToAddCount = 3;

    [Serializable]
    public class RubikCubeEvent : UnityEvent<RubikCube> { }

    [Header("Cube events")]
    public RubikCubeEvent OnCubeChanged;

    private RubikCube _rubikCube = null;

    private int _dimensions = 3;

    private PositionOnCube _characterCornerPosition = new PositionOnCube();
    private PositionOnCube _characterEdgePosition = new PositionOnCube();

    //Lists Of Possible CubletPositions
    private List<Vector3Int> _possiblePositionsForCornerElements;
    private List<Vector3Int> _possiblePositionsForEdgeElements;
    
    //End of Lists of Possible CubletPositions

    private void Start()
    {
        ChooseCubeDimension();
        InitializePosibleCubletPositionsLists();
        
        SpawnNewCube(_dimensions);
        AddGameplayElements();
    }

    private void ChooseCubeDimension()
    {
        int currentLevel = GameManager.Instance.GetCurrentLevel();

        if (currentLevel < 5)
            _dimensions = 2;
        else if (currentLevel < 9)
            _dimensions = 3;
        else
            _dimensions = Random.Range(2, 5);

        ChangeCameraFieldOfView();
    }

    private void ChangeCameraFieldOfView()
    {
        if (_dimensions == 2)
            Camera.main.fieldOfView = 24;
        else if (_dimensions == 3)
            Camera.main.fieldOfView = 28;
        else if (_dimensions == 4)
            Camera.main.fieldOfView = 35;
    }

	private void SpawnNewCube(int dimensions)
	{
        _rubikCube = Instantiate<RubikCube>(_rubikCubePrefab, transform);
        
        _rubikCube.Initialize(dimensions, _cubletPrefab);
        SetupCubeEvents();
    }

    private void SetupCubeEvents()
    {
        _rubikCube.OnChanged += OnCubeChangedCallback;
    }

    private void OnCubeChangedCallback()
    {
        OnCubeChanged?.Invoke(_rubikCube);
    }


//DecorateCube
    private void AddGameplayElements()
    {
        AddCharactersToCube();
        AddCollectableStars();
    }

    private void AddCharactersToCube()
    {
        ChoosePositionForCornerCharacter();
        GameManager.Instance.cornerCharacter = AddElementToCublet(_characterCornerPrefab, _characterCornerPosition, true).GetComponent<Character>();
        AddBubbleSpeechToCharacter(GameManager.Instance.cornerCharacter);
        
        ChoosePositionForEdgeCharacter();
        GameManager.Instance.edgeCharacter = AddElementToCublet(_characterEdgePrefab, _characterEdgePosition, true).GetComponent<Character>();
    }

    private void AddBubbleSpeechToCharacter(Character character)
    {
        if (GameManager.Instance.GetCurrentLevel() > GameManager.Instance.numberOfLevelsWithTutorial)
            return;

        GameObject bubble = Instantiate(_bubbleSpeechPrefab, character.transform.position, character.transform.rotation, character.transform);
        bubble.transform.localPosition = new Vector3(0, 1.5f, 0);
    }

    private void AddCollectableStars()
    {
        for (int i = 0; i < _starsToAddCount; i++)
            AddElementToCublet(_collectableStarPrefab, ChoosePositionForCollectableElement());
    }

    private void ChoosePositionForCornerCharacter()
    {
        _characterCornerPosition = ChooseCubletPositionFrom(ref _possiblePositionsForCornerElements);
    }

    private void ChooseFaceIndexForElementPosition(ref PositionOnCube elementPosition)
    {
        List<int> visibleFacesIndexes = new List<int>();
        for (int i = 0; i < 6; i++)
        {
            Vector3Int cubletIndex = elementPosition.cubletPositionIndexes;
            Transform cubletFace = _rubikCube.GetCubletFace(cubletIndex.x, cubletIndex.y, cubletIndex.z, i);

            if (cubletFace.gameObject.activeSelf && cubletFace.childCount == 0 && FaceIsDirectlyVisible(i))
                visibleFacesIndexes.Add(i);
        }

        elementPosition.cubletFaceIndex = visibleFacesIndexes[Random.Range(0, visibleFacesIndexes.Count)];
    }

    private bool FaceIsDirectlyVisible(int faceIndex)
    {
        if (faceIndex == 1 || faceIndex == 3 || faceIndex == 4)
            return true;

        return false;
    }

    private PositionOnCube ChoosePositionForCollectableElement()
    {
        int cornerOrEdge = Random.Range(0, 2);
        PositionOnCube resultPosition = new PositionOnCube();

        if (cornerOrEdge == 0)
            resultPosition = ChooseCubletPositionFrom(ref _possiblePositionsForCornerElements);
        else
            resultPosition = ChooseCubletPositionFrom(ref _possiblePositionsForEdgeElements);

        return resultPosition;
    }

    private PositionOnCube ChooseCubletPositionFrom(ref List<Vector3Int> possibleCubletPositions)
    {
        PositionOnCube cubletPosition = new PositionOnCube();
        int randomIndex = Random.Range(0, possibleCubletPositions.Count);
        cubletPosition.cubletPositionIndexes = possibleCubletPositions[randomIndex];
        possibleCubletPositions.RemoveAt(randomIndex);

        ChooseFaceIndexForElementPosition(ref cubletPosition);

        return cubletPosition;
    }

    private void ChoosePositionForEdgeCharacter()
    {
        int randomIndex = 0;

        do {
            randomIndex = Random.Range(0, _possiblePositionsForEdgeElements.Count);
            _characterEdgePosition.cubletPositionIndexes = _possiblePositionsForEdgeElements[randomIndex];

            ChooseFaceIndexForElementPosition(ref _characterEdgePosition);
        } while (CornerAndEdgeCharactersAreTooClose(_characterEdgePosition));

        _possiblePositionsForEdgeElements.RemoveAt(randomIndex);
    }

    private bool CornerAndEdgeCharactersAreTooClose(PositionOnCube potentialEdgePosition)
    {
        return _characterEdgePosition.cubletPositionIndexes == _characterCornerPosition.cubletPositionIndexes || 
                (Vector3.Distance(potentialEdgePosition.cubletPositionIndexes, _characterCornerPosition.cubletPositionIndexes) <= 1f && potentialEdgePosition.cubletFaceIndex == _characterCornerPosition.cubletFaceIndex);
    }

    private GameObject AddElementToCublet(GameObject elementPrefab, PositionOnCube elementPosition, bool setParent = false)
    {
        Vector3Int cubletIndex = elementPosition.cubletPositionIndexes;
        Transform faceToPlaceTransform = _rubikCube.GetCubletFace(cubletIndex.x, cubletIndex.y, cubletIndex.z, elementPosition.cubletFaceIndex);

        GameObject element = Instantiate(elementPrefab, faceToPlaceTransform.position, faceToPlaceTransform.rotation);
        
        if (setParent)
            element.transform.SetParent(faceToPlaceTransform);

        element.transform.localPosition = UpdateElementPosition(element.transform.localPosition, elementPosition.cubletFaceIndex);
        element.transform.localRotation = UpdateElementRotation(elementPosition.cubletFaceIndex);
        
        return element;
    }

    private Vector3 UpdateElementPosition(Vector3 oldPosition, int faceIndex)
    {
        Vector3 addedPosition = InitializerFixedKnowledge.GetNormalForCubletFace(faceIndex);

        return new Vector3(oldPosition.x + addedPosition.x, oldPosition.y + addedPosition.y, oldPosition.z + addedPosition.z);
    }

    private Quaternion UpdateElementRotation(int faceIndex)
    {
        Quaternion updatedRotation = Quaternion.identity;
        updatedRotation.eulerAngles = InitializerFixedKnowledge.GetRotationForCubletFace(faceIndex);
        return updatedRotation;
    }

    private void InitializePosibleCubletPositionsLists()
    {
        _possiblePositionsForCornerElements = InitializerFixedKnowledge.GetPosiblePositionsForCornerElements(_dimensions);

        if (_dimensions == 2)
            _possiblePositionsForEdgeElements = _possiblePositionsForCornerElements;
        else if (_dimensions == 3)
            _possiblePositionsForEdgeElements = InitializerFixedKnowledge.GetPosiblePositionsForEdgeElements3x3();
        else if (_dimensions == 4)
            _possiblePositionsForEdgeElements = InitializerFixedKnowledge.GetPosiblePositionsForEdgeElements4x4();
    }
}

public class PositionOnCube
{
    public Vector3Int cubletPositionIndexes;
    public int cubletFaceIndex;

    public PositionOnCube()
    {
        cubletPositionIndexes = new Vector3Int(0, 0, 0);
        cubletFaceIndex = 0;
    }
}
