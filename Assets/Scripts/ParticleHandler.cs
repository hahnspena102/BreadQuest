using UnityEngine;
using System.Collections;

public class ParticleHandler : MonoBehaviour
{
    [SerializeField] GameObject available;
    [SerializeField] GameObject unavailable;

    public void PlayParticle(float x, float y) {
        Transform selectedParticle = available.gameObject.transform.GetChild(0);
        ParticleSystem particle = selectedParticle.GetComponent<ParticleSystem>();
                
        particle.transform.position = new Vector2(x, y);
        particle.Play();
        StartCoroutine(ToggleParticle(selectedParticle));
        
         
             
      
    }

    private IEnumerator ToggleParticle(Transform child)
    {
        Transform originalParent = child.parent; 

        child.SetParent(unavailable.transform); 
        yield return new WaitForSeconds(2f); 

        child.SetParent(originalParent);
    }
}
