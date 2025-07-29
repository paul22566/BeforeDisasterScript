using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Portal : MonoBehaviour
{
    public int pointNumber;//script(FirstFloorOneController)
    public string SceneName;//script(FirstFloorOneController)
    public static bool isPortal;//正在地圖移動
    public bool ChangeBGM;//如果原本沒有bgm就不用打勾 script(3f-1Controller，FirstFloorOneController)
    public float OnlyVolumeChange;//只有音量改變(不使用就0)
    private FadeOutUI FadeOut;
    public enum Face { Null, Right, Left };
    public  Face AssignFace;

    public bool isButtonPortal;
    private OldPlayerAnimationController _aniController;
    private InteractableObject _interactable;

    private enum PortalTarget { Loading, Restore, Title};
    [SerializeField] private PortalTarget Target;

    private void Start()
    {
        if (GameObject.Find("player") != null)
        {
            _aniController = GameObject.Find("player").GetComponent<OldPlayerAnimationController>();
        }
        else
        {
            return;
        }

        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas = GameObject.FindGameObjectWithTag("UI").transform;

            FadeOut = IdentifyID.FindObject(UICanvas, UIID.FadeOut).GetComponent<FadeOutUI>();
        }

        if (isButtonPortal)
        {
            _interactable = this.GetComponent<InteractableObject>();
            _interactable._interact += OnInteract;
        }
    }

    public void BeginChangeScene()
    {
        LoadScene.SceneName = SceneName;
        BackgroundSystem.startPointNumber = pointNumber;
        /*往上走時跳躍
         if (BackgroundSystem.startPointNumber >= 6)
        {
            Rigid2D.AddForce(new Vector2(0, ImpulsePowerY / 2), ForceMode2D.Force);
        }*/
        if (ChangeBGM)
        {
            MusicController.ChangeBGM();
        }
        if (OnlyVolumeChange != 0)
        {
            MusicController.ChangeBGMVolumeTarget = OnlyVolumeChange;
        }
        switch (AssignFace)
        {
            case Face.Null:
                switch (PlayerController._player.face)
                {
                    case Creature.Face.Right:
                        BackgroundSystem.PlayerAssignFace = BackgroundSystem.Face.Right;
                        break;
                    case Creature.Face.Left:
                        BackgroundSystem.PlayerAssignFace = BackgroundSystem.Face.Left;
                        break;
                }
                break;
            case Face.Left:
                BackgroundSystem.PlayerAssignFace = BackgroundSystem.Face.Left;
                break;
            case Face.Right:
                BackgroundSystem.PlayerAssignFace = BackgroundSystem.Face.Right;
                break;
        }
        FadeOut._fadeOutEnd += OnBeginLoadScene;
        FadeOut.BeginFadeOut();
        BackgroundSystem.CantPause = true;
        isPortal = true;
    }//開始移動並寫入移動條件

    public void OnBeginLoadScene()
    {
        switch (Target)
        {
            case PortalTarget.Loading:
                SceneManager.LoadScene("Loading");
                break;
            case PortalTarget.Restore:
                SceneManager.LoadScene("RestoreSpace");
                break;
            case PortalTarget.Title:
                PauseMenuController.isPauseMenuOpen = false;
                PauseMenuController.OpenAnyMenu = false;
                SceneManager.LoadScene("LoadingTitle");
                break;
        }
    }//處理不同狀況的FadeOut

    public void ChangePortalMap(string NewTarget)
    {
        SceneName = NewTarget;
    }

    private void OnInteract()
    {
        _aniController.WaitAniPlay();
        BeginChangeScene();
    }
}
