using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialog : MonoBehaviour
{
    public int Order;
    private GameObject ThisDialog;//�`�N���󶶧�
    private Transform _dialogtransform;
    private FloatController _floatController = new FloatController();
    private bool DialogAppear;

    private float Height = 2.41f;
    private RaycastHit2D GroundCheck;
    private RaycastHit2D SpecialGroundCheck;

    [SerializeField] private bool hasJudgementBool;//���S��bool�M�w��r�O�_�X�{

    private void Awake()
    {
        ThisDialog = this.gameObject.transform.GetChild(Order).gameObject;
        _dialogtransform = ThisDialog.transform;
    }
    // Start is called before the first frame update
    void Start()
    {
        _floatController.FloatVarInisialize(_dialogtransform, 4, 0.25f);

        //�]�w�}�B����
        GroundCheck =  Physics2D.Raycast(this.transform.position, -Vector2.up, 20, 1024);
        SpecialGroundCheck = Physics2D.Raycast(this.transform.position, -Vector2.up, 20, 32768);
        if (GroundCheck && SpecialGroundCheck)
        {
            if (GroundCheck.distance < SpecialGroundCheck.distance)
            {
                SetFloatCenter(GroundCheck);
            }
            else
            {
                SetFloatCenter(SpecialGroundCheck);
            }
        }
        else
        {
            if (GroundCheck)
            {
                SetFloatCenter(GroundCheck);
            }
            if (SpecialGroundCheck)
            {
                SetFloatCenter(SpecialGroundCheck);
            }
        }
    }
    private void Update()
    {
        if (DialogAppear)
        {
            _dialogtransform.position = new Vector3(PlayerController.PlayerPlaceX, _dialogtransform.position.y, _dialogtransform.position.z);
            _floatController.Float(_dialogtransform, Time.deltaTime);
        }
    }

    public void EnterPlayer(Collider2D Collider)
    {
        if (Collider.tag == "Player")
        {
            DialogAppear = true;
            _floatController.FloatReset();
            ThisDialog.SetActive(true);
        }
    }

    public void EnterPlayer(Collider2D Collider, ref bool JudgementBool)
    {
        print("�ݧR��");
        if (Collider.tag == "Player" && !JudgementBool)
        {
            DialogAppear = true;
            _floatController.FloatReset();
            ThisDialog.SetActive(true);
        }
    }

    public void ExitPlayer(Collider2D Collider)
    {
        if (Collider.tag == "Player")
        {
            DialogAppear = false;
            ThisDialog.SetActive(false);
        }
    }

    private void SetFloatCenter(RaycastHit2D _ray)
    {
        _dialogtransform.position = new Vector2(_dialogtransform.position.x, _ray.point.y + Height);
        _floatController.FloatReset(_dialogtransform);
    }

    public void TurnOffDialog()
    {
        ThisDialog.SetActive(false);
    }
}
