using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    private IWeapon weapon;

    // Start is called before the first frame update
    void Start()
    {
        weapon = gameObject.GetComponentInChildren<IWeapon>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            PerformWeaponAttack();
        }
    }

    public void PerformWeaponAttack()
    {
        weapon.PerformAttack();
    }

}