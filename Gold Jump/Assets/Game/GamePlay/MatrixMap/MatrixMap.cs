using System;
using System.Collections.Generic;
using System.Linq;
using Game.UI.MainMenu;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.GamePlay.MatrixMap
{
    public class MatrixMap : MonoBehaviour
    {
        
#if UNITY_EDITOR
        [ContextMenu("InstallAll")]
        public void InstallAllEditor()
        {
            InstallAll();
        }
        [ContextMenu("InstantiateAll")]
        public void InstantiateAllEditor()
        {
            InstantiateAll();
        }
        [ContextMenu("ClearAll")]
        public void ClearAllEditor()
        {
            ClearAllEditMode();
        }
        [ContextMenu("Generate")]
        public void GenerateEditor()
        {
            Generate();
        }
#endif
        [SerializeField] private MenuControl control;
        public AnimationCurve selectablePercent;
        public List<ElementOfList> toSelect;
        public GameObject listWithSelectable;
        public MapTemplate mapTemplate;
        public List<ElementMatrix> dailyMatrix;
        private bool _connected = false;
        private void ConnectToElementsOfMatrix()
        {
            foreach (var element in dailyMatrix) 
                element.OnElementSelectedConfirm += ElementOnOnElementSelectedConfirm;
            _connected = true;

        }
        private void ElementOnOnElementSelectedConfirm(ElementOfList obj)
        {
            obj.relativeTo.Create();
            toSelect.Remove(obj);
            Destroy(obj.gameObject);
            foreach (var matrix in dailyMatrix)
                if (matrix.Status == false) return;
            control.OpenGetCoin();
        }
        private void OnEnable()
        {
            Debug.Log("Eneble");
            if (!_connected) {ConnectToElementsOfMatrix();}
            Initialization();

        }
        private void Initialization()
        {
            InstallAll();
            Generate();
        }
        private void ClearAllEditMode()
        {
            foreach (var matrixElement in dailyMatrix) matrixElement.ClearEditor();
            foreach (var selectElement in toSelect) DestroyImmediate(selectElement);
            toSelect.Clear();
        }
        private void InstallAll() { foreach (var matrixElement in dailyMatrix) matrixElement.prefab = mapTemplate.template[matrixElement.index]; }
        private void InstantiateAll() { foreach (var matrixElement in dailyMatrix) matrixElement.Create(); }
        private void Generate()
        {
            var valueOfCount = Random.Range(0,1f);
            int count = (int)(selectablePercent.Evaluate(valueOfCount)) + 1;
            var uniqueElements = GetUniqueRandomElements(1, 9, count);
            for (int indexation = 1; indexation < 10; indexation++) if (!uniqueElements.Contains(indexation)) dailyMatrix[indexation-1].Create();
            foreach (var element in uniqueElements) PoolTnSelectableList(element);
        }
        private void PoolTnSelectableList(int element)
        {
            var el = dailyMatrix[element - 1];
            ClearByElement(el);
            PoolToSelectableByElement(el);
        }
        private void ClearAll()
        {
            foreach (var matrixElement in dailyMatrix) matrixElement.Clear();
            foreach (var selectElement in toSelect) Destroy(selectElement);
            toSelect.Clear();
        }
        private void PoolToSelectableByElement(ElementMatrix element)
        {
            var obj = Instantiate(element.prefab, listWithSelectable.transform);
            var component = obj.AddComponent<ElementOfList>();
            component.relativeTo = element;
            toSelect.Add(component);
        }
        private void ClearByElement(ElementMatrix element)=>element.Clear();
        private List<int> GetUniqueRandomElements(int min, int max, int count)
        {
            List<int> elements = new List<int>();
            for (int i = min; i <= max; i++)
                elements.Add(i);
            Shuffle(elements);
            return elements.GetRange(0, Mathf.Min(count, elements.Count));
        }
        private void Shuffle(List<int> list)
        {
            System.Random rng = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (list[k], list[n]) = (list[n], list[k]);
            }
        }
    }
}