using UnityEngine;
using System.Collections;

using Utils;

[RequireComponent(typeof(BoxCollider2D))]
public class PrismLayer : MonoBehaviour 
{
	private const float COLOR_LERP_SPEED = 0.5f;

	private SpriteRenderer m_prismLayerRenderer = null;

	private Color m_defaultColor 				= Color.white;
	private Color m_targetColor					= Color.white;

	private UnitColor m_unitColor				= UnitColor.COLOR_CYAN;

	private float m_lerpTime 					= 0.0f;

	private void Awake()
	{
		m_prismLayerRenderer = GetComponent<SpriteRenderer> ();
	}

	private void Update()
	{
		updatePrismLayerColor ();
	}

	private void updatePrismLayerColor()
	{
		if(GameManager.getInstance().getGameState() != GameState.ACTION_PHASE)
		{
			return;
		}

		m_lerpTime += Time.deltaTime / COLOR_LERP_SPEED;
		m_prismLayerRenderer.color = Color.Lerp (m_defaultColor, m_targetColor, m_lerpTime);
	}

	public void setPrismLayerColor(Color p_color)
	{
		m_targetColor = p_color;
		m_lerpTime = 0.0f;
	}

	public void setUnitColor(UnitColor p_unitColor)
	{
		m_unitColor = p_unitColor;

		Color color = GameManager.getInstance().getUnitColor(m_unitColor);
		setPrismLayerColor(color);
	}

	public void resetPrismLayerColor()
	{
		setPrismLayerColor (m_defaultColor);
	}

	public Color getColor()
	{
		return m_prismLayerRenderer.color;
	}

	public UnitColor getUnitColor()
	{
		return m_unitColor;
	}

//	private void OnTriggerEnter2D(Collider2D p_collider2d)
//	{
////		Debug.Log ("Trigger Enter 2D!");
//		IUnit enemy = p_collider2d.GetComponent<Enemy>();
//		if(enemy != null)
//		{
//			if(m_unitColor != enemy.getUnitColor())
//			{
//				PrismLayerHandler.getInstance().subtractDurability();
//			}
//			else
//			{
//				ScoreHandler.getInstance().addScore(Constants.DEFAULT_SCORE);
//			}
//
//			enemy.onDestroy();
//		}
//	}
}
