using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {


    public bool canDoubleJump = false;
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
    }

    public MoveSettings moveSetting = new MoveSettings();
    public PhysSettings physSetting = new PhysSettings();
    public InputSettings inputSetting = new InputSettings();

    Vector3 velocity = Vector3.zero;
    Quaternion targetRotation;
    Rigidbody rb;
    float forwardInput, turnInput, jumpInput;



    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }

    bool Grounded() {
        return Physics.Raycast(transform.position, Vector3.down, moveSetting.distToGrounded, moveSetting.ground);
    }

    void Start() {
        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        forwardInput = turnInput = jumpInput = 0;
    }

    void GetInput() {
        forwardInput = Input.GetAxis(inputSetting.FORWARD_AXIS);
        turnInput = Input.GetAxis(inputSetting.TURN_AXIS);
        jumpInput = Input.GetAxisRaw(inputSetting.JUMP_AXIS);
    }


    void Update() {
        GetInput();
        Turn();

        Debug.DrawRay(transform.position, Vector3.down, Color.red);
    }

    void FixedUpdate() {
        Run();
        Jump();
        DoubleJump();
        rb.velocity = transform.TransformDirection(velocity);
    }

    void Run() {
        if(Mathf.Abs(forwardInput) > inputSetting.inputDelay) {
            //move
            velocity.z = moveSetting.forwardVel * forwardInput;

        }else {
            //no velocity
            velocity.z = 0;
        }
    }


    void Turn() {
        if (Mathf.Abs(turnInput) > inputSetting.inputDelay) {
            targetRotation *= Quaternion.AngleAxis(moveSetting.rotateVel * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }

    void DoubleJump() {
        if(Input.GetKeyDown(KeyCode.Space) && canDoubleJump) {
            //velocity.y = moveSetting.jumpVel;
            velocity.y = moveSetting.doubleJumpVel;
            canDoubleJump = false;
        }
    }
    void Jump() {
        if(jumpInput > 0 && Grounded()) {
            velocity.y = moveSetting.jumpVel;
            canDoubleJump = true;
        }
        else if(jumpInput == 0 && Grounded()) {
            velocity.y = 0;
        }
        else {
            velocity.y -= physSetting.downAccel;
        }
    }


}
