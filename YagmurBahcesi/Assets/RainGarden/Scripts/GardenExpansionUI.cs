using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
namespace RainGarden
{
    public sealed partial class GardenUI
    {
        int catalogTab,catalogPage;
        Text stampLabel;
        Texture2D albumTexture;
        string activityFeedback;
        string[] ActivityNames=>new[]{game.L("Piknik Sepeti","Picnic Basket"),game.L("Bulutun Yolu","Cloud Path"),game.L("Yaprak Kaptanı","Leaf Captain")};
        Sprite ActivitySprite(int kind)=>kind==0?game.art.Expansion(14):game.art.Prop(kind==1?0:6);
        void BuildExpansionControls()
        {
            var wallet=Panel(main,Vector2.zero,new Vector2(220,74),cream);Anchor(wallet,new Vector2(.5f,1),new Vector2(-80,-65));
            Picture(wallet,game.art.Prop(4),new Vector2(-73,0),new Vector2(44,44));
            stampLabel=Label(wallet,game.State.stamps+" "+game.L("pul","stamps"),new Vector2(25,0),new Vector2(140,55),25,ink,TextAnchor.MiddleCenter);
            ExpansionButton(game.L("Bahçem","My garden"),game.art.Expansion(2),new Vector2(110,102),()=>ShowCollection());
            ExpansionButton(game.L("Keşifler","Discoveries"),game.art.Friend(4),new Vector2(270,102),ShowProjects);
            var memory=Button(main,game.L("Anılar","Memories"),Vector2.zero,new Vector2(135,76),cream,ink,()=>ShowAlbum(),22);
            Anchor((RectTransform)memory.transform,new Vector2(1,0),new Vector2(-235,90));
            WorldActivity(0,new Vector2(-.2f,-1.65f));
            WorldActivity(1,new Vector2(-7.4f,2.6f));
            WorldActivity(2,new Vector2(6.8f,1.9f));
        }
        void ExpansionButton(string title,Sprite sprite,Vector2 offset,Action action)
        {
            var button=Button(main,"",Vector2.zero,new Vector2(142,136),cream,ink,action);
            Anchor((RectTransform)button.transform,new Vector2(0,0),offset);
            Picture(button.transform,sprite,new Vector2(0,20),new Vector2(79,77));
            Label(button.transform,title,new Vector2(0,-41),new Vector2(137,35),20,ink,TextAnchor.MiddleCenter);
        }
        void WorldActivity(int kind,Vector2 world)
        {
            Vector3 screen=Camera.main.WorldToScreenPoint(world);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(main,screen,null,out Vector2 local);
            var b=Button(main,"",local,new Vector2(92,80),new Color(cream.r,cream.g,cream.b,.96f),ink,()=>OpenActivity(kind));
            b.name="Activity entry "+kind;Picture(b.transform,ActivitySprite(kind),new Vector2(-5,3),new Vector2(65,59));
            var arrow=Picture(b.transform,game.art.triangle,new Vector2(30,-21),new Vector2(17,21));arrow.color=green;
        }
        public void ShowCollection(int tab=0,int page=0)
        {
            catalogTab=tab;catalogPage=page;
            var p=OpenModal(game.L("Bahçem","My garden"),new Vector2(1180,860));
            var items=tab==0?GardenCatalog.Flowers:GardenCatalog.Decorations;
            Label(p,game.State.stamps+" "+game.L("bahçe pulu","garden stamps"),new Vector2(0,265),new Vector2(370,55),26,green,TextAnchor.MiddleCenter);
            Button(p,game.L("Çiçekler","Flowers"),new Vector2(-365,265),new Vector2(240,64),tab==0?green:cream,tab==0?cream:ink,()=>ShowCollection(0),25);
            Button(p,game.L("Süsler","Decorations"),new Vector2(365,265),new Vector2(240,64),tab==1?green:cream,tab==1?cream:ink,()=>ShowCollection(1),25);
            for(int j=0;j<4;j++)
            {
                int index=page*4+j;if(index>=items.Length)break;var item=items[index];bool owns=GardenCatalog.Owns(game.State,item.id);
                var card=Panel(p,new Vector2(-420+j*280,-5),new Vector2(255,395),Color.white);
                Picture(card,tab==0?GardenCatalog.FlowerSprite(game.art,index):GardenCatalog.DecorSprite(game.art,index),new Vector2(0,75),new Vector2(175,170));
                Label(card,game.L(item.tr,item.en),new Vector2(0,-37),new Vector2(235,65),25,ink,TextAnchor.MiddleCenter,FontStyle.Bold);
                string action=owns?(tab==0?game.L("Dik","Plant"):game.State.placed[index]?game.L("Kaldır","Put away"):game.L("Yerleştir","Place")):item.cost+" "+game.L("pul","stamps");
                var button=Button(card,action,new Vector2(0,-128),new Vector2(210,75),green,cream,()=>{
                    if(!GardenCatalog.Owns(game.State,item.id))
                    {
                        if(game.State.stamps<item.cost){ShowActivities();return;}
                        if(!game.Purchase(item.id))return;
                    }
                    if(tab==0)ShowPlanting(item.id);else{game.ToggleDecoration(index);ShowCollection(tab,page);}
                },25);button.name="Catalog "+item.id;
                if(!owns)Label(card,game.L("Yeni tür","New discovery"),new Vector2(0,170),new Vector2(235,35),17,green,TextAnchor.MiddleCenter);
            }
            int pages=(items.Length+3)/4;
            Button(p,"<",new Vector2(-420,-290),new Vector2(100,72),cream,ink,()=>ShowCollection(tab,(page+pages-1)%pages),33);
            Label(p,(page+1)+" / "+pages,new Vector2(0,-290),new Vector2(180,60),24,ink,TextAnchor.MiddleCenter);
            Button(p,">",new Vector2(420,-290),new Vector2(100,72),cream,ink,()=>ShowCollection(tab,(page+1)%pages),33);
            Label(p,game.L("Açtığın türler senin. Yeniden kullanmak ücretsiz.","Your discoveries stay yours. Reuse them freely."),new Vector2(0,-372),new Vector2(1000,44),21,green,TextAnchor.MiddleCenter);
        }
        public void ShowPlanting(string id)
        {
            int flower=GardenCatalog.FlowerIndex(id);var p=OpenModal(game.L("Nereye dikelim?","Where shall it grow?"),new Vector2(1050,820));
            Picture(p,GardenCatalog.FlowerSprite(game.art,flower),new Vector2(0,185),new Vector2(130,150));
            for(int i=0;i<6;i++)
            {
                int slot=i;Vector2 pos=new Vector2(-300+(i%3)*300,10-(i/3)*180);
                var b=Button(p,"",pos,new Vector2(240,155),new Color(.88f,.92f,.8f),ink,()=>{if(game.Plant(id,slot)){CloseModal();Rebuild();}});
                b.name="Plant slot "+slot;Picture(b.transform,game.art.Expansion(12),new Vector2(-45,5),new Vector2(95,100));
                Label(b.transform,(i+1).ToString(),new Vector2(60,3),new Vector2(80,70),42,ink,TextAnchor.MiddleCenter);
            }
            Label(p,game.L("Eski çiçeğin koleksiyonunda kalır.","Your previous flower stays in the collection."),new Vector2(0,-315),new Vector2(920,65),23,ink,TextAnchor.MiddleCenter);
        }
        public void ShowProjects()
        {
            var p=OpenModal(game.L("Dostların keşif defteri","Our discovery book"),new Vector2(1170,920));
            for(int project=0;project<4;project++)
            {
                int index=project;var card=Panel(p,new Vector2(-280+(project%2)*560,140-(project/2)*300),new Vector2(520,275),Color.white);
                Picture(card,game.art.Friend(project==3?4:project==1?2:project==2?1:0),new Vector2(-190,40),new Vector2(110,130));
                Label(card,game.L(GardenProjects.NamesTR[project],GardenProjects.NamesEN[project]),new Vector2(55,85),new Vector2(375,60),24,ink,TextAnchor.MiddleCenter,FontStyle.Bold);
                for(int j=0;j<3;j++)
                {
                    int step=project*3+j;float y=23-j*48;
                    var dot=Picture(card,game.art.circle,new Vector2(-103,y),new Vector2(20,20));dot.color=game.State.discoveries[step]?green:new Color(.86f,.87f,.8f);
                    Label(card,game.L(GardenProjects.StepsTR[step],GardenProjects.StepsEN[step]),new Vector2(70,y),new Vector2(320,40),19,ink,TextAnchor.MiddleLeft);
                }
                var b=Button(card,GardenProjects.Finished(game.State,project)?game.L("Tamamlandı","Discovered"):game.L("Birlikte deneyelim","Let's explore"),new Vector2(0,-105),new Vector2(340,50),cream,green,()=>GuideProject(index),20);
                b.name="Project "+index;
            }
            Label(p,game.L("Acele yok. Keşifler seni bekler.","No rush. Your discoveries will wait."),new Vector2(0,-375),new Vector2(1000,50),24,green,TextAnchor.MiddleCenter);
        }
        void GuideProject(int project)
        {
            int step=project*3;while(step<project*3+3&&game.State.discoveries[step])step++;
            if(step==1||step==2||step==8||step==10){ShowCollection(step==8?1:0);return;}
            if(step==4||step==7||step==9||step==11){if(step==11)ShowActivities();else OpenActivity(step==4?2:step==7?0:1);return;}
            CloseModal();game.UI.Hint(step<12?game.L(GardenProjects.StepsTR[step],GardenProjects.StepsEN[step]):game.L("Sumi'ye dokun; birlikte keşfedin.","Tap Sumi and explore together."));
        }
        public void ShowActivities()
        {
            var p=OpenModal(game.L("Birlikte ne oynayalım?","What shall we play?"),new Vector2(1110,740));
            for(int i=0;i<3;i++)
            {
                int kind=i;var b=Button(p,"",new Vector2(-345+i*345,-15),new Vector2(300,340),Color.white,ink,()=>OpenActivity(kind));
                b.name="Choose activity "+kind;Picture(b.transform,ActivitySprite(i),new Vector2(0,65),new Vector2(185,170));
                Label(b.transform,ActivityNames[i],new Vector2(0,-65),new Vector2(280,65),26,ink,TextAnchor.MiddleCenter,FontStyle.Bold);
                Label(b.transform,game.L("Oyna","Play"),new Vector2(0,-130),new Vector2(270,50),24,green,TextAnchor.MiddleCenter);
            }
            Label(p,game.L("Her tamamlanan oyunla 1 bahçe pulu.","One garden stamp for every completed game."),new Vector2(0,-270),new Vector2(1000,60),24,green,TextAnchor.MiddleCenter);
        }
        public void OpenActivity(int kind)
        {if(game.BeginActivity(kind)){activityFeedback=null;ShowActivity();}}
        public void ShowActivity()
        {
            var a=game.State.activity;if(a==null||!a.Valid())return;
            var p=OpenModal(ActivityNames[a.kind],new Vector2(1180,920));
            if(GardenActivities.Complete(a)){ShowActivityReward(p);return;}
            string[] hintsTR={"Resimdeki meyveleri dostlarınla paylaş.","Kanallara dokun; su çiçeğe ulaşsın.","Rüzgârı seç; yaprağı çiçekli iskeleye götür."};
            string[] hintsEN={"Share the pictured fruit with your friends.","Tap the channels to bring water to the flower.","Choose the breeze. Reach the flower landing."};
            Label(p,activityFeedback??game.L(hintsTR[a.kind],hintsEN[a.kind]),new Vector2(0,290),new Vector2(1050,70),25,ink,TextAnchor.MiddleCenter);
            if(a.kind==0)DrawPicnic(p,a);else if(a.kind==1)DrawCloud(p,a);else DrawBoat(p,a);
            var help=Button(p,game.L("Birlikte yapalım","Help me"),new Vector2(-260,-355),new Vector2(320,78),cream,green,()=>ActivityAction(-1,true),25);help.name="Activity help";
            var back=Button(p,game.L("Bahçeye dön","Back to garden"),new Vector2(245,-355),new Vector2(400,78),green,cream,()=>{CloseModal();Rebuild();},25);back.name="Activity leave";
            Label(p,game.L("Kaldığın yer saklanır.","Your place is saved."),new Vector2(0,-419),new Vector2(700,36),19,green,TextAnchor.MiddleCenter);
        }
        public void ActivityAction(int input,bool help=false)
        {
            bool ok=game.ActivityInput(input,help);
            activityFeedback=ok?null:game.L("Birlikte başka bir yol deneyelim.","Let's try another way together.");
            game.Sound(ok?2:0);ShowActivity();
        }
        void DrawPicnic(RectTransform p,GardenActivityState a)
        {
            int[] order=GardenActivities.PicnicOrder(a);
            Picture(p,game.art.Friend(a.variant),new Vector2(-420,30),new Vector2(180,240));
            for(int i=0;i<order.Length;i++)
            {
                float x=-(order.Length-1)*70+i*140;var card=Panel(p,new Vector2(x,110),new Vector2(125,145),i<a.step?new Color(.79f,.88f,.67f):i==a.step?new Color(1,.88f,.57f):Color.white);
                Picture(card,game.art.Prop(6+order[i]),new Vector2(0,8),new Vector2(84,93));
                if(i<a.step){var dot=Picture(card,game.art.circle,new Vector2(0,-53),new Vector2(17,17));dot.color=green;}
            }
            for(int fruit=1;fruit<=2;fruit++)
            {
                int selected=fruit;var b=Button(p,"",new Vector2(fruit==1?-150:150,-135),new Vector2(235,195),Color.white,ink,()=>ActivityAction(selected));
                b.name="Picnic fruit "+fruit;Picture(b.transform,game.art.Prop(6+fruit),new Vector2(0,10),new Vector2(130,145));
            }
            Label(p,a.step+" / "+order.Length,new Vector2(400,-155),new Vector2(160,55),32,green,TextAnchor.MiddleCenter);
        }
        void DrawCloud(RectTransform p,GardenActivityState a)
        {
            int[] path=GardenActivities.CloudPath(a);
            Picture(p,game.art.Prop(0),new Vector2(-405,160-(path[0]/3)*150),new Vector2(130,110));
            Picture(p,GardenCatalog.FlowerSprite(game.art,2),new Vector2(335,160-(path[path.Length-1]/3)*150),new Vector2(105,135));
            for(int i=0;i<9;i++)
            {
                int cell=i;int mask=GardenActivities.Mask(a,i);Vector2 pos=new Vector2(-190+(i%3)*150,160-(i/3)*150);
                var b=Button(p,"",pos,new Vector2(137,137),new Color(.87f,.9f,.8f),ink,()=>ActivityAction(cell));b.name="Cloud cell "+cell;
                if(mask==0){Picture(b.transform,game.art.Prop(10),Vector2.zero,new Vector2(52,55));b.interactable=false;continue;}
                int pathIndex=Array.IndexOf(path,i);bool wet=true;for(int j=0;j<=pathIndex;j++)wet&=GardenActivities.Connected(a,path[j]);
                Color water=wet?new Color(.34f,.7f,.8f):new Color(.66f,.76f,.75f);
                var center=Picture(b.transform,game.art.circle,Vector2.zero,new Vector2(30,30));center.color=water;
                Vector2[] dirs={Vector2.up,Vector2.right,Vector2.down,Vector2.left};
                for(int d=0;d<4;d++)if((mask&(1<<d))!=0){var line=Panel(b.transform,dirs[d]*34,d%2==0?new Vector2(25,76):new Vector2(76,25),water);line.GetComponent<Image>().raycastTarget=false;}
            }
        }
        void DrawBoat(RectTransform p,GardenActivityState a)
        {
            int[] stops=GardenActivities.BoatStops(a);
            for(int i=0;i<15;i++)
            {
                Vector2 pos=new Vector2(-365+(i%5)*132,140-(i/5)*140);
                var tile=Panel(p,pos,new Vector2(123,131),new Color(.76f,.9f,.9f));tile.GetComponent<Image>().raycastTarget=false;
                if(GardenActivities.Rock(a,i))Picture(tile,game.art.Prop(10),Vector2.zero,new Vector2(85,85));
                int stop=Array.IndexOf(stops,i);
                if(stop>=0){Picture(tile,game.art.Expansion(11),new Vector2(0,-35),new Vector2(98,55));var marker=Picture(tile,game.art.Prop(4),new Vector2(32,31),new Vector2(42,42));marker.color=stop<a.step?new Color(.6f,.8f,.65f):stop==a.step?Color.white:new Color(1,1,1,.35f);}
                if(a.boatCell==i)Picture(tile,game.art.Prop(6),new Vector2(0,12),new Vector2(105,90));
            }
            Vector2[] offsets={new Vector2(410,100),new Vector2(495,0),new Vector2(410,-100),new Vector2(325,0)};
            for(int d=0;d<4;d++)
            {
                int direction=d;var b=Button(p,"",offsets[d],new Vector2(84,84),green,cream,()=>ActivityAction(direction));b.name="Boat direction "+direction;
                var arrow=Picture(b.transform,game.art.triangle,Vector2.zero,new Vector2(33,38));arrow.color=cream;arrow.rectTransform.localRotation=Quaternion.Euler(0,0,90-d*90);
            }
            Label(p,a.step+" / "+stops.Length,new Vector2(410,-210),new Vector2(160,55),30,green,TextAnchor.MiddleCenter);
        }
        void ShowActivityReward(RectTransform p)
        {
            if(!game.State.activity.claimed){game.ClaimActivity(game.State.activity.id);}
            Picture(p,game.art.Friend(game.State.activity.kind,1),new Vector2(-240,75),new Vector2(250,290));
            var stamp=Picture(p,game.art.circle,new Vector2(180,85),new Vector2(180,180));stamp.color=new Color(.94f,.79f,.42f);
            Picture(p,game.art.Prop(4),new Vector2(180,90),new Vector2(120,120));
            Label(p,game.L("Birlikte başardık!","We did it together!"),new Vector2(0,-115),new Vector2(1000,65),38,ink,TextAnchor.MiddleCenter,FontStyle.Bold);
            Label(p,game.State.activity.claimed?game.L("1 bahçe pulu koleksiyonuna eklendi.","1 garden stamp was added to your collection."):game.L("Ödül kaydedilemedi. Yeniden dene.","Could not save your stamp. Try again."),new Vector2(0,-185),new Vector2(1000,60),25,green,TextAnchor.MiddleCenter);
            var choose=Button(p,game.L("Bahçem için seç","Choose for my garden"),new Vector2(-240,-315),new Vector2(430,86),green,cream,()=>ShowCollection(),25);choose.name="Reward collection";
            Button(p,game.L("Bahçeye dön","Back to garden"),new Vector2(250,-315),new Vector2(410,86),cream,ink,()=>{CloseModal();Rebuild();},25);
        }
        public void ShowAlbum(int page=0)
        {
            var p=OpenModal(game.L("Bahçemin anıları","Garden memories"),new Vector2(1140,880));
            string[] files=Directory.Exists(game.AlbumDirectory)?Directory.GetFiles(game.AlbumDirectory,"*.png"):new string[0];Array.Sort(files);
            if(albumTexture!=null){Destroy(albumTexture);albumTexture=null;}
            if(files.Length>0)
            {
                page=Mathf.Clamp(page,0,files.Length-1);int current=page;
                try
                {
                    albumTexture=new Texture2D(2,2);albumTexture.LoadImage(File.ReadAllBytes(files[files.Length-1-page]));
                    float ratio=(float)albumTexture.width/albumTexture.height;
                    var size=ratio>960f/490?new Vector2(960,960/ratio):new Vector2(490*ratio,490);
                    var r=Rect("Garden memory",p,new Vector2(0,40),size);var raw=r.gameObject.AddComponent<RawImage>();raw.texture=albumTexture;raw.raycastTarget=false;
                }
                catch(Exception){Label(p,game.L("Bu anı açılamadı.","This memory could not be opened."),Vector2.zero,new Vector2(900,100),28,ink,TextAnchor.MiddleCenter);}
                Button(p,"<",new Vector2(-350,-250),new Vector2(110,64),cream,ink,()=>ShowAlbum((current+1)%files.Length),30);
                Label(p,(page+1)+" / "+files.Length,new Vector2(0,-250),new Vector2(200,50),24,green,TextAnchor.MiddleCenter);
                Button(p,">",new Vector2(350,-250),new Vector2(110,64),cream,ink,()=>ShowAlbum((current+files.Length-1)%files.Length),30);
            }
            else {Picture(p,game.art.Expansion(15),new Vector2(0,25),new Vector2(210,220));Label(p,game.L("Kendi bahçenden bir anı sakla.","Keep a memory of your own garden."),new Vector2(0,-150),new Vector2(1000,65),28,ink,TextAnchor.MiddleCenter);}
            Button(p,game.L("Yeni anı kaydet","Keep a new memory"),new Vector2(0,-335),new Vector2(590,90),green,cream,game.CaptureMemory,27);
        }
    }
}
