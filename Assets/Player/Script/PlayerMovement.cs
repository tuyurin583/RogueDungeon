using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //アニメーター
    Animator animator;
    //キャラクターコントローラー
    CharacterController characterController;
    //RigidBody
    Rigidbody rb;

    //移動速度
    [SerializeField]
    public float moveSpeeed = 5f;
    //ジャンプ力
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
        //WASDの入力を取得して移動方向を計算
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        Vector3 moveDirection = (transform.forward * verticalInput + transform.right * horizontalInput).normalized * moveSpeeed;

        //ジャンプ処理
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("jump");
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            animator.SetBool("IsInAir", true);
        }

        //移動を適用
        rb.velocity = new Vector3(moveDirection.x, rb.velocity.y, moveDirection.z);
       
        animator.SetFloat("InputX", horizontalInput);
        animator.SetFloat("InputY", verticalInput);

    }
}
