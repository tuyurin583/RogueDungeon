using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //�U���͈̔�
    [SerializeField]
    public float attackRange = 2f;
    //�U���̃_���[�W(�U����)
    public float atk = 10f;
    //�U���p�̃��C���[�}�X�N
    public LayerMask enemyLayer;
    //�A�j���[�^�[
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
        else
        {
            animator.SetBool("Attack", false);
        }
    }

    public void Attack()
    {
        //�U���A�j���[�V�������Đ�
        animator.SetBool("Attack", true);
        Collider[] hitEnemy = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

       
    }

	private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
