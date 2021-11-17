using UnityEngine;

namespace Gisha.GameOff_2021.Utilities
{
    public class ParallaxEffect : MonoBehaviour
    {
        [SerializeField] private float parallaxSpeed = 2f;
        
        private Material _mat;
        private float _xOffset = 0;

        private void Awake()
        {
            _mat = GetComponent<MeshRenderer>().material;
        }

        private void Update()
        {
            _xOffset += parallaxSpeed * Time.deltaTime;
            _mat.mainTextureOffset = transform.right * _xOffset;
        }
    }
}