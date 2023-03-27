using System.Collections;
using UnityEngine;

namespace StateMachine.Sample
{
    public class SM_CodeSample : MonoBehaviour
    {
        [SerializeField] StateMachineCode stateMachine;

        private SubstateCode substate1;
        private SubstateCode substate2, substate2a, substate2b;

        private void Start()
        {
            stateMachine = new StateMachineCode();

            substate1 = stateMachine.Add("state1").On(Green).Off(Red);
            substate2 = stateMachine.Add("state2").On(Blue);
            substate2a = stateMachine.Add("state2a", substate2).On(Black).Off(Red);
            substate2b = stateMachine.Add("state2b", substate2).On(White);

            StartCoroutine(Run());
        }

        private void Green() => print("green");
        private void Red() => print("red");
        private void Blue() => print("blue");
        private void Black() => print("black");
        private void White() => print("white");


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
}
