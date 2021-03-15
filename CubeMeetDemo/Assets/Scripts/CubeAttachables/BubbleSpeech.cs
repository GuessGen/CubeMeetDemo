using UnityEngine;

[RequireComponent(typeof(Animator))]
public class BubbleSpeech : MonoBehaviour
{
    private void Start()
    {
        Invoke(nameof(PlayEndAnimation), 5f);
    }

    private void PlayEndAnimation() 
    {
        GetComponent<Animator>().SetBool("Destroy", true);
    }

    private void DestroyCurrentObject()
    {
        Destroy(gameObject);
    }
}
