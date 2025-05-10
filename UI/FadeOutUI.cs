using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FadeOutUI : MonoBehaviour
{
    private float DefaultFadeOutTimeSet = 1.5f;
    private float FadeOutTime;
    private float AniSpeed;
    private Animator FadeOutAni;

    public delegate void FadeOutEnd();
    public FadeOutEnd _fadeOutEnd;
    // Start is called before the first frame update
    void Start()
    {
        FadeOutAni = this.GetComponent<Animator>();
        FadeOutTime = DefaultFadeOutTimeSet;
        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        FadeOutTime -= Time.deltaTime;
        if (FadeOutTime <= 0 && _fadeOutEnd!=null)
        {
            _fadeOutEnd();
            FadeOutTime = DefaultFadeOutTimeSet;
        }
    }

    public void BeginFadeOut()
    {
        this.gameObject.SetActive(true);
        AniSpeed = 1 / (DefaultFadeOutTimeSet - 0.5f);
        FadeOutAni.speed = AniSpeed;

        FadeOutTime = DefaultFadeOutTimeSet;
    }
    public void BeginFadeOut(float _fadeOutTime)
    {
        this.gameObject.SetActive(true);
        AniSpeed = 1 / (_fadeOutTime - 0.5f);
        FadeOutAni.speed = AniSpeed;

        FadeOutTime = _fadeOutTime;
    }
}
