using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Demon : BaseEnemy
{
    public enum DemonState
    {
        Idle,
        Move,
        Attack1,
        Attack2,
        ModeChange,
        Damage,
        Death,
		Freeze
	}
    public DemonState state = DemonState.Idle;
    public GameObject target;
	[SerializeField]
	private float searchAngle = 45f;
	[SerializeField]
	private SphereCollider searchArea;
	[SerializeField]
	private bool IsWait=false;
	[SerializeField,Header("待機時間")]
	private float waitTime = 3f;
	[SerializeField, Range(0.0f, 1.0f), Header("次の攻撃に移る確率")]
	private float nextAttack = 0.3f;

	// Start is called before the first frame update
	void Start()
    {
        //HPの初期設定
        currnethp = Maxhp;
		agent = GetComponent<NavMeshAgent>();
		animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case DemonState.Idle:
                IdleState();
                break;
            case DemonState.Move:
                MoveState();
                break;
            case DemonState.Attack1:
				Attack1State();
                break;
			case DemonState.Attack2:
				Attack2State();
				break;
        }
    }

    private void IdleState()
    {
		if (IsWait)
		{
			//すでに待機状態の場合なにもしない
			return;
		}

		
		if (!target)
		{
			// プレイヤーを見つけたときに待機コルーチンを開始
			StartCoroutine(WaitForIdle(waitTime));
		}
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
			animator.SetBool("Run", true);
			//自分とターゲットの距離がattackRangeよりも小さい時
			if (distanceToTarget <= attackRange)
			{
				state = DemonState.Attack1;
			}
		}
		else
		{
			// 移動を停止
			agent.SetDestination(transform.position);
			// 移動アニメーションを停止
			animator.SetBool("Run", false); 
			state = DemonState.Idle;
		}
	}

    private void Attack1State()
    {
		animator.SetBool("Run", false);
		agent.isStopped = true;
		TLAttack[0].Play();
		
    }

	public void NextAttack1()
	{
		//ランダムに次の攻撃に推移するか判断
		float randomChance = Random.value;
		if (randomChance <= nextAttack)
		{
			agent.isStopped = false;
			state = DemonState.Attack2;
		}
		else
		{
			TLAttack[0].Stop();
			agent.isStopped = false;
			state = DemonState.Idle;
		}
	}

	private void Attack2State()
	{
		TLAttack[0].Stop();
		agent.isStopped=true;
		TLAttack[1].Play();
		
	}
	public void FreezeState()
	{
		TLAttack[1].Stop();
		agent.isStopped = false;
		state= DemonState.Idle;
	}

	public void OnDetectObject(Collider collider)
	{
		if (collider.CompareTag("Player"))
		{
			target = collider.gameObject;
			state = DemonState.Move;
		}
	}

	private IEnumerator WaitForIdle(float seconds)
	{
		// 待機中の状態を示すフラグを設定
		IsWait = true;
		// 指定した秒数待機
		yield return new WaitForSeconds(seconds);
		// 待機が終了したらフラグをリセット
		IsWait = false;
		// 待機後の状態へ遷移
		state = DemonState.Move;
	}

#if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Handles.color = Color.red;
		Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
	}
#endif
}
