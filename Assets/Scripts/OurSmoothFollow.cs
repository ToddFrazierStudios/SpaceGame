using UnityEngine;
using System.Collections;

public class OurSmoothFollow : MonoBehaviour {

	public Transform target;
	public float distance = 10.0f;
	public float height = 5.0f;
	public float heightDamping = 3.0f;
	public float rotationDamping = 3.0f;
	public float distanceDamping = 3.0f;
	
	// Update is called once per frame
	void LateUpdate () {
		if (!target) {
			return;
		}

		Vector3 desiredRotation = target.eulerAngles;
		Vector3 currentRotation = transform.eulerAngles;

		currentRotation = new Vector3(Mathf.LerpAngle (currentRotation.x, desiredRotation.x, rotationDamping * Time.deltaTime), Mathf.LerpAngle (currentRotation.y, desiredRotation.y, rotationDamping * Time.deltaTime), Mathf.LerpAngle (currentRotation.z, desiredRotation.z, rotationDamping * Time.deltaTime));

		float currentDistance = transform.position.z;
		currentDistance = Mathf.Lerp (currentDistance, target.transform.localPosition.z - distance, heightDamping * Time.deltaTime);

		float currentHeight = Vector3.Project(transform.position - target.position, target.transform.up).y;
		currentHeight = Mathf.Lerp (currentHeight, target.transform.localPosition.y + height, heightDamping * Time.deltaTime);

		Vector3 currentPosition = transform.position;
		Vector3 desiredPosition = target.position;
		desiredPosition = desiredPosition - target.forward * distance;
		desiredPosition = desiredPosition + target.up * height;
		float currentPositionX = transform.position.x;
		float currentPositionY = transform.position.y;
		float currentPositionZ = transform.position.z;
		currentPositionX = Mathf.Lerp(currentPositionX, desiredPosition.x, distanceDamping * Time.deltaTime);
		currentPositionY = Mathf.Lerp(currentPositionY, desiredPosition.y, distanceDamping * Time.deltaTime);
		currentPositionZ = Mathf.Lerp(currentPositionZ, desiredPosition.z, distanceDamping * Time.deltaTime);

		transform.eulerAngles = currentRotation;
		transform.position = desiredPosition;
	}
}
