using UnityEngine;

public class ParticleHandler : MonoBehaviour
{
    public void PlayParticle(float x, float y) {
        if (transform.childCount > 0)
        {
            Transform firstChild = transform.GetChild(0);
            if (firstChild.childCount > 0)
            {
                Transform selectedChild = firstChild.GetChild(0);
                ParticleSystem particle = selectedChild.GetComponent<ParticleSystem>();

                if (particle != null)
                {
                    particle.transform.position = new Vector2(x, y);
                    particle.Play();
                }
                else
                {
                    Debug.LogWarning("No ParticleSystem found on selected child.");
                }
            }
            else
            {
                Debug.LogWarning("First child has no children.");
            }
        }
        else
        {
            Debug.LogWarning("No children found under this object.");
        }
    }
}
