using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommonHurtedJudjement : MonoBehaviour
{
    private PlayerController _playerController;

    // Start is called before the first frame update
    void Start()
    {
        _playerController = this.transform.parent.GetComponent<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "monsterAtk" || collision.tag == "Cocktail" || collision.tag == "ExplosionBottle")
        {
            _playerController.RecordMonsterAtkData(collision.transform.GetComponent<NormalMonsterAtk>(), collision.transform);
        }
        if (collision.tag == "CaptureAtk" && !_playerController.WeakInvincible)
        {
            _playerController.HurtedByCaptureAtk(collision.GetComponent<CaptureAtk>());
        }
        if (collision.tag == "CaptureAtkEnd" && !_playerController.WeakInvincible)
        {
            _playerController.HurtedByCaptureAtkEnd();
        }
    }
}
