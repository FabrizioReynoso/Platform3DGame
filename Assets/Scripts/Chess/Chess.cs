using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chess : MonoBehaviour
{
    public ChessPiece[] whitePiecesList;

    [SerializeField] AudioSource unlockRelic;
    [SerializeField] AudioSource buzzerChess;
    [SerializeField] BoxChess[] boxChessList;
    [SerializeField] Cannon[] _cannonList;
    [SerializeField] Relic relic;

    List<ChessPiece> selectedPiecesList = new List<ChessPiece>();

    public List<ChessPiece> SelectedPiecesList{ get{ return selectedPiecesList; } set{ selectedPiecesList = value; } }

    public void PuzzleRestart(){

        buzzerChess.Play();
        selectedPiecesList = new List<ChessPiece>();

        foreach (ChessPiece chessPiece in whitePiecesList){

            chessPiece.DeactivateMaterialColor();
        }    
    }

    public void DisableTrap(){

        unlockRelic.Play();
        relic.Active();

        foreach (BoxChess boxChess in boxChessList){

            boxChess.trap = false;
        }

        foreach (Cannon cannon in _cannonList){

            if (cannon.Fire == true){

                cannon.Fire = false;
            }
        }
    }
}
