using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraContoller : MonoBehaviour
{
    [SerializeField] private Transform _ball;
    private Vector3 _distance;

    // // ball position
    public static bool onLeftWall = false;
    public static bool onRightWall = false;
    public static bool onGround = false;
    void Start()
    {
        onLeftWall = false;
        onRightWall = false;
        onGround = true;
        _distance = transform.position - _ball.position;
    }

    void LateUpdate()
    {
        Vector3 newPosition = new Vector3(transform.position.x, transform.position.y, _distance.z + _ball.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, 0.6f);
        if (onGround)
            transform.rotation = Quaternion.Euler(20, 0, 0);
        else if (onLeftWall)
            transform.rotation = Quaternion.Euler(20, 0, -90);
        else if (onRightWall)
            transform.rotation = Quaternion.Euler(20, 0, 90);
    }
}
