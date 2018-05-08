using UnityEngine;
using System.Collections;

using Utils;

public class Debriefing : GameScreen 
{
	public override void onScreenInitialize ()
	{

	}

	public override void onScreenUpdate (float p_deltaTimeMs)
	{
		updateScreenActive ();
	}

	public override string getScreenId ()
	{
		return Constants.SCREEN_DEBRIEFING;
	}

	private void updateScreenActive()
	{
		if (GameManager.getInstance ().getGameState () == GameState.DEBRIEFING) 
		{
			setChildrenActive(true);
		} 
		else
		{
			setChildrenActive(false);
		}
	}

	public void onRetryClicked()
	{
		GameManager.getInstance ().reset ();
		ScoreHandler.getInstance ().reset ();
		Application.LoadLevel (Constants.SCENE_MAIN);
	}
}
