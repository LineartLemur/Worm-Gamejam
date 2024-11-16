using UnityEngine;

public class EnableComponent : MonoBehaviour
{
	public Behaviour target; // inspector

	public new bool enabled; // inspector


	public void Awake()
	{
		enabled = target.enabled;
	}

	public void Update()
	{
		if (enabled != target.enabled)
		{
			target.enabled = enabled;
		}
	}
}