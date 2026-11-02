using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using RainGarden;

public static class GardenBuilder
{
    public const string Root="Assets/RainGarden";
    public const string ScenePath=Root+"/Scenes/Garden.unity";
    static string Workspace => Directory.GetParent(Application.dataPath).Parent.FullName;
    [MenuItem("Rain Garden/Prepare project and scene")]
    public static void Setup()
    {
        Directory.CreateDirectory(Root+"/Generated");Directory.CreateDirectory(Root+"/Scenes");AssetDatabase.Refresh();
        var art=BuildArt();Settings();
        if(!File.Exists(ScenePath))BuildScene(art);
        else RefreshSceneArt(art);
        EditorBuildSettings.scenes=new[]{new EditorBuildSettingsScene(ScenePath,true)};
        AssetDatabase.SaveAssets();
        Debug.Log("RAIN_GARDEN_SETUP_OK");
    }
    static void Settings()
    {
        PlayerSettings.companyName="Rain Garden Development";
        PlayerSettings.productName="Yagmur Bahcesi";
        PlayerSettings.bundleVersion="0.2.0";
        PlayerSettings.SetApplicationIdentifier(NamedBuildTarget.Android,"com.raingarden.yagmurbahcesi.dev");
        PlayerSettings.defaultInterfaceOrientation=UIOrientation.AutoRotation;
        PlayerSettings.allowedAutorotateToPortrait=false;PlayerSettings.allowedAutorotateToPortraitUpsideDown=false;
        PlayerSettings.allowedAutorotateToLandscapeLeft=true;PlayerSettings.allowedAutorotateToLandscapeRight=true;
        PlayerSettings.defaultScreenWidth=1600;PlayerSettings.defaultScreenHeight=900;PlayerSettings.fullScreenMode=FullScreenMode.Windowed;
        PlayerSettings.resizableWindow=true;PlayerSettings.runInBackground=false;
        PlayerSettings.colorSpace=ColorSpace.Gamma;
        PlayerSettings.SplashScreen.show=false;
        var appIcon=AssetDatabase.LoadAssetAtPath<Texture2D>(Root+"/Art/AppIcon.png");
        if(appIcon!=null)PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown,new[]{appIcon});
        PlayerSettings.Android.minSdkVersion=AndroidSdkVersions.AndroidApiLevel26;
        PlayerSettings.Android.targetSdkVersion=(AndroidSdkVersions)36;
        PlayerSettings.Android.targetArchitectures=AndroidArchitecture.ARM64;
        PlayerSettings.Android.bundleVersionCode=2;
        PlayerSettings.Android.forceInternetPermission=false;
        PlayerSettings.Android.forceSDCardPermission=false;
        PlayerSettings.SetScriptingBackend(NamedBuildTarget.Android,ScriptingImplementation.IL2CPP);
        PlayerSettings.SetManagedStrippingLevel(NamedBuildTarget.Android,ManagedStrippingLevel.Low);
        PlayerSettings.stripEngineCode=true;
        QualitySettings.vSyncCount=0;QualitySettings.antiAliasing=0;QualitySettings.shadows=ShadowQuality.Disable;
        Physics2D.gravity=Vector2.zero;
        var settings=new SerializedObject(AssetDatabase.LoadAllAssetsAtPath("ProjectSettings/ProjectSettings.asset")[0]);
        var input=settings.FindProperty("activeInputHandler");if(input!=null)input.intValue=0;
        settings.ApplyModifiedPropertiesWithoutUndo();
    }
    static Texture2D Texture(string name)
    {
        string path=Root+"/Art/"+name+".png";
        var importer=(TextureImporter)AssetImporter.GetAtPath(path);
        importer.textureType=TextureImporterType.Default;importer.isReadable=true;importer.alphaIsTransparency=name!="Garden";
        importer.mipmapEnabled=false;importer.npotScale=TextureImporterNPOTScale.None;importer.maxTextureSize=2048;
        importer.textureCompression=TextureImporterCompression.Uncompressed;importer.filterMode=FilterMode.Bilinear;
        importer.wrapMode=TextureWrapMode.Clamp;importer.SaveAndReimport();
        return AssetDatabase.LoadAssetAtPath<Texture2D>(path);
    }
    static Sprite SpriteAsset(Texture2D texture,Rect rect,string name,Vector2 pivot,GardenArt art,bool trim=true)
    {
        if(trim)
        {
            var pixels=texture.GetPixels32();int minX=(int)rect.xMax,minY=(int)rect.yMax,maxX=(int)rect.xMin,maxY=(int)rect.yMin;
            for(int y=(int)rect.yMin;y<(int)rect.yMax;y++)for(int x=(int)rect.xMin;x<(int)rect.xMax;x++)
                if(pixels[y*texture.width+x].a>40){minX=Math.Min(minX,x);maxX=Math.Max(maxX,x);minY=Math.Min(minY,y);maxY=Math.Max(maxY,y);}
            if(maxX>minX && maxY>minY)rect=new Rect(minX,minY,maxX-minX+1,maxY-minY+1);
        }
        var s=Sprite.Create(texture,rect,pivot,100,0,SpriteMeshType.FullRect);s.name=name;AssetDatabase.AddObjectToAsset(s,art);return s;
    }
    static GardenArt BuildArt()
    {
        string path=Root+"/Generated/GardenArt.asset";
        var art=AssetDatabase.LoadAssetAtPath<GardenArt>(path);
        if(art!=null)
        {
            if(art.revision<2){RebuildProps(art,Texture("Objects"));art.revision=2;EditorUtility.SetDirty(art);}
            if(art.revision<3)BuildExpansionArt(art);
            return art;
        }
        art=ScriptableObject.CreateInstance<GardenArt>();AssetDatabase.CreateAsset(art,path);
        var background=Texture("Garden");var friends=Texture("Friends");var props=Texture("Objects");
        art.background=SpriteAsset(background,new Rect(0,0,background.width,background.height),"Garden background",new Vector2(.5f,.5f),art,false);
        art.friends=new Sprite[16];int[] rowEdges={0,365,660,948,1254};
        for(int row=0;row<4;row++)for(int col=0;col<4;col++)
        {
            int left=Mathf.RoundToInt(col*friends.width/4f),right=Mathf.RoundToInt((col+1)*friends.width/4f);
            int top=Mathf.RoundToInt(rowEdges[row]/1254f*friends.height),bottom=Mathf.RoundToInt(rowEdges[row+1]/1254f*friends.height);
            art.friends[row*4+col]=SpriteAsset(friends,new Rect(left,friends.height-bottom,right-left,bottom-top),"Friend_"+row+"_"+col,new Vector2(.5f,0),art);
        }
        RebuildProps(art,props);art.revision=2;
        art.font=AssetDatabase.LoadAssetAtPath<Font>(Root+"/Fonts/Nunito.ttf");
        art.roundPanel=Primitive("Round panel",128,(x,y)=>
        {float qx=Mathf.Max(Mathf.Abs(x-63.5f)-39.5f,0),qy=Mathf.Max(Mathf.Abs(y-63.5f)-39.5f,0);return Mathf.Clamp01(24-Mathf.Sqrt(qx*qx+qy*qy));},art,new Vector4(26,26,26,26));
        art.circle=Primitive("Circle",64,(x,y)=>Mathf.Clamp01(30.5f-Vector2.Distance(new Vector2(x,y),new Vector2(31.5f,31.5f))),art);
        art.triangle=Primitive("Play triangle",64,(x,y)=>x>10 && x<54 && Mathf.Abs(y-31.5f)<(54-x)*.58f?1:0,art);
        art.drop=Primitive("Raindrop",64,(x,y)=>
        {float dx=(x-31.5f)/18f,dy=(y-23)/20f;return (dx*dx+dy*dy<1 || (y>=23 && y<59 && Mathf.Abs(x-31.5f)<(59-y)*.49f))?1:0;},art);
        BuildAudio(art);BuildExpansionArt(art);EditorUtility.SetDirty(art);AssetDatabase.SaveAssets();return art;
    }
    static void BuildExpansionArt(GardenArt art)
    {
        art.expansion=GridSprites(Texture("Expansion"),4,4,"Expansion",art);
        art.sumi=GridSprites(Texture("Sumi"),2,2,"Sumi",art);
        art.revision=3;EditorUtility.SetDirty(art);AssetDatabase.SaveAssets();
    }
    static Sprite[] GridSprites(Texture2D texture,int cols,int rows,string prefix,GardenArt art)
    {
        var pixels=texture.GetPixels32();
        Func<bool,int,int[]> seams=(horizontal,count)=>
        {
            int length=horizontal?texture.width:texture.height,cross=horizontal?texture.height:texture.width;
            var edges=new int[count+1];edges[count]=length;
            for(int i=1;i<count;i++)
            {
                int expected=i*length/count,best=expected,bestScore=int.MaxValue,range=length/(count*5);
                for(int pos=expected-range;pos<=expected+range;pos++)
                {
                    int opaque=0;for(int j=0;j<cross;j++){int index=horizontal?j*texture.width+pos:pos*texture.width+j;if(pixels[index].a>80)opaque++;}
                    int score=opaque*1000+Math.Abs(pos-expected);if(score<bestScore){best=pos;bestScore=score;}
                }
                edges[i]=best;
            }
            return edges;
        };
        int[] xs=seams(true,cols),ys=seams(false,rows);var result=new Sprite[cols*rows];
        for(int row=0;row<rows;row++)for(int col=0;col<cols;col++)
        {
            int lower=rows-row-1;
            result[row*cols+col]=SpriteAsset(texture,new Rect(xs[col],ys[lower],xs[col+1]-xs[col],ys[lower+1]-ys[lower]),prefix+"_"+(row*cols+col),new Vector2(.5f,0),art);
        }
        return result;
    }
    static void RebuildProps(GardenArt art,Texture2D props)
    {
        var prior=art.props;art.props=new Sprite[12];
        int[] xs={0,390,730,1086},ys={0,345,727,1050,1448};
        for(int row=0;row<4;row++)for(int col=0;col<3;col++)
        {
            int left=Mathf.RoundToInt(xs[col]/1086f*props.width),right=Mathf.RoundToInt(xs[col+1]/1086f*props.width);
            int top=Mathf.RoundToInt(ys[row]/1448f*props.height),bottom=Mathf.RoundToInt(ys[row+1]/1448f*props.height);
            art.props[row*3+col]=SpriteAsset(props,new Rect(left,props.height-bottom,right-left,bottom-top),"Object_"+(row*3+col),new Vector2(.5f,0),art);
        }
        if(prior!=null)foreach(var old in prior)if(old!=null)UnityEngine.Object.DestroyImmediate(old,true);
    }
    static void RefreshSceneArt(GardenArt art)
    {
        var scene=EditorSceneManager.OpenScene(ScenePath);var game=UnityEngine.Object.FindFirstObjectByType<GardenGame>();
        foreach(var e in game.entities)
        {
            int index=e.kind==EntityKind.Plot?3:e.kind==EntityKind.Cloud?0:e.kind==EntityKind.Boat?6:e.kind==EntityKind.Fruit?7+e.index:e.kind==EntityKind.Decoration?9+e.index:-1;
            if(index>=0)e.visual.sprite=art.Prop(index);
        }
        EditorSceneManager.SaveScene(scene);
    }
    static Sprite Primitive(string name,int size,Func<float,float,float> alpha,GardenArt art,Vector4 border=default)
    {
        var tex=new Texture2D(size,size,TextureFormat.RGBA32,false);tex.name=name+" texture";
        var colors=new Color[size*size];for(int y=0;y<size;y++)for(int x=0;x<size;x++)colors[y*size+x]=new Color(1,1,1,alpha(x,y));
        tex.SetPixels(colors);tex.Apply();tex.filterMode=FilterMode.Bilinear;tex.wrapMode=TextureWrapMode.Clamp;AssetDatabase.AddObjectToAsset(tex,art);
        var sprite=Sprite.Create(tex,new Rect(0,0,size,size),new Vector2(.5f,.5f),100,0,SpriteMeshType.FullRect,border);sprite.name=name;AssetDatabase.AddObjectToAsset(sprite,art);return sprite;
    }
    static void BuildAudio(GardenArt art)
    {
        art.notes=new AudioClip[5];float[] frequencies={261.63f,329.63f,392f,440f,523.25f};
        for(int n=0;n<5;n++)
        {
            float frequency=frequencies[n];int note=n;
            art.notes[n]=Wave("note-"+note,1,t=>(Mathf.Sin(2*Mathf.PI*frequency*t)+.2f*Mathf.Sin(2*Mathf.PI*frequency*2*t))*Mathf.Exp(-5*t)*Mathf.Min(1,t*80)*.32f);
        }
        var random=new System.Random(128);
        art.rain=Wave("rain",1.6f,t=>(float)(random.NextDouble()*2-1)*.1f*Mathf.Sin(Mathf.PI*t/1.6f));
        art.ambience=Wave("garden-air",12,t=>
        {
            float v=.035f*Mathf.Sin(2*Mathf.PI*130.8128f*t)+.018f*Mathf.Sin(2*Mathf.PI*196*t);
            float envelope=Mathf.Sin(Mathf.PI*t/12);return v*envelope*envelope;
        });
    }
    static AudioClip Wave(string name,float seconds,Func<float,float> sample)
    {
        string path=Root+"/Generated/"+name+".wav";int rate=22050,count=(int)(rate*seconds);
        using(var stream=File.Create(path))using(var w=new BinaryWriter(stream))
        {
            w.Write(System.Text.Encoding.ASCII.GetBytes("RIFF"));w.Write(36+count*2);w.Write(System.Text.Encoding.ASCII.GetBytes("WAVEfmt "));w.Write(16);w.Write((short)1);w.Write((short)1);w.Write(rate);w.Write(rate*2);w.Write((short)2);w.Write((short)16);w.Write(System.Text.Encoding.ASCII.GetBytes("data"));w.Write(count*2);
            for(int i=0;i<count;i++)w.Write((short)(Mathf.Clamp(sample(i/(float)rate),-1,1)*32767));
        }
        AssetDatabase.ImportAsset(path);var importer=(AudioImporter)AssetImporter.GetAtPath(path);var config=importer.defaultSampleSettings;config.loadType=AudioClipLoadType.DecompressOnLoad;config.compressionFormat=AudioCompressionFormat.PCM;importer.defaultSampleSettings=config;importer.SaveAndReimport();return AssetDatabase.LoadAssetAtPath<AudioClip>(path);
    }
    static void BuildScene(GardenArt art)
    {
        var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);
        var camObj=new GameObject("Garden Camera",typeof(Camera),typeof(AudioListener),typeof(GardenCamera));camObj.tag="MainCamera";camObj.transform.position=new Vector3(0,0,-10);
        var camera=camObj.GetComponent<Camera>();camera.orthographic=true;camera.orthographicSize=5.4f;camera.backgroundColor=new Color(.92f,.94f,.84f);camera.clearFlags=CameraClearFlags.SolidColor;
        var root=new GameObject("Rain Garden");var game=root.AddComponent<GardenGame>();game.art=art;
        var background=new GameObject("Painted garden",typeof(SpriteRenderer));background.transform.SetParent(root.transform);background.GetComponent<SpriteRenderer>().sprite=art.background;background.GetComponent<SpriteRenderer>().sortingOrder=-100;background.transform.localScale=new Vector3(19.2f/art.background.bounds.size.x,10.8f/art.background.bounds.size.y,1);
        var list=new List<GardenEntity>();
        Vector2[] plots={new Vector2(-5.8f,.85f),new Vector2(-7.3f,-.15f),new Vector2(-5.8f,-1.5f)};
        for(int i=0;i<3;i++)list.Add(Entity(root,"Garden bed "+i,EntityKind.Plot,i,art.Prop(3),plots[i],.6f,false,new Vector2(3.5f,2.8f)));
        Vector2[] positions={new Vector2(-3.3f,-.5f),new Vector2(-2.5f,-2.7f),new Vector2(6.4f,-1),new Vector2(2,-2.5f)};
        string[] names={"Mina - rabbit","Pufi - hedgehog","Luli - duck","Kori - squirrel"};
        for(int i=0;i<4;i++)list.Add(Entity(root,names[i],EntityKind.Friend,i,art.Friend(i),positions[i],i==0?.65f:.62f,true,new Vector2(2.6f,3.4f)));
        list.Add(Entity(root,"Rain cloud",EntityKind.Cloud,0,art.Prop(0),new Vector2(-4.8f,2.4f),.65f,true,new Vector2(3.3f,2.6f)));
        list.Add(Entity(root,"Leaf boat",EntityKind.Boat,0,art.Prop(6),GardenGame.BoatStart,.4f,true,new Vector2(3.5f,2.5f)));
        list.Add(Entity(root,"Apple",EntityKind.Fruit,0,art.Prop(7),new Vector2(2.4f,-3.15f),.3f,true,new Vector2(3.7f,3.7f)));
        list.Add(Entity(root,"Pear",EntityKind.Fruit,1,art.Prop(8),new Vector2(3.55f,-3.1f),.3f,true,new Vector2(3.7f,3.7f)));
        var defaults=new GardenState();for(int i=0;i<3;i++)list.Add(Entity(root,"Decoration "+i,EntityKind.Decoration,i,art.Prop(9+i),defaults.decorations[i],.3f,true,new Vector2(3.3f,3.3f)));
        var pond=Entity(root,"Pond touch area",EntityKind.Pond,0,art.circle,GardenGame.PondCenter,1,false,new Vector2(4.6f,2.4f));pond.visual.color=Color.clear;list.Add(pond);
        var basket=Entity(root,"Picnic blanket",EntityKind.Basket,0,art.circle,GardenGame.PicnicCenter,1,false,new Vector2(3.8f,1.6f));basket.visual.color=Color.clear;basket.visual.sortingOrder=-40;list.Add(basket);
        game.entities=list.ToArray();
        foreach(var e in list)e.visual.sortingOrder=game.BaseOrder(e);
        EditorSceneManager.SaveScene(scene,ScenePath);
    }
    static GardenEntity Entity(GameObject root,string name,EntityKind kind,int index,Sprite sprite,Vector2 position,float scale,bool draggable,Vector2 hitSize)
    {
        var go=new GameObject(name);go.transform.SetParent(root.transform);go.transform.position=position;go.transform.localScale=Vector3.one*scale;
        var sr=go.AddComponent<SpriteRenderer>();sr.sprite=sprite;
        var collider=go.AddComponent<BoxCollider2D>();collider.size=hitSize;collider.offset=new Vector2(0,kind==EntityKind.Pond||kind==EntityKind.Basket?0:hitSize.y*.4f);
        var e=go.AddComponent<GardenEntity>();e.kind=kind;e.index=index;e.draggable=draggable;e.visual=sr;return e;
    }
    [MenuItem("Rain Garden/Build Windows preview")]
    public static void BuildWindows()
    {
        Setup();Build(BuildTarget.StandaloneWindows64,Workspace+"/Builds/Windows/YagmurBahcesi.exe",BuildOptions.None);
    }
    [MenuItem("Rain Garden/Build Android test APK")]
    public static void BuildAndroid()
    {
        Setup();EditorUserBuildSettings.buildAppBundle=false;Build(BuildTarget.Android,Workspace+"/Builds/Android/YagmurBahcesi-test.apk",BuildOptions.None);
    }
    [MenuItem("Rain Garden/Build Android emulator APK")]
    public static void BuildEmulator()
    {
        Setup();EditorUserBuildSettings.buildAppBundle=false;
        PlayerSettings.Android.targetArchitectures=AndroidArchitecture.X86_64;
        try { Build(BuildTarget.Android,Workspace+"/Builds/Android/YagmurBahcesi-emulator.apk",BuildOptions.Development); }
        finally { PlayerSettings.Android.targetArchitectures=AndroidArchitecture.ARM64;AssetDatabase.SaveAssets(); }
    }
    static void Build(BuildTarget target,string path,BuildOptions options)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(path));
        var report=BuildPipeline.BuildPlayer(new BuildPlayerOptions{scenes=new[]{ScenePath},locationPathName=path,target=target,options=options});
        var summary=report.summary;
        string message="Build "+target+": "+summary.result+", errors="+summary.totalErrors+", size="+summary.totalSize;
        Directory.CreateDirectory(Workspace+"/Logs");File.WriteAllText(Workspace+"/Logs/build-"+target+".txt",message);
        Debug.Log(message);if(summary.result!=BuildResult.Succeeded)throw new Exception(message);
    }
}
