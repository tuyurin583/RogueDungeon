using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //�A�j���[�^�[
    Animator animator;
    //RigidBody
    Rigidbody rb;

    //�ړ����x
    [SerializeField]
    public float moveSpeeed = 5f;
    //�W�����v��
    [SerializeField]
    public float jumpForce = 10f;
    // �n�ʂƔ��肷�郌�C���[�}�X�N
    [SerializeField]
    private LayerMask groundLayer;
    // �n�ʂɐڒn���Ă��邩�ǂ���
    private bool isGrounded;
    //�W�����v�����ǂ���
    //private bool Isjump=false;

    //�ړ��A�N�V����
    [SerializeField]
    private InputAction movement;
    //�W�����v�A�N�V����
    [SerializeField]
    private InputAction jump;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        //�ړ��A�N�V�������쐬
        movement = GetComponent<PlayerInput>().actions["Move"];
        jump = GetComponent<PlayerInput>().actions["Jump"];
    }

   

	private void FixedUpdate()
	{
        //�L�[�{�[�h�̓��͂��擾
        Vector2 moveInput = movement.ReadValue<Vector2>();
        //�ړ��������v�Z
        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized * moveSpeeed;
        //�ړ���K�p
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        //�A�j���V�������X�V
        UpdateAnimation(moveInput);

        //�n�ʂɐڒn���Ă��邩�`�F�b�N
        isGrounded = CheckGrounded();

        // �W�����v����
        if (jump.ReadValue<float>() > 0)
        {
            Jump();
        }
        else
        {
            animator.SetBool("Jump", false);
       
        }
    }

    //�A�j���V�������X�V����֐�
    private void UpdateAnimation(Vector2 moveInput)
    {
        float horizontalInput = moveInput.x;
        float verticalInput = moveInput.y;

        animator.SetFloat("InputX", horizontalInput);
        animator.SetFloat("InputY", verticalInput);
    }

    //�W�����v����
    private void Jump()
    {
       rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
       animator.SetBool("Jump", true);
    }

    // �n�ʂɐڒn���Ă��邩�`�F�b�N
    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);
    }
}
