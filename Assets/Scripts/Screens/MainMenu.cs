using UnityEngine;
using System.Collections;

using Utils;

public class MainMenu : GameScreen
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
		return Constants.SCREEN_MAIN_MENU;
	}

	private void updateScreenActive()
	{
		if(GameManager.getInstance().getGameState() == GameState.MAIN_MENU)
		{
			setChildrenActive(true);
		}
		else
		{
			setChildrenActive(false);
		}
	}
}
