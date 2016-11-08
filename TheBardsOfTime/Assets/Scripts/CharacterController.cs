using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    public float inputDelay = 0.1f;
    public float forwardVel = 12;
    public float rotateVel = 100;

    Quaternion targetRotation;
    Rigidbody rb;
    float forwardInput, turnInput;

    public Quaternion TargetRotation
    {
        get { return targetRotation; }
    }


    void Start() {
        targetRotation = transform.rotation;
        rb = GetComponent<Rigidbody>();

        forwardInput = turnInput = 0;
    }

    void GetInput() {
        forwardInput = Input.GetAxis("Vertical");
        turnInput = Input.GetAxis("Horizontal");
    }


    void Update() {
        GetInput();
        Turn();
    }

    void FixedUpdate() {
        Run();
    }

    void Run() {
        if(Mathf.Abs(forwardInput) > inputDelay) {
            //move
            rb.velocity = transform.forward * forwardInput * forwardVel;

        }else {
            //no velocity
            rb.velocity = Vector3.zero;
        }
    }


    void Turn() {
        if (Mathf.Abs(turnInput) > inputDelay) {
            targetRotation *= Quaternion.AngleAxis(rotateVel * turnInput * Time.deltaTime, Vector3.up);
        }
        transform.rotation = targetRotation;
    }


}
