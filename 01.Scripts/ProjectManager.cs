using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProjectManager : MonoBehaviour
{
    public static ProjectManager instance;
    public ParticleSystem particle;
    public AudioSource touchSound;
    public AudioClip[] touchClip;
    public float distance = 5f;
    public AudioSource main;
    public AudioClip[] mainClip;
    public AudioSource bossAudio;
    public bool bossBool;
    private float mainVol;
    private float delayTime;
    private int introCount;
    private int gameCount;
    private int bossCount;


    private void Awake()
    {
        bossAudio.volume = 0;
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SettingsImport();
    }

    public void Update()
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        if(index!=3)
        {
            bossBool = false;
            main.clip = mainClip[0];
            introCount++;
            gameCount = 0;
            main.volume = mainVol;
            if (introCount == 1)
            {
                main.clip = mainClip[0];
                main.Play();
            }

        }
        else if (index == 3)
        {
            introCount = 0;
            gameCount++;
            if (gameCount == 1)
            {
                main.clip = mainClip[1];
                main.Play();
            }
        }
        if (!bossBool)
        {
            bossCount = 0;
            if (main.volume < mainVol)
            {

                main.volume +=0.5f*Time.deltaTime;
            }
            bossAudio.volume -=0.5f*Time.deltaTime;
        }
        else
        {
            bossCount++;
            if (bossCount == 1)
            {
                bossAudio.Play();
            }
            if (bossAudio.volume < mainVol)
            {
                bossAudio.volume += 0.5f * Time.deltaTime;
            }
            main.volume -= 0.5f * Time.deltaTime;
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            SettingsImport();
        }
        delayTime += Time.deltaTime;

        if (delayTime > 180)
        {
            SceneManager.LoadScene(0);
            delayTime = 0;
        }
        if (Input.GetMouseButtonDown(0))
        {
            //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            //RaycastHit[] hit;
            //hit = Physics.RaycastAll(ray.origin, ray.direction, Mathf.Infinity);
            //if (hit.Length > 0)
            //{
            //    for (int i = 0; i < hit.Length; i++)
            //    {

            //        if (hit[i].collider.CompareTag("Trash"))
            //        {
            //            touchSound.clip = touchClip[0];
            //            touchSound.Play();

            //        }
            //        else if (hit[i].collider.CompareTag("Shark"))
            //        {
            //            touchSound.clip = touchClip[1];
            //            touchSound.Play();
            //        }
            //        else
            //        {
            //            touchSound.clip = touchClip[2];
            //            touchSound.Play();
            //        }
            //    }

            //}
            
            delayTime = 0;
            Vector3 pos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x*2, Input.mousePosition.y, distance));
            if (particle != null)
            {
                particle.transform.position = pos;
                particle.Play();
            }
            if (touchSound != null)
            {
                //touchSound.Play();
            }
        }
    }
    private void SettingsImport()
    {
        string filePath = Environment.CurrentDirectory + "\\settings.txt";
        if (System.IO.File.Exists(filePath))
        {
            string[] config = System.IO.File.ReadAllLines(filePath);
            for (int i = 0; i < config.Length; i++)
            {
                string[] arr = config[i].Split(' ');
                if (arr[0].ToLower() == "main")
                {
                    mainVol = float.Parse(arr[1]);
                }
            }
        }
        else
        {
            string[] config = new string[]
            {
                "main 0.5",
            };

            System.IO.File.Create(filePath).Close();
            System.IO.File.WriteAllLines(filePath, config);

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }
    }
}
