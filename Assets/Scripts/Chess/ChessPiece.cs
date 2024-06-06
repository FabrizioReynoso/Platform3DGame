using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessPiece : MonoBehaviour
{
    [SerializeField] Material colorMaterial;
    [SerializeField] Material selectedColorMaterial;
    [SerializeField] Chess chess; 

    Material _colorMaterial;
    Material _selectedColorMaterial;
    Material[] _partsMaterials;
    Transform _transform;
    float _timer = 2f;
    bool _activatedColor;

    public bool ActivatedColor{ set{ _activatedColor = value; } }
    public float timer{ set{ _timer = value; } }

    void Start(){

        _transform = transform;
        _colorMaterial = colorMaterial;
        _selectedColorMaterial = selectedColorMaterial;
        _partsMaterials = GetComponent<Renderer>().materials;
    }

    void Update(){

        if (_activatedColor == true && CompareTag("ChessKing")){

            if (_timer > 0){

                _timer -= Time.deltaTime;
            }

            else{

                DeactivateMaterialColor();

                var chessPieceList = FindObjectsOfType<ChessPiece>();

                foreach (ChessPiece chessPiece in chessPieceList){

                    chessPiece.ActivatedColor = false;
                    chessPiece.timer = 3f;
                }   
            }
        }
    }

    void OnCollisionEnter(Collision other){

        if (other.gameObject.GetComponent<CannonBall>() && _activatedColor == false){

            ActivateMaterialColor();

            if (CompareTag("ChessKing")){

                chess.PuzzleRestart();

                var chessPieceList = FindObjectsOfType<ChessPiece>();

                foreach (ChessPiece chessPiece in chessPieceList){

                    chessPiece.ActivatedColor = true;
                }   
            }

            else{

                if (chess.SelectedPiecesList.Contains(this) == false){

                    chess.SelectedPiecesList.Add(this);

                    if (chess.whitePiecesList.Length == chess.SelectedPiecesList.Count){

                        chess.DisableTrap();
                    }
                }
            }
        }
    }

    public void DeactivateMaterialColor(){

        foreach (Material material in _partsMaterials){

            material.SetColor("_Color", _colorMaterial.color);

            if (_transform.childCount > 0){

                _transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", _colorMaterial.color);
            }
        }   
    }

    void ActivateMaterialColor(){

        foreach (Material material in _partsMaterials){

            material.SetColor("_Color", _selectedColorMaterial.color);

            if (_transform.childCount > 0){

                _transform.GetChild(0).GetComponent<Renderer>().material.SetColor("_Color", _selectedColorMaterial.color);
            }
        }    
    }
}
