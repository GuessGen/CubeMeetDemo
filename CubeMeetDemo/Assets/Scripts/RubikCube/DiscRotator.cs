using UnityEngine;

 [RequireComponent(typeof(RubikCube))]
 [RequireComponent(typeof(BoxCollider))]
public class DiscRotator : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float _discRotationDragThreshold = 0.5f;
    [SerializeField] private float _discRotationSpeed = 1;

    private RubikCube _rubikCube = null;
    private BoxCollider _cubeBoxCollider = null;

    private Plane _collisionPlaneIncreaser = new Plane();

    private Vector3 _startDraggingPointInLocalSpace;
    private Vector3 _startDraggingNormalInLocalSpace;
    private Vector3 _currentDraggedCubeTangent;
    private Vector3 _currentDraggedCubeBitangent;

    
    private Camera _camera = null;
    private bool _isDragging;

    private void Awake()
    {
        _rubikCube = GetComponent<RubikCube>();
        _cubeBoxCollider = GetComponent<BoxCollider>();
        _camera = Camera.main;

        _isDragging = false;
    }

    private void Update()
    {
        if (!GameManager.Instance.gameplayInputAllowed) //added
            return;

        Vector3 raycastPosition = Input.mousePosition;
        bool wantToStartDragRotate = Input.GetMouseButtonDown(0);

        if (!_isDragging && !_rubikCube.IsDiscRotating && wantToStartDragRotate)
        {
            CheckDiscDragStartAndChangeState(raycastPosition);
            return;
        }

        if (_isDragging)
        {
            DoDiscDragInProgress(raycastPosition);
        }
    }

    private void CheckDiscDragStartAndChangeState(Vector3 raycastTarget)
    {
        RaycastHit hit;
        Ray ray = _camera.ScreenPointToRay(raycastTarget);

        if (_cubeBoxCollider.Raycast(ray, out hit, Vector3.Distance(_camera.transform.position, transform.position) * 2))
        {
            CalculateDraggedCubeInformation(hit);

            _isDragging = true;
        }
    }

    private void CalculateDraggedCubeInformation(RaycastHit hit)
    {
        CalculateDraggedNormalAndPointInLocalSpace(hit);
        CalculateDraggedPlaneIncreaser(hit);
        CalculateDraggedCubeTangents(hit);
    }

    private void CalculateDraggedNormalAndPointInLocalSpace(RaycastHit hit)
    {
        _startDraggingNormalInLocalSpace = transform.InverseTransformDirection(hit.normal);
        _startDraggingPointInLocalSpace = transform.InverseTransformPoint(hit.point);
    }

    private void CalculateDraggedPlaneIncreaser(RaycastHit hit)
    {
        _collisionPlaneIncreaser.normal = hit.normal;
        _collisionPlaneIncreaser.distance = - (_rubikCube.CubeSideDimension * transform.lossyScale.x)/2.0f - Vector3.Dot(hit.normal, transform.position);
    }

    private void CalculateDraggedCubeTangents(RaycastHit hit)
    {
        Vector3 absoluteNormal = _startDraggingNormalInLocalSpace.GetAbsRounded();

        if (absoluteNormal.x > 0.5)
        {
            _currentDraggedCubeTangent = Vector3.up;
            _currentDraggedCubeBitangent = Vector3.forward;
        }
        else if (absoluteNormal.y > 0.5)
        {
            _currentDraggedCubeTangent = Vector3.forward;
            _currentDraggedCubeBitangent = Vector3.right;
        }
        else if (absoluteNormal.z > 0.5)
        {
            _currentDraggedCubeTangent = Vector3.right;
            _currentDraggedCubeBitangent = Vector3.up;
        }
    }

    private void DoDiscDragInProgress(Vector3 pRaycastTarget)
    {
        float hit;
        Ray ray = _camera.ScreenPointToRay(pRaycastTarget);

        if (_collisionPlaneIncreaser.Raycast(ray, out hit))
        {
            Vector3 currentHitPointInMySpace = transform.InverseTransformPoint(ray.GetPoint(hit));
            Vector3 differenceVector = currentHitPointInMySpace - _startDraggingPointInLocalSpace;

            float distanceAlongTangent = Vector3.Dot(differenceVector, _currentDraggedCubeTangent);
            float distanceAlongBitangent = Vector3.Dot(differenceVector, _currentDraggedCubeBitangent);

            float absDistanceAlongTangent = Mathf.Abs(distanceAlongTangent);
            float absDistanceAlongBitangent = Mathf.Abs(distanceAlongBitangent);

            if (Mathf.Max(absDistanceAlongTangent, absDistanceAlongBitangent) < _discRotationDragThreshold) 
                return;

            Vector3 localRotationAxis = absDistanceAlongTangent < absDistanceAlongBitangent ? _currentDraggedCubeTangent : _currentDraggedCubeBitangent;

            float discIndex = Vector3.Dot(localRotationAxis, _startDraggingPointInLocalSpace);
            discIndex = Mathf.Clamp(discIndex + _rubikCube.CubeSideDimension / 2.0f, 0, _rubikCube.CubeSideDimension - 1);

            Vector3 directionIndicator = Vector3.Cross(_startDraggingNormalInLocalSpace, localRotationAxis);
            
            bool isPositiverotation = Vector3.Dot(differenceVector, directionIndicator) < 0;


            _rubikCube.RotateDisc(localRotationAxis, (int)discIndex, isPositiverotation, _discRotationSpeed);
        }
        
        _isDragging = false;
    }
}
