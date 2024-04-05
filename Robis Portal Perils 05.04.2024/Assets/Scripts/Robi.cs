using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Robi : MonoBehaviour, IDataPersistence
{
    Rigidbody2D rb;
    Animator animator;
    [SerializeField] Transform groundCheckCollider;
    [SerializeField] LayerMask groundLayer;

    const float groundCheckRadius = 0.2f;
    [SerializeField] float speed = 1;
    float horizontalValue;
    float runSpeedModifier = 2f;

    bool isRunning = false;
    bool facingRight = true;
    bool isDialoguePlaying = false;

    public VectorValue startingPosition;

    void Start()
    {
        transform.position = startingPosition.initialValue;
    }

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (CanMove() == false)
            return;

        //Store the Horizontal Value
        horizontalValue = Input.GetAxisRaw("Horizontal");

        //If lShift is clicked enable isRunning
        if (Input.GetKeyDown(KeyCode.LeftShift))
            isRunning = true;
        //If lShift is released disable isRunning
        if (Input.GetKeyUp(KeyCode.LeftShift))
            isRunning = false;
    }

    void FixedUpdate()
    {
        Move(horizontalValue);
    }

    public void LoadData(GameData data)
    {
        this.transform.position = startingPosition.initialValue;
    }

    public void SaveData(GameData data)
    {
        data.playerPosition = this.transform.position;
    }

    public bool CanMove()
    {
        DialogueManager dialogueManager = DialogueManager.GetInstance();

        bool can = true;
        if (dialogueManager != null && dialogueManager.dialogueIsPlaying)
        {
            can = false;
            isDialoguePlaying = true; // Set the flag to true if dialogue is playing
        }
        else
        {
            isDialoguePlaying = false; // Set the flag to false if dialogue is not playing
        }

        return can;
    }


    public void Move(float dir)
    {
        if (isDialoguePlaying)
        {
            rb.velocity = Vector2.zero;
            animator.SetFloat("xVelocity", 0f);
            return;
        }

        float xVal = dir * speed * 100 * Time.fixedDeltaTime;
        // If we are running, multiply with the running modifier
        if (isRunning)
            xVal *= runSpeedModifier;

        Vector2 targetVelocity = new Vector2(xVal, rb.velocity.y);
        rb.velocity = targetVelocity;

        if (facingRight && dir < 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            facingRight = false;
        }
        else if (!facingRight && dir > 0)
        {
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            facingRight = true;
        }

        // Set the float xVelocity according to the x value of the RigidBody velocity
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
    }

}