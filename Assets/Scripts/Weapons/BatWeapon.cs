using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BatWeapon : MonoBehaviour
{
    public int damage = 1;

    void OnTriggerEnter(Collider other){

        var bot = other.GetComponent<Bot>();

        if (bot){

            bot.ToDamage(damage);
        }
    }
}
