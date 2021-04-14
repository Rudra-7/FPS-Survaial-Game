using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSprintAndCrouch : MonoBehaviour
{
    private PlayerMovement playerMovement;


    public float sprintSpeed = 10f;
    public float moveSpeed = 5f;
    public float crouchSpeed = 2f;

    private Transform look_Root;
    private float standHeight = 1.6f;
    private float crouchHeight = 1f;

    private bool isCrouching;
    private bool isSprinting;

    private PlayerFootsteps playerFootsteps;

    private float sprintVolume = 1f;
    private float crouchVolume = 0.1f;
    private float walkVolumeMin = 0.2f, walkVolumeMax = 0.6f;

    private float walkStepDistance = 0.4f;
    private float sprintStepDistance = 0.25f;
    private float crouchStepDistance = 0.5f;

    private PlayerStats playerStats;

    private float sprintValue = 100f;
    public float sprintThreshold = 10;

    // Start is called before the first frame update
    void Awake()
    {
        playerMovement = GetComponent<PlayerMovement>();

        look_Root = transform.GetChild(0);

        playerFootsteps = GetComponentInChildren<PlayerFootsteps>();

        playerStats = GetComponent<PlayerStats>();
    }

    private void Start()
    {
        playerFootsteps.volumeMin = walkVolumeMin;
        playerFootsteps.volumeMax = walkVolumeMax;
        playerFootsteps.stepDistance = walkStepDistance;
    }

    // Update is called once per frame
    void Update()
    {
        Sprint();
        Crouch();
    }

    void Sprint()
    {
        // if we have stamina then sprint
        if (sprintValue > 0f)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift) && !isCrouching && moveSpeed >= 0f)
            {
                playerMovement.speed = sprintSpeed;
                playerFootsteps.stepDistance = sprintStepDistance;
                playerFootsteps.volumeMin = sprintVolume;
                playerFootsteps.volumeMax = sprintVolume;

            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift) && !isCrouching)
        {
            playerMovement.speed = moveSpeed;

            playerFootsteps.stepDistance = walkStepDistance;
            playerFootsteps.volumeMin = walkVolumeMin;
            playerFootsteps.volumeMax = walkVolumeMax;
            
        }

        if(Input.GetKey(KeyCode.LeftShift) && !isCrouching)
        {
            sprintValue -= sprintThreshold * Time.deltaTime;

            if(sprintValue <= 0f)
            {
                sprintValue = 0f;

                //reset the speed and sound

                playerMovement.speed = moveSpeed;
                playerFootsteps.stepDistance = walkStepDistance;
                playerFootsteps.volumeMin = walkVolumeMin;
                playerFootsteps.volumeMax = walkVolumeMax;
            }
            playerStats.DisplayStaminaStats(sprintValue);

        }
        else
        {
            if(sprintValue != 100f)
            {
                sprintValue += (sprintThreshold / 2f) * Time.deltaTime;

                playerStats.DisplayStaminaStats(sprintValue);

                if(sprintValue > 100f)
                {
                    sprintValue = 100f;
                }
            }
        }


    }// sprint method

    void Crouch()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            if (isCrouching)//if we are crouching - stand up
            {
                look_Root.localPosition = new Vector3(0f, standHeight, 0f);
                playerMovement.speed = moveSpeed;

                playerFootsteps.stepDistance = walkStepDistance;
                playerFootsteps.volumeMin = walkVolumeMin;
                playerFootsteps.volumeMax = walkVolumeMax;



                isCrouching = false;
            }
            else // if we are not crouching - crouch
            {
                look_Root.localPosition = new Vector3(0f, crouchHeight, 0f);
                playerMovement.speed = crouchSpeed;

                playerFootsteps.stepDistance = crouchStepDistance;
                playerFootsteps.volumeMin = crouchVolume;
                playerFootsteps.volumeMax = crouchVolume;

                isCrouching = true;

            }
        }


    }// crouch method
}// class end














