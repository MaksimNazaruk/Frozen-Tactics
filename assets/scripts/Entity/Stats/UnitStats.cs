using System.Collections;

public class UnitStats : EntityStats {

	public enum UnitBehaviourStyle	{ Safe, Normal, Aggressive };
	public UnitBehaviourStyle behaviourStyle;

	public float speed;
}
