using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour

{
    [SerializeField]private TMPro.TextMeshProUGUI textBox;
    [SerializeField]private List<Image> icons = new List<Image>();
    private CanvasGroup canvasGroup;
    private bool reset;
    
    void Start(){
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private List<List<(int, string)>> dialogue = new List<List<(int, string)>>
    {
        new List<(int, string)>{(1, "SirGluten. This journey will be perilous, are you ready?"), (0, "Yes!")},
        new List<(int, string)>{(1, "Looks like the terrain goes uphill, press Space to jump."), (0, "HUUP!")},
        new List<(int, string)>{(1, "Looks like a far jump... Hold Shift to sprint. Be careful not to sprint too much or else you'll grow exhausted.")},
        new List<(int, string)>{(1, "You can also double jump for more verticality! It will also use some of your Yeast meter.")},

        new List<(int, string)>{(1, "We're approaching enemies... remember how to fight?"), (0, "Ha! Of course, it's Enter/Return to Attack.")},
        new List<(int, string)>{(1, "Nice job! More Marshmoblins above. Stay vigilant.")},

        new List<(int, string)>{(0, "Ooh!  A gummy plant!"), (1, "Yep! These plants can heal 1 HP if you swing at them. Give it a try!")},
        new List<(int, string)>{(1, "GRAHOBLIN! HE'S CHARGING AT US! FIGHT!")},
        new List<(int, string)>{(0, "Hope you're not afraid of heights!"),(1, "I can fly..."),(0, "Oh yeah.")},
        new List<(int, string)>{(1, "Woah! A Chocoblin is nearby, watch out for those fireballs!")},

        new List<(int, string)>{(1, "A hoard of s'more goblins nearby!")},
        new List<(int, string)>{(0, "Looks like we're nearing the cave!"), (1, "The big boss is near. Prepare yourself!")}
        

        //new List<(int, string)>{(1, "Watch out! That's a Marshmoblin! They'll throw their spears at you any chance they get.")},
    };

    public void StartDialogue(int index)
    {
        StopDialogue();
        StartCoroutine(DisplayDialogue(index));
    }

    public IEnumerator DisplayDialogue(int index) {
        if (index < 0 || index >= dialogue.Count) yield break;

        reset = true;
        yield return new WaitForSeconds(0.5f);
        reset = false;
        canvasGroup.alpha = 1f;

        for (int i = 0; i < dialogue[index].Count; i++){
            textBox.text = "";
            string fullText = dialogue[index][i].Item2;

            for (int j = 0; j < icons.Count; j++)
            {
                // TODO: ADD A RESET DIALOGUE
                if (reset) {
                    reset = false;
                    yield break;
                }
                icons[j].gameObject.SetActive(j == dialogue[index][i].Item1);
            }

            yield return new WaitForSeconds(0.5f);

            foreach (char letter in fullText)
            {
                textBox.text += letter;
                yield return new WaitForSeconds(0.02f);
            }

            yield return new WaitForSeconds(1f);
        }

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
        textBox.text = "";
        foreach (var icon in icons) icon.gameObject.SetActive(false);
        canvasGroup.alpha = 0f;
    }
}
