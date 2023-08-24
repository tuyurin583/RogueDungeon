using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class SwordSkelton : BaseEnemy
{
	public enum SkeltonState
	{
		//アイドル
		Idle,
		//移動
		Move,
		//攻撃
		Attack,
		//ブロック
		Block,
		//ダメージ
		Damage,
		//死亡
		Death,
		Freeze,
	}
	//キャラの状態
	public SkeltonState state=SkeltonState.Idle;
	//巡回する位置
	[SerializeField]
	private Transform[] patrolPositions;
	//現在の目的地
	private int currnetPointIndex=0;
	[SerializeField]
	private SphereCollider searchArea;

	// Start is called before the first frame update
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator=GetComponent<Animator>();
		//最初の目的地を設定
		agent.SetDestination(patrolPositions[0].position);

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
			case SkeltonState.Block:
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
		agent.isStopped=false;
		animator.SetBool("Move", true);
		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			// 目的地の番号を１更新
			currnetPointIndex=(currnetPointIndex + 1) % patrolPositions.Length;
			// 目的地を次の場所に設定
			agent.SetDestination(patrolPositions[currnetPointIndex].position);
		}
	}

	private void AttackState()
	{
		//キャラの移動を止める
		agent.isStopped = true;
		animator.SetBool("Run", false);
		timeline[0].Play();
	}

	public void FreezeState()
	{
		//キャラを動けるようにする
		agent.isStopped = false;
		Debug.Log("フリーズ");
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
			//視野内にプレイヤーがいたら追いかける
			if (angle <= searchAngle)
			{
				agent.destination = collider.transform.position;
				state = SkeltonState.Attack;
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
