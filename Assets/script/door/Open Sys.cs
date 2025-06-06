using UnityEngine;
using UnityEngine.UIElements;

public class OpenSys : MonoBehaviour
{
    public GameObject pivot;
    public float Speed = 90f;

    private float currentAngle = 0f;
    private float targetAngle = 0f;

    private bool active;

    void Update()
    {
        if (active)
        {
            targetAngle = 90;
        }
        else
        {
            targetAngle = 0;
        }
        Debug.Log(targetAngle);

        currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, Speed * Time.deltaTime);

        pivot.transform.localRotation = Quaternion.Euler(0f, currentAngle, 0f);
    }

    private void OnTriggerStay(Collider other)
    {
        active = true;
    }

    private void OnTriggerExit(Collider other)
    {
        active = false;
    }
}
