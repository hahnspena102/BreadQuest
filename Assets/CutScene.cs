using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class DialogueManagerV2 : MonoBehaviour
{
    [SerializeField] private List<TMPro.TextMeshProUGUI> textBoxes = new List<TMPro.TextMeshProUGUI>(); // List of textboxes for each image
    [SerializeField] private List<Image> icons = new List<Image>(); // List of images to cycle through
    private CanvasGroup canvasGroup;
    private bool reset;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private List<List<(int, string)>> dialogue = new List<List<(int, string)>>
    {
        new List<(int, string)>{(0, "The Flour Kingdom was just raided by a group of golblins...")},  // Image 1 and text 1
        new List<(int, string)>{(1, "Sir Gluten, a recently appointed knight, puts on his armour to defend his kingdom's honor!")},  // Image 2 and text 2
        new List<(int, string)>{(2, "Before he goes out to fight, he sees a fairy. The fairy says, 'Halt! It is too dangerous for you right now, heed my instructions...'")},  // Image 3 and text 3
    };

    public void StartDialogue(int index)
    {
        StopDialogue();
        StartCoroutine(DisplayDialogue(index));
    }

    public IEnumerator DisplayDialogue(int index)
    {
        if (index < 0 || index >= dialogue.Count) yield break;

        reset = true;
        yield return new WaitForSeconds(0.5f);
        reset = false;
        canvasGroup.alpha = 1f;

        // Cycle through each line of dialogue for the current index
        for (int i = 0; i < dialogue[index].Count; i++)
        {
            // Set the text for each corresponding text box
            for (int j = 0; j < icons.Count; j++)
            {
                // Enable the appropriate image and text box (only one should be active at a time)
                icons[j].gameObject.SetActive(j == dialogue[index][i].Item1);
                textBoxes[j].text = dialogue[index][i].Item2; // Assign text for the respective box
                textBoxes[j].gameObject.SetActive(j == dialogue[index][i].Item1); // Show only the active text box
            }

            // Wait for a brief period before showing the full text
            yield return new WaitForSeconds(0.5f);

            // Show the text one letter at a time for the active text box
            foreach (char letter in textBoxes[dialogue[index][i].Item1].text)
            {
                textBoxes[dialogue[index][i].Item1].text += letter;
                yield return new WaitForSeconds(0.02f);
            }

            yield return new WaitForSeconds(1f); // Pause after the text is fully typed out
        }

        // Fade out after all dialogue is shown
        yield return new WaitForSeconds(1f);

        float fadeDuration = 1f;
        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(1, 0, elapsedTime / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }
    }

    private void StopDialogue()
    {
        StopAllCoroutines();
        foreach (var textBox in textBoxes) textBox.text = "";
        foreach (var icon in icons) icon.gameObject.SetActive(false);
        canvasGroup.alpha = 0f;
    }
}
