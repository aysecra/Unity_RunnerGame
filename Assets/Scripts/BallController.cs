
using System.Collections;
using TMPro;
using UnityEngine;

public class BallController : MonoBehaviour
{
    [SerializeField] private GameObject Btn_Reload, Btn_Play;
    [SerializeField] private GameObject[] _layers = new GameObject[3];
    [SerializeField] private TextMeshProUGUI _txtScore;

    private Vector3 moveTo;
    private float speed = 13f;
    private int _score = 0;
    private int _layerIndex = 0;
    private bool _canMove = true;


    void Awake()
    {
        Movement.Rb = GetComponent<Rigidbody>();
        Movement.Transform = transform;
        CameraContoller.onGround = true;
        _canMove = true;
        _score = 0;
    }

    void FixedUpdate()
    {
        // take swipe direction
        int direction = Movement.SwipeDirection();

        // ball on the ground
        if (CameraContoller.onGround)
        {
            // right or left
            if (direction == 1 || direction == -1)
                moveInGround(direction);
        }

        // ball on the left wall
        else if (CameraContoller.onLeftWall)
        {
            // right or right
            if (direction == 1 || direction == -1)
                moveInWall(direction, -1);
        }

        // ball on the right wall
        else if (CameraContoller.onRightWall)
        {
            // right or right
            if (direction == 1 || direction == -1)
                moveInWall(direction, 1);
        }

        // target not reached
        if (transform.position != moveTo)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
            if (CameraContoller.onRightWall || CameraContoller.onLeftWall)
            {
                if (transform.position.y >= moveTo.y - 2)
                    layersChangeActive();
            }
        }

        // forward
        if (_canMove)
        {
            Movement.MoveForward();
            _txtScore.text = _score.ToString();
        } 
        else
        {
            Movement.Rb.velocity = Vector3.zero;
            allLayersFalse();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // collect gems and increase score
        if (other.gameObject.tag == "Gem")
        {
            Destroy(other.gameObject);
            _score++;
            //print(_score);
        }

        // move from the right wall
        else if (other.gameObject.tag == "WallRight")
        {
            CameraContoller.onRightWall = true;
            CameraContoller.onLeftWall = false;
            CameraContoller.onGround = false;

            other.isTrigger = false;

            transform.position = new Vector3(5, 5f, transform.position.z);
            transform.rotation = Quaternion.Euler(0, 0, 180);
            _layers[_layerIndex].SetActive(true);
        }

        //move from the left wall
        else if (other.gameObject.tag == "WallLeft")
        {
            CameraContoller.onRightWall = false;
            CameraContoller.onLeftWall = true;
            CameraContoller.onGround = false;

            other.isTrigger = false;

            transform.position = new Vector3(-5, 5f, transform.position.z);
            transform.rotation = Quaternion.Euler(0, 0, 0);
            _layers[_layerIndex].SetActive(true);
        }

        else if (other.gameObject.tag == "Obstacle")
        {
            GameOver();
        }

        else if (other.gameObject.tag == "Target")
        {
            _canMove = false;
            transform.position = new Vector3(transform.position.x - 1.5f, transform.position.y, transform.position.z);
            Btn_Play.SetActive(true);
        }
    }

    private void moveInGround(int direction)
    {
        Vector3 startPos = transform.position;
        moveTo = startPos + (new Vector3(6, 0, 0) * direction);

        transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);
    }

    private void moveInWall(int direction, int wall)
    {
        Vector3 startPos = transform.position;
        moveTo = startPos + (new Vector3(0, 7, 0) * direction * wall);

        transform.position = Vector3.MoveTowards(transform.position, moveTo, speed * Time.deltaTime);

        _layerIndex += (direction * wall == 1) ? 1 : -1;
        print(_layerIndex);
        if (_layerIndex < 0)
        {
            if (CameraContoller.onLeftWall)

                transform.position = new Vector3(-5, 2, transform.position.z);

            else if (CameraContoller.onRightWall)
                transform.position = new Vector3(5, 2, transform.position.z);

            _layers[0].SetActive(false);

            CameraContoller.onRightWall = false;
            CameraContoller.onLeftWall = false;
            CameraContoller.onGround = true;

            _layerIndex = 0;
        }
    }

    private void layersChangeActive()
    {
        foreach (GameObject layer in _layers)
            layer.SetActive(false);
        _layers[_layerIndex].SetActive(true);
    }

    private void allLayersFalse()
    {
        foreach (GameObject layer in _layers)
            layer.SetActive(false);
    }
    private void GameOver()
    {
        _canMove = false;
        Btn_Reload.SetActive(true);
    }
}
