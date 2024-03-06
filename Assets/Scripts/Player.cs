using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UIElements;

enum CarLane
{
    Left,
    Right,
}

public class PlayerCar : MonoBehaviour
{
    // Variables
    private bool booster = false;
    private bool laneChanged = false;
    private CarLane _currentLane = CarLane.Right;

    private IEnumerator lastTransitionX, lastTransitionY;
    private Vector2 fingerDownPosition, fingerUpPosition;
    private float lastPositionX;


    // Constants
    private const string BOOST_KEY = "boost";
    private const float laneChangeDuration = 0.6F;
    private const float minDistanceForSwipe = 0.5f;
    private const float leftLaneX = -1.47F, rightLaneX = 1.47F;
    private const float normalCarY = -3.57F, boostedCarY = -2.81F;


    // Game Components
    private Animator animator;
    [SerializeField] private GameObject boosterObject;
    [SerializeField] private AudioSource audioData, boosterAudioData;
    [SerializeField] private AudioClip curRunningAudio, accidentAudio;


    private CarLane CurrentLane
    {
        get => _currentLane;
        set
        {
            laneChanged = true;
            _currentLane = value;
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // Start is called before the first frame update
    void Start()
    {
        audioData.clip = curRunningAudio;
        audioData.loop = true;
        audioData.volume = 0.7F;
        audioData.Play();
    }

    // Update is called once per frame
    void Update()
    {
        TouchInput();
        KeyboardInput();
        ComplexInput();
        Animate();

        if (GlobalVariables.Instance.gameOver)
        {
            StopAllCoroutines();
        }

        {
            int newSpeed = 8;

            if (booster)
            {
                if (!boosterAudioData.isPlaying)
                {
                    boosterAudioData.Play();
                }
                newSpeed = 10;

                if (lastTransitionY == null)
                // Idle -> Boost Transition
                {
                    lastTransitionY = GlobalVariables.TransitionPositionY(gameObject, transform.position.y, boostedCarY, 0.5f);
                    StartCoroutine(lastTransitionY);
                }
            }
            else if (boosterAudioData.isPlaying)
            {
                boosterAudioData.Stop();

                // Boost -> Idle Transition
                if (lastTransitionY != null)
                    StopCoroutine(lastTransitionY);

                StartCoroutine(GlobalVariables.TransitionPositionY(gameObject, transform.position.y, normalCarY, 0.5f));
                lastTransitionY = null;
            }

            if (!GlobalVariables.Instance.gameOver)
            {
                GlobalVariables.Instance.speed = newSpeed;
            }
        }

        lastPositionX = transform.position.x;
    }

    void Animate()
    {
        animator.SetBool(BOOST_KEY, booster);

        if (GlobalVariables.Instance.gameOver) return;

        if (laneChanged)
        {
            if (lastTransitionX != null)
                StopCoroutine(lastTransitionX);

            if (CurrentLane == CarLane.Right)
            {
                lastTransitionX = GlobalVariables.TransitionPositionX(gameObject, transform.position.x, rightLaneX, laneChangeDuration);
                StartCoroutine(lastTransitionX);
            }
            else
            {
                lastTransitionX = GlobalVariables.TransitionPositionX(gameObject, transform.position.x, leftLaneX, laneChangeDuration);
                StartCoroutine(lastTransitionX);
            }
            laneChanged = false;
        }

        // Sticking to a position outside lane problem
        if (
            lastPositionX == transform.position.x &&
            lastPositionX != leftLaneX && lastPositionX != rightLaneX)
        {
            float distanceBetweenLanes = Math.Abs(rightLaneX - leftLaneX);

            if (CurrentLane == CarLane.Right &&
                0 < transform.position.x && transform.position.x < rightLaneX)
            {
                float duration = laneChangeDuration * Math.Abs(
                    (rightLaneX - transform.position.x) / distanceBetweenLanes
                );

                lastTransitionX = GlobalVariables.TransitionPositionX(gameObject, transform.position.x, rightLaneX, duration);
                StartCoroutine(lastTransitionX);
            }
            else if (CurrentLane == CarLane.Left &&
                0 > transform.position.x && transform.position.x > leftLaneX)
            {
                float duration = laneChangeDuration * Math.Abs(
                    (leftLaneX - transform.position.x) / distanceBetweenLanes
                );

                lastTransitionX = GlobalVariables.TransitionPositionX(gameObject, transform.position.x, leftLaneX, duration);
                StartCoroutine(lastTransitionX);
            }
        }
    }

    void ComplexInput()
    {
        if (
            !(Input.touches.Length == 2 || Input.GetKey(KeyCode.LeftShift))
            && booster)
        {
            booster = false;
        }
    }

    void KeyboardInput()
    {
        if (GlobalVariables.Instance.gameOver)
        { return; }

        if (Input.GetKeyDown(KeyCode.LeftArrow) && Input.GetKeyDown(KeyCode.RightArrow))
        { return; }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            booster = true;
        }

        if (Input.GetKeyDown(KeyCode.RightArrow) && transform.position.x <= 0)
        {
            CurrentLane = CarLane.Right;
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow) && transform.position.x >= 0)
        {
            CurrentLane = CarLane.Left;
        }

    }

    void TouchInput()
    {
        if (GlobalVariables.Instance.gameOver)
        { return; }

        if (Input.touches.Length == 2)
        {
            booster = true;
        }
        else if (Input.touchCount > 0)
        {
            foreach (Touch touch in Input.touches)
            {
                if (touch.phase == TouchPhase.Began)
                {
                    fingerDownPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    fingerUpPosition = Camera.main.ScreenToWorldPoint(touch.position);
                }

                if (touch.phase == TouchPhase.Ended)
                {
                    fingerUpPosition = Camera.main.ScreenToWorldPoint(touch.position);
                    DetectSwipe();
                }
            }
        }

    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(GlobalVariables.CAR_TAG))
        {
            audioData.Stop();
            audioData.clip = accidentAudio;
            audioData.loop = false;
            audioData.Play();

            GlobalVariables.Instance.GameOver();
        }
    }


    void DetectSwipe()
    {
        if (Vector2.Distance(fingerDownPosition, fingerUpPosition) >= minDistanceForSwipe)
        {
            Vector2 direction = fingerUpPosition - fingerDownPosition;
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                if (direction.x > 0)
                {
                    CurrentLane = CarLane.Right;
                }
                else
                {
                    CurrentLane = CarLane.Left;
                }
            }
        }
    }
}
