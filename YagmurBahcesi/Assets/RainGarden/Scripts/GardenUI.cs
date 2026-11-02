using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace RainGarden
{
    public sealed partial class GardenUI : MonoBehaviour
    {
        GardenGame game;
        Canvas canvas;
        RectTransform safe, main, modal;
        Text hint;
        Image hintPanel;
        Image[] tools;
        float hintUntil;
        bool home=true, parentSettings;
        Rect lastSafe;
        int lastWidth,lastHeight;
        readonly Color ink=new Color(.22f,.32f,.25f);
        readonly Color cream=new Color(.99f,.975f,.91f);
        readonly Color green=new Color(.34f,.48f,.33f);
        public bool BlocksWorld => home || modal!=null;

        public void Initialize(GardenGame owner)
        {
            game=owner;
            var go=new GameObject("Garden interface",typeof(RectTransform),typeof(Canvas),typeof(CanvasScaler),typeof(GraphicRaycaster));
            go.transform.SetParent(transform);
            canvas=go.GetComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;canvas.sortingOrder=100;
            var scale=go.GetComponent<CanvasScaler>();scale.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scale.referenceResolution=new Vector2(1920,1080);scale.matchWidthOrHeight=1;
            safe=Rect("Safe area",go.transform,Vector2.zero,Vector2.zero);safe.anchorMin=Vector2.zero;safe.anchorMax=Vector2.one;safe.offsetMin=safe.offsetMax=Vector2.zero;
            if(EventSystem.current==null)new GameObject("Touch UI",typeof(EventSystem),typeof(StandaloneInputModule));
            UpdateSafeArea();
        }
        public void ShowHome() {home=true;CloseModal();Rebuild();}
        public void ShowGame() {home=false;CloseModal();Rebuild();}
        public void Rebuild()
        {
            if(main!=null){main.gameObject.SetActive(false);Destroy(main.gameObject);}
            main=Rect("Main",safe,Vector2.zero,Vector2.zero);main.anchorMin=Vector2.zero;main.anchorMax=Vector2.one;main.offsetMin=main.offsetMax=Vector2.zero;
            if(home)BuildHome();else BuildGame();
            if(parentSettings){CloseModal();ShowParents();}
        }
        void BuildHome()
        {
            float width=Screen.safeArea.width/Mathf.Max(1,Screen.safeArea.height)*1080;
            float center=-Mathf.Min(435,width*.235f);
            var panel=Panel(main,new Vector2(center,0),new Vector2(width<1600?620:680,785),new Color(1,.988f,.944f,.96f));
            Label(panel,game.L("M İ N İ K   D O S T L A R","L I T T L E   F R I E N D S"),new Vector2(0,290),new Vector2(600,40),23,green,TextAnchor.MiddleCenter);
            Label(panel,game.L("Yağmur","Rain"),new Vector2(0,190),new Vector2(600,135),86,ink,TextAnchor.MiddleCenter,FontStyle.Bold);
            Label(panel,game.L("Bahçesi","Garden"),new Vector2(0,85),new Vector2(600,135),86,ink,TextAnchor.MiddleCenter,FontStyle.Bold);
            Label(panel,game.L("Küçük dokunuşlar,\nkocaman keşifler.","Little touches.\nWonderful discoveries."),new Vector2(0,-20),new Vector2(570,100),31,ink,TextAnchor.MiddleCenter);
            var play=Button(panel,game.L("Bahçeye gir","Let's play"),new Vector2(0,-170),new Vector2(460,105),green,cream,game.StartGarden,32);
            var arrow=Picture(play.transform,game.art.triangle,new Vector2(-170,0),new Vector2(26,32));arrow.color=cream;
            Label(panel,game.L("Acele yok. Her şey senin hızında.","No rush. A little world at your pace."),new Vector2(0,-280),new Vector2(580,65),23,green,TextAnchor.MiddleCenter);
            var badge=Picture(main,game.art.Friend(0),new Vector2(Mathf.Min(410,width*.26f),-20),new Vector2(350,475));badge.preserveAspect=true;
            TopButton(game.State.language.ToUpperInvariant(),new Vector2(-330,-65),new Vector2(95,75),()=>{game.ToggleLanguage();});
            TopButton(game.State.sound?game.L("Ses açık","Sound on"):game.L("Sessiz","Muted"),new Vector2(-195,-65),new Vector2(150,75),game.ToggleSound);
            TopButton(game.L("Yetişkin","Grown-ups"),new Vector2(-105,65),new Vector2(170,70),ShowGate,false);
            LabelAnchored(main,game.L("Reklamsız · Çevrimdışı · Sana ait bir bahçe","Ad-free · Offline · A garden of your own"),new Vector2(0,38),new Vector2(1000,40),19,ink,new Vector2(.5f,0));
        }
        void BuildGame()
        {
            var name=Panel(main,new Vector2(0,0),new Vector2(380,96),new Color(cream.r,cream.g,cream.b,.94f));Anchor(name,new Vector2(0,1),new Vector2(230,-72));
            Label(name,game.L("Yağmur Bahçesi","Rain Garden"),new Vector2(0,10),new Vector2(350,46),33,ink,TextAnchor.MiddleCenter,FontStyle.Bold);
            Label(name,game.L("Dokun. Keşfet. Birlikte büyüt.","Touch. Discover. Grow together."),new Vector2(0,-25),new Vector2(350,30),17,green,TextAnchor.MiddleCenter);
            TopButton(game.L("Menü","Home"),new Vector2(-115,-65),new Vector2(155,76),game.Home);
            TopButton(game.State.sound?game.L("Ses açık","Sound on"):game.L("Sessiz","Muted"),new Vector2(-285,-65),new Vector2(155,76),game.ToggleSound);
            TopButton(game.L("Yetişkin","Grown-ups"),new Vector2(-460,-65),new Vector2(165,76),ShowGate);
            var tray=Panel(main,new Vector2(0,0),new Vector2(620,134),new Color(cream.r,cream.g,cream.b,.97f));Anchor(tray,new Vector2(.5f,0),new Vector2(0,94));
            tools=new Image[4];
            string[] tr={"Keşfet","Yağmur","Güneş","Rüzgâr"},en={"Explore","Rain","Sun","Breeze"};
            for(int i=0;i<4;i++)
            {
                int index=i;float x=-225+i*150;
                var b=Button(tray,"",new Vector2(x,0),new Vector2(132,114),cream,ink,()=>game.SelectTool((GardenTool)index));
                tools[i]=b.GetComponent<Image>();
                Picture(b.transform,i==0?game.art.Friend(0):game.art.Prop(i-1),new Vector2(0,13),new Vector2(78,70));
                Label(b.transform,game.L(tr[i],en[i]),new Vector2(0,-38),new Vector2(130,32),20,ink,TextAnchor.MiddleCenter);
            }
            RefreshTools();
            var help=Button(main,"?",Vector2.zero,new Vector2(88,88),cream,ink,ShowHelp,40);Anchor((RectTransform)help.transform,new Vector2(1,0),new Vector2(-90,90));
            var hintRect=Panel(main,Vector2.zero,new Vector2(900,66),new Color(cream.r,cream.g,cream.b,.93f));Anchor(hintRect,new Vector2(.5f,1),new Vector2(0,-165));
            hintPanel=hintRect.GetComponent<Image>();hintPanel.raycastTarget=false;
            hint=Label(hintRect,"",Vector2.zero,new Vector2(850,60),23,ink,TextAnchor.MiddleCenter);hint.gameObject.SetActive(false);hintPanel.enabled=false;
            BuildExpansionControls();
        }
        public void RefreshTools()
        {
            if(tools==null)return;
            for(int i=0;i<tools.Length;i++)if(tools[i]!=null)tools[i].color=(int)game.Tool==i?new Color(.83f,.9f,.72f):cream;
        }
        public void Hint(string text)
        {
            if(hint==null)return;hint.text=text;hint.gameObject.SetActive(true);hintPanel.enabled=true;hintUntil=Time.unscaledTime+5;
        }
        public void ShowHintPulse() {if(!BlocksWorld)Hint(game.L("Bulutu toprağa taşı; yaprağı gölete bırak.","Bring a cloud to the soil, or a leaf to the pond."));}
        void ShowHelp()
        {
            var p=OpenModal(game.L("Bahçenin küçük sırları","Little garden discoveries"),new Vector2(1080,730));
            string[] tr={"Bulutu toprağa taşı.\nFilizlere güneş ver.","Yaprağı suya bırak.\nRüzgârla yola çıksın.","Meyveleri paylaş.\nDostlarınla piknik yap."};
            string[] en={"Bring rain to the soil.\nGive sprouts sunshine.","Put the leaf in water.\nSend it on a breeze.","Share a little fruit.\nHave a picnic together."};
            int[] icons={0,6,7};
            for(int i=0;i<3;i++) {float x=-345+i*345;Picture(p,game.art.Prop(icons[i]),new Vector2(x,65),new Vector2(190,190));Label(p,game.L(tr[i],en[i]),new Vector2(x,-110),new Vector2(320,120),25,ink,TextAnchor.MiddleCenter);}
            Button(p,game.L("Hadi deneyelim","Let's try"),new Vector2(0,-255),new Vector2(420,86),green,cream,CloseModal,29);
        }
        void ShowGate()
        {
            parentSettings=false;
            int a=UnityEngine.Random.Range(23,49),b=UnityEngine.Random.Range(17,38),answer=a+b;
            var p=OpenModal(game.L("Yetişkinler için","For grown-ups"),new Vector2(820,610));
            Label(p,game.L("Devam etmek için sonucu seçin.","Choose the answer to continue."),new Vector2(0,110),new Vector2(700,60),27,ink,TextAnchor.MiddleCenter);
            Label(p,a+" + "+b+" = ?",new Vector2(0,5),new Vector2(600,90),56,ink,TextAnchor.MiddleCenter,FontStyle.Bold);
            int correct=UnityEngine.Random.Range(0,3);
            for(int i=0;i<3;i++)
            {
                bool right=i==correct;int value=right?answer:answer+(i-correct)*7;
                Button(p,value.ToString(),new Vector2(-240+i*240,-130),new Vector2(190,90),cream,ink,()=>{if(right)ShowParents();else ShowGate();},34);
            }
        }
        void ShowParents()
        {
            var p=OpenModal(game.L("Senin bahçen, sizin kurallarınız","Your garden, your family's pace"),new Vector2(1020,820));parentSettings=true;
            Label(p,game.L("4–6 yaş için sakin keşif ve birlikte oyun.\nReklam, sohbet, satın alma ekranı ve bildirim yok.\nBahçe bu cihazda saklanır. İstediğiniz an ara verebilirsiniz.","Calm discovery and shared play for ages 4–6.\nNo ads, chat, purchase screens or notifications.\nThe garden stays on this device. Take a break anytime."),new Vector2(0,190),new Vector2(880,150),24,ink,TextAnchor.MiddleCenter);
            Button(p,game.L("Dil: Türkçe","Language: English"),new Vector2(-230,35),new Vector2(420,76),cream,ink,game.ToggleLanguage,26);
            Button(p,game.State.sound?game.L("Ses: Açık","Sound: On"):game.L("Ses: Kapalı","Sound: Off"),new Vector2(230,35),new Vector2(420,76),cream,ink,game.ToggleSound,26);
            Button(p,game.State.reducedMotion?game.L("Hareket: Azaltılmış","Motion: Reduced"):game.L("Hareket: Normal","Motion: Normal"),new Vector2(-230,-65),new Vector2(420,76),cream,ink,game.ToggleMotion,24);
            Button(p,game.L("Gizlilik ve bilgiler","Privacy and information"),new Vector2(230,-65),new Vector2(420,76),cream,ink,ShowPrivacy,24);
            Button(p,game.L("Bahçeyi yeniden başlat","Start a fresh garden"),new Vector2(0,-175),new Vector2(550,76),new Color(.94f,.86f,.76f),ink,ConfirmReset,25);
            Label(p,game.L("Birlikte keşfedin. İstediğiniz zaman ara verin.","Explore together. Take a break whenever you like."),new Vector2(0,-285),new Vector2(890,50),22,green,TextAnchor.MiddleCenter);
        }
        void ShowPrivacy()
        {
            var p=OpenModal(game.L("Gizlilik ve bilgiler","Privacy and information"),new Vector2(1020,830));
            Label(p,game.L("Bu sürüm hesap açmanızı istemez.\nOyun içi analitik, reklam veya sosyal bağlantı içermez.\nİlerleme ve ayarlar yalnızca cihazın uygulama alanında saklanır.\nUygulama kaldırıldığında yerel kayıt kaybolabilir.\n\nÖğrenme fırsatları: neden-sonuç, paylaşma ve yaratıcı oyun.\nÖlçülmüş eğitimsel başarı veya terapi iddiası yoktur.\n\nGörseller: bu proje için yapay zekâ ile üretilmiştir.\nYazı tipi: Nunito, SIL Open Font License.\nSesler: proje içinde üretilen özgün yumuşak tonlar.\n\nYayıncı ve destek bilgileri mağaza yayını öncesi eklenecektir.","This build does not ask you to create an account.\nIt includes no in-game analytics, ads or social links.\nProgress and settings stay in the device's app storage.\nUninstalling may remove your saved garden.\n\nPlay offers opportunities for cause and effect and creativity.\nNo measured learning or therapeutic outcomes are claimed.\n\nArtwork: AI-generated for this project.\nTypeface: Nunito, SIL Open Font License.\nAudio: original gentle tones generated for the project.\n\nPublisher and support details will be added before release."),new Vector2(0,-10),new Vector2(900,600),24,ink,TextAnchor.MiddleCenter);
        }
        void ConfirmReset()
        {
            var p=OpenModal(game.L("Yeni bir bahçe?","A fresh garden?"),new Vector2(820,570));
            Label(p,game.L("Bahçe, pul bakiyesi, açılan türler ve keşifler sıfırlanacak.\nBu işlem geri alınamaz.","The garden, stamps, unlocked items and discoveries will be reset.\nThis cannot be undone."),new Vector2(0,40),new Vector2(720,140),27,ink,TextAnchor.MiddleCenter);
            Button(p,game.L("Vazgeç","Cancel"),new Vector2(-190,-155),new Vector2(320,85),cream,ink,ShowParents,27);
            Button(p,game.L("Yeni bahçe","Fresh garden"),new Vector2(190,-155),new Vector2(320,85),green,cream,game.ResetGarden,27);
        }
        RectTransform OpenModal(string title,Vector2 size)
        {
            CloseModal();
            modal=Rect("Modal",safe,Vector2.zero,Vector2.zero);modal.anchorMin=Vector2.zero;modal.anchorMax=Vector2.one;modal.offsetMin=modal.offsetMax=Vector2.zero;
            var shade=modal.gameObject.AddComponent<Image>();shade.color=new Color(.14f,.22f,.16f,.5f);
            var p=Panel(modal,Vector2.zero,size,cream);
            float available=Screen.safeArea.width/Mathf.Max(1,Screen.safeArea.height)*1080;
            p.localScale=Vector3.one*Mathf.Min(1,(available-60)/size.x);
            Label(p,title,new Vector2(0,size.y/2-78),new Vector2(size.x-150,100),34,ink,TextAnchor.MiddleCenter,FontStyle.Bold);
            Button(p,"×",new Vector2(size.x/2-55,size.y/2-55),new Vector2(65,65),new Color(.91f,.92f,.85f),ink,CloseModal,38);
            return p;
        }
        public void CloseModal()
        {
            parentSettings=false;
            if(modal!=null){modal.gameObject.SetActive(false);Destroy(modal.gameObject);modal=null;}
        }
        void TopButton(string text,Vector2 offset,Vector2 size,Action action,bool top=true)
        {
            var b=Button(main,text,Vector2.zero,size,cream,ink,action,22);Anchor((RectTransform)b.transform,new Vector2(1,top?1:0),offset);
        }
        RectTransform Rect(string name,Transform parent,Vector2 position,Vector2 size)
        {
            var go=new GameObject(name,typeof(RectTransform));go.transform.SetParent(parent,false);
            var r=(RectTransform)go.transform;r.anchorMin=r.anchorMax=r.pivot=new Vector2(.5f,.5f);r.anchoredPosition=position;r.sizeDelta=size;return r;
        }
        void Anchor(RectTransform r,Vector2 anchor,Vector2 offset) {r.anchorMin=r.anchorMax=anchor;r.anchoredPosition=offset;}
        RectTransform Panel(Transform parent,Vector2 position,Vector2 size,Color color)
        {
            var r=Rect("Panel",parent,position,size);var im=r.gameObject.AddComponent<Image>();im.sprite=game.art.roundPanel;im.type=Image.Type.Sliced;im.color=color;return r;
        }
        Text Label(Transform parent,string value,Vector2 position,Vector2 size,int fontSize,Color color,TextAnchor alignment,FontStyle style=FontStyle.Normal)
        {
            var r=Rect("Text",parent,position,size);var t=r.gameObject.AddComponent<Text>();t.font=game.art.font;t.text=value;t.fontSize=fontSize;t.color=color;t.alignment=alignment;t.fontStyle=style;t.raycastTarget=false;t.horizontalOverflow=HorizontalWrapMode.Wrap;t.verticalOverflow=VerticalWrapMode.Truncate;return t;
        }
        void LabelAnchored(Transform parent,string value,Vector2 position,Vector2 size,int fontSize,Color color,Vector2 anchor)
        {var t=Label(parent,value,position,size,fontSize,color,TextAnchor.MiddleCenter);Anchor(t.rectTransform,anchor,position);}
        Image Picture(Transform parent,Sprite sprite,Vector2 position,Vector2 size)
        {
            var r=Rect("Picture",parent,position,size);var im=r.gameObject.AddComponent<Image>();im.sprite=sprite;im.preserveAspect=true;im.raycastTarget=false;return im;
        }
        Button Button(Transform parent,string text,Vector2 position,Vector2 size,Color background,Color foreground,Action action,int fontSize=25)
        {
            var r=Panel(parent,position,size,background);r.name="Button "+text;
            var b=r.gameObject.AddComponent<Button>();b.targetGraphic=r.GetComponent<Image>();
            var colors=b.colors;colors.highlightedColor=new Color(.96f,.99f,.9f);colors.pressedColor=new Color(.8f,.86f,.73f);colors.selectedColor=Color.white;b.colors=colors;
            b.navigation=new Navigation{mode=Navigation.Mode.None};b.onClick.AddListener(()=>{game.Sound(0);action();});
            if(!string.IsNullOrEmpty(text))Label(r,text,Vector2.zero,size-new Vector2(16,8),fontSize,foreground,TextAnchor.MiddleCenter,FontStyle.Bold);
            return b;
        }
        void UpdateSafeArea()
        {
            Rect area=Screen.safeArea;lastSafe=area;lastWidth=Screen.width;lastHeight=Screen.height;
            if(lastWidth==0 || lastHeight==0)return;
            safe.anchorMin=new Vector2(area.xMin/lastWidth,area.yMin/lastHeight);
            safe.anchorMax=new Vector2(area.xMax/lastWidth,area.yMax/lastHeight);safe.offsetMin=safe.offsetMax=Vector2.zero;
        }
        void Update()
        {
            if(stampLabel!=null)stampLabel.text=game.State.stamps+" "+game.L("pul","stamps");
            if(Screen.safeArea!=lastSafe || Screen.width!=lastWidth || Screen.height!=lastHeight){UpdateSafeArea();Rebuild();}
            if(hint!=null && Time.unscaledTime>hintUntil){hint.gameObject.SetActive(false);hintPanel.enabled=false;}
        }
    }
}
