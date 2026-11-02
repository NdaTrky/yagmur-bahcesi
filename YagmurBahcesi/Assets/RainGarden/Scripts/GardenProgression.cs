using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
namespace RainGarden
{
    public sealed partial class GardenGame
    {
        readonly List<GameObject> landmarks=new List<GameObject>();
        readonly List<SpriteRenderer> visitors=new List<SpriteRenderer>();
        bool sumiMoving;
        public bool Transaction(Func<GardenState,bool> mutation)
        {
            var next=State.Clone();if(!mutation(next))return false;next.Validate();
            if(!save.Write(next)){UI.Hint(L("Değişiklik kaydedilemedi. Yeniden deneyin.","Could not save this change. Please try again."));return false;}
            State=next;ApplyState();return true;
        }
        void ExpandWorld()
        {
            var list=new List<GardenEntity>(entities);
            for(int i=3;i<6;i++)
            {
                Vector2 position=GardenCatalog.ExtraPlots[i-3];
                WorldSprite("Growing pot "+i,art.Expansion(12),position-Vector2.up*.2f,.29f,12);
                list.Add(NewEntity(EntityKind.Plot,i,art.Prop(3),position,.5f,false,new Vector2(2.6f,2.6f)));
            }
            for(int i=3;i<12;i++)list.Add(NewEntity(EntityKind.Decoration,i,GardenCatalog.DecorSprite(art,i),State.decorations[i],i==5||i==8?.4f:.3f,true,new Vector2(3.2f,3.2f)));
            list.Add(NewEntity(EntityKind.Friend,4,art.Friend(4),new Vector2(-4.3f,-1.8f),.24f,true,new Vector2(3.6f,2.3f)));
            entities=list.ToArray();
            for(int i=0;i<6;i++)
            {
                var sr=WorldSprite("Butterfly visitor "+i,art.Expansion(15),Vector2.zero,.09f,42);
                sr.gameObject.SetActive(false);visitors.Add(sr);
            }
        }
        GardenEntity NewEntity(EntityKind kind,int index,Sprite sprite,Vector2 position,float scale,bool draggable,Vector2 hit)
        {
            var go=new GameObject(kind+" "+index,typeof(SpriteRenderer),typeof(BoxCollider2D),typeof(GardenEntity));
            go.transform.SetParent(transform);go.transform.position=position;go.transform.localScale=Vector3.one*scale;
            var sr=go.GetComponent<SpriteRenderer>();sr.sprite=sprite;
            var collider=go.GetComponent<BoxCollider2D>();collider.size=hit;collider.offset=Vector2.up*hit.y*.4f;
            var e=go.GetComponent<GardenEntity>();e.kind=kind;e.index=index;e.visual=sr;e.draggable=draggable;return e;
        }
        SpriteRenderer WorldSprite(string name,Sprite sprite,Vector2 position,float scale,int order)
        {
            var go=new GameObject(name,typeof(SpriteRenderer));go.transform.SetParent(transform);go.transform.position=position;go.transform.localScale=Vector3.one*scale;
            var sr=go.GetComponent<SpriteRenderer>();sr.sprite=sprite;sr.sortingOrder=order;return sr;
        }
        void RefreshLandmarks()
        {
            if(landmarks.Count==0)
            {
                // Each project reveals one more permanent part, without replacing user decorations.
                int[] assets={12,6,7,1,9,11,14,8,6};
                Vector2[] positions={new Vector2(-2.2f,1.3f),new Vector2(-1.7f,1.55f),new Vector2(-2.5f,1.7f),
                    new Vector2(7.2f,1.4f),new Vector2(7.65f,1.4f),new Vector2(6.8f,1.45f),
                    new Vector2(.9f,-1.7f),new Vector2(-.2f,-1.3f),new Vector2(-1.6f,-1.4f)};
                for(int i=0;i<9;i++)landmarks.Add(WorldSprite("Project piece "+i,i==3?art.Prop(10):art.Expansion(assets[i]),positions[i],i==5?.38f:.27f,10).gameObject);
            }
            for(int p=0;p<3;p++)for(int i=0;i<3;i++)landmarks[p*3+i].SetActive(GardenProjects.Progress(State,p)>i);
            if(Find(EntityKind.Friend,4)!=null)Find(EntityKind.Friend,4).gameObject.SetActive(GardenProjects.Sumi(State));
        }
        void RememberFlower(int plot)
        {
            if(State.plants[plot]!=2)return;State.discoveries[0]=true;
            var seen=new List<string>(State.plantedSpecies);
            if(!seen.Contains(State.flowerIds[plot]))seen.Add(State.flowerIds[plot]);State.plantedSpecies=seen.ToArray();
            if(seen.Count>=3)State.discoveries[10]=true;
        }
        public bool Purchase(string id)
        {
            return Transaction(s=>{if(!GardenCatalog.Purchase(s,id))return false;if(GardenCatalog.FlowerIndex(id)>=2)s.discoveries[1]=true;return true;});
        }
        public bool Plant(string id,int slot)
        {
            bool ok=Transaction(s=>{
                if(slot<0||slot>=6||GardenCatalog.FlowerIndex(id)<0||!GardenCatalog.Owns(s,id))return false;
                s.flowerIds[slot]=id;s.plants[slot]=1;s.colors[slot]=0;
                if(GardenCatalog.FlowerIndex(id)>=2)s.discoveries[2]=true;return true;
            });
            if(ok){Burst(Find(EntityKind.Plot,slot).rest);UI.Hint(L("Tohum yerinde! Biraz yağmurla büyüsün.","Your seed is planted! Help it grow with rain."));}
            return ok;
        }
        public bool ToggleDecoration(int index)
        {
            return Transaction(s=>{
                if(index<0||index>=12||!GardenCatalog.Owns(s,GardenCatalog.Decorations[index].id))return false;
                s.placed[index]=!s.placed[index];if(index>=3&&s.placed[index])s.discoveries[8]=true;return true;
            });
        }
        public bool BeginActivity(int kind)
        {
            if(kind<0||kind>2)return false;
            return Transaction(s=>GardenActivities.Begin(s,kind));
        }
        public bool ActivityInput(int input,bool help=false)
        {
            bool ok=Transaction(s=>{
                var a=s.activity;if(a==null||!a.Valid()||a.claimed)return false;
                return help?GardenActivities.Help(a):a.kind==0?GardenActivities.PickFruit(a,input):a.kind==1?GardenActivities.Rotate(a,input):GardenActivities.Sail(a,input);
            });
            if(ok&&GardenActivities.Complete(State.activity))ClaimActivity(State.activity.id);
            return ok;
        }
        public bool ClaimActivity(string id)=>Transaction(s=>GardenActivities.Claim(s,id));
        void FlowerResponse(GardenEntity flower)
        {
            int behavior=GardenCatalog.Flowers[GardenCatalog.FlowerIndex(State.flowerIds[flower.index])].behavior;
            if(behavior==0){visitors[flower.index].gameObject.SetActive(true);Find(EntityKind.Friend,0).React();}
            else if(behavior==1)Sound(flower.index);
            else if(behavior==2){flower.React(2);Burst(flower.rest+Vector3.up*.6f);}
            else{Ripple(flower.rest);Find(EntityKind.Friend,2).React();}
        }
        void DecorationResponse(GardenEntity entity)
        {
            entity.React();Sound(entity.index);Burst(entity.rest+Vector3.up*.5f,5);
            if(entity.index==0||entity.index==3)Wind();
            if(entity.index==7)Water(NextPlot());
            if(entity.index==10)Ripple(entity.rest+Vector3.up*.5f);
            if(entity.index==11)ShareFruit(1);
            if(entity.index==4)Find(EntityKind.Friend,2).React(3);
            if(entity.index==6)StartCoroutine(LanternGlow(entity));
        }
        IEnumerator LanternGlow(GardenEntity entity)
        {
            for(int i=0;i<5;i++){Burst(entity.rest+Vector3.up*.7f,3);yield return new WaitForSeconds(.3f);}
        }
        IEnumerator SumiWalk()
        {
            if(sumiMoving)yield break;sumiMoving=true;
            var sumi=Find(EntityKind.Friend,4);Vector3 start=sumi.rest,target=start+Vector3.right*(start.x>5?-.8f:.8f);
            float duration=State.reducedMotion?.1f:3;
            for(float t=0;t<duration;t+=Time.deltaTime){if(sumi.dragging)break;sumi.SetRest(Vector3.Lerp(start,target,t/duration));yield return null;}
            sumiMoving=false;
        }
        void LateUpdate()
        {
            for(int i=0;i<visitors.Count;i++)
            {
                int flower=GardenCatalog.FlowerIndex(State.flowerIds[i]);bool show=State.plants[i]==2&&GardenCatalog.Flowers[flower].behavior==0;
                var sr=visitors[i];sr.gameObject.SetActive(show);if(!show)continue;
                float t=State.reducedMotion?0:Time.time;
                sr.transform.position=Find(EntityKind.Plot,i).rest+new Vector3(Mathf.Sin(t*.8f+i)*.45f,1.4f+Mathf.Cos(t+i)*.15f);
                sr.transform.localScale=new Vector3(.07f+Mathf.Abs(Mathf.Sin(t*4))*.04f,.09f,1);
            }
        }
        public string AlbumDirectory=>System.IO.Path.Combine(System.IO.Path.GetDirectoryName(SavePath),"garden-album");
        public void CaptureMemory(){UI.CloseModal();StartCoroutine(Snapshot());}
        IEnumerator Snapshot()
        {
            yield return new WaitForEndOfFrame();
            var texture=ScreenCapture.CaptureScreenshotAsTexture();if(texture==null)yield break;
            bool saved=false;
            try
            {
                Directory.CreateDirectory(AlbumDirectory);string path=System.IO.Path.Combine(AlbumDirectory,DateTime.Now.ToString("yyyyMMdd-HHmmss-fff")+".png");File.WriteAllBytes(path,texture.EncodeToPNG());
                var files=Directory.GetFiles(AlbumDirectory,"*.png");Array.Sort(files);
                // Only this app's bounded snapshot folder is managed here.
                for(int i=0;i<files.Length-6;i++)File.Delete(files[i]);saved=true;
            }
            catch(Exception){UI.Hint(L("Anı kaydedilemedi.","Could not save the memory."));}
            finally{Destroy(texture);}
            if(saved)UI.Hint(L("Bahçenin anısı albümde.","A garden memory is in your album."));
        }
    }
}
