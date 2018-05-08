using Utils;

public interface IUnit
{
	void onSpawn();
	void onUpdate();
	void onDestroy();

	string getUnitId();
	UnitColor getUnitColor();
	void setUnitColor(UnitColor p_color);
}
