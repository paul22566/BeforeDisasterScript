using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvincibleAni : MonoBehaviour
{
    private int SprListNumber = 15;
    public float InvincibleTimerSet;
    private float InvincibleTimer;
    private PlayerController _playerController;
    private PlayerAnimationController _aniController;
    private SpriteRenderer[] MoveSpr = new SpriteRenderer[15];
    private SpriteRenderer[] AtkSpr = new SpriteRenderer[15];
    private SpriteRenderer[] JumpSpr = new SpriteRenderer[15];
    private SpriteRenderer[] RestoreSpr = new SpriteRenderer[15];
    private SpriteRenderer[] CocktailCriticAtkSpr = new SpriteRenderer[15];
    private SpriteRenderer[] CAtkSpr = new SpriteRenderer[15];
    private SpriteRenderer[] ThrowSpr = new SpriteRenderer[15];
    private SpriteRenderer[] ShootSpr = new SpriteRenderer[15];
    private SpriteRenderer[] BlockSpr = new SpriteRenderer[15];
    private SpriteRenderer[] SecondJumpSpr = new SpriteRenderer[15];
    private SpriteRenderer[] SecondAtkSpr = new SpriteRenderer[15];
    private SpriteRenderer[] ImpulseJumpSpr = new SpriteRenderer[15];

    // Start is called before the first frame update
    void Start()
    {
        SprListInitialize(MoveSpr, 0);
        SprListInitialize(AtkSpr, 4);
        SprListInitialize(JumpSpr, 1);
        SprListInitialize(RestoreSpr, 14);
        SprListInitialize(CocktailCriticAtkSpr, 16);
        SprListInitialize(CAtkSpr, 6);
        SprListInitialize(ThrowSpr, 8);
        SprListInitialize(ShootSpr, 9);
        SprListInitialize(BlockSpr, 10);
        SprListInitialize(SecondJumpSpr, 2);
        SprListInitialize(SecondAtkSpr, 5);
        SprListInitialize(ImpulseJumpSpr, 15);
        _playerController = this.GetComponent<PlayerController>();
        _aniController = this.GetComponent<PlayerAnimationController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerController.HurtedInvincible)
        {
            if (InvincibleTimer <= 0)
            {
                for (int i = 0; i < SprListNumber; i++)
                {
                    switch (_aniController.NowAni)
                    {
                        case PlayerAnimationController.AniStatus.Wait:
                            MoveSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Walk:
                            MoveSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.WalkThrow:
                            MoveSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Atk:
                            AtkSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Jump:
                            JumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Fall:
                            JumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.JumpAtk:
                            JumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.JumpCAtk:
                            JumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.JumpThrow:
                            JumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Restore:
                            RestoreSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.CocktailCriticAtk:
                            CocktailCriticAtkSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.CAtk:
                            CAtkSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Throw:
                            ThrowSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.ShootWait:
                            ShootSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Shoot:
                            ShootSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.JumpShoot:
                            ShootSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Block:
                            BlockSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.SecondJump:
                            SecondJumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.SecondAtk:
                            SecondAtkSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.ImpulseJump:
                            ImpulseJumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                    }
                }
                InvincibleTimer = InvincibleTimerSet;
            }

            InvincibleTimer -= Time.deltaTime;

            if (InvincibleTimer <= 0.1)
            {
                for (int i = 0; i < SprListNumber; i++)
                {
                    switch (_aniController.NowAni)
                    {
                        case PlayerAnimationController.AniStatus.Wait:
                            MoveSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.Walk:
                            MoveSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.WalkThrow:
                            MoveSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.Atk:
                            AtkSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.Jump:
                            JumpSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.Fall:
                            JumpSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.JumpAtk:
                            JumpSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.JumpCAtk:
                            JumpSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.JumpThrow:
                            JumpSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.Restore:
                            RestoreSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.CocktailCriticAtk:
                            CocktailCriticAtkSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.CAtk:
                            CAtkSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.Throw:
                            ThrowSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.ShootWait:
                            ShootSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.Shoot:
                            ShootSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.JumpShoot:
                            ShootSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.Block:
                            BlockSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.SecondJump:
                            SecondJumpSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.SecondAtk:
                            SecondAtkSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                        case PlayerAnimationController.AniStatus.ImpulseJump:
                            ImpulseJumpSpr[i].color = new Color(1, 1, 1, 1);
                            break;
                    }
                }
            }
            if (InvincibleTimer <= 0)
            {
                for (int i = 0; i < SprListNumber; i++)
                {
                    switch (_aniController.NowAni)
                    {
                        case PlayerAnimationController.AniStatus.Wait:
                            MoveSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Walk:
                            MoveSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.WalkThrow:
                            MoveSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Atk:
                            AtkSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Jump:
                            JumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Fall:
                            JumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.JumpAtk:
                            JumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.JumpCAtk:
                            JumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.JumpThrow:
                            JumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Restore:
                            RestoreSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.CocktailCriticAtk:
                            CocktailCriticAtkSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.CAtk:
                            CAtkSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Throw:
                            ThrowSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.ShootWait:
                            ShootSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Shoot:
                            ShootSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.JumpShoot:
                            ShootSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.Block:
                            BlockSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.SecondJump:
                            SecondJumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.SecondAtk:
                            SecondAtkSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                        case PlayerAnimationController.AniStatus.ImpulseJump:
                            ImpulseJumpSpr[i].color = new Color(1, 1, 1, 0.5f);
                            break;
                    }
                }
                InvincibleTimer = InvincibleTimerSet;
            }
        }
        else
        {
            for (int i = 0; i < SprListNumber; i++)
            {
                switch (_aniController.NowAni)
                {
                    case PlayerAnimationController.AniStatus.Wait:
                        MoveSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.Walk:
                        MoveSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.WalkThrow:
                        MoveSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.Atk:
                        AtkSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.Jump:
                        JumpSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.Fall:
                        JumpSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.JumpAtk:
                        JumpSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.JumpCAtk:
                        JumpSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.JumpThrow:
                        JumpSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.Restore:
                        RestoreSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.CocktailCriticAtk:
                        CocktailCriticAtkSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.CAtk:
                        CAtkSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.Throw:
                        ThrowSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.ShootWait:
                        ShootSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.Shoot:
                        ShootSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.JumpShoot:
                        ShootSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.Block:
                        BlockSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.SecondJump:
                        SecondJumpSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.SecondAtk:
                        SecondAtkSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                    case PlayerAnimationController.AniStatus.ImpulseJump:
                        ImpulseJumpSpr[i].color = new Color(1, 1, 1, 1);
                        break;
                }
            }
        }
    }

    private void SprListInitialize(SpriteRenderer[] Target, int ChildNumber)
    {
        for (int i = 0; i < SprListNumber; i++)
        {
            Target[i] = transform.GetChild(0).GetChild(ChildNumber).transform.GetChild(0).transform.GetChild(i).GetComponent<SpriteRenderer>();
        }
    } 
}
