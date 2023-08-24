using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class BaseEnemy : MonoBehaviour,IDamageable
{
	//PlayablesDirector�R���|�[�l���g
	[SerializeField]
	protected PlayableDirector[] timeline;
	//�A�j���[�^�[
	protected Animator animator;
	//���M�b�h�{�f�B�[
    Rigidbody rb;
    [SerializeField]
    //�ő�HP
    protected float Maxhp=50f;
    //���݂�HP
    public float currnethp;
	// ���S���������s���ꂽ���̃t���O
	protected bool isDead = false;
	// ���S���̃G�t�F�N�g
	public GameObject deathEffect;
	//NavMeshAgent
	protected NavMeshAgent agent;
	[SerializeField,Header("����")]
	public float searchAngle = 45f;
	[SerializeField, Header("�ړ����x")]
	protected float Speed;


	// Start is called before the first frame update
	void Start()
    {
        animator=GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
		agent=GetComponent<NavMeshAgent>();
        //HP�̏����ݒ�
        currnethp = Maxhp;
    }


    public void Damage(float damage)
    {
        //���Ɏ��S���Ă�����_���[�W���󂯂Ȃ�
        if (isDead) return;
        currnethp -= damage;
		//Debug.Log(currnethp);
		if (currnethp <= 0)
		{
			Death();
		}

	}

    public void Death()
    {
		// ���Ɏ��S���Ă����珈�����Ȃ�
		if (isDead) return;
		//Debug.Log("Death");
		isDead = true;
		// �A�j���[�V��������
		animator.SetTrigger("Death");
		if (deathEffect != null)
		{
			//�G�t�F�N�g�Đ�
			Instantiate(deathEffect, transform.position, Quaternion.identity);
		}
		// �I�u�W�F�N�g���폜
		Destroy(gameObject);
		// ���S�ʒm�𑗐M
		SendMessage("OnDeath", SendMessageOptions.DontRequireReceiver);

	}

}
