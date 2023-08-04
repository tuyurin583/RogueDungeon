using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    //アニメーター
    Animator animator;
    //RigidBody
    Rigidbody rb;

    //移動速度
    [SerializeField]
    public float moveSpeeed = 5f;
    //ジャンプ力
    [SerializeField]
    public float jumpForce = 10f;
    // 地面と判定するレイヤーマスク
    [SerializeField]
    private LayerMask groundLayer;
    // 地面に接地しているかどうか
    private bool isGrounded;
    //ジャンプ中かどうか
    //private bool Isjump=false;

    //移動アクション
    [SerializeField]
    private InputAction movement;
    //ジャンプアクション
    [SerializeField]
    private InputAction jump;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        //移動アクションを作成
        movement = GetComponent<PlayerInput>().actions["Move"];
        jump = GetComponent<PlayerInput>().actions["Jump"];
    }

   

	private void FixedUpdate()
	{
        //キーボードの入力を取得
        Vector2 moveInput = movement.ReadValue<Vector2>();
        //移動方向を計算
        Vector3 moveDirection = (transform.forward * moveInput.y + transform.right * moveInput.x).normalized * moveSpeeed;
        //移動を適用
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
        //アニメションを更新
        UpdateAnimation(moveInput);

        //地面に接地しているかチェック
        isGrounded = CheckGrounded();

        // ジャンプ処理
        if (jump.ReadValue<float>() > 0)
        {
            Jump();
        }
        else
        {
            animator.SetBool("Jump", false);
       
        }
    }

    //アニメションを更新する関数
    private void UpdateAnimation(Vector2 moveInput)
    {
        float horizontalInput = moveInput.x;
        float verticalInput = moveInput.y;

        animator.SetFloat("InputX", horizontalInput);
        animator.SetFloat("InputY", verticalInput);
    }

    //ジャンプ処理
    private void Jump()
    {
       rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
       animator.SetBool("Jump", true);
    }

    // 地面に接地しているかチェック
    private bool CheckGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, 0.1f, groundLayer);
    }
}
