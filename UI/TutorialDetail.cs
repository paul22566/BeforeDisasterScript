using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialDetail : MonoBehaviour
{
    private TutorialWindow _tutorialWindow;
    // Start is called before the first frame update
    void Start()
    {
        _tutorialWindow = this.transform.parent.parent.GetComponent<TutorialWindow>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton1))
        {
            _tutorialWindow.isTutorialDetailAppear = false;
            this.gameObject.SetActive(false);
        }
    }
}
