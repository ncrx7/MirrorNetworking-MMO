using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOutManager : MonoBehaviour
{
    public float speed = 1f;  
    [SerializeField] private Image fadeOutScreen;
    private Color fadeColor;

    public void ShowScreenNoDelay()
    {
        fadeColor.a = 1;
        fadeOutScreen.color = fadeColor;
        fadeOutScreen.gameObject.SetActive(true);
    }

    public IEnumerator FadeIn()
    {
        float alpha = fadeOutScreen.color.a;
        fadeOutScreen.gameObject.SetActive(true);

        while(alpha < 1)
        {
            yield return new WaitForSeconds(0.01f);
            alpha += 0.01f * speed;
            fadeColor.a = alpha;
            fadeOutScreen.color = fadeColor;
        }
    }

    public IEnumerator FadeOut()
    {
        float alpha = fadeOutScreen.color.a;
        
        while(alpha > 0)
        {
            yield return new WaitForSeconds(0.01f);
            alpha -= 0.01f * speed;
            fadeColor.a = alpha;
            fadeOutScreen.color = fadeColor;

            if(fadeColor.a <= 0)
            {
                fadeOutScreen.gameObject.SetActive(false);
            }
        }
    }
}
