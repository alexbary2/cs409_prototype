using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserController : MonoBehaviour {
    //Transform object used to change user's position/view
    public Transform viewT;
    //Force modifiers for force and torque
    public float forceMod = 5, torqueMod = 0.5f;
    //Vector3 objects representing values for torque & force
    public Vector3 forceVal, torqueVal;


    // initialize fuel rate and battery life
    public float fuelrate = 1000000;
    public float batterylife = 10000000;
    public float initialFuelrate = 1000000/100;
    public float initialBatterylife = 10000000/100;
    public int decreasedrate = 100;

    public float time = 0;

    public Text fuelText;
    public Text batteryText;
    public Text speedText;

    private int infoCycle = 0;
    private int timer = 0;

    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        SetFuelText();
        SetBatteryText();
        SetSpeedText();
    }

    private void OnCollisionEnter(Collision collision)
    {
        rb.angularVelocity = Vector3.zero;
    }

    private void OnCollisionStay(Collision collision)
    {
        rb.angularVelocity = Vector3.zero;
    }

    private void FixedUpdate()
    {
        //Checks to see if it should quit right away
        if (Input.GetKey(KeyCode.Escape)) {
            Application.Quit();
        }

        //Cycles through information
        if (Input.GetKey("i") && timer<0) {
            if(infoCycle < 2) {
                infoCycle++;
            } else {
                infoCycle = 0;
            }
            timer = 30;
        }
        timer--;

        SetFuelText();
        SetBatteryText();
        SetSpeedText();
        
        // disable the controls if either fuelrate or batterylife is empty
        if ( fuelrate <= 0 || batterylife <= 0 ) {
            return;
        }

        batterylife -= decreasedrate * Time.deltaTime;

        //force & torque initializations
        forceVal = new Vector3(0, 0, 0);
        torqueVal = new Vector3(0, 0, 0);

                //force calculations:
        //forward propulsion
        if (Input.GetKey("w"))
        {
            updateForce(viewT.forward);
        }
        //left propulsion
        if (Input.GetKey("a"))
        {
            updateForce(-viewT.right);
        }
        //backwards propulsion
        if (Input.GetKey("s"))
        {
            updateForce(-viewT.forward);
        }
        //right propulsion
        if (Input.GetKey("d"))
        {
            updateForce(viewT.right);
        }
        //upwards propulsion
        if (Input.GetKey("space"))
        {
            updateForce(viewT.up);
        }
        //downwards propulsion
        if (Input.GetKey("c"))
        {
            updateForce(-viewT.up);
        }

                    //torque/rotation calcs
        //roll left
        if (Input.GetKey("q"))
        {
            updateTorque(viewT.forward);
        }
        //roll right
        if (Input.GetKey("e"))
        {
            updateTorque(-viewT.forward);
        }
        //yaw right
        if (Input.GetKey("right"))
        {
            updateTorque(viewT.up);
        }
        //yaw left
        if (Input.GetKey("left"))
        {
            updateTorque(-viewT.up);
        }
        //pitch forwards (down)
        if (Input.GetKey("up"))
        {
            updateTorque(viewT.right);
        }
        //pitch backwards (up)
        if (Input.GetKey("down"))
        {
            updateTorque(-viewT.right);
        }
        //stabilization
        if (Input.GetKey("z"))
        {
            fuelrate -= decreasedrate * Time.deltaTime;
            forceVal = -rb.velocity * forceMod * 0.3f;
            //torqueVal = -rb.angularVelocity.normalized * torqueMod * 1f;
            if (rb.angularVelocity.magnitude > 2f * Time.deltaTime)
            {
               rb.AddTorque(-(rb.angularVelocity/4) * torqueMod, ForceMode.Force);
            }
            else
            {
                rb.angularVelocity = Vector3.zero;
            }

            Debug.Log("Stabilizing User");
        }

        //apply force
        rb.AddForce(forceVal);
        //apply torque
        rb.AddTorque(torqueVal);
    }

    private void updateForce(Vector3 forceChange) {
        forceVal += forceChange * forceMod;
        fuelrate -= decreasedrate * Time.deltaTime;
    }
    private void updateTorque(Vector3 torqueChange) {
        torqueVal += torqueChange * torqueMod;
        fuelrate -= decreasedrate * Time.deltaTime;
    }

    private void SetFuelText() {
        if(infoCycle != 2) {
            fuelText.text = "Fuel: " + (fuelrate/initialFuelrate).ToString("N2") + "%";
        } else {
            fuelText.text = "";
        }

    }

    private void SetBatteryText() {
        if(infoCycle != 2) {
            batteryText.text = "Battery: " + (batterylife/initialBatterylife).ToString("N2") + "%";
        } else {
            batteryText.text = "";
        }
        
    }

    private void SetSpeedText() {
        if(infoCycle != 1) {
            speedText.text = "Speed: " + rb.velocity.magnitude.ToString("N2") + " m/s";
        } else {
            speedText.text = "";
        }
    }

}