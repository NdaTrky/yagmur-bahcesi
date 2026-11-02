using UnityEngine;
namespace RainGarden
{
    public enum EntityKind { Friend, Plot, Cloud, Boat, Fruit, Decoration, Pond, Basket }
    public sealed class GardenEntity : MonoBehaviour
    {
        public EntityKind kind;
        public int index;
        public bool draggable;
        public SpriteRenderer visual;
        public Vector3 rest;
        public Vector3 baseScale=Vector3.one;
        [System.NonSerialized] public bool dragging;
        float response, phase;
        GardenGame game;
        public void Bind(GardenGame owner) {game=owner;rest=transform.position;baseScale=transform.localScale;phase=index*.9f;}
        public void React(float duration=1.2f)
        {
            response=duration;
            if(kind==EntityKind.Friend) visual.sprite=game.art.Friend(index,1);
        }
        public void SetRest(Vector3 value) {rest=value;transform.position=value;}
        void Update()
        {
            if(game==null || dragging) return;
            float time=Time.time+phase, motion=game.State.reducedMotion?0:1;
            response=Mathf.Max(0,response-Time.deltaTime);
            if(kind==EntityKind.Friend)
            {
                float hop=response>0?Mathf.Abs(Mathf.Sin(response*8))*.16f*motion:0;
                transform.position=rest+Vector3.up*(hop+Mathf.Sin(time*1.6f)*.018f*motion);
                transform.localScale=baseScale*(1+Mathf.Sin(time*1.6f)*.012f*motion);
                visual.sprite=game.art.Friend(index,response>0?1:(time%6f<.16f?2:0));
            }
            else if(kind==EntityKind.Cloud) transform.position=rest+Vector3.up*Mathf.Sin(time*.7f)*.06f*motion;
            else if(kind==EntityKind.Boat && game.State.boatLaunched)
            {
                float pace=game.WindUntil>Time.time?1.6f:.45f; phase+=Time.deltaTime*pace;
                transform.position=rest+new Vector3(Mathf.Sin(phase)*.8f,Mathf.Cos(phase*.7f)*.13f)*motion;
                transform.rotation=Quaternion.Euler(0,0,Mathf.Sin(phase*2)*4*motion);
            }
            else if(response>0) transform.localScale=baseScale*(1+Mathf.Sin(response*9)*.06f*motion);
            else transform.localScale=baseScale;
        }
    }
    public sealed class GardenParticle : MonoBehaviour
    {
        public Vector3 velocity;
        public float life=1;
        float remaining;
        SpriteRenderer sprite;
        Color color;
        Vector3 initialScale;
        void Start() {remaining=life;sprite=GetComponent<SpriteRenderer>();color=sprite.color;initialScale=transform.localScale;}
        void Update()
        {
            remaining-=Time.deltaTime;
            if(remaining<=0) {Destroy(gameObject);return;}
            transform.position+=velocity*Time.deltaTime;
            sprite.color=new Color(color.r,color.g,color.b,Mathf.Min(1,remaining*3)*color.a);
            if(velocity.sqrMagnitude<.01f) transform.localScale=initialScale*(1+(life-remaining)*1.4f);
        }
    }
}
