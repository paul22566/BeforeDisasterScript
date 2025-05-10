using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowSprite : MonoBehaviour
{
    private GameObject Player;
    private Transform PlayerDashTransform;
    private SpriteRenderer playerSprite;
    private SpriteRenderer thisSprite;
    private Color color;
    [Header("�ɶ�����Ѽ�")]
    public float activeTime;
    public float activeStart;

    [Header("���z���ױ���Ѽ�")]
    private float alpha;
    public float alphaSet;
    public float alphaMultipier;

    private void OnEnable()
    {
        Player = GameObject.Find("player");
        PlayerDashTransform = Player.transform.GetChild(0).GetChild(3).transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = PlayerDashTransform.GetComponent<SpriteRenderer>();

        alpha = alphaSet;

        thisSprite.sprite = playerSprite.sprite;
        transform.position = PlayerDashTransform.position;
        transform.localScale = PlayerDashTransform.localScale;
        transform.rotation = PlayerDashTransform.rotation;

        activeStart = Time.time;
    }
    // Update is called once per frame
    void Update()
    {
        if (playerSprite.flipX == true)
        {
            thisSprite.flipX = true;
        }
        if (playerSprite.flipX == false)
        {
            thisSprite.flipX = false;
        }
        alpha *= alphaMultipier;

        color = new Color(1, 1, 1, alpha);

        thisSprite.color = color;

        if(Time.time >= activeStart + activeTime)
        {
            //��^��H��
            ShadowPool.instance.ReturnPool(this.gameObject);
        }
    }
}
