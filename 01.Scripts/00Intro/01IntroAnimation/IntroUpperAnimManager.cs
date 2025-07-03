using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroUpperAnimManager : MonoBehaviour
{
    public Animator introAnim;
    public Image fade;
    private int count;
    private bool clicked;

    private void Start()
    {
        introAnim.speed = 0;
        //StartCoroutine(FadeOut());
        fade.color = new Color(0, 0, 0, 0);
        StartCoroutine(AnimPlay());
        clicked = false;
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            count++;
            if (count == 1)
            {

                StartCoroutine(FadeIn());
                clicked = true;
                ProjectManager.instance.touchSound.clip = ProjectManager.instance.touchClip[2];
                ProjectManager.instance.touchSound.Play();
            }
        }
    }

    IEnumerator FadeIn()
    {
        for(float i = 0; i<1.1f; i += 0.1f)
        {
            fade.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        if (clicked)
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(2);
        }
    }

    IEnumerator AnimPlay()
    {
        yield return new WaitForSeconds(1f);
        introAnim.speed = 1;
        yield return new WaitForSeconds(20f);
        clicked = false;
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        for (float i = 1; i>-0.1f; i -= 0.1f)
        {
            fade.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.08f);
        }
        
    }

}
