using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

[RequireComponent(typeof(NavMeshAgent))]
public class Golem : BaseEnemy, IDamageable
{
	public enum GolemState
	{
		Idle,
		Move,
		Attack,
		Damage,
		Death,
		Freeze
	}
	public GolemState state = GolemState.Idle;
	[SerializeField]
	private SphereCollider searchArea;
	[SerializeField]
	private GameObject target;
	// Start is called before the first frame update
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		currnethp = Maxhp;

		
	}

	// Update is called once per frame
	void Update()
	{
		switch (state)
		{
			case GolemState.Idle:
				IdleState();
				break;
			case GolemState.Move:
				MoveState();
				break;
			case GolemState.Attack:
				AttackState();
				break;
		}
	}

	private void IdleState()
	{

	}

	private void MoveState()
	{
		//ターゲットがいる時
		if (target)
		{
			//ターゲットの座標に向ける
			transform.LookAt(target.transform);
			//プレイヤーを追いかける
			agent.destination = target.transform.position;
			// 自分とターゲットの距離を計算
			float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
			//自分とターゲットの距離がattackdistanceよりも小さい時
			if (distanceToTarget <= attackDistance)
			{
				//Attack状態に推移する
				state = GolemState.Attack;
			}
		}
		else
		{
			state = GolemState.Idle;
		}
		
	}

	private void AttackState()
	{
		agent.isStopped = true;
		TLAttack[0].Play();
		state = GolemState.Freeze;
	}


	public void FreezeState()
	{
		TLAttack[0].Stop();
		agent.isStopped = false;
		state = GolemState.Move;
	}

	public void OnDetectObject(Collider collider)
	{
		if (collider.CompareTag("Player"))
		{
			animator.SetBool("Run", true);
			target = collider.gameObject;
			state= GolemState.Move;
		
		}
	}


	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Player")
		{
			target = null;
		}
	}




	//ダメージ処理
	public void Damage(float damage)
    {
		//既に死亡していたらダメージを受けない
		if (IsDead) return;

		//ダメージ用のタイムラインを再生
		TLDamage.Play();
		currnethp -= damage;
		Debug.Log("GolemHP" + currnethp);
		if (currnethp <= 0)
		{
			Death();
		}
	}
    //死亡処理
    public void Death()
    {
		// 既に死亡していたら処理しない
		if (IsDead) return;
		IsDead = true;
		this.enabled = false;
		//死亡用のタイムラインを再生
		TLDeath.Play();
		//敵の動きを止める
		agent.isStopped = true;
	}

	public void DeathObject()
	{
		Destroy(this.gameObject);
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.color = Color.red;
		Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
	}
#endif
}
