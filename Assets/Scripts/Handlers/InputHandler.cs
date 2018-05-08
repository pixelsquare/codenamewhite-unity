using UnityEngine;
using System.Collections;

using Utils;

public class InputHandler : MonoBehaviour 
{
	private Vector2 m_touchStartPos 	= new Vector2 ();

	private void Update()
	{
		updateStartButton ();
		updateMouseInput();
	}

	private void updateStartButton()
	{
		GameState gameState = GameManager.getInstance ().getGameState ();
		if(gameState != GameState.MAIN_MENU)
		{
			return;
		}

		if(Input.GetMouseButtonDown(0))
		{
			gameState = GameState.ACTION_PHASE;
			GameManager.getInstance ().setGameState (gameState);
		}
	}

	private void updateMouseInput()
	{
		if(GameManager.getInstance().getGameState() != GameState.ACTION_PHASE)
		{
			return;
		}

		if(Input.GetMouseButtonDown(0))
		{
			m_touchStartPos = Input.mousePosition;
		}

		if(Input.GetMouseButtonUp(0))
		{
			Vector2 endPosition = Input.mousePosition;
			Vector2 touchDirection = m_touchStartPos - endPosition;
			
			if(touchDirection.x > 0.0f)
			{
//				Debug.Log("Direction Right!");
				PrismLayerHandler.getInstance().rotatePrismColorsClockwise();

			}
			
			if(touchDirection.x < 0.0f)
			{
//				Debug.Log("Direction Left!");
				PrismLayerHandler.getInstance().rotatePrismColorsCounterClockwise();
			}
		}
	}
}
