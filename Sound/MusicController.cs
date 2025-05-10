using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class MusicController : MonoBehaviour
{
    private static bool HasOpenController;
    private float _deltaTime;

    public static float BGMVolume = 0.7f;//script(PauseMenuController，titlecontroller，playerData)
    private float SolidBGMVolume = 0;

    public AudioClip TitleMusic;
    public AudioClip BackGroundMusic1;//前半段
    public AudioClip BackGroundMusic2;//後半段
    public AudioClip UnderGroundMusic;
    public AudioClip Boss1Music;
    public AudioClip Boss2Music;
    public AudioClip Boss3Music;
    public AudioClip RestRoomMusic;
    public AudioClip ThirdTwoMusic;
    public AudioClip DripSound;
    public AudioClip WindSound;
    public AudioClip UnfullRadioSong;
    public AudioClip SecretRoomMusic;

    private AudioSource TitleMusicSource;
    private AudioSource BackGroundMusic1Source;//前半段
    private AudioSource BackGroundMusic2Source;//後半段
    private AudioSource UnderGroundMusicSource;
    private AudioSource Boss1MusicSource;
    private AudioSource Boss2MusicSource;
    private AudioSource Boss3MusicSource;
    private AudioSource RestRoomMusicSource;
    private AudioSource ThirdTwoMusicSource;
    private AudioSource DripSoundSource;
    private AudioSource WindSource;
    private AudioSource UnfullRadioSongSource;
    private AudioSource SecretRoomSource;

    private static int PlayNumber;
    public static bool isPlayNewBGM = true;//決定剛進新場景時是否播放新音樂，中途操作無須在意此開關
    public static float ChangeBGMVolumeTarget = 0;//進入新場景且只調整音量時用這個
    private static bool isFadeOutBGM;//script(backGroundSystem，RestRoomController，Boss1RoomController，Boss2RoomController，Boss3Controller，thirdFloor2Controller)
    private static bool isFadeInBGM;//script(Boss1RoomController，boss3controller)
    private static float ChangeSpeed = 1;
    private static float TargetVolume;
    // Start is called before the first frame update
    void Start()
    {
        if (HasOpenController)
        {
            Destroy(this.gameObject);
            return;
        }
        if (!HasOpenController)
        {
            HasOpenController = true;
        }
        
        InisializeBGM(ref TitleMusicSource, TitleMusic);
        InisializeBGM(ref BackGroundMusic1Source, BackGroundMusic1);
        InisializeBGM(ref BackGroundMusic2Source, BackGroundMusic2);
        InisializeBGM(ref UnderGroundMusicSource, UnderGroundMusic);
        InisializeBGM(ref Boss1MusicSource, Boss1Music);
        InisializeBGM(ref Boss2MusicSource, Boss2Music);
        InisializeBGM(ref Boss3MusicSource, Boss3Music);
        InisializeBGM(ref RestRoomMusicSource, RestRoomMusic);
        InisializeBGM(ref ThirdTwoMusicSource, ThirdTwoMusic);
        InisializeBGM(ref DripSoundSource, DripSound);
        InisializeBGM(ref WindSource, WindSound);
        InisializeBGM(ref UnfullRadioSongSource, UnfullRadioSong);
        InisializeBGM(ref SecretRoomSource, SecretRoomMusic);
    }

    // Update is called once per frame
    void Update()
    {
        _deltaTime = Time.deltaTime;

        switch (PlayNumber)
        {
            case 0:
                AllBGMFalse();
                break;
            case 1:
                BGMVolumePlay(BackGroundMusic1Source);
                break;
            case 2:
                BGMVolumePlay(BackGroundMusic2Source);
                break;
            case 3:
                BGMVolumePlay(UnderGroundMusicSource);
                break;
            case 4:
                BGMVolumePlay(Boss1MusicSource);
                break;
            case 5:
                BGMVolumePlay(Boss2MusicSource);
                break;
            case 6:
                BGMVolumePlay(Boss3MusicSource);
                break;
            case 7:
                BGMVolumePlay(RestRoomMusicSource);
                break;
            case 8:
                BGMVolumePlay(ThirdTwoMusicSource);
                break;
            case 9:
                BGMVolumePlay(DripSoundSource);
                break;
            case 10:
                BGMVolumePlay(WindSource);
                break;
            case 11:
                BGMVolumePlay(TitleMusicSource);
                break;
            case 12:
                BGMVolumePlay(UnfullRadioSongSource);
                break;
            case 13:
                BGMVolumePlay(SecretRoomSource);
                break;
        }
        if (isFadeInBGM)
        {
            BGMFadeIn(ChangeSpeed, TargetVolume);
        }
        if (isFadeOutBGM)
        {
            BGMFadeOut(ChangeSpeed, TargetVolume);
        }
    }

    private void InisializeBGM(ref AudioSource Source, AudioClip Clip)
    {
        Source = this.AddComponent<AudioSource>();

        Source.clip = Clip;
        Source.loop = true;
    }

    private void BGMVolumePlay(AudioSource Source)
    {
        if (!Source.isPlaying)
        {
            Source.Play();
        }
        Source.volume = SolidBGMVolume * BGMVolume;
    }//(2)

    public static void PlayBGM(int Number)
    {
        PlayNumber = Number;
    }

    public static void ChangeBGM()
    {
        isPlayNewBGM = true;
        BeginFadeOutBGM();
    }//下一張地圖是否要撥放新音樂

    public static void BeginFadeInBGM(float _changeSpeed, float _TargetVolume)
    {
        if (PlayNumber == 0)
        {
            isPlayNewBGM = false;
            return;
        }
        isFadeOutBGM = false;
        isFadeInBGM = true;
        ChangeSpeed = _changeSpeed;
        TargetVolume = _TargetVolume;
    }

    public static void BeginFadeInBGM()//預設版
    {
        if(PlayNumber == 0)
        {
            isPlayNewBGM = false;
            return;
        }
        isFadeOutBGM = false;
        isFadeInBGM = true;
        ChangeSpeed = 0.5f;
        TargetVolume = 1;
    }

    public static void BeginFadeOutBGM(float _changeSpeed, float _TargetVolume)
    {
        if (PlayNumber == 0)
        {
            return;
        }
        isFadeInBGM = false;
        isFadeOutBGM = true;
        ChangeSpeed = _changeSpeed;
        TargetVolume = _TargetVolume;
    }

    public static void BeginFadeOutBGM()//預設版
    {
        isFadeInBGM = false;
        isFadeOutBGM = true;
        ChangeSpeed = 1;
        TargetVolume = 0;
    }

    private void BGMFadeIn(float ChangeSpeed, float _targerVolume)
    {
        SolidBGMVolume += ChangeSpeed * _deltaTime;
        if(SolidBGMVolume >= _targerVolume)
        {
            ChangeBGMVolumeTarget = 0;
            SolidBGMVolume = _targerVolume;
            isPlayNewBGM = false;
            isFadeInBGM = false;
        }
    }

    private void BGMFadeOut(float ChangeSpeed, float _targerVolume)
    {
        SolidBGMVolume -= ChangeSpeed * _deltaTime;
        if (SolidBGMVolume <= _targerVolume)
        {
            ChangeBGMVolumeTarget = 0;
            SolidBGMVolume = _targerVolume;
            isFadeOutBGM = false;
        }
        if (SolidBGMVolume <= 0)
        {
            PlayNumber = 0;
        }
    }

    private void AllBGMFalse()
    {
        SolidBGMVolume = 0;
        if (TitleMusicSource.isPlaying)
        {
            TitleMusicSource.Stop();
        }
        if (BackGroundMusic1Source.isPlaying)
        {
            BackGroundMusic1Source.Stop();
        }
        if (BackGroundMusic2Source.isPlaying)
        {
            BackGroundMusic2Source.Stop();
        }
        if (UnderGroundMusicSource.isPlaying)
        {
            UnderGroundMusicSource.Stop();
        }
        if (Boss1MusicSource.isPlaying)
        {
            Boss1MusicSource.Stop();
        }
        if (Boss2MusicSource.isPlaying)
        {
            Boss2MusicSource.Stop();
        }
        if (Boss3MusicSource.isPlaying)
        {
            Boss3MusicSource.Stop();
        }
        if (RestRoomMusicSource.isPlaying)
        {
            RestRoomMusicSource.Stop();
        }
        if (ThirdTwoMusicSource.isPlaying)
        {
            ThirdTwoMusicSource.Stop();
        }
        if (DripSoundSource.isPlaying)
        {
            DripSoundSource.Stop();
        }
        if (WindSource.isPlaying)
        {
            WindSource.Stop();
        }
        if (UnfullRadioSongSource.isPlaying)
        {
            UnfullRadioSongSource.Stop();
        }
        if (SecretRoomSource.isPlaying)
        {
            SecretRoomSource.Stop();
        }
    }
}
