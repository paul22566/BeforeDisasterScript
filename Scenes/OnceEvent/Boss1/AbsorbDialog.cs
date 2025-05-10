using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbsorbDialog : MonoBehaviour
{
    public int Order;
    private GameObject ThisDialog;//ª`·Nª«¥ó¶¶§Ç
    private Transform _dialogtransform;
    private FloatController _floatController = new FloatController();
    // Start is called before the first frame update
    void Start()
    {
        ThisDialog = this.gameObject.transform.GetChild(Order).gameObject;

        _dialogtransform = ThisDialog.transform;

        _floatController.FloatVarInisialize(_dialogtransform, 4, 0.25f);
    }
    private void Update()
    {
        _floatController.Float(_dialogtransform, Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !GameEvent.AbsorbBoss1)
        {
            _floatController.FloatReset();
            ThisDialog.SetActive(true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ThisDialog.SetActive(false);
        }
    }
}
