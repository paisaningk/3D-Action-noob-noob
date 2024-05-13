using System.Collections.Generic;
using UnityEngine;

namespace CharacterComponent
{
    public class DropWeapon : MonoBehaviour
    {
        public List<Rigidbody> weaponObject;

        //Trigger in Player anim dead
        public void Drop()
        {
            foreach (var variable in weaponObject)
            {
                variable.gameObject.AddComponent<BoxCollider>();
                variable.transform.parent = null;
                variable.useGravity = true;
            }
        }
    }
}