using UnityEngine;
using System.Collections;

public class Fight2D : Unit
{

	
	static GameObject NearTarget(Vector3 position, Collider2D[] array)
	// функция возвращает ближайший объект из массива, относительно указанной позиции
	{
		Collider2D current = null;
		float dist = Mathf.Infinity;

		foreach (Collider2D coll in array)
		{
			float curDist = Vector3.Distance(position, coll.transform.position);

			if (curDist < dist)
			{
				current = coll;
				dist = curDist;
			}
		}

		return (current != null) ? current.gameObject : null;
	}

	
	public static void Action(Vector2 point, float radius, int layerMask, int damage, bool allTargets)
	// point - точка контакта
	// radius - радиус поражения
	// layerMask - номер слоя, с которым будет взаимодействие
	// damage - наносимый урон
	// allTargets - должны-ли получить урон все цели, попавшие в зону поражения
	{
		Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, 1 << layerMask);

		if (!allTargets)
		{
			GameObject obj = NearTarget(point, colliders);
			if (obj != null && obj.GetComponent<Unit>())
			{
				obj.GetComponent<Unit>().ReceiveDamage(damage);
			}
			return;
		}

		foreach (Collider2D hit in colliders)
		{
			if (hit.GetComponent<Unit>())
			{
				hit.GetComponent<Unit>().ReceiveDamage(damage);
			}
		}
	}
}