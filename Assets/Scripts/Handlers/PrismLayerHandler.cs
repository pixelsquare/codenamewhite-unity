using UnityEngine;
using System.Collections.Generic;

using Utils;

public class PrismLayerHandler : MonoBehaviour 
{
	private static PrismLayerHandler s_instance				= null;
	public static PrismLayerHandler getInstance()
	{
		return s_instance;
	}

	[SerializeField] private GameObject m_refPrismLayer		= null;

	private PrismLayerData[] m_prismLayerData				= null;
	private List<Color> m_prismLayerColors					= null;

	private void Awake()
	{
		DontDestroyOnLoad (gameObject);
		if(s_instance == null)
		{
			s_instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void initialize(int p_dataLength, PrismLayerData p_baseData, Color[] p_colors)
	{
		int[] radii = getLayerDataRadii (p_dataLength, p_baseData.getRadius());
		m_prismLayerData = new PrismLayerData[p_dataLength];

		for(int i = 0; i < p_dataLength; i++)
		{
			m_prismLayerData[i] = p_baseData;
			m_prismLayerData[i].setRadius(radii[i]);
			m_prismLayerData[i].initialize(transform.position);
		}

		initializePrismLayer (p_dataLength);
		setLayerColors (p_colors);
	}

	public void setLayerColors(Color[] p_colors)
	{
		m_prismLayerColors = new List<Color> ();
		m_prismLayerColors.AddRange (p_colors);
		updatePrismLayerColors ();
	}

	public void setLayerDurability(int[] p_layerDurability)
	{
		for(int i = 0; i < m_prismLayerData.Length; i++)
		{
			m_prismLayerData[i].setDurability(p_layerDurability[i]);
		}
	}

	private void initializePrismLayer(int p_dataLength)
	{
		for(int i = 0; i < p_dataLength; i++)
		{
			createPrismLayer(i);
		}
	}

	private void createPrismLayer(int p_prismLayerIdx)
	{
		int segmentDataLen = m_prismLayerData [p_prismLayerIdx].getSegmentDataLength ();

		// Collection of prism layers to be placed inside prism layer data
		PrismLayer[] prismLayers = new PrismLayer[segmentDataLen];

		for(int i = 0; i < segmentDataLen; i++)
		{
			GameObject prismLayer = Instantiate(m_refPrismLayer) as GameObject;
			prismLayer.name = string.Format("line_layer_{0}", i);
			
			setPrismLayerTransform(prismLayer.transform, p_prismLayerIdx, i);

			PrismLayer prismLayerComponent = prismLayer.GetComponentInChildren<PrismLayer>();
			prismLayers[i] = prismLayerComponent;
		}

		m_prismLayerData [p_prismLayerIdx].setPrismLayers (prismLayers);
	}

	private void setPrismLayerTransform(Transform p_layerT, int p_prismLayerIdx, int p_segmentDataIdx)
	{
		SegmentData segmentData = m_prismLayerData [p_prismLayerIdx].getSegmentData (p_segmentDataIdx);
		Vector2 targetDirection = segmentData.getDestinationTo() - segmentData.getDestinationFrom();
		
		// Set the position and (look) rotation of the segment pivot
		Transform prismLayerT = p_layerT;
		prismLayerT.SetParent(transform, false);
		prismLayerT.position = segmentData.getDestinationFrom();
		prismLayerT.rotation = Quaternion.LookRotation(prismLayerT.forward, targetDirection);
		
		// Stretch the pivot's length
		Segment segment = prismLayerT.GetComponent<Segment> ();
		segment.setPivotLength (segmentData.getSegmentLength());
	}

	private int[] getLayerDataRadii(int p_len, int p_maxRadius)
	{
		int[] resultRadii = new int[p_len];
		int radius = p_maxRadius / (p_len + 1);
		int currentRadius = p_maxRadius;

		for(int i = 1; i <= p_len; i++)
		{
			currentRadius -= radius;
			resultRadii[i - 1] = currentRadius;
		}
		
		return resultRadii;
	}

	public void updatePrismLayerColors()
	{
		int currentLayer = GameManager.getInstance().getCurrentLayer();

		for(int i = 0; i < m_prismLayerData.Length; i++)
		{
			PrismLayer prismLayer = m_prismLayerData[currentLayer].getPrismLayer(i);

			// Retrieving unit color using the real color
			UnitColor unitColor = GameManager.getInstance().getUnitColor(m_prismLayerColors[i]);
			prismLayer.setUnitColor(unitColor);
		}
	}

	public void resetPrismLayerColor()
	{
		int currentLayer = GameManager.getInstance().getCurrentLayer();
		int prismLayerLen = m_prismLayerData[currentLayer].getSegmentDataLength();
		
		for(int i = 0; i < prismLayerLen; i++)
		{
			PrismLayer prismLayer = m_prismLayerData[currentLayer].getPrismLayer(i);
			prismLayer.resetPrismLayerColor();
		}
	}

	public void rotatePrismColorsClockwise()
	{
		Color baseColor = m_prismLayerColors [0];
		m_prismLayerColors.RemoveAt (0);
		m_prismLayerColors.Add (baseColor);

		updatePrismLayerColors ();
	}

	public void rotatePrismColorsCounterClockwise()
	{
		int colorsLen = m_prismLayerColors.Count;
		Color lastColor = m_prismLayerColors [colorsLen - 1];
		m_prismLayerColors.RemoveAt (colorsLen - 1);
		m_prismLayerColors.Insert (0, lastColor);

		updatePrismLayerColors ();
	}

	public void disablePrismLayers(int p_idx)
	{
		m_prismLayerData [p_idx].disableAllPrismLayers ();
	}

	public void subtractDurability(int p_count = 1)
	{
		int curLayer = GameManager.getInstance().getCurrentLayer();
		int durability = m_prismLayerData[curLayer].getDurability();

		durability -= p_count;
		m_prismLayerData[curLayer].setDurability(durability);

		updatePrismLayerDurability();
	}

	public void updatePrismLayerDurability()
	{
		int curLayer = GameManager.getInstance().getCurrentLayer();
		int durability = m_prismLayerData[curLayer].getDurability();

		if(durability <= 0)
		{
			GameManager.getInstance().disableCurrentLayer();
			GameManager.getInstance().moveToNextLayer();
		}
	}

	public bool isAllPrismLayersDestroyed()
	{
		int destroyedLayerCount = 0;
		for(int i = 0; i < m_prismLayerData.Length; i++)
		{
			if(m_prismLayerData[i].getDurability() == 0)
			{
				destroyedLayerCount++;
			}
		}

		return destroyedLayerCount == m_prismLayerData.Length;
	}

	# if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		if(m_prismLayerData == null || m_prismLayerData.Length <= 0)
		{
			return;
		}
		
		for(int i = 0; i < m_prismLayerData.Length; i++)
		{
			m_prismLayerData[i].onDrawGizmos();
		}
	}
	# endif
}
