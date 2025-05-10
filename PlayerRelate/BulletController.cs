using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private GameObject Player;
    private Transform _transform;
    public float Speed;
    private bool GoRight;
    private float Distance;
    public GameObject Sound;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.Find("player");
        _transform = this.transform;
        Instantiate(Sound, _transform.localPosition, Quaternion.identity);
        if (Player.transform.position.x <= _transform.position.x)
        {
            GoRight = true;
        }
        else
        {
            GoRight = false;
        }
    }

    private void Update()
    {
        Distance = Mathf.Abs(_transform.position.x - Player.transform.position.x);
        if(Distance > 24)
        {
            Destroy(this.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (GoRight)
        {
            _transform.position += new Vector3(Speed, 0, 0);
        }
        else
        {
            _transform.position += new Vector3(-Speed, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "LeftWall")
        {
            Destroy(this.gameObject);
        }
        if (collision.tag == "RightWall")
        {
            Destroy(this.gameObject);
        }
        if (collision.tag == "SpecialPlatform")
        {
            Destroy(this.gameObject);
        }
        if (collision.tag == "platform")
        {
            Destroy(this.gameObject);
        }
    }
}
