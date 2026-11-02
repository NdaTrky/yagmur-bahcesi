using UnityEngine;
namespace RainGarden
{
    public sealed class GardenCamera : MonoBehaviour
    {
        void LateUpdate()
        {
            var view=GetComponent<Camera>();
            view.orthographicSize=Mathf.Max(5.4f,9.6f/Mathf.Max(.5f,view.aspect));
        }
    }
}
