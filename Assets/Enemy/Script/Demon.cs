using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

public class Demon : BaseEnemy,IDamageable
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
	[SerializeField,Header("�ҋ@����")]
	private float waitTime = 3f;
	[SerializeField, Range(0.0f, 1.0f), Header("���̍U���Ɉڂ�m��")]
	private float nextAttack = 0.3f;
	private bool IsDamage = false;

	// Start is called before the first frame update
	void Start()
    {
        //HP�̏����ݒ�
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
			//���łɑҋ@��Ԃ̏ꍇ�Ȃɂ����Ȃ�
			return;
		}

		
		if (!target)
		{
			// �v���C���[���������Ƃ��ɑҋ@�R���[�`�����J�n
			StartCoroutine(WaitForIdle(waitTime));
		}
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
			animator.SetBool("Run", true);
			//�����ƃ^�[�Q�b�g�̋�����attackRange������������
			if (distanceToTarget <= attackRange)
			{
				state = DemonState.Attack1;
			}
		}
		else
		{
			// �ړ����~
			agent.SetDestination(transform.position);
			// �ړ��A�j���[�V�������~
			animator.SetBool("Run", false); 
			state = DemonState.Idle;
		}
	}

    private void Attack1State()
    {
		if (IsDamage == false)
		{
			animator.SetBool("Run", false);
			agent.isStopped = true;
			TLAttack[0].Play();
		}
    }

	public void NextAttack1()
	{
		//�����_���Ɏ��̍U���ɐ��ڂ��邩���f
		float randomChance = Random.value;
		if (randomChance <= nextAttack)
		{
			agent.isStopped = false;
			state = DemonState.Attack2;
		}
		else
		{
			agent.isStopped = false;
			state = DemonState.Freeze;
		}
	}

	private void Attack2State()
	{
		if (IsDamage == false)
		{
			TLAttack[0].Stop();
			agent.isStopped = true;
			TLAttack[1].Play();
		}
		
	}
	public void FreezeState()
	{
		TLAttack[0].Stop();
		TLAttack[1].Stop();
		IsDamage = false;
		TLDamage.Stop();
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
		// �ҋ@���̏�Ԃ������t���O��ݒ�
		IsWait = true;
		// �w�肵���b���ҋ@
		yield return new WaitForSeconds(seconds);
		// �ҋ@���I��������t���O�����Z�b�g
		IsWait = false;
		// �ҋ@��̏�Ԃ֑J��
		state = DemonState.Move;
	}


	public void Damage(float damage)
	{
		//���Ɏ��S���Ă�����_���[�W���󂯂Ȃ�
		if (IsDead) return;
		IsDamage = true;
		currnethp -= damage;
		Debug.Log("DemonHP" + currnethp);
		//�_���[�W�p�̃^�C�����C�����Đ�
		TLDamage.Play();
		if (currnethp <= 0)
		{
			Death();
		}
		
	}

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
		DeathEffectActive();
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
