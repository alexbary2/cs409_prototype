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
    public int decreasedrate = 100;

    public float time = 0;


    public UnityEngine.UI.Text batterylife_text;


    Rigidbody rb;

    void Start()
    {
        batterylife_text = GetComponent<UnityEngine.UI.Text>();
        rb = GetComponent<Rigidbody>();
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

        //SetBatterylifeText();
        
        // disable the controls if either fuelrate or batterylife is empty
        if ( fuelrate <= 0 || batterylife <= 0 ){
            return;
        }

        fuelrate -= decreasedrate * Time.deltaTime;
        batterylife -= decreasedrate * Time.deltaTime;

        //force & torque initializations
        forceVal = new Vector3(0, 0, 0);
        torqueVal = new Vector3(0, 0, 0);

                //force calculations:
        //forward propulsion
        if (Input.GetKey("w"))
        {
            forceVal += viewT.forward * forceMod;
        }
        //left propulsion
        if (Input.GetKey("a"))
        {
            forceVal += -viewT.right * forceMod;
        }
        //backwards propulsion
        if (Input.GetKey("s"))
        {
            forceVal += -viewT.forward * forceMod;
        }
        //right propulsion
        if (Input.GetKey("d"))
        {
            forceVal += viewT.right * forceMod;
        }
        //upwards propulsion
        if (Input.GetKey("space"))
        {
            forceVal += viewT.up * forceMod;
        }
        //downwards propulsion
        if (Input.GetKey("c"))
        {
            forceVal += -viewT.up * forceMod;
        }

                    //torque/rotation calcs
        //roll left
        if (Input.GetKey("q"))
        {
            torqueVal += viewT.forward * torqueMod;
        }
        //roll right
        if (Input.GetKey("e"))
        {
            torqueVal += -viewT.forward * torqueMod;
        }
        //yaw right
        if (Input.GetKey("right"))
        {
            torqueVal += viewT.up * torqueMod;
        }
        //yaw left
        if (Input.GetKey("left"))
        {
            torqueVal += -viewT.up * torqueMod;
        }
        //pitch forwards (down)
        if (Input.GetKey("up"))
        {
            torqueVal += viewT.right * torqueMod;
        }
        //pitch backwards (up)
        if (Input.GetKey("down"))
        {
            torqueVal += -viewT.right * torqueMod;
        }
        //stabilization
        if (Input.GetKey("z"))
        {
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



    //void SetBatterylifeText(){

    //    //batterylife_text.text = batterylife.ToString() ;

    //}


}
