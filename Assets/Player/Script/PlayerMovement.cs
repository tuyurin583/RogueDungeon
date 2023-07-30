using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //�A�j���[�^�[
    Animator animator;
    //�L�����N�^�[�R���g���[���[
    CharacterController characterController;
    //RigidBody
    Rigidbody rb;

    //�ړ����x
    [SerializeField]
    public float moveSpeeed = 5f;
    //�W�����v��
    [SerializeField]
    public float jumpForce = 10f;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        //WASD�̓��͂��擾���Ĉړ��������v�Z
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized * moveSpeeed;

        //�W�����v����
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("IsInAir", true);
        }

        //�ړ���K�p
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
       
        animator.SetFloat("InputX", horizontalInput);
        animator.SetFloat("InputY", verticalInput);

    }
}
