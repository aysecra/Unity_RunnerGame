
using UnityEngine;

static class Movement
{
    private static Rigidbody _rb;
    private static Transform _transform;


    private static float _speed = 35f;
    private static Vector2 startPosition;
    private static int pixelDistToDetect = 20;
    private static bool fingerTouch = false;
    public static Vector3 moveTo = new Vector3(0, 2, 0);


    public static Transform Transform { set => _transform = value; }
    public static Rigidbody Rb { get => _rb; set => _rb = value; }

    public static int SwipeDirection()
    {
        // swipe
        if (Input.touchCount > 0)
        {
            Touch touch = Input.touches[0];

            // take start position
            if (touch.phase == TouchPhase.Began)
            {
                startPosition = touch.position;
                fingerTouch = true;
                // Debug.Log(startPosition);
            }

            if (fingerTouch == true)
            {
                // right
                if (touch.position.x >= startPosition.x + pixelDistToDetect)
                {
                    //Debug.Log("Swipe right");
                    fingerTouch = false;
                    return 1;
                }

                // left
                else if (touch.position.x <= startPosition.x - pixelDistToDetect)
                {
                    //Debug.Log("Swipe left");
                    fingerTouch = false;
                    return -1;
                }
            }
        }
        return 0;
    }

    public static void MoveForward()
    {
        _rb.velocity = (Vector3.forward * _speed);
    }

}
