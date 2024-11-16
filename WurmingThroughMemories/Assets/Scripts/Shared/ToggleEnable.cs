using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Toggle))]
public class ToggleEnable : MonoBehaviour
{
	public Transform target; // inspector

	private Toggle toggle;

	public void Awake()
	{
		toggle = GetComponent<Toggle>();
		toggle.onValueChanged.AddListener(SetToggle);
		target.gameObject.SetActive(toggle.isOn);
	}

	private void SetToggle(bool on)
	{
		target.gameObject.SetActive(on);
	}
}