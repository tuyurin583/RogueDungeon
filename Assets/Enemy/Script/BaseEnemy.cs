using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class BaseEnemy : MonoBehaviour,IDamageable
{
	//PlayablesDirectorコンポーネント
	[SerializeField]
	protected PlayableDirector[] timeline;
	//アニメーター
	protected Animator animator;
	//リギッドボディー
    Rigidbody rb;
    [SerializeField]
    //最大HP
    protected float Maxhp=50f;
    //現在のHP
    public float currnethp;
	// 死亡処理が実行されたかのフラグ
	protected bool isDead = false;
	// 死亡時のエフェクト
	public GameObject deathEffect;
	//NavMeshAgent
	protected NavMeshAgent agent;
	[SerializeField,Header("視野")]
	public float searchAngle = 45f;
	[SerializeField, Header("移動速度")]
	protected float Speed;


	// Start is called before the first frame update
	void Start()
    {
        animator=GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
		agent=GetComponent<NavMeshAgent>();
        //HPの初期設定
        currnethp = Maxhp;
    }


    public void Damage(float damage)
    {
        //既に死亡していたらダメージを受けない
        if (isDead) return;
        currnethp -= damage;
		//Debug.Log(currnethp);
		if (currnethp <= 0)
		{
			Death();
		}

	}

    public void Death()
    {
		// 既に死亡していたら処理しない
		if (isDead) return;
		//Debug.Log("Death");
		isDead = true;
		// アニメーション制御
		animator.SetTrigger("Death");
		if (deathEffect != null)
		{
			//エフェクト再生
			Instantiate(deathEffect, transform.position, Quaternion.identity);
		}
		// オブジェクトを削除
		Destroy(gameObject);
		// 死亡通知を送信
		SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);

	}

}
