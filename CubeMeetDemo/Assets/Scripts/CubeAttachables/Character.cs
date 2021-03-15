using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Character : MonoBehaviour
{
    [SerializeField] private GameObject _visualModel;

    private Animator _animator;
    private string[] _casualAnimationParameters = { "Lose", "Lose2", "Wave", "No", "Shrug", "Yes" };

    private void Start() 
    {
        _animator = GetComponent<Animator>();
    }

    public void RotateTowards(Character rotateTo)
    {
        StartCoroutine(RotateTo(rotateTo._visualModel.transform, 0.2f));
    }

    public float DistanceTo(Character target)
    {
        return Vector3.Distance(transform.position, target.transform.position);
    }

    public void PlayWinAnimation()
    {
        _animator.SetBool("Win", true);
    }

    public void PlayRandomCasualAnimation()
    {
        int randomIndex = Random.Range(0, _casualAnimationParameters.Length);

        _animator.SetBool(_casualAnimationParameters[randomIndex], true);
    }

    IEnumerator RotateTo(Transform rotateTo, float inTime) 
    {
        Vector3 fromAngle = _visualModel.transform.localRotation.eulerAngles;
        _visualModel.transform.LookAt(rotateTo);
        
        Vector3 toAngle = new Vector3(0f, _visualModel.transform.localRotation.eulerAngles.y, 0f);
        _visualModel.transform.localRotation = Quaternion.Euler(fromAngle);

        for(var t = 0f; t < 1; t += Time.deltaTime/inTime) 
        {
            _visualModel.transform.localRotation = Quaternion.Euler(Vector3.Lerp(fromAngle, toAngle, t));
            yield return null;
        }
    }
}
