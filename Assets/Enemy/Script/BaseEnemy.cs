using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Playables;

public class BaseEnemy : MonoBehaviour
{
	//アニメーター
	protected Animator animator;
	//リギッドボディー
    Rigidbody rb;
	//NavMeshAgent
	protected NavMeshAgent agent;
	[SerializeField,Header("視野")]
	public float searchAngle = 45f;
	[SerializeField]
	protected float attackDistance=5;
	protected bool IsDead = false;
	[Header("HP")]
	[SerializeField]
	//最大HP
	protected float Maxhp = 50f;
	[SerializeField]
	//現在のHP
	protected float currnethp;
	[Header("TL")]
	[SerializeField]
	protected PlayableDirector[] TLAttack;
	[SerializeField]
	protected PlayableDirector TLDamage;
	[SerializeField]
	protected PlayableDirector TLDeath;
	

	// Start is called before the first frame update
	void Start()
    {
        animator=GetComponent<Animator>();
        rb=GetComponent<Rigidbody>();
		agent=GetComponent<NavMeshAgent>();
		


	}

}
