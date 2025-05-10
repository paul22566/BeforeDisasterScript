using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicJudgement : MonoBehaviour
{
    public int Number;//NoMusic 0 ¡ABGM1 1¡A BGM2 2¡AUM 3¡ABM1 4¡ABM2 5¡ABM3 6¡ARM 7¡ATF2M 8¡ADrip 9¡Awind 10¡ATitle 11¡AUnfullRidioSong 12¡ASecretRoom 13
    //script(musiccontroller¡ARestRoomController)
    private void Start()
    {
        if (MusicController.isPlayNewBGM)
        {
            MusicController.PlayBGM(Number);
            MusicController.BeginFadeInBGM();
        }
        if (MusicController.ChangeBGMVolumeTarget != 0)
        {
            if (MusicController.ChangeBGMVolumeTarget == 1)
            {
                MusicController.BeginFadeInBGM();
            }
            else
            {
                MusicController.BeginFadeOutBGM(1, MusicController.ChangeBGMVolumeTarget);
            }
        }
    }
}
