using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SameScenePortal : MonoBehaviour
{
    private Transform _playerTransform;
    private InteractableObject _interactableObject;
    private AniMethod _aniMethod;

    public Transform TargetPoint;
    private bool isMoving;
    private bool isTranslate;
    private float MoveTimer;
    private float MoveTimerSet = 0.8f;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("player") != null)
        {
            _playerTransform = GameObject.Find("player").transform;
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _aniMethod = GameObject.Find("FollowSystem").GetComponent<AniMethod>();
        }

        _interactableObject = this.GetComponent<InteractableObject>();

        _interactableObject._interact += OnInteract;
    }

    private void FixedUpdate()
    {
        if (_playerTransform == null)
        {
            return;
        }

        if (isMoving)
        {
            MoveTimer -= Time.fixedDeltaTime;

            if (MoveTimer <= MoveTimerSet - 0.5f)
            {
                if(!isTranslate)
                {
                    _playerTransform.position = TargetPoint.position;
                    Portal.isPortal = false;
                    isTranslate = true;
                }
            }

            if (MoveTimer <= 0)
            {
                isTranslate = false;
                isMoving = false;
                _aniMethod.CloseTemporaryBlackScreen();
            }
        }
    }

    private void OnInteract()
    {
        Portal.isPortal = true;
        isMoving = true;
        MoveTimer = MoveTimerSet;
        _aniMethod.OpenTemporaryBlackScreen();
    }
}
