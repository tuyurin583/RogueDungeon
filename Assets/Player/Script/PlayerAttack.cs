using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    //攻撃の範囲
    [SerializeField]
    public float attackRange = 2f;
    //攻撃のダメージ(攻撃力)
    public float atk = 10f;
    //攻撃用のレイヤーマスク
    public LayerMask enemyLayer;
    //アニメーター
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
        //攻撃アニメーションを再生
        animator.SetBool("Attack", true);
        Collider[] hitEnemy = Physics.OverlapSphere(transform.position, attackRange, enemyLayer);

       
    }

	private void OnDrawGizmosSelected()
	{
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
	}
}
