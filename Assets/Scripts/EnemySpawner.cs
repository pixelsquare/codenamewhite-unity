using UnityEngine;
using System.Collections;

using Utils;

public class EnemySpawner : MonoBehaviour 
{
	[SerializeField] private float m_spawnDelay				= 1.0f;
	[SerializeField] private Vector2 m_size 				= new Vector3 ();
	[SerializeField] private float m_safeZoneSize			= 500.0f;
	[SerializeField] private bool m_useCanvasSizeWidth 		= false;
	[SerializeField] private bool m_useCanvasSizeHeight 	= false;
	[SerializeField] private Enemy[] m_enemies 				= null;

	private Canvas m_canvas									= null;
	private float m_spawnTimer 								= 0.0f;

	private void Start()
	{
		m_canvas = GameObject.FindObjectOfType<Canvas> ();
		m_spawnTimer = m_spawnDelay;

		initializeSpawnBox ();
	}

	private void Update()
	{
		updateEnemySpawn ();
	}

	public void startSpawning(float p_spawnDelay = 1.0f)
	{
		m_spawnDelay = p_spawnDelay;
		m_spawnTimer = m_spawnDelay;
	}

	private void initializeSpawnBox()
	{
		RectTransform canvasRectT = m_canvas.GetComponent<RectTransform> ();

		if(m_useCanvasSizeWidth)
		{
			m_size.x = canvasRectT.rect.width * 0.5f;
		}

		if(m_useCanvasSizeHeight)
		{
			m_size.y = canvasRectT.rect.height * 0.5f;
		}
	}

	private void updateEnemySpawn()
	{
		if(GameManager.getInstance().getGameState() != GameState.ACTION_PHASE)
		{
			return;
		}

		if(m_spawnTimer > 0.0f)
		{
			m_spawnTimer -= Time.deltaTime;

			if(m_spawnTimer <= 0.0f)
			{
				spawnEnemy();
				m_spawnTimer = m_spawnDelay;
			}
		}
	}

	private void spawnEnemy()
	{
		if(m_enemies == null || m_enemies.Length <= 0)
		{
			return;
		}

		int enemyIdx = getRandomEnemyIdx ();
		Vector2 enemyRandomPos = getRandomPosition ();

		GameObject enemy = Instantiate (m_enemies [enemyIdx].gameObject) as GameObject;
		enemy.transform.position = enemyRandomPos;
		enemy.transform.SetParent(transform, false);

		IUnit enemyUnit = enemy.GetComponent<Enemy>();
		enemyUnit.setUnitColor(getRandomUnitColor());
	}

	private int getRandomEnemyIdx()
	{
		System.Random random = new System.Random ();
		int result = random.Next (0, m_enemies.Length);
		return result;
	}

	public Vector2 getRandomPosition() 
	{
		int randPosX = Random.Range((int)-m_size.x, (int)m_size.x);
		int randPosY = Random.Range((int)-m_size.x, (int)m_size.x);

		// Add the difference between the current position and safezone value, 
		// to prevent enemies from spawning inside the safezone circle
		randPosX += (Mathf.Abs(randPosX) < m_safeZoneSize) ? (int)(m_safeZoneSize - Mathf.Abs(randPosX)) : 0;
		randPosY += (Mathf.Abs(randPosY) < m_safeZoneSize) ? (int)(m_safeZoneSize - Mathf.Abs(randPosY)) : 0;

		return new Vector2 (randPosX, randPosY);
	}

	private UnitColor getRandomUnitColor()
	{
		int enumLen = System.Enum.GetNames(typeof(UnitColor)).Length;

		System.Random random = new System.Random();
		int randIdx = random.Next(1, enumLen + 1);

		return (UnitColor) randIdx;
	}

# if UNITY_EDITOR
	private void OnDrawGizmos()
	{
		Gizmos.DrawLine (transform.position + new Vector3(-m_size.x, m_size.y, 0f), 
		                 transform.position + new Vector3(m_size.x, m_size.y, 0f));
		
		Gizmos.DrawLine (transform.position + new Vector3(m_size.x, m_size.y, 0f), 
		                 transform.position + new Vector3(m_size.x, -m_size.y, 0f));
		
		Gizmos.DrawLine (transform.position + new Vector3(m_size.x, -m_size.y, 0f), 
		                 transform.position + new Vector3(-m_size.x, -m_size.y, 0f));
		
		Gizmos.DrawLine (transform.position + new Vector3(-m_size.x, -m_size.y, 0f), 
		                 transform.position + new Vector3(-m_size.x, m_size.y, 0f));


		for(int i = 0; i < 360; i++)
		{
			Vector3 pointA = new Vector3();
			pointA.x = transform.position.x + Mathf.Cos(Mathf.Deg2Rad * i) * m_safeZoneSize;
			pointA.y = transform.position.y + Mathf.Sin(Mathf.Deg2Rad * i) * m_safeZoneSize;

			Vector3 pointB = new Vector3();
			pointB.x = transform.position.x + Mathf.Cos(Mathf.Deg2Rad * (i + 1)) * m_safeZoneSize;
			pointB.y = transform.position.y + Mathf.Sin(Mathf.Deg2Rad * (i + 1)) * m_safeZoneSize;

			Gizmos.DrawLine(pointA, pointB);
		}

//		Gizmos.DrawRay(transform.position, Vector3.right * m_size.x);
	}
# endif
}
