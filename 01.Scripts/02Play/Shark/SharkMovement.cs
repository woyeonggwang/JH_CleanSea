using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SharkMovement : MonoBehaviour
{

    public MainSystem mainSystem;

    public Animator targetAnim;
    public Animator sharkAnim;
    public Transform target;
    public GameObject colliderBanner; //쓰레기 터치 못하게 막는 콜라이더 
    public Animator cameraAnim;
    
    public Image[] crackedImage;  //금간 유리 이미지
    public Sprite[] crackedSpr; //금간 유리 스프라이트
    public bool direction;
    public float moveSpeed;
    public Image gauge;
    public Image[] redImage;
    [HideInInspector] public float hitedCount;
    [HideInInspector] public bool angry;
    [HideInInspector] public bool canHit;
    private bool stop;
    private float guageTarget;
    private float gaugeVal;
    public int crackIdx;
    private int runCount; //도망갈때 사용하는 카운트
    private int crackIndex;//금간 이미지 인덱스 
    private int angryPlayCount;
    public bool move;

    private void Start()
    {
        
        for(int i=0; i < crackedImage.Length; i++)
        {

            crackedImage[i].gameObject.SetActive(false);
        }
        angry = false;
        crackIndex = 0;
        canHit = false;
        targetAnim.SetBool("move", true);
        targetAnim.speed = 0;
        hitedCount = 0;
        angryPlayCount = 0;
        runCount = 2;
        gaugeVal = 1;
    }


    private void Update()
    {
        
        switch (crackIdx)
        {
            case 0:

                for (int i = 0; i < crackedImage.Length; i++)
                {

                    crackedImage[i].gameObject.SetActive(false);
                }
                break;
            case 1:
                if (direction)
                {

                    crackedImage[0].gameObject.SetActive(true);
                    crackedImage[1].gameObject.SetActive(false);
                    crackedImage[0].sprite = crackedSpr[0];
                }
                else
                {
                    crackedImage[0].gameObject.SetActive(false);
                    crackedImage[1].gameObject.SetActive(true);
                    crackedImage[1].sprite = crackedSpr[0];
                }
                break;
            case 2:
                for (int i = 0; i < crackedImage.Length; i++)
                {

                    crackedImage[i].sprite = crackedSpr[1];
                }
                break;
        }
        if (angry)
        {
            
            targetAnim.speed = 1;
            switch (direction)
            {
                case true:
                    targetAnim.SetBool("move0", true);
                    break;
                case false:
                    targetAnim.SetBool("move1", true);
                    break;
            }

            //crackedImage.sprite = crackedSpr[crackIndex];
            //if (crackIndex < 2)
            //{
            //    crackIndex++;
            //} //부셔진 유리
            colliderBanner.SetActive(true);
            float dist = Vector3.Distance(transform.position, target.position);
            if (move)
            {
                if (dist > 3f)
                {

                    sharkAnim.SetBool("attack", false);
                    sharkAnim.SetBool("hit", false);
                    transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);
                    Vector3 dir = target.position - transform.position;
                    dir.y = 0;
                    Quaternion rot = Quaternion.LookRotation(dir);
                    transform.rotation = Quaternion.Lerp(transform.rotation, rot, 1f);
                    stop = false;
                }
                else
                {
                    stop = true;
                }
            }
            else
            {

                if (dist > 3f)
                {

                }
                else
                {
                    if (stop)
                    {

                        gauge.color = new Color(1, 1, 1, 1);
                        gauge.fillAmount = gaugeVal - hitedCount/5;
                        angryPlayCount++;
                        if (angryPlayCount == 1)
                        {
                            canHit = false;
                            StartCoroutine(AngryPlay());
                        }
                        if (hitedCount >= 5)
                        {
                            angry = false;
                            canHit = false;
                            mainSystem.sharkAngry = false;
                            runCount++;
                            if (runCount == 1)
                            {
                                mainSystem.chapter++;
                            }
                        }
                
                    }
                }
            }
            
        }
        else
        {
            stop = false;   
            crackIdx = 0;
            gauge.color = new Color(1, 1, 1, 0);
            gaugeVal = 1;
            runCount = 0;
            hitedCount = 0;
            targetAnim.SetBool("move0", false);
            targetAnim.SetBool("move1", false);
            colliderBanner.SetActive(false);
            float dist = Vector3.Distance(transform.position, target.position);
            if (dist > 3f)
            {
                sharkAnim.SetBool("attack", false);
                sharkAnim.SetBool("hit", false);
                transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed);
                Vector3 dir = target.position - transform.position;
                dir.y = 0;
                Quaternion rot = Quaternion.LookRotation(dir);
                transform.rotation = Quaternion.Lerp(transform.rotation, rot, 1f);
            }
            else
            {
                
            }
        }

        
    }

    

    IEnumerator AngryPlay()
    {
        sharkAnim.SetBool("attack", true);
        yield return new WaitForSeconds(0.4f);
        if (crackIdx < 3)
        {
            crackIdx++;
            
        }
        if (direction)
        {
            redImage[0].gameObject.SetActive(true);
            redImage[1].gameObject.SetActive(false);
        }
        else
        {
            redImage[0].gameObject.SetActive(false);
            redImage[1].gameObject.SetActive(true);
        }
        cameraAnim.SetBool("state", true);

        yield return new WaitForSeconds(0.6f);
        cameraAnim.SetBool("state", false);
        if (direction)
        {
            redImage[0].gameObject.SetActive(false);
            redImage[1].gameObject.SetActive(false);
        }
        else
        {
            redImage[0].gameObject.SetActive(false);
            redImage[1].gameObject.SetActive(false);
        }
        sharkAnim.SetBool("attack", false);
        canHit = true;
        
        
        yield return new WaitForSeconds(4f);
        angryPlayCount = 0;
    }

    

    IEnumerator CrackPlay()
    {
        yield return new WaitForSeconds(0.5f);
    }

}
