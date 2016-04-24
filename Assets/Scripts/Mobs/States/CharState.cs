using System;


public abstract class CharState
{
	public readonly States state;

	public CharState (States state)
	{
		this.state = state;
	}

	virtual public void enter(CharController owner){}
	abstract public void update(CharController owner);
	virtual public void exit(CharController owner){}

}


