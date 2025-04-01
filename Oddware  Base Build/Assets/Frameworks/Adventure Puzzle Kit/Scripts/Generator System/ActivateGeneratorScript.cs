using UnityEngine;

namespace GeneratorSystem
{
    public class ActivateGeneratorScript : MonoBehaviour
    {
        [SerializeField] private GameObject generator3DAudio = null;
        [SerializeField] private Renderer[] thisMaterial = null;

        public void PowerLights()
        {
            generator3DAudio.SetActive(true);

            foreach (Renderer emissiveMaterial in thisMaterial)
            {
                emissiveMaterial.material.EnableKeyword("_EMISSION");
            }
        }
    }
}
