using UnityEngine;
using System.Collections;

public class ScorePop : MonoBehaviour 
{
	private const float FLOAT_SPEED					= 30.0f;

	[SerializeField] private float m_destroyTime	= 1.0f;
 	[SerializeField] private TextMesh m_textMesh	= null;

	private string m_text							= "";
	
	private void Start()
	{
		Destroy(gameObject, m_destroyTime);
	}

	private void Update()
	{
		transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.up, FLOAT_SPEED * Time.deltaTime);
	}

	public void setText(string p_text)
	{
		m_text = p_text;
		updateText();
	}

	private void updateText()
	{
		if(null == m_textMesh)
		{
			return;
		}

		m_textMesh.text = m_text;
	}
}
