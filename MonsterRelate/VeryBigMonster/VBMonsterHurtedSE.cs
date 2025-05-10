using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VBMonsterHurtedSE : MonoBehaviour
{
    private VeryBigMonsterController _controller;
    private MonsterBasicData _basicData;

    private CollisionType RightUpArmCollision;
    private CollisionType DownBodyCollision;
    private FollowObject RightUpArmPoint;
    private FollowRotate RightUpArmRotate;
    private FollowObject DownBodyPoint;
    private FollowRotate DownBodyRotate;

    public Transform WaitRightUpArm;
    public Transform WalkRightUpArm;
    public Transform JumpRightUpArm;
    public Transform AtkRightUpArm;
    public Transform Atk1_5RightUpArm;
    public Transform Atk2RightUpArm;
    public Transform Atk3RightUpArm;
    public Transform Atk4RightUpArm;
    public Transform SummonRightUpArm;
    public Transform CaptureRightUpArm;
    public Transform WeakRightUpArm;

    public Transform WaitDownBody;
    public Transform WalkDownBody;
    public Transform JumpDownBody;
    public Transform AtkDownBody;
    public Transform Atk1_5DownBody;
    public Transform Atk2DownBody;
    public Transform Atk3DownBody;
    public Transform Atk4DownBody;
    public Transform SummonDownBody;
    public Transform CaptureDownBody;
    public Transform WeakDownBody;
    // Start is called before the first frame update
    void Start()
    {
        _controller = this.transform.parent.GetComponent<VeryBigMonsterController>();
        _basicData = this.transform.parent.GetComponent<MonsterBasicData>();

        RightUpArmCollision = this.transform.GetChild(0).GetComponent<CollisionType>();
        DownBodyCollision = this.transform.GetChild(1).GetComponent<CollisionType>();
        RightUpArmPoint = this.transform.GetChild(0).GetComponent<FollowObject>();
        RightUpArmRotate = this.transform.GetChild(0).GetComponent<FollowRotate>();
        DownBodyPoint = this.transform.GetChild(1).GetComponent<FollowObject>();
        DownBodyRotate = this.transform.GetChild(1).GetComponent<FollowRotate>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_controller.isSecondPhase)
        {
            RightUpArmCollision._type = CollisionType.Type.Skin;
            DownBodyCollision._type = CollisionType.Type.Skin;
        }
        else
        {
            RightUpArmCollision._type = CollisionType.Type.Metal;
            DownBodyCollision._type = CollisionType.Type.Metal;
        }

        switch (_basicData.face)
        {
            case MonsterBasicData.Face.Right:
                RightUpArmRotate.FixedAngle = 0;
                DownBodyRotate.FixedAngle = 0;
                break;
            case MonsterBasicData.Face.Left:
                RightUpArmRotate.FixedAngle = 180;
                DownBodyRotate.FixedAngle = 180;
                break;
        }

        switch (_controller.NowAni)
        {
            case VeryBigMonsterController.AniStatus.wait:
                RightUpArmPoint.Target = WaitRightUpArm;
                RightUpArmRotate.Target = WaitRightUpArm;
                DownBodyPoint.Target = WaitDownBody;
                DownBodyRotate.Target = WaitDownBody;
                break;
            case VeryBigMonsterController.AniStatus.walk:
                RightUpArmPoint.Target = WalkRightUpArm;
                RightUpArmRotate.Target = WalkRightUpArm;
                DownBodyPoint.Target = WalkDownBody;
                DownBodyRotate.Target = WalkDownBody;
                break;
            case VeryBigMonsterController.AniStatus.Dash:
                RightUpArmPoint.Target = WalkRightUpArm;
                RightUpArmRotate.Target = WalkRightUpArm;
                DownBodyPoint.Target = WalkDownBody;
                DownBodyRotate.Target = WalkDownBody;
                break;
            case VeryBigMonsterController.AniStatus.Jump:
                RightUpArmPoint.Target = JumpRightUpArm;
                RightUpArmRotate.Target = JumpRightUpArm;
                DownBodyPoint.Target = JumpDownBody;
                DownBodyRotate.Target = JumpDownBody;
                break;
            case VeryBigMonsterController.AniStatus.Atk1:
                RightUpArmPoint.Target = AtkRightUpArm;
                RightUpArmRotate.Target = AtkRightUpArm;
                DownBodyPoint.Target = AtkDownBody;
                DownBodyRotate.Target = AtkDownBody;
                break;
            case VeryBigMonsterController.AniStatus.Atk1_5:
                RightUpArmPoint.Target = Atk1_5RightUpArm;
                RightUpArmRotate.Target = Atk1_5RightUpArm;
                DownBodyPoint.Target = Atk1_5DownBody;
                DownBodyRotate.Target = Atk1_5DownBody;
                break;
            case VeryBigMonsterController.AniStatus.Atk2:
                RightUpArmPoint.Target = Atk2RightUpArm;
                RightUpArmRotate.Target = Atk2RightUpArm;
                DownBodyPoint.Target = Atk2DownBody;
                DownBodyRotate.Target = Atk2DownBody;
                break;
            case VeryBigMonsterController.AniStatus.Atk3:
                RightUpArmPoint.Target = Atk3RightUpArm;
                RightUpArmRotate.Target = Atk3RightUpArm;
                DownBodyPoint.Target = Atk3DownBody;
                DownBodyRotate.Target = Atk3DownBody;
                break;
            case VeryBigMonsterController.AniStatus.Atk4:
                RightUpArmPoint.Target = Atk4RightUpArm;
                RightUpArmRotate.Target = Atk4RightUpArm;
                DownBodyPoint.Target = Atk4DownBody;
                DownBodyRotate.Target = Atk4DownBody;
                break;
            case VeryBigMonsterController.AniStatus.Summon:
                RightUpArmPoint.Target = SummonRightUpArm;
                RightUpArmRotate.Target = SummonRightUpArm;
                DownBodyPoint.Target = SummonDownBody;
                DownBodyRotate.Target = SummonDownBody;
                break;
            case VeryBigMonsterController.AniStatus.Capture:
                RightUpArmPoint.Target = CaptureRightUpArm;
                RightUpArmRotate.Target = CaptureRightUpArm;
                DownBodyPoint.Target = CaptureDownBody;
                DownBodyRotate.Target = CaptureDownBody;
                break;
            case VeryBigMonsterController.AniStatus.Weak:
                RightUpArmPoint.Target = WeakRightUpArm;
                RightUpArmRotate.Target = WeakRightUpArm;
                DownBodyPoint.Target = WeakDownBody;
                DownBodyRotate.Target = WeakDownBody;
                break;
        }
    }
}
