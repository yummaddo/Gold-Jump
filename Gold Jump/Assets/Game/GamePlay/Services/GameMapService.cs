using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Core;
using UnityEngine;

namespace Game.GamePlay.Services
{
    public class GameMapService : AbstractServices
    {
        public RectTransform root;
        public int lastHeightPanel = 0;
        public Transform rootOfCamera;
        public LevelScene levelScene;
        private int _currentIterator = 0;
        private int _currentPlayerIteratorOn = 0;
        
        private float _delta = 0;
        public List<GameObject> panels = new List<GameObject>(); 
#if UNITY_EDITOR
        [ContextMenu("Generate")]
        public void LevelGenerationEditor()
        {
            LevelGeneration();
        }
#endif
        private async UniTask LevelPreGeneration()
        {
            var level = levelScene.data;
            float screenWidth = Camera.main!.orthographicSize * Screen.width / Screen.height;
            var startPosition = Vector3.zero;
            startPosition.y = _currentIterator * level.panelsIteratorSize;
            lastHeightPanel = 0;
            var deltaOfPreGenerator = level.preGenerateSize; 
            while (!destroyCancellationToken.IsCancellationRequested)
            {
                if (lastHeightPanel < rootOfCamera.position.y + deltaOfPreGenerator)
                {
                    _currentIterator += IterationCreator(level,screenWidth);
                    startPosition.y = _currentIterator;
                }
                await UniTask.Yield();
            }
        }
        private void  LevelGeneration()
        {
            var level = levelScene.data;
            float screenWidth = Camera.main!.orthographicSize * 2.0f * Screen.width / Screen.height;
            Vector3 startPosition = Vector3.zero;
            startPosition.y = _currentIterator * level.panelsIteratorSize;
            while (startPosition.y < level.height)
            {
                _currentIterator += IterationCreator(level,screenWidth);
                startPosition.y = _currentIterator;
            }
        }
        private int IterationCreator( Level level, float screenWidth )
        {
            var valueOfPlatform = Random.Range(0,1f);
            var valueOfPlatformWidth = Random.Range(0,1f);
            var valueOfPlatformIs = Random.Range(0,1f);
            var valueOfIterator = Random.Range(0,1f);
            
            var chanceOfIterator = level.spawnPlatformNextIterationChance.Evaluate(valueOfIterator);
            var chanceOfPlatformIs = level.spawnPlatformChance.Evaluate(valueOfPlatformIs);
            var chanceOfPlatformWidth = level.chanceOfWidth.Evaluate(valueOfPlatformWidth);
            var chanceOfPlatformType = level.dispersionOfBasePlatformType.Evaluate(valueOfPlatform);

            if (chanceOfPlatformIs < 0)
            {
                Debug.Log("next");
                return 1;
            }
            var iterationResultValue = (int)(chanceOfIterator * level.iterationBaseSize);
            Debug.Log(iterationResultValue);
            var platformWidth = (screenWidth*-0.5f) + chanceOfPlatformWidth * screenWidth;
            var platformHeight = ( _delta+_currentIterator+iterationResultValue );
            var platformPosition = new Vector3(platformWidth, platformHeight,0);
            var platformType = level.platformListDictionary[chanceOfPlatformType];
            Create(platformType, platformPosition);
            return iterationResultValue;
        }

        private void Create(GameObject platformPrefab ,Vector3 position)
        {
            var obj = Instantiate(platformPrefab, root);
            var positionOfTheNewPanel = obj.transform.position;
            positionOfTheNewPanel += position;
            obj.transform.position = positionOfTheNewPanel;
            lastHeightPanel = (int)positionOfTheNewPanel.y;
        }

        public override void OnAwake()
        {
            levelScene = Application.Instance.LevelsSetting.currentSceneLevel;
            LevelGeneration();
        }
        public override void OnStart()
        {
        }
    }
}