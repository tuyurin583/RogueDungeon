using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    //㩂̃v���n�u
    [SerializeField]
    public GameObject bombPrefab;
	//㩂̐����͈�
    [SerializeField]
	public float bombSpawnRange = 10f;
	//㩂̐������o
	[SerializeField]
    public float bombSpawnInterval = 5f;
    //㩂̐����^�C�}�[
    private float bombSpawnTimer;
    //㩂̍ő�l
    [SerializeField]
    public int MaxBombCount = 10;
    //㩂̐�����
    public int currnetBombCount = 0;
   

	// Start is called before the first frame update
	void Start()
    {
        
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
        }
    }

    private void IdleState()
    {
        //�^�C�}�[�����炷
        bombSpawnTimer -= Time.deltaTime;

        //���e�𐶐����邩����
        if (bombSpawnTimer <= 0f&&currnetBombCount<MaxBombCount)
        {
            //���e�𐶐�����֐��Ăяo��
            SpawnBomb();
            //�^�C�}�[���Z�b�g
            bombSpawnTimer=bombSpawnInterval;
            //�������𑝂₷
            currnetBombCount++;
        }
        state = DemonState.Move;
    }

    private void SpawnBomb()
    {
		//�f�[�����̌��݂̈ʒu���烉���_���Ȉʒu�ɃI�t�Z�b�g���v�Z����
	    Vector3 offset = new Vector3(Random.Range(-bombSpawnRange, bombSpawnRange), 0, Random.Range(-bombSpawnRange, bombSpawnRange));
		//�I�t�Z�b�g�����ʒu��Y��+1����
		Vector3 spawnPosition = transform.position + offset + Vector3.up;
		//���e�̃v���n�u���X�|�[���ʒu�ɃC���X�^���X������i�ʂ̕ϐ��ɑ������j
		GameObject bomb = Instantiate(bombPrefab, spawnPosition, transform.rotation);
		//���e�Ɏ�����e�Ƃ��Đݒ肷��i���e���������Ƃ��ɐ����������炷���߁j
		bomb.transform.parent = transform;
	}


	private void MoveState()
    {
        state = DemonState.Idle;
    }
}
