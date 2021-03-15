using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;

[RequireComponent(typeof(BoxCollider))]
public class RubikCube : MonoBehaviour
{
    public bool IsInitialized { private set; get; }
    public int CubeSideDimension { private set; get; }
    public bool IsDiscRotating { private set; get; }

    public event System.Action OnChanged;
    
    private int _maximumCubletIndex = -1;

    private Transform[,,] _cublets = null;
    private Transform[,] _cubletBackingStore = null;

    private Transform _pivotToRotateArround = null;

    public void Initialize(int dimensions, GameObject cubletPrefab)
    {
        if (IsInitialized)
        {
            throw new System.Exception("Initialized cube cannot be initialized again");
        }

        Assert.IsFalse(dimensions < 2, "The dimensions of the cube cannot be smaller than 2");
        Assert.IsNotNull(cubletPrefab, "The cublet prefab cannot be null");

        IsInitialized = true;
        
        _pivotToRotateArround = new GameObject("Pivot").transform;
        _pivotToRotateArround.SetParent(transform, false);

        GenerateCublets(dimensions, cubletPrefab);

        _cubletBackingStore = new Transform[CubeSideDimension, CubeSideDimension];

        GetComponent<BoxCollider>().size = Vector3.one * CubeSideDimension;
    }

    public Transform GetCubletFace(int x, int y, int z, int faceIndexInParent)
    {
        return _cublets[x, y, z].GetChild(faceIndexInParent);
    }

    private void GenerateCublets(int dimensions, GameObject cubletPrefab)
    {
        CubeSideDimension = dimensions;
        _maximumCubletIndex = CubeSideDimension - 1;

        _cublets = new Transform[CubeSideDimension, CubeSideDimension, CubeSideDimension];
        Vector3 bottomLeftBack = Vector3.one * -_maximumCubletIndex / 2.0f;
        
        for (int x = 0; x < CubeSideDimension; x++)
        {
            for (int y = 0; y < CubeSideDimension; y++)
            {
                for (int z = 0; z < CubeSideDimension; z++)
                {
                    if (x == 0 || y == 0 || z == 0 || x == _maximumCubletIndex || y == _maximumCubletIndex || z == _maximumCubletIndex)
                    {
                        GameObject cubletGO = Instantiate(cubletPrefab, bottomLeftBack + new Vector3(x, y, z), Quaternion.identity);
                        Transform cubletTransform = cubletGO.transform;
                        _cublets[x, y, z] = cubletTransform;

                        if (cubletTransform.childCount == 6)
                        {
                            cubletTransform.GetChild(0).gameObject.SetActive(x == 0);
                            cubletTransform.GetChild(1).gameObject.SetActive(x == _maximumCubletIndex);
                            cubletTransform.GetChild(2).gameObject.SetActive(y == 0);
                            cubletTransform.GetChild(3).gameObject.SetActive(y == _maximumCubletIndex);
                            cubletTransform.GetChild(4).gameObject.SetActive(z == 0);
                            cubletTransform.GetChild(5).gameObject.SetActive(z == _maximumCubletIndex);
                        }

                        cubletTransform.SetParent(transform, false);
                    }
                }
            }
        }
    }

    
    public void RotateDisc(Vector3 axis, int layer, bool positiveRotation, float rotateSpeed)
    {
        if (IsDiscRotating) 
            return;

        StartCoroutine(RotateDiscCoroutine(axis, layer, positiveRotation, rotateSpeed));
    }

    public IEnumerator RotateDiscCoroutine(Vector3 axis, int layer, bool positiveRotation, float rotateSpeed)
    {
        if (IsDiscRotating) yield break;
        IsDiscRotating = true;

        Assert.IsTrue(Vector3.Dot(axis, Vector3.up) == 1 || Vector3.Dot(axis, Vector3.forward) == 1 || Vector3.Dot(axis, Vector3.right) == 1, "Axis assertion failed");
        Assert.IsTrue(layer >= 0 && layer <= _maximumCubletIndex, "Invalid layer passed");
        
        Vector3 uAxis = Vector3.zero;
        Vector3 vAxis = Vector3.zero;


        if (axis.x == 1)				
        {
            uAxis = Vector3.up;			
            vAxis = Vector3.forward;	
        } 
        else if (axis.y == 1)			
        {
            uAxis = Vector3.forward;	
            vAxis = Vector3.right;		
        } 
        else if (axis.z == 1)			
        {
            uAxis = Vector3.right;		
            vAxis = Vector3.up;			
        }

        float centerIndex = _maximumCubletIndex / 2.0f;
        Vector2 center = new Vector2(centerIndex, centerIndex);

        for (int u = 0; u < CubeSideDimension; u++)
        {
            for (int v = 0; v < CubeSideDimension; v++)
            {
                if (u == 0 || v == 0 || u == _maximumCubletIndex || v == _maximumCubletIndex || layer == 0 || layer == _maximumCubletIndex)
                {
                    Vector3 cubletIndex = axis * layer + uAxis * u + vAxis * v;
                    Transform cublet = _cublets[(int)cubletIndex.x, (int)cubletIndex.y, (int)cubletIndex.z];
                    cublet.parent = _pivotToRotateArround;

                    Vector2 originalUV = new Vector2(u, v);
                    Vector2 translatedUV = originalUV - center;

                    Vector2 rotatedUV = new Vector2(-translatedUV.y, translatedUV.x);

                    if (!positiveRotation) 
                        rotatedUV *= -1;
                    
                    rotatedUV += center;
              
                    _cubletBackingStore[(int)rotatedUV.x, (int)rotatedUV.y] = cublet;
                }
            }
        }

        for (int u = 0; u < CubeSideDimension; u++)
        {
            for (int v = 0; v < CubeSideDimension; v++)
            {
                Vector3 cubletIndex = axis * layer + uAxis * u + vAxis * v;
                if (u == 0 || v == 0 || u == _maximumCubletIndex || v == _maximumCubletIndex || layer == 0 || layer == _maximumCubletIndex)
                {
                    _cublets[(int)cubletIndex.x, (int)cubletIndex.y, (int)cubletIndex.z] = _cubletBackingStore[u,v];
                }
            }
        }

        yield return StartCoroutine(RotatePivotCoroutineVisually(axis, positiveRotation, rotateSpeed));
    }


    private IEnumerator RotatePivotCoroutineVisually(Vector3 axis, bool positiveRotation, float rotateSpeed)
    {
        Quaternion currentRotation = _pivotToRotateArround.localRotation;

        float angle = positiveRotation ? 90 : -90;
     
        Quaternion targetRotation = Quaternion.Euler(axis * angle);

        float rotated = 0;
        while (rotated < 1)
        {
            rotated += Time.deltaTime * rotateSpeed;
            _pivotToRotateArround.localRotation = Quaternion.Lerp (currentRotation, targetRotation, rotated);
            yield return null;
        }

        _pivotToRotateArround.localRotation = currentRotation;
        _pivotToRotateArround.Rotate(axis, angle, Space.Self);

        int pivotChildCount = _pivotToRotateArround.childCount;
        Transform child = null;
        while (pivotChildCount > 0)
        {
            child = _pivotToRotateArround.GetChild(--pivotChildCount);
            child.parent = transform;
            
            child.localScale = Vector3.one;
        }

        _pivotToRotateArround.localRotation = Quaternion.identity;
        IsDiscRotating = false;

        OnChanged?.Invoke();
    }
}