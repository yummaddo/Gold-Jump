using System;
using UnityEngine;

namespace Game.GamePlay.MatrixMap
{
    public class ElementMatrix : MonoBehaviour
    {
        public GameObject prefab;
        public GameObject targetView;
        public int index = 1;
        [SerializeField] [HideInInspector] private GameObject sourcedPrefab;
        internal event Action<ElementOfList> OnElementSelectedConfirm;
        internal bool Status { get; private set; } = false;
        private void OnTriggerEnter2D(Collider2D col)
        {
            if (Status) return;
            if (col.gameObject.TryGetComponent<ElementOfList>(out var element))
            {
                if (element.relativeTo.index == index)
                    OnElementSelectedConfirm?.Invoke(element);
            }
        }
        public void Create()
        {
            sourcedPrefab = GameObject.Instantiate(prefab, targetView.transform);
            Status = true;
        }
        public void Clear()
        {
            if (sourcedPrefab)
            {
                Status = false;
                Destroy(sourcedPrefab);
            }
        }
        public void ClearEditor()
        {
            if (sourcedPrefab) DestroyImmediate(sourcedPrefab);
        }
        private void Reset()
        {
            targetView = gameObject;
        }
    }
}