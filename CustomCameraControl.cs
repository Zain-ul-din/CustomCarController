
using UnityEngine;
using System.Collections;

namespace CustomControls
{
	public class CustomCameraControl : MonoBehaviour
	{

	public Transform target;
	[SerializeField] private Transform carCOM;

	public float distance= 1.5f;

	public int cameraSpeed= 5;



	public float xSpeed= 175.0f;

	public float ySpeed= 75.0f;

	public float pinchSpeed;

	private float lastDist = 0;

	private float curDist = 0;



	public int yMinLimit= 10; //Lowest vertical angle in respect with the target.

	public int yMaxLimit= 80;

	public int xLimMin = -90, xLimMax = 140;

	public float minDistance= .5f; //Min distance of the camera from the target

	public float maxDistance= 1.5f;



	private float x= 0.0f;

	private float y= 0.0f;

	private Touch touch;


	public bool canDrag,isGarage;

	public float newXValue = -15f;

	IEnumerator  Start (){

		Vector3 angles= transform.eulerAngles;

		x = angles.y;

		y = angles.x;



		// Make the rigid body not change rotation

		if (GetComponent<Rigidbody>())

			GetComponent<Rigidbody>().freezeRotation = true;

		yield return new  WaitForSeconds(0.3f);
		


		x = newXValue;
	}


	public float yOffset;
    Vector3 refVel = Vector3.zero;
	void  Update (){
		if (canDrag) {
			if (target) {



				//Zooming with mouse

				distance += Input.GetAxis ("Mouse ScrollWheel") * distance;

				distance = Mathf.Clamp (distance, minDistance, maxDistance);





				if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Moved) {

					//One finger touch does orbit

					touch = Input.GetTouch (0);

					x += touch.deltaPosition.x * xSpeed * 0.02f;

					y -= touch.deltaPosition.y * ySpeed * 0.02f;

				}

				if (Input.touchCount > 1 && (Input.GetTouch (0).phase == TouchPhase.Moved || Input.GetTouch (1).phase == TouchPhase.Moved)) {

					//Two finger touch does pinch to zoom

					var touch1 = Input.GetTouch (0);

					var touch2 = Input.GetTouch (1);

					curDist = Vector2.Distance (touch1.position, touch2.position);

					if (curDist > lastDist) {

						distance += Vector2.Distance (touch1.deltaPosition, touch2.deltaPosition) * pinchSpeed / 10;

					} else {

						distance -= Vector2.Distance (touch1.deltaPosition, touch2.deltaPosition) * pinchSpeed / 10;

					}



					lastDist = curDist;

				}

				//Detect mouse drag;
				#if UNITY_EDITOR
				if (Input.GetMouseButton (0)) {

					x += Input.GetAxis ("Mouse X") * xSpeed * 0.02f;

					y -= Input.GetAxis ("Mouse Y") * ySpeed * 0.02f;       

				}
				#else
				if(Input.touchCount == 2)   {

				x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;

				y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;       

				}
				#endif

				y = ClampAngle (y, yMinLimit, yMaxLimit);





				Quaternion rotation = Quaternion.Euler (y, x, 0);

				Vector3 vTemp = new Vector3 (0.0f, 0.0f, -distance);

				Vector3 position = rotation * vTemp + new Vector3(target.position.x,target.position.y+yOffset,target.position.z);



				transform.position = Vector3.Lerp (transform.position, position, cameraSpeed );

				transform.rotation = rotation;

				target.rotation = Quaternion.Lerp(target.rotation , carCOM.rotation , 0.4f);

			}

		}
	}



	static float  ClampAngle ( float angle ,   float min ,   float max  ){

		if (angle < -360)

			angle += 360;

		if (angle > 360)

			angle -= 360;

		return Mathf.Clamp (angle, min, max);

	}



}
}