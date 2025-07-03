using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashManager : MonoBehaviour
{
    private ParticleSystem bubbleParticle;
    public MainSystem mainSystem;
    public bool down;
    public bool clicked;
    private float outlineVal;
    private float outlineFloat;
    private int disableCount;
    private float upperPosY;
    private float moveSpeed;
    private float downPosY;
    private int bubbleCount;
    private void Start()
    {
        transform.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", 0);
        bubbleParticle = transform.GetChild(0).GetComponent<ParticleSystem>();
        upperPosY = transform.localPosition.y;
        downPosY = upperPosY - 4f;
        down = false;
        clicked = false;
    }

    private void Update()
    {
        if (!clicked)
        {
            if (down)
            {
                bubbleCount = 0;
                bubbleParticle.loop = true;
                float dist = Vector3.Distance(transform.localPosition, new Vector3(transform.localPosition.x, downPosY, transform.localPosition.z));
                if (dist > 0.1f)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, downPosY, transform.localPosition.z),0.01f);
                }
                else
                {
                    down = false;
                }
            
            }
            else
            {
                bubbleCount++;
                if (bubbleCount == 1)
                {
                    StartCoroutine(BubblePlay());
                }
            }
        }
        else
        {
            if (down)
            {
                float dist = Vector3.Distance(transform.localPosition, new Vector3(transform.localPosition.x, downPosY, transform.localPosition.z));
                if (dist > 0.1f)
                {
                    transform.localPosition = Vector3.MoveTowards(transform.localPosition, new Vector3(transform.localPosition.x, downPosY, transform.localPosition.z), 0.01f);
                }
                else
                {
                    down = false;
                }
            }
            disableCount++;
            if (disableCount == 1)
            {
                StartCoroutine(DisablePlay());
            }
        }
    }

    IEnumerator DisablePlay()
    {
        mainSystem.trashIndex++;
        for (int k = 0; k < 2; k++)
        {

            mainSystem.gaugeTrash[k].fillAmount += 0.023f;
            mainSystem.gaugeStar[k].localPosition = new Vector3(mainSystem.gaugeStar[k].localPosition.x, mainSystem.gaugeStar[k].localPosition.y + 11.9f, mainSystem.gaugeStar[k].localPosition.z);
        }
        for (float i=0; i <1.1f; i +=0.01f)
        {
            transform.GetComponent<MeshRenderer>().material.SetFloat("_Cutoff", i);
            yield return new WaitForSeconds(0.01f);
        }
        yield return new WaitForSeconds(1f);
        gameObject.SetActive(false);
    }

    IEnumerator BubblePlay()
    {
        yield return new WaitForSeconds(Random.Range(2, 6));
        bubbleParticle.Play();
        yield return new WaitForSeconds(Random.Range(5, 30));
        bubbleCount = 0;
    }


}
