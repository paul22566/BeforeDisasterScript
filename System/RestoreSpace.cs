using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestoreSpace : MonoBehaviour
{
    private GameObject playerObject;
    private PlayerController _playerController;
    private PlayerAnimationController _aniController;
    private GameObject DieUI;
    private GameObject FadeOutUI;
    private itemManage _itemManage;

    public GameObject BlackBackground;
    public GameObject WhiteBackground;
    // Start is called before the first frame update
    void Start()
    {
        if (GameObject.FindGameObjectWithTag("UI") != null)
        {
            Transform UICanvas;
            UICanvas = GameObject.FindGameObjectWithTag("UI").transform;
            DieUI = IdentifyID.FindObject(UICanvas, UIID.Die);
            FadeOutUI = IdentifyID.FindObject(UICanvas, UIID.FadeOut);
        }

        if (LoadScene.LoadSceneWithWhiteBackground)
        {
            BlackBackground.SetActive(false);
            WhiteBackground.SetActive(true);
        }

        DieUI.SetActive(false);
        FadeOutUI.SetActive(false);
        playerObject = GameObject.Find("player");
        _itemManage = GameObject.Find("FollowSystem").GetComponent<itemManage>();
        _playerController = playerObject.GetComponent<PlayerController>();
        _aniController = playerObject.GetComponent<PlayerAnimationController>();
        _aniController.ReSetAni();
        _playerController.Hp = playerObject.transform.GetComponent<PlayerController>().MaxHp;
        _playerController.isSaveGame = false;
        _playerController.CantDoAnyThing = false;
        _itemManage.RestoreUseItem();

        SceneManager.LoadScene("Loading");
    }
}
