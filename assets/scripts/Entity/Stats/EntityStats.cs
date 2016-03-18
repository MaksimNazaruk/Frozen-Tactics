using System.Collections;

public class EntityStats {

	public int id;
	public int commanderId;

	public enum BasicType { BasicTypeBuilding, BasicTypeUnit };
	public BasicType basicType;

	public string title;

	public float fullHealth;
	public float currentHealth;

	// public float armor;
	// public float armorType;

	public float size;

	public float visionRange;
	public float attackRange;

}
