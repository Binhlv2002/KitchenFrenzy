using UnityEngine;

public class PlayerVisual : MonoBehaviour
{
    [SerializeField] private Transform characterParent;
    private GameObject currentCharacter;

    public void SetPlayerCharacter(GameObject newCharacter)
    {
        if (currentCharacter != null)
        {
            Destroy(currentCharacter);
        }

        currentCharacter = Instantiate(newCharacter, characterParent);
        currentCharacter.transform.localPosition = Vector3.zero;
        currentCharacter.transform.localRotation = Quaternion.identity;
    }

    public Animator GetAnimator()
    {
        if (currentCharacter != null)
        {
            return currentCharacter.GetComponent<Animator>();
        }
        return null;
    }
}
