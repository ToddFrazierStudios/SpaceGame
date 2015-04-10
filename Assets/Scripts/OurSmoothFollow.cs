using UnityEngine;
using System.Collections;

public class OurSmoothFollow : MonoBehaviour {

	public Transform target;
	public float distance = 10.0f;
	public float height = 5.0f;
	public float heightDamping = 3.0f;
	public float rotationDamping = 3.0f;
	public float distanceDamping = 3.0f;
    public float scale;
	
	// Update is called once per frame
	void LateUpdate () {
		if (!target) {
			return;
		}

		Quaternion desiredRotation = target.rotation;
		Quaternion currentRotation = transform.rotation;

//		currentRotation = new Quaternion(Mathf.LerpAngle (currentRotation.x, desiredRotation.x, rotationDamping * Time.deltaTime), 
//		                                 Mathf.LerpAngle (currentRotation.y, desiredRotation.y, rotationDamping * Time.deltaTime), 
//		                                 Mathf.LerpAngle (currentRotation.z, desiredRotation.z, rotationDamping * Time.deltaTime),
//		                                 1);
        //currentRotation = target.rotation;
        currentRotation = Quaternion.Slerp (currentRotation, desiredRotation, rotationDamping * Time.deltaTime);

        //float currentDistance = transform.position.z;
        //currentDistance = Mathf.Lerp (currentDistance, target.transform.localPosition.z - distance, heightDamping * Time.deltaTime);

        //float currentHeight = Vector3.Project(transform.position - target.position, target.transform.up).y;
        //currentHeight = Mathf.Lerp (currentHeight, target.transform.localPosition.y + height, heightDamping * Time.deltaTime);

//		Vector3 currentPosition = transform.position;
		Vector3 desiredPosition = target.position;
        desiredPosition = desiredPosition - target.forward * distance;
		desiredPosition = desiredPosition + target.up * height;
        Vector3 currentPosition = transform.position;
        //float currentPositionX = transform.position.x;
        //float currentPositionY = transform.position.y;
        //float currentPositionZ = transform.position.z;
        //currentPositionX = Mathf.Lerp(currentPositionX, desiredPosition.x, distanceDamping * scale);
        //currentPositionY = Mathf.Lerp(currentPositionY, desiredPosition.y, distanceDamping * scale);
        //currentPositionZ = Mathf.Lerp(currentPositionZ, desiredPosition.z, distanceDamping * scale);

        transform.rotation = currentRotation;
        //transform.position = desiredPosition;
        //transform.position = new Vector3(currentPositionX, currentPositionY, currentPositionZ);
        //transform.position = Vector3.Slerp(currentPosition, desiredPosition, distanceDamping * Time.deltaTime);
	}
}
