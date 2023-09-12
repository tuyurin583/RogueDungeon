using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class BaseEnemy : MonoBehaviour
{
	//�A�j���[�^�[
	protected Animator animator;
	//���M�b�h�{�f�B�[
    Rigidbody rb;
	//NavMeshAgent
	protected NavMeshAgent agent;
	[SerializeField,Header("����")]
	public float searchAngle = 45f;
	[SerializeField]
	protected float attackDistance=5;
	protected bool IsDead = false;
	[Header("HP")]
	[SerializeField]
	//�ő�HP
	protected float Maxhp = 50f;
	[SerializeField]
	//���݂�HP
	protected float currnethp;
	[Header("TL")]
	[SerializeField]
	protected PlayableDirector[] TLAttack;
	[SerializeField]
	protected PlayableDirector TLDamage;
	[SerializeField]
	protected PlayableDirector TLDeath;
	//�h���b�v����A�C�e���̃v���n�u
	public GameObject itemPrefab;
	// �h���b�v�m��
	public float dropChance = 0.5f; 


	// Start is called before the first frame update
	void Start()
    {
        animator=GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
		agent=GetComponent<NavMeshAgent>();
	}

	public void DropItem()
	{
		//�ݒ肵���A�C�e�����m���Ńh���b�v������
		if (Random.value < dropChance)
		{
			Instantiate(itemPrefab, transform.position + Vector3.up, Quaternion.identity);
		}
	}

}
