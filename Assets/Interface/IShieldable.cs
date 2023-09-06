using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IShieldable 
{
	// シールドの耐久値を減少させる
	public void  TakeShieldDamage(float damage);
	
}
