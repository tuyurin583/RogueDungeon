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
	//���񂷂�ʒu
	[SerializeField]
	private Transform[] patrolPositions;
	//���݂̖ړI�n
	private int currnetPointIndex = 0;
	[SerializeField]
	private SphereCollider searchArea;
	// Start is called before the first frame update
	void Start()
    {
		agent = GetComponent<NavMeshAgent>();
		animator= GetComponent<Animator>();
		//�ŏ��̖ړI�n��ݒ�
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
			// �ړI�n�̔ԍ����P�X�V
			currnetPointIndex = (currnetPointIndex + 1) % patrolPositions.Length;
			// �ړI�n�����̏ꏊ�ɐݒ�
			agent.SetDestination(patrolPositions[currnetPointIndex].position);
		}
	}

	private void AttackState()
	{
		//�L�����̈ړ����~�߂�
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
		//�L�����𓮂���悤�ɂ���
		agent.isStopped = false;
		TLAttack[1].Stop();
		state = GolemState.Idle;
	}

	public void OnDetectObject(Collider collider)
	{
		if (collider.CompareTag("Player"))
		{
			animator.SetBool("Run", true);
			// ���g�i�G�j�ƃv���C���[�̋���
			var positionDiff = collider.transform.position - transform.position;
			// �G���猩���v���C���[�̕���
			var angle = Vector3.Angle(transform.forward, positionDiff);
			var distanceToPlayer = (collider.transform.position - transform.position).magnitude;
			//������Ƀv���C���[��������ǂ�������
			if (angle <= searchAngle)
			{
				agent.destination = collider.transform.position;
				//�v���C���[�Ƃ̋��������l�ȉ��ɂȂ�����U����Ԃɂ���
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

	//�_���[�W����
	public void Damage(float damage)
    {
		//���Ɏ��S���Ă�����_���[�W���󂯂Ȃ�
		if (IsDead) return;

		//�_���[�W�p�̃^�C�����C�����Đ�
		TLDamage.Play();
		currnethp -= damage;
		Debug.Log("HP" + currnethp);
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
		Destroy(gameObject);
	}
}
