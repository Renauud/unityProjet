using UnityEngine;

public class MerchantController : MonoBehaviour
{
    private void Start()
    {
        DialogueManager.Instance.OnDialogueEnd += GiveSword;
    }

    private void GiveSword()
    {
        DialogueManager.Instance.GiveSword();
        Debug.Log("le joueur a obtenu une épée");
    }
}