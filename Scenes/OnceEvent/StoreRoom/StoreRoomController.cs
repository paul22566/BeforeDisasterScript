using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreRoomController : MonoBehaviour
{
    private PlayerData _PlayerData;
    //�D�v
    public GameObject Snake1;
    public GameObject Snake2;
    public Transform Snake1Point;
    public Transform Snake2Point;
    private float Snake1TimerSet = 10.1f;
    private float Snake2TimerSet = 17.8f;
    private float Snake1Timer;
    private float Snake2Timer;
    private float _fixedDeltaTime;
    //�}�����s
    public InteractableObject Button;
    public SideDoor LeftDoor;
    //�K�~
    private float DieTimer = 1.45f;
    public GameObject DrunkMan;
    public GameObject ItemLight1;//���
    public GameObject ItemLight2;//�D�L
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.Find("FollowSystem") != null)
        {
            _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        }

        if (GameEvent.OpenStoreRoomDoor)
        {
            Button.isValidable = false;
            Destroy(LeftDoor.gameObject);
        }

        if (GameEvent.DrunkManDie)
        {
            Destroy(DrunkMan);
            if (!itemManage.CheckDocumentExist(3))
            {
                ItemLight1.SetActive(true);
            }
        }

        if (itemManage.CheckDocumentExist(3))
        {
            Destroy(ItemLight1);
        }

        Button._interact += OpenDooor;

        Snake1Timer = Snake1TimerSet;
        Snake2Timer = Snake2TimerSet;
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        DrunkManItem();

        SnakeShadow();
    }

    private void SnakeShadow()
    {
        Snake1Timer -= _fixedDeltaTime;
        Snake2Timer -= _fixedDeltaTime;

        if (Snake1Timer <= 0)
        {
            Instantiate(Snake1, Snake1Point.position, Quaternion.identity);
            Snake1Timer = Snake1TimerSet;
        }
        if (Snake2Timer <= 0)
        {
            Instantiate(Snake2, Snake2Point.position, Quaternion.identity);
            Snake2Timer = Snake2TimerSet;
        }
    }

    private void DrunkManItem()
    {
        if (!itemManage.CheckDocumentExist(3) && GameEvent.DrunkManDie)
        {
            DieTimer -= _fixedDeltaTime;

            if (DieTimer <= 0)
            {
                ItemLight1.SetActive(true);
            }
        }
    }

    private void OpenDooor()
    {
        if (!GameEvent.OpenStoreRoomDoor)
        {
            GameEvent.OpenStoreRoomDoor = true;
            _PlayerData.CommonSave();
            LeftDoor.OpenDoor();
            Button.OnceTimeInteractSuccess();
        }
    }

    public void DrunkManSveData()
    {
        _PlayerData.CommonSave();
    }
}
