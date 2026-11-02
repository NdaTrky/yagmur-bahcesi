using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;

namespace RainGarden
{
    public enum GardenTool { Explore, Rain, Sun, Wind }
    public sealed class GardenGame : MonoBehaviour
    {
        public GardenArt art;
        public GardenEntity[] entities;
        public GardenState State {get;private set;}
        public GardenUI UI {get;private set;}
        public GardenTool Tool {get;private set;}
        public bool Playing {get;private set;}
        public float WindUntil {get;private set;}
        public string SavePath => save.Path;
        public static readonly Vector2 PondCenter = new Vector2(4.7f,.25f);
        public static readonly Vector2 PicnicCenter = new Vector2(-.3f,-2.6f);
        public static readonly Vector2 BoatStart = new Vector2(4.1f,-1.4f);
        GardenSave save;
        AudioSource voice, ambient;
        readonly List<GameObject> picnicObjects=new List<GameObject>();
        GardenEntity held;
        Vector3 holdOffset, holdStart;
        Vector2 pointerStart;
        int finger=-999;
        bool moved;
        float idleTime, lastSound;
        int fruitSequence;
        public bool SmokeMode {get;private set;}

        void Awake()
        {
            Application.targetFrameRate=60;
            Input.simulateMouseWithTouches=false;
            SmokeMode=Debug.isDebugBuild && Array.Exists(Environment.GetCommandLineArgs(),v=>v=="--garden-smoke");
            string root=SmokeMode?System.IO.Path.Combine(Application.temporaryCachePath,"rain-garden-smoke"):Application.persistentDataPath;
            save=new GardenSave(System.IO.Path.Combine(root,"garden-v1.json"));
            State=SmokeMode?new GardenState():save.Load();
            foreach(var e in entities) { e.Bind(this);e.visual.sortingOrder=BaseOrder(e); }
            voice=gameObject.AddComponent<AudioSource>();voice.playOnAwake=false;voice.spatialBlend=0;
            ambient=gameObject.AddComponent<AudioSource>();ambient.playOnAwake=false;ambient.spatialBlend=0;ambient.loop=true;ambient.clip=art.ambience;ambient.volume=.09f;
            UI=gameObject.AddComponent<GardenUI>();UI.Initialize(this);
            ApplyState();SetSound(State.sound);UI.ShowHome();
            if(SmokeMode) gameObject.AddComponent<GardenSmoke>();
        }
        public string L(string tr,string en) => State.language=="tr"?tr:en;
        public void StartGarden()
        {
            Playing=true;State.visited=true;UI.ShowGame();Save();idleTime=Time.time;
            UI.Hint(L("Buluta dokun. Bahçede neler olacak?","Tap the cloud. What will grow?"));
            Sound(1);
        }
        public void Home() {CancelPointer();Playing=false;Save();UI.ShowHome();}
        public void SelectTool(GardenTool tool)
        {
            Tool=tool;UI.RefreshTools();Sound((int)tool);
            string[] tr={"Dokun, taşı, keşfet.","Toprağa dokun; yağmur gelsin.","Filize dokun; güneşle açılsın.","Yaprağa dokun; rüzgârla yüzsün."};
            string[] en={"Touch, move, discover.","Tap a garden bed to bring rain.","Tap a sprout to help it bloom.","Tap the leaf to catch the breeze."};
            UI.Hint(L(tr[(int)tool],en[(int)tool]));
        }
        public GardenEntity Find(EntityKind kind,int index=0) => Array.Find(entities,e=>e.kind==kind && e.index==index);
        public void ApplyState()
        {
            for(int i=0;i<3;i++)
            {
                var e=Find(EntityKind.Plot,i);int growth=State.plants[i];
                e.visual.sprite=art.Prop(growth==2?4+State.colors[i]:3);
                e.visual.color=new Color(1,1,1,growth==0?.6f:1);
                e.baseScale=Vector3.one*(growth==0?.25f:growth==1?.46f:.66f);
                e.transform.localScale=e.baseScale;
                var decoration=Find(EntityKind.Decoration,i);decoration.SetRest(State.decorations[i]);
            }
            var boat=Find(EntityKind.Boat);boat.SetRest(State.boatLaunched?(Vector3)PondCenter:(Vector3)BoatStart);
            RebuildPicnic();
        }
        public void Interact(GardenEntity entity)
        {
            if(entity==null || !Playing || UI.BlocksWorld) return;
            idleTime=Time.time;
            switch(entity.kind)
            {
                case EntityKind.Plot:
                    if(Tool==GardenTool.Rain) Water(entity.index);
                    else if(Tool==GardenTool.Sun) Sunshine(entity.index);
                    else if(State.plants[entity.index]==2) {State.colors[entity.index]=1-State.colors[entity.index];ApplyState();Burst(entity.rest+Vector3.up*.6f);Save();}
                    else { Water(entity.index); }
                    entity.React();break;
                case EntityKind.Cloud: Water(NextPlot());break;
                case EntityKind.Friend:
                    entity.React();Sound(entity.index);
                    if(entity.index==2 && State.boatLaunched) Ripple(PondCenter);
                    else Burst(entity.rest+Vector3.up*1.2f,6);
                    break;
                case EntityKind.Boat: LaunchBoat();if(Tool==GardenTool.Wind) Wind();break;
                case EntityKind.Pond:
                    if(Tool==GardenTool.Wind) Wind();else {Ripple(PondCenter);Find(EntityKind.Friend,2).React();Sound(2);}break;
                case EntityKind.Fruit: ShareFruit(entity.index+1);break;
                case EntityKind.Basket: ShareFruit((fruitSequence++%2)+1);break;
                case EntityKind.Decoration:
                    entity.React();Burst(entity.rest+Vector3.up*.5f,5);Sound(3);break;
            }
        }
        int NextPlot() {for(int i=0;i<3;i++)if(State.plants[i]<2)return i;return UnityEngine.Random.Range(0,3);}
        public void Water(int plot)
        {
            if(plot<0 || plot>2)return;
            GardenRules.Rain(State,plot);ApplyState();StartCoroutine(RainAt(Find(EntityKind.Plot,plot).rest));
            Find(EntityKind.Friend,0).React();Save();Sound(0);
            UI.Hint(State.plants[plot]==2?L("Merhaba, çiçek! Rengini değiştirmek için dokun.","Hello, flower! Tap to change its color."):L("Bir filiz! Güneşi ya da yağmuru dene.","A sprout! Try sunshine or more rain."));
        }
        public void Sunshine(int plot)
        {
            bool grew=GardenRules.Sun(State,plot);ApplyState();Burst(Find(EntityKind.Plot,plot).rest+Vector3.up*.5f,12);Save();Sound(2);
            UI.Hint(grew?L("Güneşle açıldı!","A sunny new bloom!"):L("Önce biraz yağmur iyi gelir.","A little rain will help first."));
        }
        public void LaunchBoat()
        {
            State.boatLaunched=true;Find(EntityKind.Boat).SetRest(PondCenter);Find(EntityKind.Friend,2).React();Ripple(PondCenter);Save();Sound(3);
            UI.Hint(L("Yaprak yüzüyor! Rüzgârı dene.","The leaf is sailing! Try the breeze."));
        }
        public void Wind()
        {
            if(!State.boatLaunched)LaunchBoat();WindUntil=Time.time+5;
            Find(EntityKind.Friend,2).React();StartCoroutine(WindTrail());Sound(4);
            UI.Hint(L("Hafif bir esinti...","A gentle little breeze..."));
        }
        public void ShareFruit(int fruit)
        {
            GardenRules.Picnic(State,fruit);RebuildPicnic();Save();Sound(fruit+1);
            Find(EntityKind.Friend,1).React();Find(EntityKind.Friend,3).React();
            Burst(PicnicCenter+Vector2.up*.3f,6);UI.Hint(L("Birlikte piknik yapalım.","A little picnic for everyone."));
        }
        void RebuildPicnic()
        {
            foreach(var obj in picnicObjects) if(obj!=null)Destroy(obj);picnicObjects.Clear();
            for(int i=0;i<4;i++)if(State.picnic[i]>0)
            {
                var go=new GameObject("Picnic fruit "+i);go.transform.SetParent(transform);
                go.transform.position=new Vector3(-1.35f+(i%2)*1.15f,-2.5f-(i/2)*.55f,0);
                go.transform.localScale=Vector3.one*.28f;
                var sprite=go.AddComponent<SpriteRenderer>();sprite.sprite=art.Prop(6+State.picnic[i]);sprite.sortingOrder=25;picnicObjects.Add(go);
            }
        }
        public void MoveDecoration(int index,Vector2 point)
        {
            State.decorations[index]=new Vector2(Mathf.Clamp(point.x,-7.8f,7.8f),Mathf.Clamp(point.y,-3.7f,1.4f));
            Find(EntityKind.Decoration,index).SetRest(State.decorations[index]);Save();
        }
        public void ToggleLanguage() {State.language=State.language=="tr"?"en":"tr";Save();UI.Rebuild();}
        public void ToggleSound() {SetSound(!State.sound);Save();UI.Rebuild();}
        public void ToggleMotion() {State.reducedMotion=!State.reducedMotion;Save();UI.Rebuild();}
        public void SetSound(bool value) {State.sound=value;voice.mute=!value;if(value && !ambient.isPlaying)ambient.Play();if(!value)ambient.Stop();}
        public bool Save() {bool ok=save.Write(State);if(!ok && UI!=null) UI.Hint(L("Bahçe kaydedilemedi. Cihazda boş alanı kontrol edin.","Could not save. Please check device storage."));return ok;}
        public void ResetGarden()
        {
            var next=new GardenState{language=State.language,sound=State.sound,reducedMotion=State.reducedMotion,visited=true};
            State=next;ApplyState();Save();UI.CloseModal();
        }
        public void Sound(int note)
        {
            if(!State.sound || Time.unscaledTime-lastSound<.08f || art.notes.Length==0)return;
            lastSound=Time.unscaledTime;voice.pitch=1;voice.PlayOneShot(art.notes[Mathf.Abs(note)%art.notes.Length],.3f);
        }
        IEnumerator RainAt(Vector3 point)
        {
            if(State.sound) voice.PlayOneShot(art.rain,.17f);
            for(int i=0;i<24;i++)
            {
                Particle(art.drop,point+new Vector3(UnityEngine.Random.Range(-.7f,.7f),2.1f),new Vector3(.1f,-3),new Color(.53f,.75f,.8f,.9f),.11f,.65f);
                if(i%3==0)yield return new WaitForSeconds(.06f);
            }
            Burst(point+Vector3.up*.5f,6);
        }
        IEnumerator WindTrail()
        {
            for(int i=0;i<12;i++) {Particle(art.Prop(6),new Vector3(1.8f,UnityEngine.Random.Range(.4f,1.3f)),Vector3.right*1.8f,Color.white,.055f,1.6f);yield return new WaitForSeconds(.12f);}
        }
        public void Burst(Vector3 point,int count=9)
        {
            if(State.reducedMotion)count=3;
            for(int i=0;i<count;i++)Particle(art.circle,point,new Vector3(UnityEngine.Random.Range(-.6f,.6f),UnityEngine.Random.Range(.25f,.9f)),new Color(1,.87f,.55f,.8f),UnityEngine.Random.Range(.035f,.07f),.8f);
        }
        public void Ripple(Vector3 point)
        {
            Particle(art.circle,point,Vector3.zero,new Color(.85f,.98f,1,.3f),.35f,.8f);
        }
        void Particle(Sprite sprite,Vector3 position,Vector3 velocity,Color color,float scale,float life)
        {
            var go=new GameObject("Gentle effect");go.transform.SetParent(transform);go.transform.position=position;go.transform.localScale=Vector3.one*scale;
            var sr=go.AddComponent<SpriteRenderer>();sr.sprite=sprite;sr.color=color;sr.sortingOrder=70;
            var effect=go.AddComponent<GardenParticle>();effect.velocity=velocity;effect.life=life;
        }
        public GardenEntity Hit(Vector2 world)
        {
            GardenEntity best=null;int order=int.MinValue;
            foreach(var collider in Physics2D.OverlapPointAll(world))
            {
                var e=collider.GetComponent<GardenEntity>();if(e==null)continue;
                int rank=e.kind==EntityKind.Pond?-50:e.visual.sortingOrder;
                if(rank>order){best=e;order=rank;}
            }
            return best;
        }
        public void PointerDown(Vector2 screen,int id)
        {
            if(!Playing || UI.BlocksWorld || finger!=-999)return;
            if(EventSystem.current!=null && (id==-1?EventSystem.current.IsPointerOverGameObject():EventSystem.current.IsPointerOverGameObject(id)))return;
            finger=id;pointerStart=screen;moved=false;
            Vector3 world=Camera.main.ScreenToWorldPoint(new Vector3(screen.x,screen.y,10));world.z=0;
            held=Hit(world);
            if(held!=null){holdStart=held.rest;holdOffset=held.transform.position-world;}
        }
        public void PointerMove(Vector2 screen,int id)
        {
            if(finger!=id || held==null || !held.draggable)return;
            if((screen-pointerStart).magnitude>Mathf.Max(12,Screen.height*.015f))moved=true;
            if(!moved)return;
            held.dragging=true;
            Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(screen.x,screen.y,10));p.z=0;
            held.transform.position=p+holdOffset;held.visual.sortingOrder=90;
        }
        public void PointerUp(Vector2 screen,int id,bool cancel=false)
        {
            if(finger!=id)return;
            if(held!=null)
            {
                var e=held;e.dragging=false;e.visual.sortingOrder=BaseOrder(e);
                Vector3 p=Camera.main.ScreenToWorldPoint(new Vector3(screen.x,screen.y,10));p.z=0;
                e.SetRest(holdStart);
                if(!cancel)
                {
                    if(moved)Drop(e,p);else Interact(e);
                }
            }
            held=null;finger=-999;moved=false;
        }
        public int BaseOrder(GardenEntity e) => e.kind==EntityKind.Cloud?50:e.kind==EntityKind.Boat||e.kind==EntityKind.Fruit?40:e.kind==EntityKind.Friend?30:e.kind==EntityKind.Pond?-50:e.kind==EntityKind.Basket?-40:20;
        public void Drop(GardenEntity e,Vector2 p)
        {
            if(UI.BlocksWorld)return;
            if(e.kind==EntityKind.Decoration)MoveDecoration(e.index,p);
            else if(e.kind==EntityKind.Friend)
            {e.SetRest(new Vector3(Mathf.Clamp(p.x,-7.8f,7.8f),Mathf.Clamp(p.y,-3.1f,1.8f)));e.React();Sound(e.index);}
            else if(e.kind==EntityKind.Cloud)
            {
                int target=0;float distance=float.MaxValue;
                for(int i=0;i<3;i++){float d=Vector2.Distance(p,Find(EntityKind.Plot,i).rest);if(d<distance){distance=d;target=i;}}
                if(distance<4)Water(target);else {StartCoroutine(RainAt(p));Ripple(p);}
            }
            else if(e.kind==EntityKind.Boat && Vector2.Distance(p,PondCenter)<3)LaunchBoat();
            else if(e.kind==EntityKind.Fruit)
            {
                bool valid=Vector2.Distance(p,PicnicCenter)<2.6f;
                for(int i=0;i<4;i++)valid|=Vector2.Distance(p,Find(EntityKind.Friend,i).rest)<1.5f;
                if(valid)ShareFruit(e.index+1);else UI.Hint(L("Meyveyi örtüye ya da bir dostuna götür.","Bring the fruit to the blanket or a friend."));
            }
            idleTime=Time.time;
        }
        void CancelPointer() {if(held!=null){held.dragging=false;held.SetRest(holdStart);held.visual.sortingOrder=BaseOrder(held);}held=null;finger=-999;moved=false;}
        void Update()
        {
            if(Input.GetKeyDown(KeyCode.Escape)){if(UI.BlocksWorld)UI.CloseModal();else if(Playing)Home();}
            if(!Playing || UI.BlocksWorld)return;
            if(Input.touchCount>0)
            {
                for(int i=0;i<Input.touchCount;i++)
                {
                    var touch=Input.GetTouch(i);
                    if(touch.phase==TouchPhase.Began)PointerDown(touch.position,touch.fingerId);
                    else if(touch.phase==TouchPhase.Ended || touch.phase==TouchPhase.Canceled)PointerUp(touch.position,touch.fingerId,touch.phase==TouchPhase.Canceled);
                    else PointerMove(touch.position,touch.fingerId);
                }
            }
            else
            {
                if(Input.GetMouseButtonDown(0))PointerDown(Input.mousePosition,-1);
                if(Input.GetMouseButton(0))PointerMove(Input.mousePosition,-1);
                if(Input.GetMouseButtonUp(0))PointerUp(Input.mousePosition,-1);
            }
            if(Time.time-idleTime>14) {idleTime=Time.time;Find(EntityKind.Cloud).React();UI.ShowHintPulse();}
        }
        void OnApplicationPause(bool paused) {if(paused){CancelPointer();Save();}}
        void OnApplicationFocus(bool focused) {if(!focused){CancelPointer();if(State!=null)Save();}}
        void OnApplicationQuit() {if(State!=null)Save();}
    }
}
