using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GraduallyShowWord : MonoBehaviour
{
    private Text _text;
    private string NowWord = "";
    private string TotalWord;
    private int NowShowNumber = 0;
    private int MaxWordNumber;
    public float Speed;
    [HideInInspector] public bool CanBegin;
    private bool isShowEnd;
    private float ShowWordTime;
    private float _time;
    private bool ShowWordTimeSet;
    public GraduallyShowWord NextLine;
    public float NextLineShowTime;

    public AudioClip ShowWordSound;
    private AudioSource ShowWordSource;
    // Start is called before the first frame update
    void Start()
    {
        if(this.gameObject.GetComponent<Text>() != null)
        {
            _text = this.gameObject.GetComponent<Text>();
            TotalWord = _text.text;
            _text.text = NowWord;
        }
        MaxWordNumber = TotalWord.Length;

        ShowWordSource = this.AddComponent<AudioSource>();
        ShowWordSource.clip = ShowWordSound;
    }

    private void Update()
    {
        ReShow();

        SEController.CalculateSystemSound(ShowWordSource);
    }

    private void FixedUpdate()
    {
        if (!CanBegin || isShowEnd)
        {
            return;
        }

        _time = Time.time;

        if (!ShowWordTimeSet)
        {
            ShowWordSource.Play();
            ShowWordTime = _time - Speed;
            ShowWordTimeSet = true;
        }

        ShowWord();

        if(NowShowNumber == MaxWordNumber)
        {
            ShowEnd();
        }
    }

    private void ShowWord()//(2)
    {
        if (NowShowNumber != MaxWordNumber && _time >= ShowWordTime + Speed)
        {
            NowWord = NowWord.Insert(NowShowNumber, TotalWord.Substring(NowShowNumber, 1));
            NowShowNumber += 1;
            ShowWordTime += Speed;
        }
        _text.text = NowWord;
    }

    private void ShowEnd()
    {
        if (NextLine != null)
        {
            if (_time >= ShowWordTime + NextLineShowTime)
            {
                isShowEnd = true;
                NextLine.CanBegin = true;
            }
        }
        else
        {
            isShowEnd = true;
        }
    }

    private void ReShow()//´ú¸Õ±M¥Î
    {
        if (!GameEvent.EnterGameWithNormalWay)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                NowWord = "";
                _text.text = NowWord;
                NowShowNumber = 0;
                ShowWordTimeSet = false;
                isShowEnd = false;
                CanBegin = false;
            }
        }
    }
}
