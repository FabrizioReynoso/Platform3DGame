using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonStatic : Cannon
{
    [SerializeField] bool _fire;

    void Start(){

        fire = _fire;
    }
}
