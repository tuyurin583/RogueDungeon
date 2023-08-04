using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    //プレイヤーへの参照
    public Transform playerTransform;
    //カメラとプレイヤーの距離
    public float distance = 5f;
    //カメラの高さ
    public float height = 2f;
    //カメラの回転速度
    public float rotationSpeed = 5f;
    //カメラの回転角度の制限
    public float minYAngle=-20f;
    public float maxYAngle = 80f;
    //マウス移動の入力値を保存する変数
    private Vector2 mouseInput;

    //回転角度の制限値
    private const float MaxAngle = 180f;
    private const float FullCircle = 360f;

    // Start is called before the first frame update
    void Start()
    {
        //プレイヤーのTransformを取得
        playerTransform = transform.parent;
        //カーソルをロックして非表示にする
        Cursor.lockState = CursorLockMode.Locked;
    }

	// Update is called once per frame
	private void Update()
	{
        //マウスの移動量を取得
        mouseInput = Mouse.current.delta.ReadValue() * rotationSpeed;

        //カメラの回転角度を制限する
        float currentAngle = transform.eulerAngles.x;
        float desiredAngle = currentAngle + mouseInput.y;

        if (desiredAngle > MaxAngle)
        {
            desiredAngle -= FullCircle;
        }
        desiredAngle = Mathf.Clamp(desiredAngle, minYAngle, maxYAngle);
        mouseInput.y = desiredAngle - currentAngle;

        //プレイヤーの回転をマウスのX移動に応じて更新
        playerTransform.Rotate(Vector3.up * mouseInput.x);
        // カメラ自体のX軸方向の回転をマウスのY移動に応じて更新
        transform.localRotation = Quaternion.Euler(desiredAngle, 0f, 0f);

        // カメラの位置を計算
        Vector3 offset = -transform.forward * distance + Vector3.up * height;
        transform.position = playerTransform.position + offset;
    }
}
