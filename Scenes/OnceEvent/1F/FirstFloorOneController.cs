using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstFloorOneController : MonoBehaviour
{
    public Portal Door;
    public GameObject ShortCutDoor;
    public InteractableObject ExplosionPoint;
    private PlayerData _playerData;

    private TutorialWindow _tutorialWindow;

    private void Awake()
    {
        if (GameEvent.PassBoss1)
        {
            Door.pointNumber = 1;
            Door.SceneName = "NightOutside";
            Door.ChangeBGM = true;
        }
        if (GameEvent.OpenOriginalDoor)
        {
            Destroy(ShortCutDoor);
        }
    }

    private void Start()
    {
        if (ExplosionPoint != null)
        {
            ExplosionPoint._interact += OnInteractItem;
        }
        if (GameObject.Find("FollowSystem") != null)
        {
            _playerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }

        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas;
            UICanvas = GameObject.FindGameObjectWithTag("UI").transform;
            _tutorialWindow = IdentifyID.FindObject(UICanvas, UIID.TutorialMenu).GetComponent<TutorialWindow>();
        }
    }

    private void OnInteractItem()
    {
        _tutorialWindow.TutorialGet(3);
    }
}
