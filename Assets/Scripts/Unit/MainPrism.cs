using UnityEngine;
using System.Collections;

using Utils;

public class MainPrism : MonoBehaviour, IUnit
{
	[SerializeField] private int m_rotationSpeed				= 3;
	[SerializeField] private SpriteRenderer m_spriteRenderer	= null;

	private int m_angle											= 0;
	private UnitColor m_unitColor								= UnitColor.COLOR_CYAN;

	private void OnEnable()
	{
		onSpawn();
	}

	private void OnDisable()
	{
		onDestroy();
	}

	private void Start()
	{
		m_spriteRenderer = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		onUpdate();
	}

	public void setMainPrismColor(Color p_color)
	{
		m_spriteRenderer.color = p_color;
	}

	public void onSpawn()
	{

	}

	public void onUpdate()
	{
		if(GameManager.getInstance().getGameState() == GameState.DEBRIEFING)
		{
			return;
		}

		m_angle += m_rotationSpeed;

		if(m_angle % 360 == 0)
		{
			m_angle = 0;
		}

		transform.rotation = Quaternion.Euler(Vector3.forward * -m_angle);
	}

	public void onDestroy()
	{

	}

	public string getUnitId()
	{
		return Constants.MAIN_PRISM;
	}

	public UnitColor getUnitColor()
	{
		return m_unitColor;
	}
	
	public void setUnitColor(UnitColor p_unitColor)
	{
		m_unitColor = p_unitColor;

		Color color = GameManager.getInstance().getUnitColor(m_unitColor);
		setMainPrismColor(color);
	}
}
