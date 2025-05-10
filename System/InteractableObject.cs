using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [HideInInspector] public bool isValidable = true;

    public delegate void Interact();
    public event Interact _interact;

    [HideInInspector] public bool inRange;

    private Dialog _dialog;

    //重複性互動
    private bool isCoolDown;
    public float CooldownTimeSet;
    private float CooldownTime;
    private BoxCollider2D _boxCollider;

    public AudioClip OpenSuccessSound;
    public AudioClip OpenFailSound;

    private AudioSource OpenSuccessSource;
    private AudioSource OpenFailSource;

    private void Awake()
    {
        _dialog = this.GetComponent<Dialog>();
        _boxCollider = this.GetComponent<BoxCollider2D>();
    }
    private void Start()
    {
        SEController.inisializeAudioSource(ref OpenSuccessSource, OpenSuccessSound, this.transform);
        SEController.inisializeAudioSource(ref OpenFailSource, OpenFailSound, this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        SEController.CalculateSystemSound(OpenSuccessSource);
        SEController.CalculateSystemSound(OpenFailSource);

        if (!isValidable)
        {
            return;
        }

        if (inRange && PlayerController.CanInteract)
        {
            _interact();
            PlayerController.CanInteract = false;
        }

        if (isCoolDown)
        {
            CooldownTime -= Time.deltaTime;

            if (CooldownTime <= 0)
            {
                isCoolDown = false;
                _boxCollider.enabled = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isValidable)
        {
            inRange = true;
            _dialog.EnterPlayer(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player" && isValidable)
        {
            _dialog.ExitPlayer(collision);
            inRange = false;
        }
    }

    public void InteractSuccess()
    {
        _dialog.TurnOffDialog();
        isCoolDown = true;
        _boxCollider.enabled = false;
        CooldownTime = CooldownTimeSet;
        OpenSuccessSource.Play();
    }//重複性事件成功互動

    public void OnceTimeInteractSuccess()
    {
        _dialog.TurnOffDialog();
        isValidable = false;
        OpenSuccessSource.Play();
    }//一次性事件成功互動

    public void OnceTimeInteractFail()
    {
        OpenFailSource.Play();
    }//一次性事件失敗互動
}
