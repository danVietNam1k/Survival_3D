using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Animator))]
public class HorseController : MonoBehaviour
{
    public Transform saddlePoint;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float acceleration = 2f;
    public float deceleration = 2f;

    private float currentSpeed = 0f;
    private bool isMounted = false;

    private float gravity = -9.81f;
    private Vector3 verticalVelocity = Vector3.zero;

    private Animator animator;
    private Transform rider;
    private CharacterController controller;
    public GameObject tutorial;

    [Header("Audio")]
    public AudioSource footstepSource;
    public AudioClip[] walkClip;
    public AudioClip[] runClip;

    private float stepTimer = 0f;
    private float stepInterval = 0.5f;

    private const float walkStepInterval = 6f; // make sure its the same length as your step clip 
    private const float runStepInterval = 1f;

    private const float idleToWalkThreshold = 3f;
    private const float walkToRunThreshold = 4f;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMounted || rider == null)
            return;

        HandleMovement();
        UpdateAnimations();
        Dismount();
    }
    private void LateUpdate()
    {
        

    }
    private void HandleMovement()
    {
        float vertical = Input.GetAxis("Vertical");

        // Determine target speed
        float targetSpeed = 0f;
        if (vertical > 0.1f)
            targetSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

        // Accelerate / Decelerate
        currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed,
            (currentSpeed < targetSpeed ? acceleration : deceleration) * Time.deltaTime);

        // Turn
        float horizontal = Input.GetAxis("Horizontal");
        if (Mathf.Abs(horizontal) > 0.1f)
            transform.Rotate(Vector3.up * horizontal * 120f * Time.deltaTime);

        // Apply gravity
        if (controller.isGrounded)
            verticalVelocity.y = -1f; // Small push down to stay grounded
        else
            verticalVelocity.y += gravity * Time.deltaTime;

        // Final movement vector
        Vector3 move = transform.forward * currentSpeed + verticalVelocity;
        controller.Move(move * Time.deltaTime);
    }

    private void UpdateAnimations()
    {
        animator.SetFloat("Speed", currentSpeed);

        //PlayHorseSounds();
    }

    private void PlayHorseSounds()
    {

        if (controller.isGrounded && currentSpeed > 0f)
        {
            stepTimer -= Time.deltaTime;

            if (stepTimer <= 0f)
            {
                // Set footstep interval based on walking or running
                //stepInterval = currentSpeed > walkSpeed ? runStepInterval / currentSpeed : walkStepInterval/ currentSpeed;
                float t = Mathf.Clamp01(currentSpeed / runSpeed);
                stepInterval = Mathf.Lerp(1f, 0.5f, t);              
                // Play correct clip
                //AudioClip clipToPlay = currentSpeed > walkSpeed ? runClip[0] : walkClip[0];
                AudioClip clipToPlay = walkClip[0];

                if(clipToPlay != null)  
               footstepSource.PlayOneShot(clipToPlay);
                
                   print(stepInterval);

                stepTimer = stepInterval;
            }
        }
        else
        {
            stepTimer = 0f; // Reset so we play immediately on next move
        }
    }

    public void Mount(Transform player)
    {
        if (isMounted)
            return;

        isMounted = true;
        rider = player;
        tutorial.SetActive(true);
        // Position the rider on the saddle
        rider.SetParent(saddlePoint);
        rider.localPosition = Vector3.zero;
        rider.localRotation = Quaternion.identity;
        rider.Find("Head/PlayerCamera").localRotation = Quaternion.identity;
        // Disable player movement
        player.GetComponent<FirstPersonController>().enabled = false; 
    }

    public void Dismount()
    {
        if (!isMounted || rider == null)
            return;
        if (Input.GetKeyDown(KeyCode.E))
        {
            tutorial.SetActive(false);

            // Detach and place nearby
            rider.SetParent(null);
            rider.position = transform.position - transform.right * 3f + Vector3.up * 1f;

            // Enable player movement
            rider.GetComponent<FirstPersonController>().enabled = true;


            isMounted = false;
            rider = null;
            currentSpeed = 0f;
            animator.SetFloat("Speed", currentSpeed);

        }

    }
    public void PlaySoundW1()
    {
        footstepSource.pitch = Random.Range(0.8f, 1.2f);
        footstepSource.PlayOneShot(walkClip[0]);

    }
    public void PlaySoundW2()
    {
        footstepSource.pitch = Random.Range(0.8f, 1.2f);
        footstepSource.PlayOneShot(walkClip[1]);

    }
    public void PlaySoundW3()
    {
        footstepSource.pitch = Random.Range(0.8f, 1.2f);
        footstepSource.PlayOneShot(walkClip[2]);

    }
    public void PlaySoundW4()
    {footstepSource.pitch = Random.Range(0.8f, 1.2f);
        footstepSource.PlayOneShot(walkClip[3]);

    }
    public void PlaySoundR4()
    {
        footstepSource.PlayOneShot(runClip[2]);

    }
    //public void PlaySoundR1()
    //{
    //    footstepSource.pitch = Random.Range(0.8f, 1.2f);
    //    footstepSource.PlayOneShot(runClip[Random.Range(0, runClip.Length)]);

    //}


}