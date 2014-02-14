using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// This is the base State class. All States inherit off of it. 
/// </summary>
namespace MayneTools.StateMachine
{
		[System.Serializable]
		public abstract class ww_State <T> where T : MonoBehaviour
		{
				protected string stateName;
				private int id; 
				private static int nextAvailableID = 0;
				public T Context { get; set; }
	
				public ww_State ()
				{
					id = nextAvailableID++; 
				}
		
				public int StateID {
						get { return id; } 
				}
	
				public string StateName {
						get { return stateName; }
				}
	
				public virtual void EnterState ()
				{
	
				}
	
				public virtual void ExecuteState ()
				{
	
				}
	
				public virtual void ExitState ()
				{
	
				}
		}
}