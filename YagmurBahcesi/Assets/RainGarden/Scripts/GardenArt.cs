using UnityEngine;
namespace RainGarden
{
    [CreateAssetMenu(menuName="Rain Garden/Art")]
    public sealed class GardenArt : ScriptableObject
    {
        public int revision;
        public Sprite background;
        public Sprite[] friends;
        public Sprite[] props;
        public Font font;
        public Sprite roundPanel, circle, triangle, drop;
        public AudioClip[] notes;
        public AudioClip rain, ambience;
        public Sprite Friend(int animal,int pose=0) => friends[Mathf.Clamp(animal,0,3)*4+Mathf.Clamp(pose,0,3)];
        public Sprite Prop(int index) => props[Mathf.Clamp(index,0,props.Length-1)];
    }
}
