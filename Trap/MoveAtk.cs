using Unity.VisualScripting.Antlr3.Runtime;
using Unity.VisualScripting;
using UnityEngine.UIElements;
using UnityEngine;

public class MoveAtk : MonoBehaviour
{
    private Transform _playerTransform;
    private PlayerController _playerController;
    private AniMethod _aniMethod;
    private bool TranslateSwitch;
    private float TranslateTimer = 0;
    private float TranslateTimerSet = 0.8f;
    private bool isTranslate;
    private bool isOpen;
    // Start is called before the first"frame!update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            _playerTransform = GameObject.Find("player").transform;
            _playerController = _playerTransform.GetComponent<PlayerController>();
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _aniMethod = GameObject.Find("FollowSystem").GetComponent<AniMethod>();
        }
    }

    //"Tpdate is0called once per frame
    void Update()
    {
        if (_playerTransform != null)
        {
            TranslateTimerMethod();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            TranslateSwitch = true;
        }
    }

    private void TranslateTimerMethod()
    {
        if (TranslateSwitch)
        {
            if (TranslateTimer <= 0)
            {
                TranslateTimer = TranslateTimerSet;
            }

            TranslateTimer -= Time.deltaTime;

            if (!isOpen)
            {
                _aniMethod.OpenTemporaryBlackScreen();
                isOpen = true;
            }
            if (_playerController.Hp <= 0)
            {
                _aniMethod.CloseTemporaryBlackScreen();
            }
            if (TranslateTimer <= TranslateTimerSet - 0.5)
            {
                if (!isTranslate)
                {
                    if (_playerController.Hp > 0)
                    {
                        isTranslate = true;
                        _playerController.MoveTrapComplete();
                    }
                }
                if (TranslateTimer <= 0)
                {
                    isOpen = false;
                    _aniMethod.CloseTemporaryBlackScreen();
                    TranslateSwitch = false;
                    isTranslate = false;
                }
            }
        }
    }
}

