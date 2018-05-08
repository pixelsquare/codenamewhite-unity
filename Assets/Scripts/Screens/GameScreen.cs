using UnityEngine;

public abstract class GameScreen : MonoBehaviour
{
	public abstract void onScreenInitialize();
	public abstract void onScreenUpdate(float p_deltaTimeMs);
	public abstract string getScreenId();

	private void Start()
	{
		onScreenInitialize ();
	}

	private void Update()
	{
		onScreenUpdate (Time.deltaTime);
	}

	protected void setChildrenActive(bool p_active)
	{
		foreach(Transform child in transform)
		{
			if(child != null && child.gameObject.activeInHierarchy != p_active)
			{
				child.gameObject.SetActive(p_active);
			}
		}
	}
}
