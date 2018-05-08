using UnityEngine;
using System.Collections.Generic;

using Utils;

public class GameManager : MonoBehaviour 
{
	private static GameManager s_instance						= null;
	public static GameManager getInstance()
	{
		return s_instance;
	}

	[SerializeField] private GameState m_startingState			= GameState.MAIN_MENU;
	[SerializeField] private int m_prismLayerCount 				= 0;
	[SerializeField] private PrismLayerData m_basePrismLayer 	= new PrismLayerData();
	[SerializeField] private int[] m_prismLayerDurability 		= null;
	[SerializeField] private Color[] m_prismColors 				= null;

	private Color[] m_secondaryColors							= null;
	private int m_currentLayer									= 0;
	private GameState m_gameState								= GameState.MAIN_MENU;

	private Dictionary<UnitColor, Color> m_secondaryColorsD		= null;

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

	private void Start()
	{
		initializeColors ();

		PrismLayerHandler.getInstance ().initialize (m_prismLayerCount, m_basePrismLayer, m_secondaryColors);
		PrismLayerHandler.getInstance ().setLayerDurability (m_prismLayerDurability);
	}

	private void Update()
	{
		updateGameStates ();
	}

	private void updateGameStates()
	{
		switch(m_gameState)
		{
			case GameState.MAIN_MENU:
			{
				break;
			}
			case GameState.ACTION_PHASE:
			{
				if(PrismLayerHandler.getInstance().isAllPrismLayersDestroyed())
				{
					setGameState(GameState.DEBRIEFING);
				}
				break;
			}
			case GameState.DEBRIEFING:
			{
				break;
			}
		}
	}

	private void initializeColors()
	{
		int colorsLen = m_prismColors.Length;
		m_secondaryColors = new Color[colorsLen];
		m_secondaryColorsD = new Dictionary<UnitColor, Color>();

		Color tmpColor = m_prismColors [0];
		for(int i = 0; i < colorsLen; i++)
		{
			m_secondaryColors[i] = (i < (colorsLen - 1)) 
				? getCombinedColor(m_prismColors[i], m_prismColors[i + 1])
				: getCombinedColor(tmpColor, m_prismColors[i]);

			m_secondaryColorsD.Add((UnitColor)(i + 1), m_secondaryColors[i]);
		}

	}

	public void moveToNextLayer()
	{
		int curLayer = m_currentLayer;
		curLayer++;

		if(curLayer >= m_prismLayerCount)
		{
			return;
		}

		PrismLayerHandler.getInstance ().resetPrismLayerColor ();
		m_currentLayer = curLayer;
		PrismLayerHandler.getInstance ().updatePrismLayerColors ();
	}

	public void disableCurrentLayer()
	{
		PrismLayerHandler.getInstance().disablePrismLayers(m_currentLayer);
	}

	private Color getCombinedColor(Color p_colorA, Color p_colorB)
	{
		return (p_colorA + p_colorB) / 2.0f;
	}

	public Color getUnitColor(UnitColor p_unitColor)
	{
		return m_secondaryColorsD[p_unitColor];
	}

	public UnitColor getUnitColor(Color p_color)
	{
		foreach(KeyValuePair<UnitColor, Color> color in m_secondaryColorsD)
		{
			if(color.Value == p_color)
			{
				return color.Key;
			}
		}

		return UnitColor.COLOR_CYAN;
	}

	public int getCurrentLayer()
	{
		return m_currentLayer;
	}

	public Color[] getSecondaryColors()
	{
		return m_secondaryColors;
	}

	public GameState getGameState()
	{
		return m_gameState;
	}

	public void setGameState(GameState p_state)
	{
		m_gameState = p_state;
	}

	public void reset()
	{
		setGameState (m_startingState);
		m_currentLayer = 0;
		initializeColors ();
		
		PrismLayerHandler.getInstance ().initialize (m_prismLayerCount, m_basePrismLayer, m_secondaryColors);
		PrismLayerHandler.getInstance ().setLayerDurability (m_prismLayerDurability);
	}
}
