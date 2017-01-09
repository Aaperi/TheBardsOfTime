﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CC : MonoBehaviour {

    private bool canDoubleJump = false;
    private int insID = 1;
    public List<GameObject> RefList = new List<GameObject>();

    [System.Serializable]
    public class MoveSettings {
        public float forwardVel = 12;
        public float rotateVel = 100;
        public float jumpVel = 25;
        public float doubleJumpVel = 50;
        public float distToGrounded = 1.6f;
        public LayerMask ground;

    }

    [System.Serializable]
    public class PhysSettings {
        public float downAccel = 0.75f;
    }

    [System.Serializable]
    public class InputSettings {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string TURN_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
        public string ATTACK = "Fire1";
        public string SKILL = "Fire2";
        public string SPELL = "Fire3";
        public string SWAP = "SwapInstrument";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rb;
    float forwardInput, turnInput, jumpInput;
    bool attackInput, spellInput, skillInput, swapInput;



    public Quaternion TargetRotation {
        get { return targetRotation; }
    }

    bool Grounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }

    void Start()
    {
        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        forwardInput = turnInput = jumpInput = 0;
        attackInput = skillInput = spellInput = swapInput = false;

        foreach (Transform child in GameObject.Find("Instruments").transform)
            RefList.Add(child.gameObject);

    }

    void GetInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS);
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS);
        attackInput = Input.GetButtonDown(inputSetting.ATTACK);
        skillInput = Input.GetButtonDown(inputSetting.SKILL);
        spellInput = Input.GetButtonDown(inputSetting.SPELL);
        swapInput = Input.GetButtonDown(inputSetting.SWAP);
    }


    void Update()
    {
        GetInput();
        Turn();

        Debug.DrawRay(transform.position, Vector3.down, Color.red);
    }

    void FixedUpdate()
    {
        Run();
        Jump();
        Combat();
        DoubleJump();
        rb.velocity = transform.TransformDirection(velocity);
    }

    void Run()
    {
        if (Mathf.Abs(forwardInput) > inputSetting.inputDelay) {
            //move
            velocity.z = moveSetting.forwardVel * forwardInput;

        }
        else {
            //no velocity
            velocity.z = 0;
        }
    }


    void Turn()
    {
        if (Mathf.Abs(turnInput) > inputSetting.inputDelay) {
            float direction = forwardInput;
            if (direction == 0)
                direction = 1;
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * turnInput * direction * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    void DoubleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canDoubleJump) {
            //velocity.y = moveSetting.jumpVel;
            velocity.y = moveSetting.doubleJumpVel;
            canDoubleJump = false;
        }
    }
    void Jump()
    {
        if (jumpInput > 0 && Grounded()) {
            velocity.y = moveSetting.jumpVel;
            canDoubleJump = true;
        }
        else if (jumpInput == 0 && Grounded()) {
            velocity.y = 0;
        }
        else {
            velocity.y -= physSetting.downAccel;
        }
    }

    void Combat()
    {
        if (attackInput) {
            RefList[insID].SendMessage("Attack");
        }

        if (skillInput) {
            RefList[insID].SendMessage("Skill");
        }

        if (spellInput) {
            RefList[insID].SendMessage("Spell");
        }

        if (swapInput) {
            if(insID == 0) {
                RefList[insID].SendMessage("UnEquip");
                insID = 1;
                RefList[insID].SendMessage("Equip");
                Debug.Log("Instrument swapped into " + insID);
            }
            else if(insID == 1) {
                RefList[insID].SendMessage("UnEquip");
                insID = 0;
                RefList[insID].SendMessage("Equip");
                Debug.Log("Instrument swapped into " + insID);
            }
        }
    }

}
