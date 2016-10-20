using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {


    public float driveAcceleration;
    public float maxTireSpeed;
    public float torqueAir;
    public float torqueGround;
    public float disToGround;
    public LayerMask driveableSurfaces;

    Rigidbody body;
    bool isGrounded = false;

    float tireSpeed = 0;

	void Start () {
        body = GetComponent<Rigidbody>();
	}
	
	void FixedUpdate () {

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        float a = Input.GetAxis("Accelerate");
        bool b = Input.GetButton("Handbrake");
        bool j = Input.GetButton("Jump");

        CheckGround();

        if (isGrounded)
        { // player is on ground:

            //Vector3 torque = new Vector3(0, h, 0);
            //body.AddRelativeTorque(torque * torqueGround);

            

            tireSpeed += a * driveAcceleration * Time.fixedDeltaTime;
            tireSpeed = Mathf.Clamp(tireSpeed, -maxTireSpeed, maxTireSpeed);

            float speedPercent = tireSpeed / maxTireSpeed;
            body.velocity = tireSpeed * transform.forward;

            if(a == 0){
                if(tireSpeed > 0)
                {
                    tireSpeed -= driveAcceleration * Time.fixedDeltaTime * .1f;
                    if(tireSpeed < 0)
                    {
                        tireSpeed = 0;
                    }
                }
                if (tireSpeed < 0)
                {
                    tireSpeed += driveAcceleration * Time.fixedDeltaTime * .1f;
                    if (tireSpeed > 0)
                    {
                        tireSpeed = 0;
                    }
                }
            }

            Quaternion newQ = Quaternion.AngleAxis(h * torqueGround * Time.deltaTime * speedPercent, transform.up) * transform.rotation;
            body.MoveRotation(newQ);

            //body.AddRelativeForce(new Vector3(0, 0, v * driveAcceleration));
            //body.velocity = transform.forward * body.velocity.magnitude;


        } else
        { // player is in air:

            Vector3 torque = new Vector3();
            if (b)
            {
                torque.z = h;
            } else
            {
                torque.y = h;
            }
            torque.x = v;

            body.AddRelativeTorque(torque * torqueAir);
        } 

    } // end FixedUpdate()

    void CheckGround()
    {

        Ray ray = new Ray(transform.position, transform.up * -1);
        RaycastHit hit;

        //Debug.DrawRay(ray.origin, ray.direction * disToGround);

        if(Physics.Raycast(ray, out hit, disToGround, driveableSurfaces))
        {
            isGrounded = true;
        } else
        {
            isGrounded = false;
        }

    }
}
