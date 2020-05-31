using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 6;
    public float gravity = -12f;
    public float jumpHeight = 1f;

    bool Inwards; //attack

    //dash
    public float dashSpeed;
    public float dashDistance;
    public Vector3 BladeAngles;
    public Transform BladeHolder;
    public GameObject Trail;
    public GameObject SwordSpark;
    public GameObject Spark;
    public GameObject DashSpark;
    public AudioSource SFX;




    public ThirdPersonCamera tps;
    public int time = 0;
    public int idleTime = 300;

    public Vector3 newRotation;

    public bool jumping;

    float jumpVelocity;

    public float turnSmoothTime = .2f;
    float turnSmoothVelocity;

    public float speedSmoothTime = 0.2f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    Animator animator;
    public Transform cam;
    CharacterController controller;

    
    // Start is called before the first frame update
    void Start()
    {
        
        Inwards = true;
        
        jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);

        animator = GetComponent<Animator>();
        cam = Camera.main.transform;
        controller = GetComponent<CharacterController>();

        Trail.SetActive(false);
        Spark.SetActive(false);
        SwordSpark.SetActive(false);
        DashSpark.SetActive(false);
    }


    void FixedUpdate()
    {

        if (!Input.anyKey)
        {
            time = time + 1;
            Debug.Log(time);
        }
        else
        {
            animator.SetTrigger("Active");
            time = 0;
        }

        if (time >= idleTime)
        {
            animator.SetTrigger("Idle");
            time = 0;
        }

    }
    void Update()
    {
        if (!Spark.GetComponent<ParticleSystem>().IsAlive())
        {
            Spark.SetActive(false);
        }

        //attack
        if (Input.GetKeyDown(KeyCode.E))
        {
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
            animator.SetTrigger("Attack In");
        }

        //dash
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Dash");
            if (!SFX.isPlaying)
            {
                SFX.Play();
            }
            else
            {
                SFX.Stop();
                SFX.Play();
            }
            
            animator.SetTrigger("Dash");
            StartCoroutine("Dash");

        }

        //input movement

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 inputDir = input.normalized;

        if(inputDir != Vector2.zero)
        {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
        }

        bool running = Input.GetKey(KeyCode.LeftShift);
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;
        Vector3 velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        

        controller.Move(velocity * Time.deltaTime);
        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude; 

        if (controller.isGrounded)
        {
            velocityY = 0;
            if (Input.GetKeyDown(KeyCode.Space))
            {   
                jumping = true;
                Jump();
            }
            else
            {
                jumping = false;
            }
        }

        

        //animator
        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f);
        animator.SetFloat("SpeedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);


    }

    void Jump()
    {
        velocityY = jumpVelocity;
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (!jumping)
        {
            return smoothTime;
        }else
        {
            return smoothTime * 10;
        }

    }

    IEnumerator Dash()
    {
        Spark.SetActive(true);
        DashSpark.SetActive(true);
        Vector3 startPos = transform.position;
        
        while (dashDistance > Vector3.Distance(startPos, transform.position))
        {
            Trail.SetActive(true);
            tps.dstFromTarget = 1.5f;
            transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, Camera.main.transform.localEulerAngles.y, transform.localEulerAngles.z);
            controller.Move(transform.TransformDirection(Vector3.forward) * dashSpeed);
            yield return null;
            
        }
        animator.SetTrigger("NoDash");
        StartCoroutine("CamBack");
        Trail.SetActive(false);
        DashSpark.SetActive(false);
        SwordSpark.SetActive(true);
        yield return new WaitForSeconds(1f);
        SwordSpark.SetActive(false);

    }

    IEnumerator CamBack()
    {
        while(tps.dstFromTarget > 1f)
        {
            tps.dstFromTarget -= .1f;
            yield return null;
        }
    }   


}
