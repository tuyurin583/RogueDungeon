using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

[RequireComponent(typeof(NavMeshAgent))]
public class SwordSkelton : BaseEnemy, IDamageable
{
	public enum SkeltonState
	{
		//アイドル
		Idle,
		//移動
		Move,
		//攻撃
		Attack,
		//ダメージ
		Damage,
		//死亡
		Death,
		Freeze,
	}
	//キャラの状態
	public SkeltonState state = SkeltonState.Idle;
	//巡回する位置
	[SerializeField]
	private Transform[] patrolPositions;
	//現在の目的地
	private int currnetPointIndex = 0;
	[SerializeField]
	private SphereCollider searchArea;
	

	// Start is called before the first frame update
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
		//最初の目的地を設定
		agent.SetDestination(patrolPositions[0].position);
		currnethp = Maxhp;
	}

	// Update is called once per frame
	void Update()
	{
		switch (state)
		{
			case SkeltonState.Idle:
				IdleState();
				break;
			case SkeltonState.Move:
				MoveState();
				break;
			case SkeltonState.Attack:
				AttackState();
				break;
		}

	}

	private void IdleState()
	{
		//キャラの移動を止める
		agent.isStopped = true;
		state = SkeltonState.Move;
	}

	private void MoveState()
	{
		//キャラが動けるようにする
		agent.isStopped = false;
		animator.SetBool("Move", true);
		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			// 目的地の番号を１更新
			currnetPointIndex = (currnetPointIndex + 1) % patrolPositions.Length;
			// 目的地を次の場所に設定
			agent.SetDestination(patrolPositions[currnetPointIndex].position);
		}
	}

	private void AttackState()
	{
		//キャラの移動を止める
		agent.isStopped = true;
		animator.SetBool("Run", false);
		TLAttack[0].Play();

	}


	public void Damage(float damage)
	{
		//既に死亡していたらダメージを受けない
		if (IsDead) return;
		
		//ダメージ用のタイムラインを再生
		TLDamage.Play();
		currnethp -= damage;
		Debug.Log("HP" + currnethp);
		if (currnethp <= 0)
		{
			Death();
		}
		
	}

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

	

	public void FreezeState()
	{
		//キャラを動けるようにする
		agent.isStopped = false;
		TLAttack[0].Stop();
		state = SkeltonState.Idle;
	}

	public void OnDetectObject(Collider collider)
	{
		if (collider.CompareTag("Player"))
		{
			animator.SetBool("Move", false);
			animator.SetBool("Run", true);
			// 自身（敵）とプレイヤーの距離
			var positionDiff = collider.transform.position - transform.position;
			// 敵から見たプレイヤーの方向
			var angle = Vector3.Angle(transform.forward, positionDiff);
			var distanceToPlayer = (collider.transform.position - transform.position).magnitude;
			//視野内にプレイヤーがいたら追いかける
			if (angle <= searchAngle)
			{
				agent.destination = collider.transform.position;
				//プレイヤーとの距離が一定値以下になったら攻撃状態にする
				if (distanceToPlayer <= attackDistance)
				{
					state = SkeltonState.Attack;
				}
			}
			else
			{
				state = SkeltonState.Idle;
			}
		}
	}

	
#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.color = Color.red;
		Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
	}
#endif
}
