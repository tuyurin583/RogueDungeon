using System.Collections;
using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using static Golem;
using static SwordSkelton;

public class Demon : BaseEnemy
{
    public enum DemonState
    {
        Idle,
        Move,
        Attack1,
        Attack2,
        Attack3,
        ModeChange,
        Damage,
        Death,
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
				int radomAttack = Random.Range(1,4);

				switch (radomAttack)
				{
					case 1:
					state=DemonState.Attack1;
					break;
					case 2:
					state=DemonState.Attack2;
					break;
					case 3:
					state=DemonState.Attack3;
					break;
				}
			}
		}
		else
		{
			agent.SetDestination(transform.position); // 移動を停止
			animator.SetBool("Run", false); // 移動アニメーションを停止
			state = DemonState.Idle;
		}
	}

    private void Attack1State()
    {

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
