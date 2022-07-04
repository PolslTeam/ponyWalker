using UnityEngine.SceneManagement;
using UnityEngine;
using System;


public class Player : MonoBehaviour {
    static int LEGS_AMOUNT = 4;
    string[] LEGS_NAMES = {"leftLeg1", "leftLeg2", "rightLeg1", "rightLeg2"};
    static int UPPER_SPEED = 150;
    static int LOWER_SPEED = 250;
    static int MOVING_TORQUE = 100;
    static int UPPER_MAX_BACK_LIMIT = 60; // [deg]
    static int UPPER_MAX_FRONT_LIMIT = 75; // [deg]

    PhysicsMaterial2D frictionHigh;
    PhysicsMaterial2D frictionLow;

    GameObject[] upperLegs = new GameObject[LEGS_AMOUNT];
    GameObject[] lowerLegs = new GameObject[LEGS_AMOUNT];
    HingeJoint2D[] upperHingeJoints = new HingeJoint2D[LEGS_AMOUNT];
    HingeJoint2D[] lowerHingeJoints = new HingeJoint2D[LEGS_AMOUNT];
    Rigidbody2D[] lowerRigidBodies = new Rigidbody2D[LEGS_AMOUNT];
    PolygonCollider2D[] lowerPoligonColiders = new PolygonCollider2D[LEGS_AMOUNT];
    void Start() {
        for(int i = 0; i < LEGS_AMOUNT; i++) {
            upperLegs[i] = GameObject.Find(LEGS_NAMES[i] + "Upper");
            lowerLegs[i] = GameObject.Find(LEGS_NAMES[i] + "Lower");
            upperHingeJoints[i] = upperLegs[i].GetComponent<HingeJoint2D>();
            lowerHingeJoints[i] = lowerLegs[i].GetComponent<HingeJoint2D>();
            lowerRigidBodies[i] = lowerLegs[i].GetComponent<Rigidbody2D>();
            lowerPoligonColiders[i] = lowerLegs[i].GetComponent<PolygonCollider2D>();
        }
        // prevent colliding between legs
        Physics2D.IgnoreCollision(lowerLegs[0].GetComponent<PolygonCollider2D>(), lowerLegs[1].GetComponent<PolygonCollider2D>());
        Physics2D.IgnoreCollision(lowerLegs[2].GetComponent<PolygonCollider2D>(), lowerLegs[3].GetComponent<PolygonCollider2D>());

        // load materials (yes, this is very stupid way to do it)
        frictionHigh = lowerPoligonColiders[0].sharedMaterial;
        frictionLow  = lowerPoligonColiders[1].sharedMaterial;
    }

    // direction that leg moves on click
    int[] UPPER_DIRS = {-1, -1, -1, -1};
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
            PolygonCollider2D lpc = lowerPoligonColiders[i];

            KeyCode key = KEYS[i];
            int udir = UPPER_DIRS[i];

            bool isKeyPressed = Input.GetKey(key);
            bool isUpperMaxLimitReached = false;

            // handle upper part of leg
            if (i < 2)
            {
                isUpperMaxLimitReached = Math.Abs(uhj.jointAngle) > UPPER_MAX_BACK_LIMIT;
            }
            else
            {
                isUpperMaxLimitReached = Math.Abs(uhj.jointAngle) > UPPER_MAX_FRONT_LIMIT;
            }
            

            if(isKeyPressed) {
                // other direction of move in frond and back legs
                float speedWithDir = UPPER_SPEED * udir;
                // move more if not reached limit
                um.motorSpeed = isUpperMaxLimitReached ? 0 : speedWithDir;
                um.maxMotorTorque = MOVING_TORQUE;
            }
            else {
                // stabilise leg to neutral position, and make is slower near the center point
                float speed = UPPER_SPEED * -uhj.jointAngle / 15;
                // but not too fast
                um.motorSpeed = Math.Min(Math.Max(speed, -UPPER_SPEED), UPPER_SPEED);

                um.maxMotorTorque = MOVING_TORQUE; // same as other one...
            }
            uhj.motor = um;

            // try to stabilize back lower legs quite hardly
            if(i < 2) {
                // float angleWanted = -lhj.jointAngle - uhj.jointAngle;
                float angleWanted = -lhj.jointAngle;
                // if key is pressed, move them slightly
                angleWanted += isKeyPressed ? 90 : 15;

                lm.motorSpeed = LOWER_SPEED * angleWanted / 50;
                lm.maxMotorTorque = MOVING_TORQUE;

                lrb.drag = !isKeyPressed ? 2 : 1;
                lpc.sharedMaterial = !isKeyPressed ? frictionHigh : frictionLow;
            }
            // front legs more relaxed
            else {
                // stabilize with relation to the torso, not upper leg
                float angleToBody = -lhj.jointAngle - uhj.jointAngle * 1.3f;
                lm.motorSpeed = (LOWER_SPEED * angleToBody) / 5;
                lm.maxMotorTorque = MOVING_TORQUE * Math.Abs(angleToBody) * 0.4f;

            }
            lhj.motor = lm;
        }
    }
}
