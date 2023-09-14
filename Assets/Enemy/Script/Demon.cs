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
	[SerializeField,Header("�ҋ@����")]
	private float waitTime = 3f;
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
			agent.SetDestination(transform.position); // �ړ����~
			animator.SetBool("Run", false); // �ړ��A�j���[�V�������~
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
		// �ҋ@���̏�Ԃ������t���O��ݒ�
		IsWait = true;
		// �w�肵���b���ҋ@
		yield return new WaitForSeconds(seconds);
		// �ҋ@���I��������t���O�����Z�b�g
		IsWait = false;
		// �ҋ@��̏�Ԃ֑J��
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
