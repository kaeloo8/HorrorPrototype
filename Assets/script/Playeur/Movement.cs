using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float sprintSpeed = 4f;
    public float jumpForce = 5f;
    public float gravityForce = 5f;
    public float staminaMax = 100f;
    public float staminaRegen = 1f;
    public float staminaCost = 1f;
    public float overRunLimit = 20f;

    private float stamina;
    private bool isGrounded = true;
    private bool isRunning = false;
    private bool isOverRun = false;

    public Slider staminaSlider;
    public Camera cam;
    private Rigidbody rb;

    public float mouseSensitivity = 2f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        rb = GetComponent<Rigidbody>();
        stamina = staminaMax;
    }

    void Update()
    {
        MoveCamera();
        MovePlayer();
        HandleStamina();
    }

    void MoveCamera()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        transform.Rotate(0f, mouseX, 0f);
        cam.transform.Rotate(-mouseY, 0f, 0f);
    }

    void MovePlayer()
    {
        Vector3 moveDir = Vector3.zero;

        if (Input.GetKey(KeyCode.W)) moveDir += transform.forward;
        if (Input.GetKey(KeyCode.S)) moveDir -= transform.forward;
        if (Input.GetKey(KeyCode.A)) moveDir -= transform.right;
        if (Input.GetKey(KeyCode.D)) moveDir += transform.right;

        moveDir.Normalize();

        float speed = moveSpeed;

        if (Input.GetKey(KeyCode.LeftShift) && !isOverRun)
        {
            isRunning = true;
            speed = sprintSpeed;
        }
        else
        {
            isRunning = false;
        }

        Vector3 velocity = new Vector3(moveDir.x * speed, rb.velocity.y, moveDir.z * speed);
        rb.velocity = velocity;

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
        }
    }

    void HandleStamina()
    {

        if (isRunning)
        {
            stamina -= staminaCost * Time.deltaTime;
        }
        else if (stamina < staminaMax)
        {
            stamina += staminaRegen * Time.deltaTime;
        }

        stamina = Mathf.Clamp(stamina, 0, staminaMax);

        if (stamina <= 0)
        {
            isOverRun = true;
        }
        if (stamina >= overRunLimit)
        {
            isOverRun = false;
        }

        if (staminaSlider != null)
        {
            staminaSlider.value = stamina / staminaMax;
            staminaSlider.fillRect.GetComponent<Image>().color = isOverRun ? Color.red : Color.green;

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }
}
