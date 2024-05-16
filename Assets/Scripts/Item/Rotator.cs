using UnityEngine;

namespace Item
{
    public class Rotator : MonoBehaviour
    {
        public float speed = 80;

        public void Update()
        {
            transform.Rotate(new Vector3(0f, speed * Time.deltaTime), Space.World);
        }
    }
}