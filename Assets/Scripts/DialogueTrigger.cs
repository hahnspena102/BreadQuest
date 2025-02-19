using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueManager dialogueManager;
    [SerializeField]private int dialogueIndex;
    [SerializeField]private bool triggered = false;
    
    private void Start() { 
        if (dialogueManager == null) {
            dialogueManager = FindFirstObjectByType<DialogueManager>();
            
            if (dialogueManager == null) {
                Debug.LogError("DialogueManager not found in the scene!");
            }
        }
    }
    
    private void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            if (!triggered) {
                triggered = true;
                dialogueManager.StartDialogue(dialogueIndex);
            }
            
        }
    }
}
