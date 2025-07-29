using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ObjectSEPlay : MonoBehaviour
{
    //此腳本適用於音效單純的prefab
    public AudioClip SE;
    private Transform _transform;
    public enum Type { FollowPlayer, FollowOther}
    public Type _type;
    public bool DieWithMainObject;
    public float ValidDistance;//0為不需要
    public float AppearTime;
    public bool LoopSE;
    private bool HasSEAppear;
    public GameObject AloneSE;
    private bool isSEPause;
    public bool isAloneSE;//是否是只做為音效存在
    private AudioSource SoundSource;
    private float PlayerDistance;

    [Header("PlayerSE")]
    private PlayerSEData _playerSEData = new PlayerSEData();
    public float Weight;

    // Start is called before the first frame update
    private void Start()
    {
        _transform = this.transform;
        if (!DieWithMainObject)
        {
            return;
        }
        SEController.inisializeAudioSource(ref SoundSource, SE, this.transform);
        if (LoopSE)
        {
            SoundSource.loop = true;
        }
        if(_type == Type.FollowPlayer)
        {
            _playerSEData.Source = SoundSource;
            _playerSEData.Weight = Weight;
            _playerSEData.Source.volume = SEController.SEVolume;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (!DieWithMainObject)
        {
            if (!HasSEAppear)
            {
                Instantiate(AloneSE, _transform.position, Quaternion.identity);
                HasSEAppear = true;
            }
            return;
        }

        if (_type == Type.FollowOther && ValidDistance != 0)
        {
            PlayerDistance = DistanceClass.CalculateAbsoluteDistance(_transform.position, new Vector3(PlayerController.PlayerPlaceX, PlayerController.PlayerPlaceY));
            if (SoundSource.isPlaying)
            {
                SoundSource.volume = SEController.FOVCalculate(PlayerDistance, ValidDistance);
            }
        }
        //聲音出現時機
        if (!HasSEAppear)
        {
            AppearTime -= Time.deltaTime;

            if (AppearTime <= 0)
            {
                HasSEAppear = true;
                switch (_type)
                {
                    case Type.FollowOther:
                        SoundSource.Play();
                        break;
                    case Type.FollowPlayer:
                        SEController.PlayPlayerSE(_playerSEData);
                        break;
                }
            }
        }

        SEPauseControll();

        if (HasSEAppear && _type == Type.FollowPlayer)
        {
            if (!SEController.CheckPlayerSEExist(_playerSEData))
            {
                Destroy(this.gameObject);
            }
        }
        if (HasSEAppear && _type == Type.FollowOther)
        {
            if (!SoundSource.isPlaying && isAloneSE)
            {
                Destroy(this.gameObject);
            }
        }
    }

    //暫停
    private void SEPauseControll()
    {
        if (!isSEPause)
        {
            if (PauseMenuController.isPauseMenuOpen || PlayerDistance > ValidDistance)
            {
                switch (_type)
                {
                    case Type.FollowPlayer:
                        _playerSEData.Source.Pause();
                        break;
                    case Type.FollowOther:
                        SoundSource.Pause();
                        break;
                }
                isSEPause = true;
            }
        }
        if (isSEPause)
        {
            if (!PauseMenuController.isPauseMenuOpen && PlayerDistance <= ValidDistance)
            {
                switch (_type)
                {
                    case Type.FollowPlayer:
                        _playerSEData.Source.UnPause();
                        break;
                    case Type.FollowOther:
                        SoundSource.UnPause();
                        break;
                }
                isSEPause = false;
            }
        }
    }
}
