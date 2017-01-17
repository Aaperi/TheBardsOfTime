using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CC : MonoBehaviour {

    private int insID = 0;
    private bool canDoubleJump = false;
    public bool targetIsLocked = false;
    private TargetManager tam;
    public GameObject target;
    public List<GameObject> RefList = new List<GameObject>();

    [System.Serializable]
    public class MoveSettings {
        public float forwardVel = 12;
        public float rotateVel = 100;
        public float jumpVel = 10;
        public float doubleJumpVel = 20;
        public float distToGrounded = 1.6f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings {
        public float downAccel = .75f;
    }

    [System.Serializable]
    public class InputSettings {
        public float inputDelay = 0.1f;
        public string FORWARD_AXIS = "Vertical";
        public string SIDE_AXIS = "Horizontal";
        public string JUMP_AXIS = "Jump";
        public string ATTACK = "Fire1";
        public string SKILL = "Fire2";
        public string SPELL = "Fire3";
        public string SWAP = "SwapInstrument";
        public string LOCK = "TargetLock";
        public string NEXT = "NextTarget";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rb;
    float forwardInput, sideInput;
    bool attackInput, spellInput, skillInput, swapInput, targetLock, jumpInput, nextTarget;



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
        tam = GameObject.Find("TargetDetection").GetComponent<TargetManager>();

        forwardInput = sideInput = 0;
        attackInput = skillInput = spellInput = swapInput = targetLock = jumpInput = false;

        foreach (Transform child in GameObject.Find("Instruments").transform)
            RefList.Add(child.gameObject);

    }

    void GetInput()
    {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);
        sideInput = Input.GetAxis(inputSetting.SIDE_AXIS);
        jumpInput = Input.GetButtonDown(inputSetting.JUMP_AXIS);
        attackInput = Input.GetButtonDown(inputSetting.ATTACK);
        skillInput = Input.GetButtonDown(inputSetting.SKILL);
        spellInput = Input.GetButtonDown(inputSetting.SPELL);
        swapInput = Input.GetButtonDown(inputSetting.SWAP);
        targetLock = Input.GetButtonDown(inputSetting.LOCK);
        nextTarget = Input.GetButtonDown(inputSetting.NEXT);
    }


    void Update()
    {
        GetInput();
        Turn();

        Debug.DrawRay(transform.position, Vector3.down, Color.red);
    }

    void FixedUpdate()
    {
        if (target == null)
            targetIsLocked = false;

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

        } else {
            //no velocity
            velocity.z = 0;
        }

        if (Mathf.Abs(sideInput) > inputSetting.inputDelay && targetIsLocked) {
            //strafe
            velocity.x = moveSetting.forwardVel * sideInput;
        } else {
            //no velocity
            velocity.x = 0;
        }
    }


    void Turn()
    {
        if (Mathf.Abs(sideInput) > inputSetting.inputDelay && !targetIsLocked) {
            float direction = forwardInput;
            if (direction == 0)
                direction = 1;
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * sideInput * direction * Time.deltaTime, Vector3.up);
            transform.rotation = targetRotation;
        } else {
            rb.angularVelocity = new Vector3(0.0f, 0.0f, 0.0f);
        }
        if (targetIsLocked) {
            var targetPosition = target.transform.position;
            targetPosition.y = transform.position.y;
            transform.LookAt(targetPosition);
        }
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
        if (jumpInput && Grounded()) {
            velocity.y = moveSetting.jumpVel;
            canDoubleJump = true;
        } else if (!jumpInput && Grounded()) {
            velocity.y = 0;
        } else {
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

        if (targetLock) {
            if (!targetIsLocked) {
                target = null;
                tam.changeTarget("First");
                if (target != null) {
                    targetIsLocked = true;
                } else {
                    targetIsLocked = false;
                    target = null;
                }
                Debug.Log(targetIsLocked);
            } else {
                targetIsLocked = false;
            }
        }

        if (nextTarget) {
            if (targetIsLocked) {
                tam.changeTarget("Next");
            }
        }

        if (swapInput) {
            if (insID == 0) {
                RefList[insID].SendMessage("UnEquip");
                insID = 1;
                RefList[insID].SendMessage("Equip");
                Debug.Log("Instrument swapped into " + insID);
            } else if (insID == 1) {
                RefList[insID].SendMessage("UnEquip");
                insID = 0;
                RefList[insID].SendMessage("Equip");
                Debug.Log("Instrument swapped into " + insID);
            }
        }
    }

}
