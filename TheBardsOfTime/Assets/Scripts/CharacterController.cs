using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour {

    public float inputDelay = 0.1f;
    public float forwardVel = 12;
    public float rotateVel = 100;
    public Collider[] attackHitBoxes;

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
        if (Input.GetKeyDown(KeyCode.F))
            Attack(attackHitBoxes[0]);
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

    void Attack(Collider col)
    {
        Collider[] cols = Physics.OverlapBox(col.bounds.center, col.bounds.extents, col.transform.rotation, LayerMask.GetMask("Hitbox"));
        foreach (Collider c in cols)
        {
            if (c.transform.parent == transform)
                continue;

            Debug.Log(c.name);

            c.SendMessageUpwards("TakeDamage", 7f);
        }
    }


}
