using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SEController : MonoBehaviour
{
    public static float SEVolume = 0.7f;//script(PauseMenuController，titlecontroller，playerData)
    private static List<PlayerSEData> PlayingPlayerSE = new List<PlayerSEData>();
    private const float ChangeSpeed = 0.8f;

    public static void PlayPlayerSE(PlayerSEData data)//(1)
    {
        PlayingPlayerSE.Add(data);
        PlayingPlayerSE.Sort((x, y) => -x.Weight.CompareTo(y.Weight));
        data.isPlaying = true;
        data.Source.Play();
        FPVCalculate(data);
    }

    public static void DetectPlayerSE()//(2)
    {
        if(PlayingPlayerSE.Count <= 0)
        {
            return;
        }

        for (int i = 0; i < PlayingPlayerSE.Count; i++)
        {
            if (PlayingPlayerSE[i].TargetVolume != PlayingPlayerSE[i].Source.volume)
            {
                if (PlayingPlayerSE[i].Source.volume < PlayingPlayerSE[i].TargetVolume)
                {
                    PlayingPlayerSE[i].Source.volume += ChangeSpeed * Time.deltaTime;
                    if(PlayingPlayerSE[i].Source.volume >= PlayingPlayerSE[i].TargetVolume)
                    {
                        PlayingPlayerSE[i].Source.volume = PlayingPlayerSE[i].TargetVolume;
                    }
                }
                if (PlayingPlayerSE[i].Source.volume > PlayingPlayerSE[i].TargetVolume)
                {
                    PlayingPlayerSE[i].Source.volume -= ChangeSpeed * Time.deltaTime;
                    if (PlayingPlayerSE[i].Source.volume <= PlayingPlayerSE[i].TargetVolume)
                    {
                        PlayingPlayerSE[i].Source.volume = PlayingPlayerSE[i].TargetVolume;
                    }
                }
            }

            if (PlayingPlayerSE[i].Source.isPlaying != PlayingPlayerSE[i].isPlaying)
            {
                PlayingPlayerSE[i].isPlaying = false;
                PlayingPlayerSE.RemoveAt(i);
            }
        }

        if (PlayingPlayerSE.Count <= 0)
        {
            return;
        }

        FPVCalculate();
    }

    public static bool CheckPlayerSEExist(PlayerSEData _playerSEData)
    {
        if (PlayingPlayerSE.Contains(_playerSEData))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ClearAllSE()//(1)****待修改
    {
        int ChildrenNumber = 0;

        PlayingPlayerSE.Clear();

        ChildrenNumber = this.transform.childCount;
        for (int i = 0; i < ChildrenNumber; i++)
        {
            Destroy(transform.GetChild(ChildrenNumber).gameObject);
        }
    }

    public static float FOVCalculate(float Distance, float ValidDistance)//FollowObject音量計算(2)
    {
        float Volume = 0;

        if (Distance > ValidDistance)
        {
            Volume = 0;
        }
        if(Distance <= ValidDistance)
        {
            Volume = Distance / -ValidDistance + 1;
        }
        return Volume * SEVolume;
    }

    private static void FPVCalculate()//FollowPlayer音量計算(2)
    {
        float CanAssignNumber = 1;
        int CalculatePlace = 0;
        float NowWeight = 0;
        int NowWeightNumber = 0;
        bool Continue = false;

        if (PlayingPlayerSE.Count == 0)
        {
            return;
        }

        do
        {
            Continue = false;

            NowWeight = PlayingPlayerSE[CalculatePlace].Weight;

            //檢查權重相同個數 && 檢查是否有下一輪
            PlayingPlayerSE.ForEach(delegate (PlayerSEData data)
            {
                if (NowWeight == data.Weight)
                {
                    NowWeightNumber += 1;
                }
                if (NowWeight > data.Weight)
                {
                    Continue = true;
                }
            });
            if (Continue)
            {
                PlayingPlayerSE.ForEach(delegate (PlayerSEData data)
                {
                    if (NowWeight == data.Weight)
                    {
                        data.TargetVolume = (CanAssignNumber * NowWeight) / NowWeightNumber * SEVolume;
                    }
                });
                CalculatePlace += NowWeightNumber;
                CanAssignNumber -= NowWeight * CanAssignNumber;
                NowWeightNumber = 0;
            }
        } while (Continue);

        PlayingPlayerSE.ForEach(delegate (PlayerSEData data)
        {
            if (NowWeight == data.Weight)
            {
                data.TargetVolume = CanAssignNumber / NowWeightNumber * SEVolume;
            }
        });
    }

    private static void FPVCalculate(PlayerSEData _playerSEData)//FollowPlayer音量計算 新建立音檔專用(1)
    {
        float CanAssignNumber = 1;
        int CalculatePlace = 0;
        float NowWeight = 0;
        int NowWeightNumber = 0;
        bool Continue = false;

        if (PlayingPlayerSE.Count == 0)
        {
            return;
        }

        do
        {
            Continue = false;

            NowWeight = PlayingPlayerSE[CalculatePlace].Weight;

            //檢查權重相同個數 && 檢查是否有下一輪
            PlayingPlayerSE.ForEach(delegate (PlayerSEData data)
            {
                if (NowWeight == data.Weight)
                {
                    NowWeightNumber += 1;
                }
                if (NowWeight > data.Weight)
                {
                    Continue = true;
                }
            });
            if (Continue)
            {
                PlayingPlayerSE.ForEach(delegate (PlayerSEData data)
                {
                    if (NowWeight == data.Weight)
                    {
                        if (data.Source.clip.name == _playerSEData.Source.clip.name)
                        {
                            data.Source.volume = (CanAssignNumber * NowWeight) / NowWeightNumber * SEVolume;
                        }
                        else
                        {
                            data.TargetVolume = (CanAssignNumber * NowWeight) / NowWeightNumber * SEVolume;
                        }
                    }
                });
                CalculatePlace += NowWeightNumber;
                CanAssignNumber -= NowWeight * CanAssignNumber;
                NowWeightNumber = 0;
            }
        } while (Continue);

        PlayingPlayerSE.ForEach(delegate (PlayerSEData data)
        {
            if (NowWeight == data.Weight)
            {
                if (data.Source.clip.name == _playerSEData.Source.clip.name)
                {
                    data.Source.volume = CanAssignNumber / NowWeightNumber * SEVolume;
                }
                else
                {
                    data.TargetVolume = CanAssignNumber / NowWeightNumber * SEVolume;
                }
            }
        });
    }

    public static float VolumeFadeIn(float NowVolumeRate, float Time, float DeltaTime)
    {
        NowVolumeRate = NowVolumeRate + (1 / Time) * DeltaTime;
        return NowVolumeRate;
    }//音量淡入(2)

    public static float VolumeFadeOut(float NowVolumeRate, float Time, float DeltaTime)
    {
        NowVolumeRate = NowVolumeRate - (1 / Time) * DeltaTime;
        return NowVolumeRate;
    }//音量淡出(2)

    public static float VolumeFadeInAndOut(ref bool TurnOnBool,ref bool TurnOffBool, float Rate, float Time, float DeltaTime)
    {
        if (TurnOnBool)
        {
            Rate = VolumeFadeIn(Rate, Time, DeltaTime);
            if (Rate >= 1)
            {
                Rate = 1;
                TurnOnBool = false;
            }
        }
        if (TurnOffBool)
        {
            Rate = VolumeFadeOut(Rate, Time, DeltaTime);
            if (Rate <= 0)
            {
                Rate = 0;
                TurnOffBool = false;
            }
        }
        return Rate;
    }//淡入淡出統合

    public static void CalculateSystemSound(AudioSource Source)
    {
        if (Source.isPlaying)
        {
            Source.volume = SEVolume;
        }
    }

    public static void CalculateSystemSound(AudioSource Source, float Rate)//方便調整動畫音效音量
    {
        if (Source.isPlaying)
        {
            Source.volume = SEVolume * Rate;
        }
    }

    public static void inisializeAudioSource(ref AudioSource Source, AudioClip Clip, Transform _transform)
    {
        Source = _transform.AddComponent<AudioSource>();
        Source.clip = Clip;
        Source.playOnAwake = false;
    }
}
