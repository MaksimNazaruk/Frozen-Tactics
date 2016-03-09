using System.Collections;

public class EntityStats {

	public int playerId;

	public enum BasicType { BasicTypeBuilding, BasicTypeUnit };
	public BasicType basicType;

	public string title;

	public float fullHealth;
	public float currentHealth;

	// public float armor;
	// public float armorType;

	public float size;

	public float buildTime;

	public float visionRange;
	public float attackRange;

}
