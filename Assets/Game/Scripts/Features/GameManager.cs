using UnityEngine;
using TextAsset = UnityEngine.TextAsset;

namespace Game.Scripts.Features
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] 
        private CameraMover cameraMover;

        [SerializeField] 
        private Character player;
        
        [SerializeField] 
        private Character enemy;

        private Data _settings;
        
        public void Run()
        {
            var text = Resources.Load<TextAsset>("data");
            _settings = JsonUtility.FromJson<Data>(text.text);
            cameraMover.Run(_settings.cameraSettings);
        }
    }
}