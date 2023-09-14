using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DemonSensor : MonoBehaviour
{
    [SerializeField]
    private SphereCollider searchArea;
	[SerializeField]
	private float searchAngle = 45f;
	private Demon demonMove;
	// Start is called before the first frame update
	void Start()
    {
        demonMove= transform.parent.GetComponent<Demon>();
		if (demonMove == null)
		{
			Debug.LogError("Demon component not found on parent object");
		}
	}

    // Update is called once per frame
    void Update()
    {
        
    }
	private void OnTriggerStay(Collider collider)
	{
		if (collider.tag == "Player")
		{
			// ���g�i�G�j�ƃv���C���[�̋���
			var positionDiff = collider.transform.position - transform.position;
			// �G���猩���v���C���[�̕���
			var angle = Vector3.Angle(transform.forward, positionDiff);
			var distanceToPlayer = (collider.transform.position - transform.position).magnitude;
			
		}
	}

	// �v���C���[�������������ǂ�����Ԃ����\�b�h
	public bool IsPlayerFound()
	{
		if (demonMove.target != null && demonMove != null)
		{
			return true;
		}
		return false;
	}

#if UNITY_EDITOR
	//�@�T�[�`����p�x�\��
	private void OnDrawGizmos()
	{
		Handles.color = Color.red;
		Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0f, -searchAngle, 0f) * transform.forward, searchAngle * 2f, searchArea.radius);
	}
#endif
}
