using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField]private DialogueManager dialogueManager;
    [SerializeField]private int dialogueIndex;
    [SerializeField]private bool triggered = false;
    
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            if (!triggered) {
                triggered = true;
                StartCoroutine(dialogueManager.DisplayDialogue(dialogueIndex));
            }
            
        }
    }
}
