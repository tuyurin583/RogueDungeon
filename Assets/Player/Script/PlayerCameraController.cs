using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCameraController : MonoBehaviour
{
    //�v���C���[�ւ̎Q��
    public Transform playerTransform;
    //�J�����ƃv���C���[�̋���
    public float distance = 5f;
    //�J�����̍���
    public float height = 2f;
    //�J�����̉�]���x
    public float rotationSpeed = 5f;
    //�J�����̉�]�p�x�̐���
    public float minYAngle=-20f;
    public float maxYAngle = 80f;
    //�}�E�X�ړ��̓��͒l��ۑ�����ϐ�
    private Vector2 mouseInput;

    //��]�p�x�̐����l
    private const float MaxAngle = 180f;
    private const float FullCircle = 360f;

    // Start is called before the first frame update
    void Start()
    {
        //�v���C���[��Transform���擾
        playerTransform = transform.parent;
        //�J�[�\�������b�N���Ĕ�\���ɂ���
        Cursor.lockState = CursorLockMode.Locked;
    }

	// Update is called once per frame
	private void Update()
	{
        //�}�E�X�̈ړ��ʂ��擾
        mouseInput = Mouse.current.delta.ReadValue() * rotationSpeed;

        //�J�����̉�]�p�x�𐧌�����
        float currentAngle = transform.eulerAngles.x;
        float desiredAngle = currentAngle + mouseInput.y;

        if (desiredAngle > MaxAngle)
        {
            desiredAngle -= FullCircle;
        }
        desiredAngle = Mathf.Clamp(desiredAngle, minYAngle, maxYAngle);
        mouseInput.y = desiredAngle - currentAngle;

        //�v���C���[�̉�]���}�E�X��X�ړ��ɉ����čX�V
        playerTransform.Rotate(Vector3.up * mouseInput.x);
        // �J�������̂�X�������̉�]���}�E�X��Y�ړ��ɉ����čX�V
        transform.localRotation = Quaternion.Euler(desiredAngle, 0f, 0f);

        // �J�����̈ʒu���v�Z
        Vector3 offset = -transform.forward * distance + Vector3.up * height;
        transform.position = playerTransform.position + offset;
    }
}
