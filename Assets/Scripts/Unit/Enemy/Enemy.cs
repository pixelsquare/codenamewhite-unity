using UnityEngine;
using System.Collections;

using Utils;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class Enemy : MonoBehaviour, IUnit
{
	[SerializeField] private SpriteRenderer m_spriteRenderer 	= null;
	[SerializeField] private ScorePop m_scorePop				= null;
	[SerializeField] private ParticleSystem m_explosion			= null;

	private float m_enemySpeed									= 50.0f;
	private Color m_enemyColor									= Color.white;

	private Rigidbody2D m_rigidBody2d							= null;
	private Collider2D m_collider2d								= null;

	private Transform m_target									= null;
	private UnitColor m_unitColor								= UnitColor.COLOR_CYAN;

	private void OnEnable()
	{
		onSpawn ();
	}

	private void OnDisable()
	{
		onDestroy ();
	}

	private void Start()
	{
		m_rigidBody2d = GetComponent<Rigidbody2D>();
		m_collider2d = GetComponent<Collider2D>();

		m_rigidBody2d.isKinematic = true;
		m_collider2d.isTrigger = true;

		initializeEnemy();
	}

	private void Update()
	{
		onUpdate();
	}

	private void initializeEnemy()
	{
		m_target = GameObject.FindObjectOfType<MainPrism>().transform;
	}

	public void setEnemyColor(Color p_color)
	{
		m_enemyColor = p_color;
		updateEnemyColor ();
	}

	public void updateEnemyColor()
	{
		if(m_spriteRenderer == null)
		{
			Debug.LogWarning("Sprite renderer is missing!");
			return;
		}

		m_spriteRenderer.color = m_enemyColor;
	}

	private void createScorePop()
	{
		GameObject scorePop = Instantiate(m_scorePop.gameObject) as GameObject;
		scorePop.transform.position = transform.position;
		ScorePop scorePopComponent = scorePop.GetComponent<ScorePop>();
		scorePopComponent.setText(Constants.DEFAULT_SCORE.ToString());
	}

	private void createExplosion()
	{
		GameObject explosion = Instantiate(m_explosion.gameObject) as GameObject;
		explosion.transform.position = transform.position;

		ParticleSystem explosionPs	= explosion.GetComponent<ParticleSystem>();
		explosionPs.startColor = m_enemyColor;
	}

	public void OnTriggerEnter2D(Collider2D p_collider2d)
	{
		PrismLayer prismLayer = p_collider2d.GetComponent<PrismLayer>();
		if(prismLayer != null)
		{
			if(prismLayer.getUnitColor() == m_unitColor)
			{
				createScorePop();
				ScoreHandler.getInstance().addScore(Constants.DEFAULT_SCORE);
			}
			else
			{
				PrismLayerHandler.getInstance().subtractDurability();
			}

			createExplosion();
			onDestroy();
		}
	}

	// Interface Implementation 

	public virtual void onSpawn()
	{
//		Debug.Log (string.Format ("Spawning enemy with ID: {0}", getUnitId ()));
	}

	public virtual void onUpdate()
	{
		if(GameManager.getInstance().getGameState() != GameState.ACTION_PHASE)
		{
			return;
		}

		if(m_target != null)
		{
			transform.position = Vector3.MoveTowards(transform.position, m_target.position, m_enemySpeed * Time.deltaTime);
		}
	}

	public virtual void onDestroy()
	{
//		Debug.Log (string.Format ("Destroying enemy with ID: {0} with Color: {1}", getUnitId (), m_unitColor));
		Destroy(gameObject);
	}

	public virtual string getUnitId()
	{
		return Constants.ENEMY;
	}

	public UnitColor getUnitColor()
	{
		return m_unitColor;
	}

	public void setUnitColor(UnitColor p_unitColor)
	{
		m_unitColor = p_unitColor;

		Color color = GameManager.getInstance().getUnitColor(m_unitColor);
		setEnemyColor(color);
	}

	// End of Interface
}
