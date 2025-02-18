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
    private List<List<(int, string)>> dialogue = new List<List<(int, string)>>
    {
        new List<(int, string)>{(1, "Watch out! There's a "),(0, "ooh, a gummy heart! It restores 1 HP!")}
    };

    public IEnumerator DisplayDialogue(int index) {
        if (index < 0 || index >= dialogue.Count) yield break;

        canvasGroup.alpha = 1f;

        for (int i = 0; i < dialogue[index].Count; i++){
            textBox.text = "";
            string fullText = dialogue[index][i].Item2;

            for (int j = 0; j < icons.Count; j++)
            {
                icons[j].gameObject.SetActive(j == dialogue[index][i].Item1);
            }

            yield return new WaitForSeconds(0.5f);

            foreach (char letter in fullText)
            {
                textBox.text += letter;
                yield return new WaitForSeconds(0.05f);
            }

            yield return new WaitForSeconds(3f);
        }

        yield return new WaitForSeconds(2f);

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
