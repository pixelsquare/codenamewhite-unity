using UnityEngine;

[System.Serializable]
public struct SegmentData
{
	private Vector2 m_destinationFrom;
	private Vector2 m_destinationTo;
	private float m_segmentLength;
	private Color m_layerColor;

	public SegmentData(Vector2 p_destFrom, Vector2 p_destTo)
	{
		m_destinationFrom = p_destFrom;
		m_destinationTo = p_destTo;

		m_segmentLength = Vector2.Distance(m_destinationFrom, m_destinationTo);
		m_layerColor = Color.white;
	}

	public Vector2 getDestinationFrom()
	{
		return m_destinationFrom;
	}

	public Vector2 getDestinationTo()
	{
		return m_destinationTo;
	}

	public float getSegmentLength()
	{
		return m_segmentLength;
	}

	public Color getLayerColor()
	{
		return m_layerColor;
	}

	public void setDestinationFrom(Vector2 p_destinationFrom)
	{
		m_destinationFrom = p_destinationFrom;
	}

	public void setDestinationTo(Vector2 p_destinationTo)
	{
		m_destinationTo = p_destinationTo;
	}

	public void setLayerColor(Color p_color)
	{
		m_layerColor = p_color;
	}
}
