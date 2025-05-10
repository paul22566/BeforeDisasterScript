using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TutorialWindow : MonoBehaviour
{
    private Transform _transform;
    private PlayerData _playerData;

    public GameObject DefaultTutorial;

    private GameObject TutorialScrollbar;
    private Scrollbar _tutorialScrollbar;
    private GameObject TutorialShowArea;
    private GameObject TutorialShowAreaBottom;
    private RectTransform _tutorialShowAreaTransform;

    [HideInInspector] public bool isTutorialOpen;//script(pauseMenuController)
    [HideInInspector] public int TutorialWindowNumber = 999;//�w999���w�]�L���w
    [HideInInspector] public bool isTutorialButtonAppear;//script(pauseMenuController)
    [HideInInspector] public bool isTutorialDetailAppear;//script(pauseMenuController�ATutorialDetail)

    public static int TotalTutorialNumber = 6;
    //�ݦs���ܼ�
    public static int TutorialGettingNumber = 3;//script(playerData)
    public static List<bool> TutorialGetList = new List<bool>();
    public static List<bool> TutorialReadList = new List<bool>();

    //���Button
    public GameObject BasicKeyBoardButton;
    public GameObject AtkSystemButton;
    public GameObject BlockSystemButton;
    public GameObject ThrowSystemButton;
    public GameObject StrongThrowSystemButton;
    public GameObject BigGunSystemButton;

    //���C��ƧǬ��Ҧ��о�(�C����l�N�|�T�w)
    private List<TutorialPage> TutorialPageList = new List<TutorialPage>();

    [HideInInspector] public List<int> TutorialButtonOrderRecord = new List<int>();//script(TutorialButton�ApauseMenuController)

    public Transform Place1;
    public Transform Place2;

    [HideInInspector] public Vector3 OrderOnePlace;//script(TutorialButton)
    [HideInInspector] public Vector3 ButtonDistance;//script(TutorialButton)

    private float TenItemHeight = 54.696f;
    private float HeightDistance = 73;
    private float NowScrollbarDistance;//�{�b���פU�C���u�b���ʼƭ�
    private int NowUpLimit = 1;//��e���ﶵ���I
    private int NowDownLimit = 3;//��e���ﶵ�C�I
    private bool isLimitReset;

    private bool ShouldPlayMoveSound;
    private bool ShouldPlayConfirmSound;
    private bool ShouldLag;
    private float MoveLagTimerSet = 0.1f;
    private float MoveLagTimer;
    private float _unScaleDeltaTime;

    private bool ShouldSave;

    // Start is called before the first frame update
    void Start()
    {
        _transform = this.transform;
        _playerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        TutorialScrollbar = this.transform.GetChild(0).GetChild(1).gameObject;
        _tutorialScrollbar = TutorialScrollbar.GetComponent<Scrollbar>();
        TutorialShowArea = this.transform.GetChild(0).GetChild(0).gameObject;
        TutorialShowAreaBottom = TutorialShowArea.transform.GetChild(0).gameObject;
        _tutorialShowAreaTransform = TutorialShowAreaBottom.GetComponent<RectTransform>();

        MoveLagTimer = MoveLagTimerSet;

        InisializeTutorialPage();

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        _unScaleDeltaTime = Time.unscaledDeltaTime * BackgroundSystem.BasicGameSpeed;
        if (isTutorialDetailAppear)
        {
            return;
        }
        if (!isTutorialOpen)
        {
            if (TutorialButton.isDestroy)
            {
                this.gameObject.SetActive(false);
            }
            return;
        }

        TutorialButton.isDestroy = false;

        OrderOnePlace = Place1.localPosition;
        ButtonDistance = new Vector3(Place1.localPosition.x, Place2.localPosition.y - Place1.localPosition.y, 0);

        //�M�wsize
        if (TutorialGettingNumber > 10)
        {
            NowScrollbarDistance = 1 / (TutorialGettingNumber - 10);
            _tutorialShowAreaTransform.sizeDelta = new Vector2(_tutorialShowAreaTransform.sizeDelta.x, TenItemHeight + (TutorialGettingNumber - 10) * HeightDistance);
        }

        //���ͫ��s
        ButtonSet();

        //new��������
        CloseReadNotice();

        //���}�о�
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            OpenTutorialDetail(TutorialPageList[TutorialWindowNumber].Text);
        }
        //�ﶵ����
        if (TutorialButton.NowTutorialButton > 1 && !ShouldLag)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || XboxControllerDetect.isControllerUpPressed || XboxControllerDetect.isCrossUpPressed)
            {
                ShouldLag = true;
                ShouldPlayMoveSound = true;
                TutorialButton.NowTutorialButton -= 1;
            }
        }
        if (TutorialButton.NowTutorialButton < TutorialGettingNumber && !ShouldLag)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow) || XboxControllerDetect.isControllerDownPressed || XboxControllerDetect.isCrossDownPressed)
            {
                ShouldLag = true;
                ShouldPlayMoveSound = true;
                TutorialButton.NowTutorialButton += 1;
            }
        }

        if (ShouldPlayMoveSound)
        {
            SelectButtonController.ItemMoveSoundPlay();
            ShouldPlayMoveSound = false;
        }
        if (ShouldPlayConfirmSound)
        {
            SelectButtonController.ConfirmSoundPlay();
            ShouldPlayConfirmSound = false;
        }

        //�D��u�b
        if (TutorialGettingNumber > 10)
        {
            if (!isLimitReset)
            {
                _tutorialScrollbar.value = 1;
                NowUpLimit = 1;
                NowDownLimit = 10;
                isLimitReset = true;
            }
            if (TutorialButton.NowTutorialButton > NowDownLimit)
            {
                _tutorialScrollbar.value = 1 - NowScrollbarDistance * (TutorialButton.NowTutorialButton - 10);
                NowDownLimit = TutorialButton.NowTutorialButton;
                NowUpLimit = NowDownLimit - 9;
            }
            if (TutorialButton.NowTutorialButton < NowUpLimit)
            {
                _tutorialScrollbar.value = 1 - (NowUpLimit - 2) * NowScrollbarDistance;
                NowUpLimit -= 1;
                NowDownLimit = NowUpLimit + 9;
            }
        }
        else
        {
            _tutorialScrollbar.value = 1;
        }

        LagTimer();
    }

    private void InisializeTutorialPage()
    {
        for (int i = 0; i < TotalTutorialNumber; i++)
        {
            GameObject TemporyButton = new GameObject();
            switch (i)
            {
                case 0:
                    TemporyButton = BasicKeyBoardButton;
                    break;
                case 1:
                    TemporyButton = AtkSystemButton;
                    break;
                case 2:
                    TemporyButton = BlockSystemButton;
                    break;
                case 3:
                    TemporyButton = ThrowSystemButton;
                    break;
                case 4:
                    TemporyButton = StrongThrowSystemButton;
                    break;
                case 5:
                    TemporyButton = BigGunSystemButton;
                    break;
            }
            //i ����m�����T�꦳����b
            TutorialPageList.Add(new TutorialPage(TemporyButton, _transform.GetChild(1).GetChild(i).gameObject));
            //���wButtonOrder�M�M�w�O�_����ReadNotice
            if (TutorialPageList[i].Button.GetComponent<TutorialButton>() != null)
            {
                TutorialPageList[i].Button.GetComponent<TutorialButton>().ButtonOrder = i;
                if (TutorialReadList[i])
                {
                    TutorialPageList[i].Button.GetComponent<TutorialButton>().TurnOffReadNotice();
                }
            }
        }
    }

    private void ButtonSet()//�D����s���P�_�P�ͦ�(1)
    {
        if (!isTutorialButtonAppear)
        {
            int TutorialListOrder = 0;
            //�M�w�n��ܴX�ӹD��
            for (int i = 0; i < TutorialGettingNumber; i++)
            {
                TutorialButtonOrderRecord.Add(0);
            }
            //�n�O�Ǹ�
            for (int i = 0; i < TutorialGettingNumber; i++)
            {
                bool AssignSuccess = false;
                while (!AssignSuccess)
                {
                    if (TutorialGetList[TutorialListOrder])
                    {
                        TutorialButtonOrderRecord[i] = TutorialListOrder;
                        AssignSuccess = true;
                    }
                    TutorialListOrder++;
                }
            }
            //��ڲ��X
            for (int i = 0; i < TutorialGettingNumber; i++)
            {
                ProduceButton(TutorialPageList[TutorialButtonOrderRecord[i]].Button, i, TutorialShowAreaBottom);
            }
            isTutorialButtonAppear = true;
        }
    }

    private void ProduceButton(GameObject Button, int Order, GameObject ShowArea)
    {
        Button.GetComponent<TutorialButton>().NowOrder = Order;
        Instantiate(Button, _transform.position, Quaternion.identity, ShowArea.transform);
    }

    private void OpenTutorialDetail(GameObject Detail)
    {
        Detail.SetActive(true);
        ShouldPlayConfirmSound = true;
        isTutorialDetailAppear = true;
    }

    private void LagTimer()
    {
        if (ShouldLag)
        {
            MoveLagTimer -= _unScaleDeltaTime;
            if (MoveLagTimer <= 0)
            {
                ShouldLag = false;
                MoveLagTimer = MoveLagTimerSet;
            }
        }
    }

    private void OpenDefaultTutorial()
    {
        DefaultTutorial.SetActive(true);
    }

    private void CloseReadNotice()
    {
        if (TutorialWindowNumber != 999)
        {
            //�����s��Ϫ�New
            if (!TutorialReadList[TutorialWindowNumber])
            {
                ShouldSave = true;
                TutorialReadList[TutorialWindowNumber] = true;
                TutorialPageList[TutorialWindowNumber].Button.GetComponent<TutorialButton>().TurnOffReadNotice();
            }
        }
    }

    public static void TutorialListReset()
    {
        TutorialGettingNumber = 3;

        TutorialGetList.Clear();
        TutorialReadList.Clear();
        for (int i = 0; i < TotalTutorialNumber; i++)
        {
            TutorialGetList.Add(false);
            TutorialReadList.Add(false);
        }

        TutorialGetList[0] = true;
        TutorialGetList[1] = true;
        TutorialGetList[2] = true;

        TutorialReadList[0] = true;
    }

    public void SaveReadStatus()
    {
        if (ShouldSave)
        {
            _playerData.NoSignSave();
            ShouldSave = false;
        }
    }

    public void TutorialGet(int TutorialID)
    {
        TutorialGetList[TutorialID] = true;
        TutorialGettingNumber += 1;
        OpenDefaultTutorial();
    }//�����L�s�ɥ\��A���HItem�ߨ쪺�ܡA�qRoomController�ާ@
}

public struct TutorialPage
{
    public GameObject Button;
    public GameObject Text;

    public TutorialPage(GameObject button, GameObject text)
    {
        Button = button;
        Text = text;
    }
}
