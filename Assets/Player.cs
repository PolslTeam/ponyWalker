using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class Player : MonoBehaviour {
    static int LEGS_AMOUNT = 4;
    string[] LEGS_NAMES = {"leftLeg1", "leftLeg2", "rightLeg1", "rightLeg2"};
    static int UPPER_SPEED = 100;
    static int LOWER_SPEED = 100;
    static int MOVING_DRAG = 500;
    static int MOVING_TORQUE = 100;
    // this torque could be smaller to make legs more soft when not moving them
    static int REST_TORQUE = 1000;


    GameObject[] upperLegs = new GameObject[LEGS_AMOUNT];
    GameObject[] lowerLegs = new GameObject[LEGS_AMOUNT];
    HingeJoint2D[] upperHingeJoints = new HingeJoint2D[LEGS_AMOUNT];
    HingeJoint2D[] lowerHingeJoints = new HingeJoint2D[LEGS_AMOUNT];
    Rigidbody2D[] lowerRigidBodies = new Rigidbody2D[LEGS_AMOUNT];
    void Start() {
        for(int i = 0; i < LEGS_AMOUNT; i++) {
            upperLegs[i] = GameObject.Find(LEGS_NAMES[i] + "Upper");
            lowerLegs[i] = GameObject.Find(LEGS_NAMES[i] + "Lower");
            upperHingeJoints[i] = upperLegs[i].GetComponent<HingeJoint2D>();
            lowerHingeJoints[i] = lowerLegs[i].GetComponent<HingeJoint2D>();
            lowerRigidBodies[i] = lowerLegs[i].GetComponent<Rigidbody2D>();
        }
        // prevent colliding between legs
        Physics2D.IgnoreCollision(lowerLegs[0].GetComponent<PolygonCollider2D>(), lowerLegs[1].GetComponent<PolygonCollider2D>());
        Physics2D.IgnoreCollision(lowerLegs[2].GetComponent<PolygonCollider2D>(), lowerLegs[3].GetComponent<PolygonCollider2D>());
    }

    // direction that leg moves on click
    int[] UPPER_DIRS = {1, 1, -1, -1};
    KeyCode[] KEYS = {KeyCode.Q, KeyCode.W, KeyCode.O, KeyCode.P};

    void Update() {
        // restart on "R"
        if(Input.GetKey(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        for(int i = 0; i < LEGS_AMOUNT; i++) {
            HingeJoint2D uhj = upperHingeJoints[i];
            HingeJoint2D lhj = lowerHingeJoints[i];
            JointMotor2D um = uhj.motor;
            JointMotor2D lm = lhj.motor;
            Rigidbody2D lrb = lowerRigidBodies[i];

            KeyCode key = KEYS[i];
            int udir = UPPER_DIRS[i];

            bool keyPressed = Input.GetKey(key);

            // handle upper part of leg
            bool isUpperMaxLimitReached = uhj.jointAngle * udir > 90;
            bool isUpperMinLimitReached = uhj.jointAngle * udir < 0;

            if(keyPressed && !isUpperMaxLimitReached) {
                um.motorSpeed = UPPER_SPEED * udir;
                um.maxMotorTorque = MOVING_TORQUE;
            }
            else if (!keyPressed && !isUpperMinLimitReached) {
                um.motorSpeed = UPPER_SPEED * -udir;
                um.maxMotorTorque = MOVING_TORQUE;
            }
            else {
                um.motorSpeed = 0;
                um.maxMotorTorque = REST_TORQUE;
            }
            uhj.motor = um;

            // handle lower part of leg
            bool isLowerMaxLimitReached = lhj.jointAngle > 90;
            bool isLowerMinLimitReached = lhj.jointAngle < 0;

            if(keyPressed && !isLowerMaxLimitReached) {
                lm.motorSpeed = LOWER_SPEED;
                lm.maxMotorTorque = MOVING_TORQUE;

                // drag only when moving leg back in the relation to the scene:
                // back legs move backward on click, front moves forward, so back
                // legs grip on click, and front legs grip when not clicking
                lrb.drag = udir == 1 ? MOVING_DRAG : 0;
            }
            else if (!keyPressed && !isLowerMinLimitReached) {
                lm.motorSpeed = -LOWER_SPEED;
                lm.maxMotorTorque = MOVING_TORQUE;
                
                lrb.drag = udir != 1 ? MOVING_DRAG : 0;
            }
            else {
                lm.motorSpeed = 0;
                lm.maxMotorTorque = REST_TORQUE;

                // don't drag when leg is not moving
                // to allow movement of horse by other legs
                lrb.drag = 0;
            }
            lhj.motor = lm;
        }
    }
}
