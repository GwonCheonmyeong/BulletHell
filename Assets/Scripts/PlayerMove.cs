using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    Camera _camera;
    CharacterController _controller;

    public float speed = 5f;
    public float runSpeed = 8f;
    public float finalSpeed;
    public float jumpPower = 10f;
    public float jumpCoolTime = 1.5f;
    public float gravity = 15;
    public float dashTime = 0.5f;
    public int Hp = 1;
    public bool toggleCameraRotation;
    public bool run;
    public bool isJumping = false;
    public bool canJumping = true;
    public bool canDash = true;
    public bool isDashing = false;
    public Vector3 MoveDir;
    public float smoothness = 10;
    private void Start()
    {
        _camera = Camera.main;
        _controller = this.GetComponent<CharacterController>();
    }
    private void Update()
    {
        if(Hp <= 0)
        {
            Time.timeScale = 0;
            gameObject.SetActive(false);
        }
        if(Input.GetKey(KeyCode.LeftAlt))
        {
            toggleCameraRotation = true;
        }
        else
        {
            toggleCameraRotation = false;
        }
        if(Input.GetKey(KeyCode.LeftShift))
        {
            run = true;
        }
        else
        {
            run = false;
        }
        if(Input.GetKeyDown(KeyCode.Space) && canJumping)
        {
            StartCoroutine(Jump());
        }
        if (canDash && Input.GetKeyDown(KeyCode.E))
        { 
            StartCoroutine(Dash());
        }
        //ม฿ทย
        MoveDir.y -= gravity * Time.deltaTime;

        if( !isDashing )
        {
            _controller.Move(MoveDir * Time.deltaTime);
        }
        InputMovement();
    }
    private void FixedUpdate()
    {
        if (isJumping)
        {
            isJumping = false;
            MoveDir.y = jumpPower;
        }
        if (isDashing)
        {
            gameObject.transform.Translate(Vector3.forward * 0.2f);
        }
    }
    private void LateUpdate()
    {
        if(toggleCameraRotation != true)
        {
            Vector3 playerRotation = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(playerRotation), Time.deltaTime * smoothness);
        }
    }
    public void InputMovement()
    {
        finalSpeed = (run) ? runSpeed : speed;

        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        Vector3 moveDirection = forward * Input.GetAxisRaw("Vertical") + right * Input.GetAxisRaw("Horizontal");

        _controller.Move(moveDirection.normalized * finalSpeed * Time.deltaTime);
    }
    public IEnumerator Jump()
    {
        isJumping = true;
        canJumping = false;
        yield return new WaitForSeconds(jumpCoolTime);
        canJumping = true;
    }
    public IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        yield return new WaitForSeconds(dashTime);
        isDashing = false;
        yield return new WaitForSeconds(2);
        canDash = true;
    }
}
