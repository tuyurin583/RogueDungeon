using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class PlayerAttack : MonoBehaviour
{
    //�U���p�̃��C���[�}�X�N
    public LayerMask enemyLayer;
    [SerializeField]
    private float atk=30;
    //�R���{�A�N�V�����p
    //�U���p�A�N�V�����t���O
    [SerializeField]
    public bool IsAttack;
    //�R���{����p�t���O
    private int combo_Flag;
    //�R���{��
    private int combo_Count;
    //�^�C�����C��
    [SerializeField]
    private PlayableDirector[] tl;
    //�A�N�V�����t���O
    private bool avoid;

    // Start is called before the first frame update
    void Start()
    {
       
    }

	private void Update()
	{
       
    }

    //�U���{�^���p
    public void OnAttack(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //�U��������𒆂���Ȃ�������
            if (!IsAttack && !avoid)
            {
                //�A�N�V�����t���O��ON�ɂ���
                IsAttack = true;
                switch (combo_Count)
                {
                    case 0:
                        //�R���{�񐔂�0�̎��^�C�����C�����Đ�����
                        tl[0].Play();
                        break;
                }
            }
        }
    }

    //�R���{�L����
    public void ComboEnable()
    {
        //�R���{���͂�����(0)�̎��A
        if (combo_Flag == 0)
        {
            //�R���{���͂�L��(������)�ɂ���
            combo_Flag = 1;
        }
    }

    //�R���{���͗p
    public void OnCombo(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //�U�������R���{���͂�(1)�̎�
            if (IsAttack && combo_Flag == 1)
            {
                //�R���{����͍ς�(2)�ɂ���
                combo_Flag = 2;
            }
        }
    }

    //�R���{����p
    public void ComboCheak()
    {
        if (combo_Flag == 2)
        {
            switch (combo_Count)
            {
                //�R���{�񐔂�0�̎��̏���(1��ڂ̃R���{�U��)
                case 0:
                    //�Đ����̃^�C�����C���̒�~
                    tl[0].Stop();
                    //���̃^�C�����C�����Đ�
                    tl[1].Play();
                    //�R���{�񐔂̍X�V
                    combo_Count = 1;
                    break;
                //�R���{�񐔂�1�̎��̏���(2��ڂ̃R���{�U��)
                case 1:
                    tl[1].Stop();
                    tl[2].Play();
                    combo_Count = 2;
                    break;
                //�R���{�񐔂�2�̎��̏���(3��ڂ̃R���{�U��)
                case 2:
                    tl[2].Stop();
                    combo_Count = 3;
                    AttackEnd();
                    break;
            }
            //�R���{���͂𖳌�(0)�ɂ���
            combo_Flag = 0;
        }
    }

    //�U���I���p
    public void AttackEnd()
    {
        if (combo_Flag == 1 || combo_Flag == 2)
        {
            //�R���{���͂𖳌�(0)�ɂ���
            combo_Flag = 0;
        }
        //�U���t���O��OFF�ɂ���
        IsAttack = false;
        Debug.Log(IsAttack);
        //�R���{�񐔂�0�ɂ���
        combo_Count = 0;
    }

	private void OnTriggerEnter(Collider collider)
	{
        GameObject hitobj = collider.gameObject;

        //�G���ǂ������ׂ�
        if (!hitobj.CompareTag("Enemy")) { return; }
        //�q�b�g�����I�u�W�F�N�g��IDamageable���擾����
        IDamageable damageHit = hitobj.GetComponent<IDamageable>();
        //�_���[�W���肪��������Ă��Ȃ���΃_���[�W������s��Ȃ�
        if (damageHit == null) { return; }
        //�_���[�W��^����
        damageHit.Damage(atk);
	}
}
