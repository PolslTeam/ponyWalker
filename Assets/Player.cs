using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class Player : MonoBehaviour {
    static int LEGS_AMOUNT = 4;
    string[] legsNames = {"leftLeg1", "leftLeg2", "rightLeg1", "rightLeg2"};

    GameObject[] upperLegs = new GameObject[LEGS_AMOUNT];
    GameObject[] lowerLegs = new GameObject[LEGS_AMOUNT];
    HingeJoint2D[] upperHingeJoints = new HingeJoint2D[LEGS_AMOUNT];
    HingeJoint2D[] lowerHingeJoints = new HingeJoint2D[LEGS_AMOUNT];
    Rigidbody2D[] lowerRigidBodies = new Rigidbody2D[LEGS_AMOUNT];

    static int UPPER_SPEED = 100;
    static int LOWER_SPEED = 100;
    static int TORQUE = 100;
    static int MOVING_DRAG = 500;

    void Start() {
        for(int i = 0; i < LEGS_AMOUNT; i++) {
            upperLegs[i] = GameObject.Find(legsNames[i] + "Upper");
            lowerLegs[i] = GameObject.Find(legsNames[i] + "Lower");
            upperHingeJoints[i] = upperLegs[i].GetComponent<HingeJoint2D>();
            lowerHingeJoints[i] = lowerLegs[i].GetComponent<HingeJoint2D>();
            lowerRigidBodies[i] = lowerLegs[i].GetComponent<Rigidbody2D>();
        }
        Physics2D.IgnoreCollision(lowerLegs[0].GetComponent<PolygonCollider2D>(), lowerLegs[1].GetComponent<PolygonCollider2D>());
        Physics2D.IgnoreCollision(lowerLegs[2].GetComponent<PolygonCollider2D>(), lowerLegs[3].GetComponent<PolygonCollider2D>());
    }

    KeyCode[] KEYS = {KeyCode.Q, KeyCode.W, KeyCode.O, KeyCode.P};
    int[] UPPER_DIRS = {1, 1, -1, -1};

    void Update() {
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

            if(Input.GetKey(key) && uhj.jointAngle*-udir > -90) {
                um.motorSpeed = UPPER_SPEED * udir;
                um.maxMotorTorque = TORQUE;
            } else if (uhj.jointAngle*-udir < 0) {
                um.motorSpeed = UPPER_SPEED * -udir;
                um.maxMotorTorque = TORQUE;
            } else {
                um.motorSpeed = 0;
                um.maxMotorTorque = TORQUE;
            }
            uhj.motor = um;

            if(Input.GetKey(key) && uhj.jointAngle > -90) {
                lm.motorSpeed = LOWER_SPEED;
                lm.maxMotorTorque = TORQUE;

                lrb.drag = udir == 1 ? MOVING_DRAG : 0;
            } else if (lhj.jointAngle > 0) {
                lm.motorSpeed = -LOWER_SPEED;
                lm.maxMotorTorque = TORQUE;
                
                lrb.drag = udir != 1 ? MOVING_DRAG : 0;
            } else {
                lm.motorSpeed = 0;
                lm.maxMotorTorque = TORQUE;

                lrb.drag = udir != 1 ? MOVING_DRAG : 0;
            }
            lhj.motor = lm;
        }
    }
}
