using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitileManager : MonoBehaviour
{
    [SerializeField] GameObject titleEffect;
    [SerializeField] GameObject titleUI;
    [SerializeField] GameObject howToPlay;
    [SerializeField] GameObject credits;
    [SerializeField] GameObject mainCamera;

    Animator animator;
    AudioSource audioSource;
    AudioSource howToPlayAudioSource;
    [SerializeField] AudioClip buttonSound;
    [SerializeField] AudioClip startSound;
    [SerializeField] AudioClip howToPlayToTitleSound;
    [SerializeField] AudioClip howToPlayBGM;

    bool onHowToPlay;
    bool onCredits;
    public bool effectEnd = false;
    public bool canControl = false;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        howToPlayAudioSource = howToPlay.GetComponent<AudioSource>();
        //自動再生及び繰り返しを防止
        if (audioSource && howToPlayAudioSource) 
        {
            howToPlayAudioSource.playOnAwake = false;
            howToPlayAudioSource.loop = false;
            audioSource.playOnAwake = false;
            audioSource.loop = true;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        titleUI.SetActive(false);
        onHowToPlay = false;
        onCredits = false;
        howToPlay.SetActive(false);
        credits.SetActive(false);
        
        animator.enabled = true;
    }
    private void Update()
    {
        animator.SetBool("EffectEnd", effectEnd);
        ControlSettings();
    }

    void ControlSettings()
    {
        if (canControl && Input.GetKeyDown(KeyCode.Space) && !onCredits) 
        {
            onHowToPlay = !onHowToPlay;
            ShowHowToControl();
            Debug.Log("onHowToControl: " + onHowToPlay);
        }

        if (canControl && Input.GetKeyDown(KeyCode.C) && !onHowToPlay)
        {
            onCredits = !onCredits;
            ShowCredits();
            Debug.Log("onCredits: " + onCredits);
        }

        if (canControl && Input.GetKeyDown(KeyCode.Return) && !onCredits && !onHowToPlay)
        {
            StartCoroutine(WaitForStartSoundEnd());
        }
    }

    void ShowHowToControl() 
    {
        
        Debug.Log("ShowHowToControl - titleUI active: " + titleUI.activeSelf);
        if (onHowToPlay)
        {
            //audioSource.PlayOneShot(buttonSound);
            
            StartCoroutine(WaitForButtonSound());

        }
        else if(!onHowToPlay)
        {
            audioSource.PlayOneShot(howToPlayToTitleSound);
            howToPlay.SetActive(false);
            titleUI.SetActive(true);
            audioSource.Play();
        }
    }
    void ShowCredits()
    {
        audioSource.PlayOneShot(buttonSound);
        if (onCredits)
        {
            titleUI.SetActive(false);
            credits.SetActive(true);
        }
        else if(!onCredits)
        {
            credits.SetActive(false);
            titleUI.SetActive(true);
        }
    }
    IEnumerator WaitForStartSoundEnd()
    {
        audioSource.PlayOneShot(startSound);
        yield return new WaitForSeconds(startSound.length);
        SceneManager.LoadScene("Stage1");
    }

    IEnumerator WaitForButtonSound()
    {
        audioSource.PlayOneShot(buttonSound);
        yield return new WaitForSeconds(buttonSound.length);
        titleUI.SetActive(false);
        howToPlay.SetActive(true);
        audioSource.Stop();
        howToPlayAudioSource.Play();
    }

    //アニメーションイベントで起動させています。
    void OnTitleEffectEnd()
    {
        effectEnd = true;
        titleUI.SetActive(true);
        audioSource.Play();
        Debug.Log(effectEnd);
    }

    void ActiveControl() 
    {
        canControl = true;
    }
}
