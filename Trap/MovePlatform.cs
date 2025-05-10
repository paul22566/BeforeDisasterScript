using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    //此腳本需要有PeriodicMove才能用
    private Transform _playerTransform;
    private PeriodicMove _periodicMove;
    private bool touchPlayer;

    private float _fixedDeltaTime;

    // Start is called before the first frame update
    void Start()
    {
        _periodicMove = this.GetComponent<PeriodicMove>();
    }

    private void FixedUpdate()
    {
        _fixedDeltaTime = Time.fixedDeltaTime;

        if (touchPlayer)
        {
            switch (_periodicMove.direction)
            {
                case PeriodicMove.Direction.Down:
                    _playerTransform.localPosition = new Vector3(_playerTransform.localPosition.x, _playerTransform.localPosition.y - _periodicMove.Speed * _fixedDeltaTime * 1.5f, 0);
                    break;
                case PeriodicMove.Direction.Left:
                    _playerTransform.localPosition = new Vector3(_playerTransform.localPosition.x - _periodicMove.Speed * _fixedDeltaTime, _playerTransform.localPosition.y, 0);
                    break;
                case PeriodicMove.Direction.Right:
                    _playerTransform.localPosition = new Vector3(_playerTransform.localPosition.x + _periodicMove.Speed * _fixedDeltaTime, _playerTransform.localPosition.y, 0);
                    break;
            }
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && PlayerController.isGround)
        {
            _playerTransform = collision.transform;
            touchPlayer = true;
            if (_periodicMove.direction == PeriodicMove.Direction.Down)
            {
                _playerTransform.localPosition = new Vector3(_playerTransform.localPosition.x, _playerTransform.localPosition.y - _periodicMove.Speed * Time.fixedDeltaTime * 2, 0);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerTransform = null;
            touchPlayer = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && PlayerController.isGround)
        {
            _playerTransform = collision.transform;
            touchPlayer = true;
            if (_periodicMove.direction == PeriodicMove.Direction.Down)
            {
                _playerTransform.localPosition = new Vector3(_playerTransform.localPosition.x, _playerTransform.localPosition.y - _periodicMove.Speed * Time.fixedDeltaTime * 2, 0);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            _playerTransform = null;
            touchPlayer = false;
        }
    }
}
