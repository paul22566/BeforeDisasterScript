using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D.Animation;
using System.IO;

public class CreatePlayer : MonoBehaviour
{
    private PlayerData _PlayerData;
    public TutorialWindow _tutorialWindow;
    public Transform PlayerNormalAniTransform;
    public Transform PlayerSpecialAni;
    public Transform PlayerCapturedAni;
    private int PlayerChildNumber;
    public GameObject SkillPowerFrame;
    public GameObject SkillPowerUi;
    public GameObject SkillPowerFrame2;
    public GameObject SkillPowerUi2;
    public GameObject SkillPoint4, SkillPoint5;
    public static bool isUseDebug;//其他script(backgroundsystem)
    public static bool isLoadGame;//其他script(backgroundsystem，CheckPoint，titleController)
    public static bool isNewGame;//其他script(backgroundsystem，CheckPoint，titleController)
    void Start()
    {
        _PlayerData = GameObject.Find("FollowSystem").GetComponent<PlayerData>();
        
        _PlayerData.AllReset();
        if (isNewGame)
        {
            CreateData(GameEvent.NowDataName);
        }
        if (isLoadGame)
        {
            LoadData(GameEvent.NowDataName);
        }

        if (GameEvent.AbsorbBoss2)
        {
            SkillPowerFrame.SetActive(false);
            SkillPowerFrame2.SetActive(true);
            SkillPowerUi.SetActive(false);
            SkillPowerUi2.SetActive(true);
            SkillPoint4.SetActive(true);
            SkillPoint5.SetActive(true);
        }

        //骨架spriteOrder初始化
        PlayerChildNumber = PlayerNormalAniTransform.childCount;

        for (int i = 0; i < PlayerChildNumber; i++)
        {
            if(PlayerNormalAniTransform.GetChild(i).childCount > 0)
            {
                PlayerSpriteInitialize(PlayerNormalAniTransform.GetChild(i).GetChild(0));
            }
        }
        //算有幾個specialAni
        PlayerChildNumber = PlayerSpecialAni.childCount;

        for (int i = 0; i < PlayerChildNumber; i++)
        {
            if (PlayerSpecialAni.GetChild(i).childCount > 0)
            {
                PlayerSpriteInitialize(PlayerSpecialAni.GetChild(i).GetChild(0));
            }
        }
        //算有幾個CapturedAni
        PlayerChildNumber = PlayerCapturedAni.childCount;

        for (int i = 0; i < PlayerChildNumber; i++)
        {
            if (PlayerCapturedAni.GetChild(i).childCount > 0)
            {
                PlayerSpriteInitialize(PlayerCapturedAni.GetChild(i).GetChild(0));
            }
        }

        //LoadScene
        if (BackgroundSystem.debugSceneName == null)
        {
            SceneManager.LoadScene("Loading");
        }
        else
        {
            SceneManager.LoadScene(BackgroundSystem.debugSceneName);
            isUseDebug = true;
            BackgroundSystem.debugSceneName = null;
        }
    }

    private void PlayerSpriteInitialize(Transform BoneTransform)
    {
        SpriteRenderer NowSprite;
        int SpriteNumber;

        SpriteNumber = BoneTransform.childCount;

        for (int i = 0; i < SpriteNumber; i++)
        {
            if(BoneTransform.GetChild(i).GetComponent<SpriteSkin>() != null)
            {
                NowSprite = BoneTransform.GetChild(i).GetComponent<SpriteRenderer>();
                NowSprite.sortingLayerName = "stage";
                NowSprite.sortingOrder += 20;
            }
        }
    }

    public void CreateData(string DataName)
    {
        _PlayerData.Save(DataName);
    }

    public void LoadData(string DataName)
    {
        string FilePath = Application.dataPath + "/StreamingAssets" + "/Save";
        string NameAndPath = "";
        NameAndPath = FilePath + "/" + DataName;
        if (File.Exists(NameAndPath))
        {
            _PlayerData.Load(DataName);
        }
    }
}
