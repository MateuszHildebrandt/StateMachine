using StateMachine;
using System.Collections;
using UnityEngine;

public class SM_CodeSample : MonoBehaviour
{
    [SerializeField] StateMachineCode stateMachine;

    private SubstateCode substate1;
    private SubstateCode substate2, substate2a, substate2b;

    private void Start()
    {
        stateMachine = new StateMachineCode();

        substate1 = new SubstateCode(stateMachine);
        substate1.onEnter += () => print("onEnter -> substate1");
        substate1.onExit += () => print("onExit -> substate1");

        substate2 = new SubstateCode(stateMachine);
        substate2.onEnter += () => print("onEnter -> substate2");
        substate2.onExit += () => print("onExit -> substate2");

        substate2a = new SubstateCode(stateMachine, substate2);
        substate2a.onEnter += () => print("onEnter -> substate2a");
        substate2a.onExit += () => print("onExit -> substate2a");

        substate2b = new SubstateCode(stateMachine, substate2);
        substate2b.onEnter += () => print("onEnter -> substate2b");
        substate2b.onExit += () => print("onExit -> substate2b");

        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        WaitForSeconds WaitForSeconds = new WaitForSeconds(3);

        substate1.Enter();
        yield return WaitForSeconds;

        substate2a.Enter();
        yield return WaitForSeconds;

        substate2b.Enter();
        yield return WaitForSeconds;
    }
}
