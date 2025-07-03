using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{


    public Image fade;

    public AudioSource narration;
    public AudioClip[] narClip;

    private void Start()
    {
        StartCoroutine(FadeOut());
        //StartCoroutine(NarrationPlay());
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            //StartCoroutine(FadeIn());
            SceneManager.LoadScene(1);
            ProjectManager.instance.touchSound.clip = ProjectManager.instance.touchClip[2];
            ProjectManager.instance.touchSound.Play();
        }
    }

    IEnumerator FadeIn()
    {
        for (float i = 0; i < 1.1f; i += 0.1f)
        {
            fade.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.1f);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }
    IEnumerator FadeOut()
    {
        for (float i = 1; i > -0.1f; i -= 0.1f)
        {
            fade.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.1f);
        }

    }

    //IEnumerator NarrationPlay()
    //{
    //    yield return new WaitForSeconds(3f);
    //    narration.clip = narClip[0];
    //    narration.Play();
    //    yield return new WaitForSeconds(narClip[0].length+2);
    //    narration.clip = narClip[1];
    //    narration.Play();



    //}

}
