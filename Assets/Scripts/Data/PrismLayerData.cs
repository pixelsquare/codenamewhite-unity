using UnityEngine;

[System.Serializable]
public struct PrismLayerData
{
	[SerializeField] private int m_radius;
	[SerializeField] private int m_segmentOffset;
	[SerializeField] private int m_sides;

	private int m_durability;
	private Vector3 m_position;
	private SegmentData[] m_segmentData;
	private PrismLayer[] m_prismLayers;

	private const int MAX_ANGLE = 360;

	public void initialize(Vector3 p_position)
	{
		m_position = p_position;

		m_segmentData = new SegmentData[m_sides];

		updateSegments();
	}
	
	private void updateSegments()
	{
		for(int i = 0; i < m_sides; i++)
		{
			float segmentTime1 = i * (MAX_ANGLE / m_sides) + m_segmentOffset;
			float segmentTime2 = (i + 1) * (MAX_ANGLE / m_sides) + m_segmentOffset;

			Vector2 curSegmentPoint = getSegmentPoint(segmentTime1);
			Vector2 nextSegmentPoint = getSegmentPoint(segmentTime2);

			SegmentData segmentData = new SegmentData(curSegmentPoint, nextSegmentPoint);
			m_segmentData[i] = segmentData;
		}
	}

	public void disableAllPrismLayers()
	{
		for(int i = 0; i < m_prismLayers.Length; i++)
		{
			m_prismLayers[i].gameObject.SetActive(false);
		}
	}

	private Vector2 getSegmentPoint(float p_time)
	{
		Vector2 result = new Vector2();
		result.x = m_position.x + Mathf.Cos(p_time * Mathf.Deg2Rad) * m_radius;
		result.y = m_position.y + Mathf.Sin(p_time * Mathf.Deg2Rad) * m_radius;
		return result;
	}

	public SegmentData getSegmentData(int p_idx)
	{
		return m_segmentData[p_idx];
	}

	public PrismLayer getPrismLayer(int p_idx)
	{
		return m_prismLayers [p_idx];
	}

	public int getSegmentDataLength()
	{
		return m_segmentData.Length;
	}
	
	public PrismLayer[] getPrismLayers()
	{
		return m_prismLayers;
	}

	public int getRadius()
	{
		return m_radius;
	}

	public int getDurability()
	{
		return m_durability;
	}

	public void setPrismLayers(PrismLayer[] p_prismLayers)
	{
		m_prismLayers = p_prismLayers;
	}

	public void setRadius(int p_radius)
	{
		m_radius = p_radius;
	}

	public void setDurability(int p_durability)
	{
		m_durability = p_durability;
	}

# if UNITY_EDITOR
	public void onDrawGizmos()
	{
		for(int i = 0; i < m_sides; i++)
		{
			float segmentTime1 = i * (MAX_ANGLE / m_sides) + m_segmentOffset;
			float segmentTime2 = (i + 1) * (MAX_ANGLE / m_sides) + m_segmentOffset;
			
			Gizmos.DrawLine(getSegmentPoint(segmentTime1), getSegmentPoint(segmentTime2));
		}
	}
# endif
}
