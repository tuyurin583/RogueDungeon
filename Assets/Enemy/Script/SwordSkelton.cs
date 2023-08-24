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
		//�A�C�h��
		Idle,
		//�ړ�
		Move,
		//�U��
		Attack,
		//�u���b�N
		Block,
		//�_���[�W
		Damage,
		//���S
		Death,
		Freeze,
	}
	//�L�����̏��
	public SkeltonState state=SkeltonState.Idle;
	//���񂷂�ʒu
	[SerializeField]
	private Transform[] patrolPositions;
	//���݂̖ړI�n
	private int currnetPointIndex=0;
	[SerializeField]
	private SphereCollider searchArea;

	// Start is called before the first frame update
	void Start()
	{
		agent = GetComponent<NavMeshAgent>();
		animator=GetComponent<Animator>();
		//�ŏ��̖ړI�n��ݒ�
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
		//�L�����̈ړ����~�߂�
		agent.isStopped = true;
		state = SkeltonState.Move;
	}

	private void MoveState()
	{
		//�L������������悤�ɂ���
		agent.isStopped=false;
		animator.SetBool("Move", true);
		if (agent.remainingDistance <= agent.stoppingDistance)
		{
			// �ړI�n�̔ԍ����P�X�V
			currnetPointIndex=(currnetPointIndex + 1) % patrolPositions.Length;
			// �ړI�n�����̏ꏊ�ɐݒ�
			agent.SetDestination(patrolPositions[currnetPointIndex].position);
		}
	}

	private void AttackState()
	{
		//�L�����̈ړ����~�߂�
		agent.isStopped = true;
		animator.SetBool("Run", false);
		timeline[0].Play();
	}

	public void FreezeState()
	{
		//�L�����𓮂���悤�ɂ���
		agent.isStopped = false;
		Debug.Log("�t���[�Y");
		state = SkeltonState.Idle;
	}

	public void OnDetectObject(Collider collider)
	{
		if (collider.CompareTag("Player"))
		{
				animator.SetBool("Move", false);
				animator.SetBool("Run", true);
			// ���g�i�G�j�ƃv���C���[�̋���
			var positionDiff = collider.transform.position - transform.position;
			// �G���猩���v���C���[�̕���
			var angle = Vector3.Angle(transform.forward, positionDiff);
			//������Ƀv���C���[��������ǂ�������
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
