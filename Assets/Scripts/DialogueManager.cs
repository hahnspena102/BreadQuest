using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour

{
    [SerializeField]private TMPro.TextMeshProUGUI textBox;
    [SerializeField]private List<Image> icons = new List<Image>();
    private CanvasGroup canvasGroup;
    void Start(){
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private List<(int, string)> dialogue = new List<(int, string)>
    {
        (0, "ooh, a gummy heart! It restores 1 HP!"),
        (1, "Huh?")
    };

    public IEnumerator DisplayDialogue(int index) {
        if (index < 0 || index >= dialogue.Count) yield break;

        canvasGroup.alpha = 1f;

        textBox.text = "";
        string fullText = dialogue[index].Item2;

        for (int i = 0; i < icons.Count; i++)
        {
            icons[i].gameObject.SetActive(i == dialogue[index].Item1);
        }

        yield return new WaitForSeconds(0.5f);

        foreach (char letter in fullText)
        {
            textBox.text += letter;
            yield return new WaitForSeconds(0.05f);
        }

        yield return new WaitForSeconds(3f);

        float fadeDuration = 1f;
        float startTime = Time.time;

        while (Time.time - startTime < fadeDuration)
        {
            float alpha = Mathf.Lerp(1, 0, (Time.time - startTime) / fadeDuration);
            canvasGroup.alpha = alpha;
            yield return null;
        }
    }
}
