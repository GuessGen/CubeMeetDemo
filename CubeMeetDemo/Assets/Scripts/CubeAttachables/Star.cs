using UnityEngine;

public class Star : MonoBehaviour
{
    [SerializeField] private ParticleSystem _confetti;
    private void OnTriggerEnter(Collider other) 
    {
        GameManager.Instance.starsCollectedInThisLevel++;
        _confetti.gameObject.SetActive(true);
        gameObject.SetActive(false);
        Destroy(gameObject.transform.parent.gameObject, 0.5f);
    }
}
