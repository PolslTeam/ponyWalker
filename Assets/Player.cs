using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;


public class Player : MonoBehaviour {
    static int LEGS_AMOUNT = 4;
    string[] LEGS_NAMES = {"leftLeg1", "leftLeg2", "rightLeg1", "rightLeg2"};
    static int UPPER_SPEED = 100;
    static int LOWER_SPEED = 150;
    static int MOVING_DRAG = 500;
    static int MOVING_TORQUE = 100;
    static int REST_TORQUE = 100;
    static int FRONT_LOW_TORQUE = 20;


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
            GameObject ul = upperLegs[i];
            GameObject ll = lowerLegs[i];

            KeyCode key = KEYS[i];
            int udir = UPPER_DIRS[i];

            bool isKeyPressed = Input.GetKey(key);

            // handle upper part of leg
            bool isUpperMaxLimitReached = Math.Abs(uhj.jointAngle) > 90;

            if(isKeyPressed) {
                um.motorSpeed = isUpperMaxLimitReached ? 0 : UPPER_SPEED * udir;
                um.maxMotorTorque = MOVING_TORQUE;
                // lowerLegs[0].GetComponent<FixedJoint2D>().connectedAnchor = new Vector2((float) -0.2457747, (float) (-0.603699 + 0.1));
            }
            else {
                um.motorSpeed = UPPER_SPEED * -uhj.jointAngle / 15;
                um.maxMotorTorque = MOVING_TORQUE;
            }
            uhj.motor = um;

            // handle lower part of leg (only front legs)
            if(i < 2) {
                // float angleWanted = -lhj.jointAngle - uhj.jointAngle;
                float angleWanted = -lhj.jointAngle;
                if(isKeyPressed)
                    angleWanted += 20;
                else
                    angleWanted -= 10;

                lm.motorSpeed = LOWER_SPEED * angleWanted / 15;
                lm.maxMotorTorque = MOVING_TORQUE;
            }
            else {
                float angleToBody = -lhj.jointAngle - uhj.jointAngle;
                lm.motorSpeed = LOWER_SPEED * angleToBody / 20;
                lm.maxMotorTorque = MOVING_TORQUE * Math.Abs(angleToBody) / 20;
            }
            lhj.motor = lm;
        }
    }
}
