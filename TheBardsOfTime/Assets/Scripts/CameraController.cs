using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    public Transform target;

    [System.Serializable]
    public class PositionSettings
    {
        public Vector3 targetPosOffset = new Vector3(0, -3f, 0);
        public float lookSmooth = 100f;
        public float distanceFromTarget = -8f;
        public float zoomSmooth = 10f;
        public float maxZoom = -2f;
        public float minZoom = -15f;
        public bool smoothFollow = true;
        public float smooth = 0.05f;

        [HideInInspector]
        public float newDistance = -8f;
        [HideInInspector]
        public float adjustmentDistance = -8f;
    }

    [System.Serializable]
    public class OrbitSettings
    {
        public float xRotation = -20f;
        public float yRotation = -180f;
        public float maxXRotation = 25f;
        public float minXRotation = -85f;
        public float vOrbitSmooth = 150f;
        public float hOrbitSmooth = 150f;
    }

    [System.Serializable]
    public class InputSettings
    {
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";
        public string ORBIT_HORIZONTAL = "OrbitHorizontal";
        public string ORBIT_VERTICAL = "OrbitVertical";
        public string ZOOM = "Mouse ScrollWheel";
        public string MOUSE_ORBIT = "MouseOrbit";
        public string MOUSE_ORBIT_VERTICAL = "MouseOrbitVertical";
    }

    [System.Serializable]
    public class DebugSettings
    {
        public bool drawDesiredCollisionLines = true;
        public bool drawAdjustedCollisionLines = true;
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();
    public CollisionHandler collision = new CollisionHandler();
    public DebugSettings debug = new DebugSettings();

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    Vector3 adjustedDestination = Vector3.zero;
    Vector3 camVel = Vector3.zero;
    CC charController;
    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput, mouseOrbitInput, vMouseOrbitInput;
    Vector3 previousMousePos = Vector3.zero;
    Vector3 currentMousePos = Vector3.zero;

	// Use this for initialization
	void Start () {
        
        SetCameraTarget(target);

        vOrbitInput = hOrbitInput = zoomInput = hOrbitSnapInput = mouseOrbitInput = vMouseOrbitInput = 0;
        MoveToTarget();
        collision.Initialize(Camera.main);
        collision.UpdateCameraClipPoints(transform.position, transform.rotation, 
            ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, 
            ref collision.desiredCameraClipPoints);

        previousMousePos = currentMousePos = Input.mousePosition;
	}
	
	// Update is called once per frame
	void Update () {
        GetInput();
        ZoomInOnTarget();	
  
	}

    public void SetCameraTarget(Transform t)
    {
        target = t;
            if (target != null)
            {
                if (target.GetComponent<CC>())
                {
                    charController = target.GetComponent<CC>();
                }
                else
                {
                    Debug.LogError("Didn't find character controller!");
                }
            }
            else
            {
                Debug.LogError("CameraController needs a target!");
            }
    }


    private void FixedUpdate()
    {
        MoveToTarget();
        LookAtTarget();
        OrbitTarget();
        MouseOrbitTarget();

        collision.UpdateCameraClipPoints(transform.position, transform.rotation, 
            ref collision.adjustedCameraClipPoints);
        collision.UpdateCameraClipPoints(destination, transform.rotation, 
            ref collision.desiredCameraClipPoints);

        for (int i = 0; i < 5; i++)
        {
            if (debug.drawDesiredCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.desiredCameraClipPoints[i], Color.white);
            }

            if (debug.drawAdjustedCollisionLines)
            {
                Debug.DrawLine(targetPos, collision.adjustedCameraClipPoints[i], Color.green);
            }
        }

        collision.CheckColliding(targetPos);
        position.adjustmentDistance = collision.GetAdjustedDistanceWithRayFrom(targetPos);
    }

    void GetInput()
    {
        vOrbitInput = Input.GetAxisRaw(input.ORBIT_VERTICAL);
        hOrbitInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL);
        hOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL_SNAP);
        zoomInput = Input.GetAxisRaw(input.ZOOM);
        // mouseOrbitInput = Input.GetAxisRaw(input.MOUSE_ORBIT);
        // vMouseOrbitInput = Input.GetAxisRaw(input.MOUSE_ORBIT_VERTICAL);
    }

    void MoveToTarget()
    {
        targetPos = target.position + Vector3.up * position.targetPosOffset.y +
            Vector3.forward * position.targetPosOffset.z + transform.TransformDirection(Vector3.right * position.targetPosOffset.x);
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward *
            position.distanceFromTarget;
        destination += targetPos;
        //transform.position = destination;

        if (collision.colliding)
        {
            adjustedDestination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * Vector3.forward *
                position.adjustmentDistance;
            adjustedDestination += targetPos;
            if (position.smoothFollow)
            {
                transform.position = Vector3.SmoothDamp(transform.position, adjustedDestination,
                    ref camVel, position.smooth); 
            } else
            {
                transform.position = adjustedDestination;
            }

        } else
        {
            if (position.smoothFollow)
            {
                transform.position = Vector3.SmoothDamp(transform.position, destination,
                    ref camVel, position.smooth); 
            } else
            {
                transform.position = destination;
            }
        }
    }

    void LookAtTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
    }

    void OrbitTarget()
    {
        if (hOrbitSnapInput > 0)
        {
            orbit.yRotation = -180f;
        }

        orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += -hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;

        if (orbit.xRotation > orbit.maxXRotation) orbit.xRotation = orbit.maxXRotation;
        if (orbit.xRotation < orbit.minXRotation) orbit.xRotation = orbit.minXRotation;
    }

    void MouseOrbitTarget()
    {

    }

    void ZoomInOnTarget()
    {
        position.distanceFromTarget += zoomInput * position.zoomSmooth * Time.deltaTime;

        if (position.distanceFromTarget > position.maxZoom) position.distanceFromTarget = position.maxZoom;
        if (position.distanceFromTarget < position.minZoom) position.distanceFromTarget = position.minZoom;
    }

    [System.Serializable]
    public class CollisionHandler
    {
        public LayerMask collisionLayer;

        [HideInInspector]
        public bool colliding = false;
        //[HideInInspector]
        public Vector3[] adjustedCameraClipPoints;
        //[HideInInspector]
        public Vector3[] desiredCameraClipPoints;

        Camera camera;

        public void Initialize(Camera cam) {
            camera = cam;
            adjustedCameraClipPoints = new Vector3[5];
            desiredCameraClipPoints = new Vector3[5];
        }

        bool CollisionDetectedAtClipPoints(Vector3[] clipPoints, Vector3 fromPosition)
        {
            for (int i = 0; i < clipPoints.Length; i++)
            {
                Ray ray = new Ray(fromPosition, clipPoints[i] - fromPosition);
                float distance = Vector3.Distance(clipPoints[i], fromPosition);
                if (Physics.Raycast(ray, distance, collisionLayer))
                {
                    return true;
                }
            }

            return false;
        }

        public void UpdateCameraClipPoints(Vector3 cameraPosition, Quaternion atRotation, ref Vector3[] intoArray)
        {
            if (!camera)
                return;

            intoArray = new Vector3[5];

            float z = camera.nearClipPlane;
            float x = Mathf.Tan(camera.fieldOfView / 3.41f) * z;
            float y = x / camera.aspect;

            intoArray[0] = (atRotation * new Vector3(-x,  y, z)) + cameraPosition;
            intoArray[1] = (atRotation * new Vector3( x,  y, z)) + cameraPosition;
            intoArray[2] = (atRotation * new Vector3(-x, -y, z)) + cameraPosition;
            intoArray[3] = (atRotation * new Vector3( x, -y, z)) + cameraPosition;
            intoArray[4] = cameraPosition - camera.transform.forward;

            /*Debug.Log(intoArray);*/
        }

        public float GetAdjustedDistanceWithRayFrom(Vector3 from)
        {
            float distance = -1f;

            for (int i = 0; i < desiredCameraClipPoints.Length; i++)
            {
                Ray ray = new Ray(from, desiredCameraClipPoints[i] - from);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (distance == -1f || hit.distance < distance)
                        distance = hit.distance;
                }
            }

            if (distance == -1f)
                return 0f;

            return distance;
        }

        public void CheckColliding(Vector3 targetPosition)
        {
            if (CollisionDetectedAtClipPoints(desiredCameraClipPoints, targetPosition))
            {
                colliding = true;
            } else
            {
                colliding = false;
            }
        }
    }
}
