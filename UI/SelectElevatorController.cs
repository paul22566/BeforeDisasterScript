using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.UI;

public class SelectElevatorController : MonoBehaviour
{
    [HideInInspector] public bool isSelectElevatorMenuOpen;

    private Button ThirdFloorButton;

    [HideInInspector] public bool isTurnOff3F;
    // Start is called before the first frame update
    void Start()
    {
        ThirdFloorButton = this.transform.GetChild(1).GetComponent<Button>();
        if (GameEvent.GoIn3F1)
        {
            TurnOff3FButton();
        }

        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (isSelectElevatorMenuOpen)
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1) || PlayerController.isHurted)
            {
                CloseSelectElevatorMenu();
                BackgroundSystem.CantPause = false;
            }
        }
    }
    
    public void OpenSelectElevatorMenu(int NowFloor)
    {
        this.gameObject.SetActive(true);
        isSelectElevatorMenuOpen = true;
        PauseMenuController.OpenAnyMenu = true;
        BackgroundSystem.CantPause = true;
        SelectButtonController.OpenSelectButtonController();
        this.GetComponent<DefaultButton>().onStart = this.transform.GetChild(4 - NowFloor).GetComponent<Button>();
        this.GetComponent<DefaultButton>().ShouldOpen = true;
    }

    public void CloseSelectElevatorMenu()
    {
        isSelectElevatorMenuOpen = false;
        this.gameObject.SetActive(false);
        PauseMenuController.OpenAnyMenu = false;
        SelectButtonController.CloseSelectButtonController();
    }//cantPause要在其他地方關掉

    public void TurnOff3FButton()
    {
        isTurnOff3F = true;
        SelectButtonController.LockButton(1, 2, "L", "U");
        ThirdFloorButton.interactable = false;
        ThirdFloorButton.GetComponent<Image>().color = new Color(0.25f, 0.25f, 0.25f, 1);
    }
}
