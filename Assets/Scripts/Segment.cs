using UnityEngine;
using System.Collections;

public class Segment : MonoBehaviour 
{
	private Transform m_segmentPivot							= null;

	private const float SEGMENT_LENGTH_THRESHOLD 				= 0.05f;

	private void Awake()
	{
		m_segmentPivot = transform.GetChild (0);
	}

	public void setPivotLength(float p_segmentLength)
	{
		Vector3 pivotScale = m_segmentPivot.localScale;
		pivotScale.x = p_segmentLength * SEGMENT_LENGTH_THRESHOLD;
		m_segmentPivot.localScale = pivotScale;
	}
}
