using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

namespace LevelComponent
{
    public class Gate : MonoBehaviour
    {
        public GameObject gateVisual;
        public Collider gateCollider;
        public float openDuration = 2f;
        public float openTargetY = -1.5f;

        private void OnValidate()
        {
            gateCollider = GetComponent<Collider>();
            gateVisual = transform.GetChild(0).gameObject;
        }

        [Button]
        public void OpenGate()
        {
            gateVisual.transform.DOMoveY(openTargetY, openDuration).OnComplete(() => { gateCollider.enabled = false; });
        }
    }
}