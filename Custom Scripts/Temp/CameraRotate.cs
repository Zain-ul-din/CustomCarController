// CameraRotate
using System.Collections;
using UnityEngine;

public class CameraRotate : MonoBehaviour
{
    public Transform targetObject;

    public Vector3 targetOffset;

    public float averageDistance = 5f;

    public float maxDistance = 20f;

    public float minDistance = 0.6f;

    public float xSpeed = 200f;

    public float ySpeed = 200f;

    public int yMinLimit = -80;

    public int yMaxLimit = 80;

    public int zoomSpeed = 40;

    public float panSpeed = 0.3f;

    public float zoomDampening = 5f;

    public float rotateOnOff = 1f;

    public float xDeg;

    public float yDeg;

    float currentDistance;

    public float desiredDistance;

    private Quaternion currentRotation;

    Quaternion desiredRotation;

    Quaternion rotation;

    Vector3 position;

    //private float idleTimer;

    private float idleSmooth;

    public static CameraRotate instance;
    void Start()
    {
    }
    private void Awake()
    {
        instance = this;
        Init();
    }



    private void OnEnable()
    {
        Init();
    }
    
    public void Init()
	{
		if (!targetObject)
		{
			GameObject gameObject = new GameObject("Cam Target");
			gameObject.transform.position = base.transform.position + base.transform.forward * averageDistance;
			targetObject = gameObject.transform;
		}

        isDragging = false;
		currentDistance = averageDistance;
		desiredDistance = averageDistance;
		position = base.transform.position;
		rotation = base.transform.rotation;
		currentRotation = base.transform.rotation;
		desiredRotation = base.transform.rotation;
		//xDeg = Vector3.Angle(Vector3.right, base.transform.right);
		//yDeg = Vector3.Angle(Vector3.up, base.transform.up);
		position = targetObject.position - (rotation * Vector3.forward * currentDistance + targetOffset);
	}

    IEnumerator SetPos(float  x, float y, float dd)
    {
        int loopCount = 20;
        float xStep = Mathf.Abs(((x - xDeg) / loopCount ));
        float yStep = Mathf.Abs(((y - yDeg) / loopCount));
        float desiredDistanceStep = Mathf.Abs(((dd - desiredDistance) / loopCount));

        for (int i = 0; i < loopCount; i++)
        {
            yield return null;
            xDeg = Mathf.MoveTowards(xDeg, x, xStep);
            yDeg = Mathf.MoveTowards(yDeg, y, yStep);
            desiredDistance = Mathf.MoveTowards(desiredDistance, dd, desiredDistanceStep);
        }
    }





    [HideInInspector]
    public bool isDragging = false;
	private void LateUpdate()
	{
        //if (Input.GetMouseButton(0) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKey(KeyCode.LeftControl))
        //{
        //    desiredDistance -= Input.GetAxis("Mouse Y") * 0.02f * (float)zoomSpeed * 0.125f * Mathf.Abs(desiredDistance);
        //}
        //else


        //if (Input.GetMouseButton(0))
        if (isDragging)
        {
            xDeg += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            xDeg = ClampAngle(xDeg, -360, 360);
            yDeg -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0f);
            currentRotation = base.transform.rotation;
            rotation = Quaternion.Lerp(currentRotation, desiredRotation, 0.02f * zoomDampening);
            base.transform.rotation = rotation;
            //idleTimer = 0f;
            idleSmooth = 0f;
        }
        else
        {
            //idleTimer += 0.02f;
            //if (idleTimer > rotateOnOff && rotateOnOff > 0f)
            //{
            //	idleSmooth += (0.02f + idleSmooth) * 0.005f;
            //	idleSmooth = Mathf.Clamp(idleSmooth, 0f, 1f);
            //	xDeg += xSpeed * 0.001f * idleSmooth;
            //}

            xDeg += (10 * Time.deltaTime);
            yDeg = ClampAngle(yDeg, yMinLimit, yMaxLimit);
            desiredRotation = Quaternion.Euler(yDeg, xDeg, 0f);
            currentRotation = base.transform.rotation;
            rotation = Quaternion.Lerp(currentRotation, desiredRotation, 0.02f * zoomDampening * 2f);
            base.transform.rotation = rotation;
        }
		//desiredDistance -= Input.GetAxis("Mouse ScrollWheel") * 0.02f * (float)zoomSpeed * Mathf.Abs(desiredDistance);
		desiredDistance = Mathf.Clamp(desiredDistance, minDistance, maxDistance);
		currentDistance = Mathf.Lerp(currentDistance, desiredDistance, 0.02f * zoomDampening);
		position = targetObject.position - (rotation * Vector3.forward * currentDistance + targetOffset);
		base.transform.position = position;
	}

	private static float ClampAngle(float angle, float min, float max)
	{
        if (angle < -360f)
        {
            angle += 360f;
        }
        if (angle > 360f)
        {
            angle -= 360f;
        }
        return Mathf.Clamp(angle, min, max);
    }


    public void OnBeginDrag()
    {
        isDragging = true;
    }
    public void OnEndrag()
    {
        isDragging = false;
    }
}