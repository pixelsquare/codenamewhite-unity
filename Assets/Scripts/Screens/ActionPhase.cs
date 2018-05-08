using UnityEngine;
using System.Collections;

using Utils;

public class ActionPhase : GameScreen
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
		return Constants.SCREEN_ACTION_PHASE;
	}

	private void updateScreenActive()
	{
		if(GameManager.getInstance().getGameState() == GameState.ACTION_PHASE)
		{
			setChildrenActive(true);
		}
		else
		{
			setChildrenActive(false);
		}
	}
}
