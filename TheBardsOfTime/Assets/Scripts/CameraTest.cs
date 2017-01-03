﻿using UnityEngine;
using System.Collections;

public class CameraTest : MonoBehaviour {

    public Transform target;

    [System.Serializable]
    public class PositionSettings {
        public Vector3 targetPosOffset = new Vector3(0, 0, 0);
        public float lookSmooth = 100f;
        public float distanceFromTarget = -8;
        public float zoomSmooth = 10;

        public float maxZoom = -2;
        public float minZoom = -15;
    }

    [System.Serializable]
    public class OrbitSettings {
        public float xRotation = -20;
        public float yRotation = -180;

        public float maxRotation = 25;
        public float minRotation = -85;

        public float vOrbitSmooth = 150;
        public float hOrbitSmooth = 150;
    }

    [System.Serializable]
    public class InputSettings {
        public string ORBIT_HORIZONTAL_SNAP = "OrbitHorizontalSnap";
        public string ORBIT_HORIZONTAL = "OrbitHorizontal";
        public string ORBIT_VERTICAL = "OrbitVertical";
        public string ZOOM = "Mouse ScrollWheel";
    }

    public PositionSettings position = new PositionSettings();
    public OrbitSettings orbit = new OrbitSettings();
    public InputSettings input = new InputSettings();

    Vector3 targetPos = Vector3.zero;
    Vector3 destination = Vector3.zero;
    CharacterController charController;
    float vOrbitInput, hOrbitInput, zoomInput, hOrbitSnapInput;

    void Start() {
        SetCameraTarget(target);

        //Move to target
        targetPos = target.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos;
        transform.position = destination;
    }

    void SetCameraTarget(Transform t) {
        target = t;
        charController = target.GetComponent<CharacterController>();
    }

    void CameraRotate() {
        vOrbitInput     = Input.GetAxisRaw(input.ORBIT_VERTICAL);
        hOrbitInput     = Input.GetAxisRaw(input.ORBIT_HORIZONTAL);
    }
    
    void GetInput() {
        hOrbitSnapInput = Input.GetAxisRaw(input.ORBIT_HORIZONTAL_SNAP);
        zoomInput = Input.GetAxisRaw(input.ZOOM);
    }
    
    void Update() {
        if (Input.GetMouseButton(1)) {
            CameraRotate();
            OrbitTarget();
        }
        GetInput();
        ZoomInTarget();
    }

    void LateUpdate() {
        //moving
        MoveToTarget();
        //rotating
        LookAtTarget();
    }

    void MoveToTarget() {
        targetPos = target.position + position.targetPosOffset;
        destination = Quaternion.Euler(orbit.xRotation, orbit.yRotation + target.eulerAngles.y, 0) * -Vector3.forward * position.distanceFromTarget;
        destination += targetPos;
        transform.position = destination;
    }

    void LookAtTarget() {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos - transform.position);
        transform.rotation = targetRotation;//Quaternion.Lerp(transform.rotation, targetRotation, position.lookSmooth * Time.deltaTime);
    }

    void OrbitTarget() {
        if(hOrbitSnapInput > 0) {
            orbit.yRotation = -180;
        }

        orbit.xRotation += -vOrbitInput * orbit.vOrbitSmooth * Time.deltaTime;
        orbit.yRotation += -hOrbitInput * orbit.hOrbitSmooth * Time.deltaTime;

        if(orbit.xRotation > orbit.maxRotation) {
            orbit.xRotation = orbit.maxRotation;
        }

        if (orbit.xRotation < orbit.minRotation) {
            orbit.xRotation = orbit.minRotation;
        }
    }

    void ZoomInTarget() {
        position.distanceFromTarget += zoomInput * position.zoomSmooth;

        if(position.distanceFromTarget > position.maxZoom) {
            position.distanceFromTarget = position.maxZoom;
        }
        if(position.distanceFromTarget < position.minZoom) {
            position.distanceFromTarget = position.minZoom;
        }
    }
}
