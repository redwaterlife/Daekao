using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ePlayerHand { R, S, P }
public enum eGameProcess { End, Running }

public class Prototype : MonoBehaviour
{
    public const float TURN_LENGTH = 3f;

    private int targetPlayer = 0;

    public int LastWonPlayer { get; private set; }
    public ePlayerHand[] PlayerHands { get; private set; }
    public eGameProcess Process { get; private set; }

    private void Awake()
    {
        Process = eGameProcess.End;
        PlayerHands = new ePlayerHand[2];
        LastWonPlayer = 0;
        ResetTurn();
    }

    private void Start()
    {
        StartTurn();
    }

    private void Update()
    {
        if (Process != eGameProcess.Running || targetPlayer == 2) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            PlayerHands[targetPlayer] = ePlayerHand.R;
            targetPlayer++;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            PlayerHands[targetPlayer] = ePlayerHand.S;
            targetPlayer++;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            PlayerHands[targetPlayer] = ePlayerHand.P;
            targetPlayer++;
        }
    }

    private void ResetTurn()
    {
        for(int i = 0; i < PlayerHands.Length; i++)
        {
            PlayerHands[i] = ePlayerHand.R;
        }
    }

    protected IEnumerator TurnProcess()
    {
        yield return new WaitForSeconds(TURN_LENGTH);
        FinishTurn();
    }

    private void StartTurn()
    {
        Debug.Log("턴을 시작합니다!");
        EndTurn();
        Process = eGameProcess.Running; // 이 순서를 절대 바꾸면 안됩니다!

        targetPlayer = 0;

        StartCoroutine(TurnProcess());
    }

    private void FinishTurn()
    {
        Debug.Log($"0: {PlayerHands[0]}, 1: {PlayerHands[1]}");

        EndTurn();
        if (PlayerHands[0] == PlayerHands[1]) StartTurn();
        else if (PlayerHands[0] == ePlayerHand.R && PlayerHands[1] == ePlayerHand.S) EndGame(0);
        else if (PlayerHands[0] == ePlayerHand.S && PlayerHands[1] == ePlayerHand.P) EndGame(0);
        else if (PlayerHands[0] == ePlayerHand.P && PlayerHands[1] == ePlayerHand.R) EndGame(0);
        else EndGame(1);
    }

    private void EndTurn()
    {
        Process = eGameProcess.End;
        StopCoroutine(TurnProcess());
        ResetTurn();
    }

    private void EndGame(int wonPlayerIndex)
    {
        Debug.Log($"게임이 끝났습니다! 승자:{wonPlayerIndex}");
    }
}
