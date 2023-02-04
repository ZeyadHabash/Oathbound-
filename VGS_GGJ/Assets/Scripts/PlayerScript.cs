using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class PlayerScript : MonoBehaviour
{
    public float moveSpeed = 3;
    public float jumpSpeed = 3;
    private bool grounded = true;
    private Rigidbody2D playerbody;
    public rootgenscript rgn;
    public dooropenscript dors;
    public AudioSource footsteps;
    public AudioSource keysfx;
    public AudioClip rightstep;
    public AudioClip leftstep;
    public AudioClip keygrab;
    bool firststep = true;
    bool moving = false;

    public Animator anim;

    private Transform playerTR;
    public int direction = 1;
    public bool switched;

    private PlayerInput playerInput;

    private InputAction moveAction;
    private InputAction jumpAction;

    void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        jumpAction = playerInput.actions["Jump"];
        moveAction = playerInput.actions["Move"];
    }
    void Start()
    {
        playerbody = gameObject.GetComponent<Rigidbody2D>();
        playerTR = gameObject.GetComponent<Transform>();
    }


    void Update()
    {
        var moveActionValue = moveAction.ReadValue<float>();
        if (Mathf.Abs(playerbody.velocity.x) > 1)
            moving = true;
        else
            moving = false;
        if(!moving || !grounded)
        {
            footsteps.Stop();
        }
        move(moveActionValue);
        if (!footsteps.isPlaying && moving && grounded)
        {
            if (firststep)
                footsteps.PlayOneShot(rightstep);
            else
                footsteps.PlayOneShot(leftstep);
            firststep = !firststep;
        }

        anim.SetFloat("speed", Mathf.Abs(playerbody.velocity.x));

        if (playerbody.velocity.x > 0)
        {
            direction = 1;
        }
        else if (playerbody.velocity.x < 0)
        {
            direction = -1;
        }

        if (direction == -1 && !switched)
        {
            switched = true;
            playerTR.localScale = new Vector3(playerTR.localScale.x * -1, playerTR.localScale.y, playerTR.localScale.z);
        }
        if (direction == 1 && switched)
        {
            switched = false;
            playerTR.localScale = new Vector3(playerTR.localScale.x * -1, playerTR.localScale.y, playerTR.localScale.z);
        }
    }



    void OnEnable()
    {
        playerInput.enabled = true;
        jumpAction.performed += jump;
    }
    void OnDisable()
    {
        playerInput.enabled = false;
        jumpAction.performed -= jump;
    }

    private void move(float moveActionValue){
        playerbody.velocity = new Vector2(moveActionValue * moveSpeed, playerbody.velocity.y);
    }

    private void jump(InputAction.CallbackContext context){
        if(grounded)
            playerbody.velocity = new Vector2(playerbody.velocity.x, jumpSpeed);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Floor")
        {
            grounded = true;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.tag == "Floor")
            grounded = false;
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "explosion")
            killPlayer();
        if (other.gameObject.tag == "enemy")
            killPlayer();
        if (other.gameObject.tag == "trap")
            killPlayer();
        if (other.gameObject.tag == "skullkey")
        {
            rgn.enraged = true;
            dors.open = true;
            Destroy(other.gameObject);
            //keysfx.PlayOneShot(keygrab);
        }
        if (other.gameObject.tag == "door")
            playerbody.velocity = new Vector2(playerbody.velocity.x, 40);
        if(other.gameObject.tag == "Power Up (JMP)")
        {
            Destroy(other.gameObject);
            StartCoroutine(Jmpboost());
        }
    }

    private void killPlayer(){
        enabled = false;
    }
    IEnumerator Jmpboost()
    {
        float tempjmp = jumpSpeed;
        jumpSpeed *= 2;
        yield return new WaitForSeconds(5);
        jumpSpeed = tempjmp;
    }
}
