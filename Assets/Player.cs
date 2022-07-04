using UnityEngine.SceneManagement;
using UnityEngine;
using System;


public class Player : MonoBehaviour {
    static int LEGS_AMOUNT = 4;
    float[] keyDown = { 0.0f, 0.0f, 0.0f, 0.0f}; 
    GameObject[] legsList = new GameObject[4];
    GameObject[] lowerLegsList = new GameObject[4];
    HingeJoint2D[] hingesList = new HingeJoint2D[4];
    //HingeJoint2D[] hingesListLow = new HingeJoint2D[4];

    public Transform player;

    private string[] LEGS_NAMES = { "leftLeg1", "leftLeg2", "rightLeg1", "rightLeg2" };
    void Start()
    {
        for (int i = 0; i < 4; i++)
        {
            legsList[i] = GameObject.Find(LEGS_NAMES[i] + "Upper");
            lowerLegsList[i] = GameObject.Find(LEGS_NAMES[i] + "Lower");
            hingesList[i] = legsList[i].GetComponent<HingeJoint2D>();
            //hingesListLow[i] = lowerLegsList[i].GetComponent<HingeJoint2D>();
        }
        Physics2D.IgnoreCollision(lowerLegsList[0].GetComponent<PolygonCollider2D>(), lowerLegsList[1].GetComponent<PolygonCollider2D>());
        Physics2D.IgnoreCollision(lowerLegsList[2].GetComponent<PolygonCollider2D>(), lowerLegsList[3].GetComponent<PolygonCollider2D>());
    }
    void Update() {
        // restart on "R"
        if(Input.GetKey(KeyCode.R)) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            return;
        }

        keyDown[0] = Input.GetKey(KeyCode.Q) ? 1.0f : 0.0f;
        keyDown[1] = Input.GetKey(KeyCode.W) ? 1.0f : 0.0f;
        keyDown[2] = Input.GetKey(KeyCode.O) ? 1.0f : 0.0f;
        keyDown[3] = Input.GetKey(KeyCode.P) ? 1.0f : 0.0f;

        for (int i = 0; i < LEGS_AMOUNT; i++) {
            HingeJoint2D uh = hingesList[i];
            //HingeJoint2D lh = hingesListLow[i];
            JointMotor2D um = uh.motor;
            //JointMotor2D lm = lh.motor;
            um.maxMotorTorque = 100;
            //lm.maxMotorTorque = 100;
            if (keyDown[i].Equals(1.0f))
            {
                um.motorSpeed = -200;
                //lm.motorSpeed = 200;
            }
            else
            {
                um.motorSpeed = 300;
                //lm.motorSpeed = -300;
            }
            hingesList[i].motor = um;
            //hingesListLow[i].motor = lm;
        }
    }
}
