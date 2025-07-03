using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainSystem : MonoBehaviour
{

    [Header("info anim")]
    public GameObject[] infoUi;  // 인포메이션 ui
    public GameObject[] countdown; //3 2 1 카운트다운
    public GameObject[] gaugeAndTimer; //게이지, 타이머 ui 
    public GameObject[] gameUi;  //게임 ui
    public GameObject[] fishGroup;
    public GameObject[] successUi;
    public GameObject[] failUi;
    public AudioSource narration;
    public AudioClip[] narClip;
    public GameObject guide;
    private bool successBool;
    private bool failBool;
    private bool done;
    private int resultCount;
    [Header("trash")]
    public Transform[] trash00;
    public Transform[] trash01;
    public Transform[] trash02;
    public GameObject level01;
    public GameObject level02;
    public Image[] gaugeTrash;
    public Transform[] gaugeStar;
    private int downCount;

    public AudioSource successNarr;
    public AudioClip[] successNarClip;
    public AudioSource failNarr;
    public AudioClip[] failNarrClip;

    [Header("shark")]
    public SharkMovement shark;
    public GameObject[] sharkUiAnimation;
    private int sharkAngryCount;

    private bool gamePlay;
    private bool infoDone;
    [HideInInspector] public int trashIndex;
    public ParticleSystem[] trashTouchParticle;
    public Text[] timer;
    private float sec;
    [HideInInspector] public int chapter;
    [HideInInspector] public bool sharkAngry;
    private void Start()
    {
        guide.SetActive(false);
        for (int i = 0; i < 2; i++)
        {
            gaugeTrash[i].fillAmount = 0;
            gameUi[i].SetActive(false);
            infoUi[i].SetActive(true);
        }
        trashIndex = -1;
        sec = 90;
        infoDone = false;
        gamePlay = false;
        done = false;
        successBool = false;
        failBool = false;
        //ObjectPool();
        StartCoroutine(InfoDelay());
        StartCoroutine(NarrationPlay());
    }
    private void Update()
    {



        if (infoDone)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {
                StartCoroutine(CountdownPlay());
            }
#else
            if(Input.touchCount>0)
            {
                StartCoroutine(CountdownPlay());
            }           
#endif

        }
        if (gamePlay)
        {
            
#if UNITY_EDITOR
            if (Input.GetMouseButtonDown(0))
            {

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit[] hit;
                hit = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
                if (hit.Length > 0)
                {
                    for (int i = 0; i < hit.Length; i++)
                    {
                        if (!sharkAngry)
                        {

                            if (hit[i].collider.CompareTag("Trash"))
                            {

                                hit[i].collider.GetComponent<TrashManager>().clicked = true;
                                ProjectManager.instance.touchSound.clip = ProjectManager.instance.touchClip[0];
                                ProjectManager.instance.touchSound.Play();
                            }
                        }
                        if (hit[i].collider.CompareTag("Shark"))
                        {
                            if (shark.canHit)
                            {

                                StartCoroutine(SharkHit());
                                ProjectManager.instance.touchSound.clip = ProjectManager.instance.touchClip[1];
                                ProjectManager.instance.touchSound.Play();
                            }
                        }
                        if (hit[i].collider.CompareTag("Untagged"))
                        {
                            ProjectManager.instance.touchSound.clip = ProjectManager.instance.touchClip[2];
                            ProjectManager.instance.touchSound.Play();
                        }
                    }
                }
            }

#else
            if (Input.touchCount > 0)
            {
                for (int i = 0; i < Input.touchCount; i++)
                {
                    Touch touch = Input.GetTouch(i);
                    if (touch.phase == TouchPhase.Began)
                    {
                        Vector3 touchPosition = Input.mousePosition;
                        touchPosition.x *= 2;
                        //touchPosition.y *= 2;
                        Ray ray = Camera.main.ScreenPointToRay(touchPosition);
                        RaycastHit[] hit;
                        hit = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
                        if (hit.Length > 0)
                        {
                            for (int j = 0; j < hit.Length; j++)
                            {
                                if (!sharkAngry)
                                {

                                    if (hit[j].collider.CompareTag("Trash"))
                                    {

                                        hit[j].collider.GetComponent<TrashManager>().clicked = true;
                                        ProjectManager.instance.touchSound.clip = ProjectManager.instance.touchClip[0];
                                        ProjectManager.instance.touchSound.Play();
                                    }
                                }
                                if (hit[j].collider.CompareTag("Shark"))
                                {
                                    if (shark.canHit)
                                    {

                                        StartCoroutine(SharkHit());
                                        ProjectManager.instance.touchSound.clip = ProjectManager.instance.touchClip[1];
                                        ProjectManager.instance.touchSound.Play();
                                    }
                                }
                                if (hit[j].collider.CompareTag("Untagged"))
                                {
                                    ProjectManager.instance.touchSound.clip = ProjectManager.instance.touchClip[2];
                                    ProjectManager.instance.touchSound.Play();
                                }
                            }
                        }
                    }
                }
            }
#endif
            ProjectManager.instance.bossBool = sharkAngry;
            if (!sharkAngry)
            {
                Timer();
                sharkAngryCount = 0;
                switch (chapter)
                {
                    case 0:
                        fishGroup[0].SetActive(true);
                        if (trashIndex > -1)
                        {
                            if (trashIndex > 2)
                            {
                                downCount++;
                                if (downCount == 1)
                                {
                                    StartCoroutine(DownPlay00());
                                }
                            }
                            if (trashIndex > 14)
                            {
                                sharkAngry = true;
                            }
                        }

                        break;
                    case 1:
                        fishGroup[1].SetActive(true);
                        if (trashIndex > -1)
                        {
                            level01.SetActive(true);
                            if (trashIndex > 10)
                            {
                                downCount++;
                                if (downCount == 1)
                                {
                                    StartCoroutine(DownPlay01());
                                }
                            }

                            if (trashIndex >= 29)
                            {
                                sharkAngry = true;
                            }
                        }

                        break;
                    case 2:
                        fishGroup[2].SetActive(true);
                        if (trashIndex > -1)
                        {
                            //level02.SetActive(true);
                            if (trashIndex >40)
                            {
                                downCount++;
                                if (downCount == 1)
                                {
                                    StartCoroutine(DownPlay02());
                                }
                            }

                        }
                        break;
                }
            }
            else
            {
                downCount = 0;
                sharkAngryCount++;
                if (sharkAngryCount == 1)
                {
                    StartCoroutine(SharkAngryPlay(Random.Range(0, 2)));
                }

            }
            if (trashIndex >= 45)
            {
                successBool = true;
                done = true;
            }


        }
        if (successBool)
        {
            resultCount++;
            if (resultCount == 1)
            {
                StartCoroutine(SuccessNarrPlay());
            }
            for (int i = 0; i < successUi.Length; i++)
            {
                successUi[i].SetActive(true);
            }
        }
        if (failBool)
        {
            resultCount++;
            if (resultCount == 1)
            {
                StartCoroutine(FailNarrPlay());
            }
            for (int i = 0; i < failUi.Length; i++)
            {
                failUi[i].SetActive(true);
            }
        }



    }

    public void Timer()
    {
        if (!done)
        {

            if (sec > 0)
            {

                sec -= Time.deltaTime;
                string timerstr = string.Format("{0:N0}", sec);
                for (int i = 0; i < timer.Length; i++)
                {
                    timer[i].text = timerstr;
                }
            }
            else
            {
                sec = 0;
                failBool = true;
            }
        }

    }

    //private void ObjectPool()
    //{
    //    for (int i = 0; i < 50; i++)
    //    {
    //        GameObject mete = Instantiate(trash, Vector3.zero, Quaternion.Euler(0f, 0f, 0f));
    //        mete.gameObject.SetActive(false);
    //        mete.transform.parent = trashListBox.transform;
    //        trashList.Add(mete);
    //    }
    //}

    IEnumerator NarrationPlay()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < narClip.Length; i++)
        {
            narration.clip = narClip[i];
            narration.Play();
            if (i == narClip.Length - 1)
            {
                yield return new WaitForSeconds(narClip[i].length);
                infoDone = true;
                guide.SetActive(true);
            }
            else
            {

                yield return new WaitForSeconds(narClip[i].length + 0.2f);
            }
        }
    }
    IEnumerator SuccessNarrPlay()
    {
        level02.SetActive(true);
        for(int i=0; i < trash00.Length; i++)
        {
            trash00[i].gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(8f);
        for (int i = 0; i < successNarClip.Length; i++)
        {
            successNarr.clip = successNarClip[i];
            successNarr.Play();
            if (i == successNarClip.Length - 1)
            {
                yield return new WaitForSeconds(successNarClip[i].length + 2f);
                SceneManager.LoadScene(0);
            }
            else
            {
                yield return new WaitForSeconds(successNarClip[i].length + 0.2f);
            }
        }
    }

    IEnumerator FailNarrPlay()
    {
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < failNarrClip.Length; i++)
        {
            failNarr.clip = failNarrClip[i];
            failNarr.Play();
            if (i == failNarrClip.Length - 1)
            {
                yield return new WaitForSeconds(successNarClip[i].length + 2f);
                SceneManager.LoadScene(0);
            }
            else
            {
                yield return new WaitForSeconds(failNarrClip[i].length + 0.2f);
            }
        }
    }

    IEnumerator SharkAngryPlay(int idx)
    {
        sharkUiAnimation[idx].SetActive(true);
        yield return new WaitForSeconds(1f);
        switch (idx)
        {
            case 0:
                shark.direction = true;

                break;
            case 1:
                shark.direction = false;

                break;
        }
        shark.angry = true;
        yield return new WaitForSeconds(2f);
        sharkUiAnimation[idx].SetActive(false);
    }

    IEnumerator CountdownPlay()
    {
        for (int i = 0; i < 2; i++)
        {

            countdown[i].SetActive(true);
            gameUi[i].SetActive(true);
            infoUi[i].SetActive(false);
        }
        gaugeAndTimer[0].SetActive(false);
        gaugeAndTimer[1].SetActive(false);
        infoDone = false;
        yield return new WaitForSeconds(5f);
        gamePlay = true;
        for (int i = 0; i < 2; i++)
        {

            countdown[i].SetActive(false);
        }
        gaugeAndTimer[0].SetActive(true);
        gaugeAndTimer[1].SetActive(true);
    }

    IEnumerator InfoDelay()
    {
        yield return new WaitForSeconds(4f);
        //infoDone = true;
    }

    IEnumerator SharkHit()
    {

        shark.sharkAnim.SetBool("hit", true);
        shark.hitedCount++;
        shark.canHit = false;
        yield return new WaitForSeconds(1f);
        shark.canHit = true;
        shark.sharkAnim.SetBool("hit", false);
    }

    IEnumerator DownPlay00()
    {
        for (int i = 10; i < 30; i++)
        {
            trash00[i].GetComponent<TrashManager>().down = true;
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator DownPlay01()
    {
        for (int i = 25; i < 50; i++)
        {
            trash00[i].GetComponent<TrashManager>().down = true;
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator DownPlay02()
    {
        for (int i = 45; i < 60; i++)
        {
            trash00[i].GetComponent<TrashManager>().down = true;
            yield return new WaitForSeconds(0.5f);
        }
    }


}
