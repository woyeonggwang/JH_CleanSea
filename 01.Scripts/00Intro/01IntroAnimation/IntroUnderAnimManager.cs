using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroUnderAnimManager : MonoBehaviour
{
    public Animator underAnimator;
    public AnimationClip underAnimClip;
    public Animator sharkAnim;
    public Image fade;
    public AudioSource angrySound;
    public GameObject trash;
    private bool clicked;
    private bool fadeBool;
    private int count;
    private void Start()
    {
        fadeBool = false;
        StartCoroutine(FadeOut());

        StartCoroutine(UnderAnimPlay());
        underAnimator.speed = 0;
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
        if (fadeBool)
        {
            fade.color = new Color(0, 0, 0, 1);
        }
    }


    public void EatAnimCheck()
    {
        sharkAnim.SetBool("eat", true);
        //trash.SetActive(false);
    }

    public void DamagedShark()
    {
        sharkAnim.SetBool("angry", true);
        angrySound.Play();
    }

    IEnumerator UnderAnimPlay()
    {
        yield return new WaitForSeconds(1f);
        underAnimator.speed = 1;
        yield return new WaitForSeconds(underAnimClip.length-3);
        StartCoroutine(FadeIn());
    }
    public void OpenMouthAnimCheck()
    {
        sharkAnim.SetBool("openmouth", true);
    }
    IEnumerator FadeIn()
    {
        for(float i=0; i < 1.1f; i += 0.1f)
        {
            fade.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.08f);
        }
        yield return new WaitForSeconds(1f);
        fadeBool = true;
        if (clicked)
        {
            SceneManager.LoadScene(3);
        }
        else
        {
            SceneManager.LoadScene(3);
        }
    }

    IEnumerator FadeOut()
    {
        for (float i = 1; i > -0.1f; i -= 0.1f)
        {
            fade.color = new Color(0, 0, 0, i);
            yield return new WaitForSeconds(0.08f);
        }
        yield return new WaitForSeconds(1f);

    }

}
