using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CC : MonoBehaviour
{
    private int insID = 0;
    private float maxDist = 20f;
    private bool canDoubleJump = false;
    public bool targetIsLocked = false;
    private MenuScript MSref;
    private TargetManager tam;
    private DialogueScript dia;
    public GameObject target;
    public List<GameObject> Instruments = new List<GameObject>();

    [HideInInspector]
    public bool inCombat = false;

    [System.Serializable]
    public class MoveSettings
    {
        public float forwardVel = 12;
        public float rotateVel = 100;
        public float jumpVel = 10;
        public float doubleJumpVel = 20;
        public float distToGrounded = 1.6f;
        public LayerMask ground;
    }

    [System.Serializable]
    public class PhysSettings
    {
        public float downAccel = .75f;
    }

    [System.Serializable]
    public class InputSettings
    {
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
        public string INTERACTION = "Interaction";
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rb;
    float forwardInput, sideInput;
    bool attackInput, spellInput, skillInput, swapInput, targetLock, jumpInput, nextTarget, intAction;



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
        dia = FindObjectOfType<DialogueScript>();
        MSref = FindObjectOfType<MenuScript>();
        tam = GetComponent<TargetManager>();

        forwardInput = sideInput = 0;
        attackInput = skillInput = spellInput = swapInput = targetLock = jumpInput = false;

        foreach (Transform child in GameObject.Find("Instruments").transform)
            Instruments.Add(child.gameObject);
        EquipByID(0);
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
        intAction = Input.GetButtonDown(inputSetting.INTERACTION);
    }


    void Update()
    {
        GetInput();
        Turn();

        Debug.DrawRay(transform.position, Vector3.down, Color.red);
    }

    void FixedUpdate()
    {
        if (target != null)
            if (!target.activeSelf)
                targetIsLocked = false;

        Run();
        Jump();
        Combat();
        DoubleJump();
        Interaction();
        rb.velocity = transform.TransformDirection(velocity);
        if (target != null)
            if (Vector3.Distance(transform.position, target.transform.position) > tam.range)
                targetIsLocked = false;
    }

    void Run()
    {
        if (Grounded()) {
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

    }


    void Turn()
    {
        if (Grounded()) {
            if (Mathf.Abs(sideInput) > inputSetting.inputDelay && !targetIsLocked) {
                float direction = forwardInput;
                if (direction == 0)
                    direction = 1;
                targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * sideInput * direction * Time.deltaTime, Vector3.up);
                transform.rotation = targetRotation;
            } else
                rb.angularVelocity = Vector3.zero;

            if (targetIsLocked && target != null) {
                var targetPosition = target.transform.position;
                targetPosition.y = transform.position.y;
                transform.LookAt(targetPosition);
            } else
                targetIsLocked = false;
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

    void LockTarget()
    {
        target = tam.getTarget("First");
        if (target != null)
            targetIsLocked = true;
    }

    void EquipByID(int ID)
    {
        foreach (GameObject go in Instruments) {
            if (go.name.Contains(Instruments[ID].name))
                go.SetActive(true);
            else
                go.SetActive(false);
        }
    }


    void Combat()
    {
        if (attackInput)
            Instruments[insID].SendMessage("Attack");

        if (skillInput)
            Instruments[insID].SendMessage("Skill");

        if (spellInput)
            Instruments[insID].SendMessage("Spell");

        if (targetLock) {
            if (!targetIsLocked) {
                target = tam.getTarget("First");
                if (target != null)
                    targetIsLocked = true;
                else
                    targetIsLocked = false;
            }
        }

        if (nextTarget)
            if (targetIsLocked)
                target = tam.getTarget("Another");

        if (swapInput) {
            insID++;
            if (insID >= Instruments.Count)
                insID = 0;
            EquipByID(insID);
        }
    }

    void Interaction()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, 4f);

        try {
            if (hit.collider.gameObject.GetComponent<Mouth>() != null) {
                if (!MSref.actionGuide.activeSelf)
                    MSref.ShowGuide("speak with " + hit.collider.name);
                if (intAction)
                    StartCoroutine(dia.dialogFromXml(hit.collider.gameObject.GetComponent<Mouth>().dialogID));
            } else {
                if (MSref.actionGuide.activeSelf)
                    MSref.HideGuide();
            }
        } catch {
            if (MSref.actionGuide.activeSelf)
                MSref.HideGuide();
        }
    }

}
