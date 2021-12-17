
// This script used for camera to follow smoothly player car

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SmoothFollow : MonoBehaviour
{

	Camera currentCamera;

	public Transform target;
	// The distance in the x-z plane to the target
	public float distance = 10.0f;
	// the height we want the camera to be above the target
	public float height = 5.0f;
	// How much we
	public float heightDamping = 2.0f;
	public float rotationDamping = 3.0f;
     
	public Vector3 offset = Vector3.zero;

	// Rigidbody for smooth rotation   
	Rigidbody carRigidBody;

	// Min and max of camera field of view that will be multiplied by vehicle speed
	public float minFov = 60f,maxFov = 74f;

	// This fove is used to close the car when braking (Similar to NFS games )    
	public float brakeFov = 44f;

	// Detect vehicle braking
	public bool isBraking;

	// Read vehicle speed
	float speed;

	// Damping value to lerp with min and max camera fov * speed
	public float speedDamp = 30f;

	// Catch VehicleController2017 component
	CustomControls.CustomCarController vehicleController;

	IEnumerator Start()
	{
		currentCamera = GetComponent<Camera> ();

		yield return new WaitForEndOfFrame ();

		// Find player car by tag after game started

		// target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

		carRigidBody = target.GetComponent<Rigidbody> ();

		vehicleController = target.GetComponent<CustomControls.CustomCarController> ();



	}

	void Update ()
	{
		// Early out if we don't have a target
		if (!target)
			return;
         
		if (!carRigidBody)
			return;

		speed = carRigidBody.velocity.magnitude * 2.23693629f;

		currentCamera.fieldOfView = Mathf.Lerp (currentCamera.fieldOfView, minFov + Mathf.Abs (speed / 5), Time.deltaTime * speedDamp);
		
		// Limit(clamp) camera field of view between min and max value
		currentCamera.fieldOfView = Mathf.Clamp (currentCamera.fieldOfView, minFov, maxFov);

		// Calculate the current rotation angles
		float wantedRotationAngle = target.eulerAngles.y;    
		Vector3 pos = target.position + Quaternion.AngleAxis (wantedRotationAngle, Vector3.up) * offset;
		float wantedHeight = height + pos.y;
     
             
		float currentRotationAngle = transform.eulerAngles.y;
		float currentHeight = transform.position.y;
         
		// Smooth rotation by rigidboy  
		rotationDamping = Mathf.Lerp (0f, 3f, (carRigidBody.velocity.magnitude * 3f) / 40f);

		// Damp the rotation around the y-axis
		currentRotationAngle = Mathf.LerpAngle (currentRotationAngle, wantedRotationAngle, rotationDamping * Time.deltaTime);
     
		// Damp the height
		currentHeight = Mathf.Lerp (currentHeight, wantedHeight, heightDamping * Time.deltaTime);



		// Convert the angle into a rotation
		Quaternion currentRotation = Quaternion.Euler (0, currentRotationAngle, 0);
		;
	        
		// Set the position of the camera on the x-z plane to:
		// distance meters behind the target
		transform.position = pos;

		transform.position -= currentRotation * Vector3.forward * distance;

		// Set the height of the camera
		transform.position = new Vector3 (transform.position.x, currentHeight, transform.position.z);
         
		// Always look at the target
		transform.LookAt (pos);
	}
}
      