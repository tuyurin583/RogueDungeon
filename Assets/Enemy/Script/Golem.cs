using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

[RequireComponent(typeof(NavMeshAgent))]
public class Golem : BaseEnemy,IDamageable
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
	GolemState state = GolemState.Idle;
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
		animator= GetComponent<Animator>();
		//最初の目的地を設定
		agent.SetDestination(patrolPositions[0].position);

		if (agent == null)
		{
			Debug.LogError("NavMeshAgent is not found on Minotaur.");
		}
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
		state = GolemState.Move;
	}

    private void MoveState()
    {
		animator.SetBool("Run", true);
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

	private void Attack2State()
	{
		TLAttack[0].Stop();
		TLAttack[1].Play();
		state = GolemState.Freeze;
	}

	public void FreezeState()
	{
		//キャラを動けるようにする
		agent.isStopped = false;
		TLAttack[1].Stop();
		state = GolemState.Idle;
	}

	public void OnDetectObject(Collider collider)
	{
		if (collider.CompareTag("Player"))
		{
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
					state = GolemState.Attack;
				}
			}
			else
			{
				state = GolemState.Idle;
			}
		}
		else
		{
			state= GolemState.Idle;
		}
	}


#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.color = Color.red;
		Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
	}
#endif

	//ダメージ処理
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
		Destroy(gameObject);
	}
}
