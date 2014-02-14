using UnityEngine;
using System; 
using System.Collections;
using System.Collections.Generic; 

/// <summary>
/// This class is in change of controlling States of generic type T. 
/// <typeparam name="T_Context"> is the class of the Context. (the class the Statemachine is effecting). This must be the same type as the ww_StatePassed<T></typeparam>
/// <typeparam name="T_StartState"> is the starting state. This should init the Context and set all default values. ResetMachine() will remove all states and add this.<T></typeparam>
/// </summary>
namespace MayneTools.StateMachine
{
		[System.Serializable]
		public abstract class ww_StateHandler<T_Context, T_StartState> : MonoBehaviour where T_Context : MonoBehaviour where T_StartState : ww_State<T_Context>
		{
				//A list of all active states
				private List<ww_State<T_Context>> states = new List<ww_State<T_Context>> ();
				//Getter/Setter for the states 
				public List<ww_State<T_Context>> States { get { return states; } }
		
				/// <summary>
				/// Addes a new state of Type T to the current active states. 
				/// </summary>
				/// <returns>The the new state that was added to the States list.</returns>
				/// <typeparam name="T">The type of state to add.</typeparam>
				public ww_State<T_Context> AddState<I_StateType> () where I_StateType : ww_State<T_Context>
				{

						//Construct new state.
						ww_State<T_Context> newState = Activator.CreateInstance (typeof(I_StateType)) as ww_State<T_Context>;

						Debug.Log (newState.ToString ());

						//Send this context to the State.
						newState.Context = this as T_Context;
	
						//Add it to our states
						States.Add (newState);
	
						//Call the enter state
						newState.EnterState (); 

	
						//Return the State that we just made. 
						return newState;
				}
	
				/// <summary>
				/// Updates all the active States in the StateMachine
				/// </summary>
				public void UpdateStates ()
				{
						for (int i = 0; i < States.Count; i++)
								States [i].ExecuteState ();
				}
	
				/// <summary>
				/// Removes the State from the Machine. 
				/// </summary>
				/// <returns><c>true</c>, if state was removed, <c>false</c> otherwise.</returns>
				/// <param name="aState">The State to remove.</param>
				/// <param name="callExit">If set to <c>true</c> call the OnExit functions of the removed sates.</param>
				public bool RemoveState (ww_State<T_Context> aState, bool callExit = true)
				{
						//If the state is null just return
						if (aState == null)
								return false; 
	
						//Should we call the exit function of the state
						if (callExit)
								aState.ExitState ();
	
						//Loop over and check for that State.
						for (int i = 0; i < States.Count; i++) {
								//We compaire the ID
								if (States [i].StateID == aState.StateID) {
										//We found it so remove it
										States.RemoveAt (i);
										return true; 
								}
						}
	
						//No State found. Return false. 
						return false; 
				}
	
				/// <summary>
				/// Removes all the States of a type.
				/// </summary>
				/// <param name="callExit">If set to <c>true</c> call the OnExit functions of the removed sates.</param>
				/// <typeparam name="T_StateType">The type of States to remove.</typeparam>
				public void RemoveStatesOfType<T_StateType> (bool callExit = false) where T_StateType : ww_State<T_Context>
				{
						//Loop over all states
						for (int i = States.Count - 1; i >= 0; i--) {
								//Is the state of type Q
								if (States [i] is T_StateType) {
										//If we want to call the exit we do it here. 
										if (callExit)
												States [i].ExitState ();
	
										//Then remove it. 
										States.RemoveAt (i);
								}
						}
				}
	
				/// <summary>
				/// Removes all States in the State Machine.
				/// </summary>
				/// <param name="callExit">If set to <c>true</c> call the OnExit functions of the removed sates.</param>
				public void RemoveAllSates (bool CallExitFunction)
				{
						//Loopover all states and remove them
						States.Clear (); 
				}
	
				/// <summary>
				/// Starts the State Machine.
				/// </summary>
				public void StartMachine ()
				{
						// Add the starting State. 
						AddState<T_StartState> (); 
				}
	
				/// <summary>
				///  This will remove all current States from the Machine and add the startingState.
				/// </summary>
				/// <param name="startAfterReset">If set to <c>true</c> the Machine will start after reset.</param>
				public void ResetMachine (bool startAfterReset = true)
				{
						//Remove All States
						RemoveAllSates (false);
	
						//Add the starting state. 
						if (startAfterReset)
								StartMachine (); 
				}
		}
}