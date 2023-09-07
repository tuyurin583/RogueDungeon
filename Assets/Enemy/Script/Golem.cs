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
		//�^�[�Q�b�g�����鎞
		if (target)
		{
			//�^�[�Q�b�g�̍��W�Ɍ�����
			transform.LookAt(target.transform);
			//�v���C���[��ǂ�������
			agent.destination = target.transform.position;
			// �����ƃ^�[�Q�b�g�̋������v�Z
			float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
			//�����ƃ^�[�Q�b�g�̋�����attackdistance������������
			if (distanceToTarget <= attackDistance)
			{
				//Attack��Ԃɐ��ڂ���
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




	//�_���[�W����
	public void Damage(float damage)
    {
		//���Ɏ��S���Ă�����_���[�W���󂯂Ȃ�
		if (IsDead) return;

		//�_���[�W�p�̃^�C�����C�����Đ�
		TLDamage.Play();
		currnethp -= damage;
		Debug.Log("GolemHP" + currnethp);
		if (currnethp <= 0)
		{
			Death();
		}
	}
    //���S����
    public void Death()
    {
		// ���Ɏ��S���Ă����珈�����Ȃ�
		if (IsDead) return;
		IsDead = true;
		this.enabled = false;
		//���S�p�̃^�C�����C�����Đ�
		TLDeath.Play();
		//�G�̓������~�߂�
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
