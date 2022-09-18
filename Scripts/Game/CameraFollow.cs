using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

	public Transform target; // child object of your car
	public float distance = 0; // distance of camera from car
	public float height;
	public float heightDamping = 2.0f; // smoothness
	public float xOffset = 0;
	public float lookAtHeight = 0.0f;
	public Rigidbody carRigidBody; // Car
	public float rotationSnapTime = .3F;
	public float distanceSnapTime;
	public float distanceMultiplier;

	private Vector3 lookAtVector;
	private float usedDistance;
	float wantedRotationAngle;
	float wantedHeight;
	float currentRotationAngle;
	float currentHeight;
	Vector3 wantedPosition;
	private float yVelocity = 0.0F;
	private float zVelocity = 0.0F;

	public bool moveForContinued = false;

	void Start()
	{
		lookAtVector = new Vector3(0, lookAtHeight, 0);
		height = GameObject.Find("/Car").GetComponent<DataForPlayer>().cameraAngle();
	}

	void LateUpdate()
	{
		moveCamera(Time.deltaTime);
	}

	public void setCameraToCar()
	{
		currentHeight = target.position.y + height;
		currentRotationAngle = target.eulerAngles.y;
		wantedPosition = target.position;
		wantedPosition.y = currentHeight;
		wantedPosition.x = target.position.x + xOffset;
		wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, 0);
		transform.position = wantedPosition;
		transform.LookAt(target.position + lookAtVector);
	}

	private void moveCamera(float time)
    {
		wantedHeight = target.position.y + height;
		currentHeight = transform.position.y;
		wantedRotationAngle = target.eulerAngles.y;
		currentRotationAngle = transform.eulerAngles.y;
		currentRotationAngle = Mathf.SmoothDampAngle(currentRotationAngle, wantedRotationAngle, ref yVelocity, rotationSnapTime);
		currentHeight = Mathf.Lerp(currentHeight, wantedHeight, heightDamping * time);
		wantedPosition = target.position;
		wantedPosition.y = currentHeight;
		wantedPosition.x = target.position.x + xOffset;
		usedDistance = Mathf.SmoothDampAngle(usedDistance, distance + (carRigidBody.velocity.magnitude * distanceMultiplier), ref zVelocity, distanceSnapTime);
		wantedPosition += Quaternion.Euler(0, currentRotationAngle, 0) * new Vector3(0, 0, -usedDistance);
		transform.position = wantedPosition;
		transform.LookAt(target.position + lookAtVector);
	}
}

