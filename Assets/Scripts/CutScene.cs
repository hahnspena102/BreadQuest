using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class CutScene : MonoBehaviour
{
    [SerializeField]private List<CanvasGroup> canvases = new List<CanvasGroup>();
    [SerializeField]private TMPro.TextMeshProUGUI textBox;
    private List<string> sentences = new List<string>(){
        "One day, the evil S’more Goblins attacked the peaceful Dough Village. Who could possibly stop such an evil force?!",
        "“I will!” Sir Gluten exclaims.",
        "Quick! After them before the goblins make away with the innocent villagers!"
    };

    private int index = 0;
    private bool inBetween = false;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return) && !inBetween) {
            inBetween = true;
            StartCoroutine(Increment());
        }
    }

    IEnumerator Increment() {
        if (index == 2) {
            SceneManager.LoadScene(1);
        }
    
        float fadeDuration = 1f;
        float elapsedTime = 0f;

        textBox.text = "";

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Lerp(0, 1, elapsedTime / fadeDuration);
            canvases[index].alpha = alpha;
            yield return null;
            
        }

        yield return new WaitForSeconds(0.5f);

        foreach (char letter in sentences[index])
        {
            textBox.text += letter;
            yield return new WaitForSeconds(0.04f);
        }

        yield return new WaitForSeconds(1f);

        index++;
        inBetween = false;
    }
}
