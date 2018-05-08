using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class ScoreHandler : MonoBehaviour 
{
	private static ScoreHandler s_instance				= null;
	public static ScoreHandler getInstance()
	{
		return s_instance;
	}

	[SerializeField] private Text m_actionScoreTxt		= null;
	[SerializeField] private Text m_debriefingScoreTxt 	= null;

	private int m_score							= 0;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
		if(null == s_instance)
		{
			s_instance = this;
		}
		else
		{
			Destroy(gameObject);
		}
	}

	public void addScore(int p_score = 1)
	{
		m_score += p_score;
		updateScoreText();
	}

	public void subtractScore(int p_score = 1)
	{
		m_score -= p_score;
		updateScoreText();
	}

	public int getScore()
	{
		return m_score;
	}

	public void reset()
	{
		m_score = 0;
		updateScoreText ();
	}

	private void updateScoreText()
	{
		updateActionScoreText ();
		updateDebriefingScoreText ();
	}

	private void updateActionScoreText()
	{
		if(null == m_actionScoreTxt)
		{
			return;
		}

		m_actionScoreTxt.text = m_score.ToString("D6");
	}

	private void updateDebriefingScoreText()
	{
		if(null == m_actionScoreTxt)
		{
			return;
		}
		
		m_debriefingScoreTxt.text = m_score.ToString("D6");
	}
}
