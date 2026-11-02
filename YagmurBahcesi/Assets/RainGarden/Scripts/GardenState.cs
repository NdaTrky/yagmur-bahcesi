using System;
using System.IO;
using System.Linq;
using UnityEngine;
namespace RainGarden
{
    [Serializable] public sealed class GardenState
    {
        public int version=2;
        public int[] plants=new int[6],colors=new int[6];
        public string[] flowerIds={"cosmos","cosmos","cosmos","daisy","cosmos","daisy"};
        public bool boatLaunched;
        public int[] picnic=new int[4];
        public Vector2[] decorations=(Vector2[])GardenCatalog.DecorPositions.Clone();
        public bool[] placed={true,true,true,false,false,false,false,false,false,false,false,false};
        public string[] unlocked={"cosmos","daisy","pinwheel","stones","mushroom"};
        public int stamps;
        public bool[] discoveries=new bool[12];
        public string[] plantedSpecies=new string[0];
        public int activityCompletions;
        public int[] activityPlays=new int[3];
        public GardenActivityState[] activities=new GardenActivityState[3];
        public int activeActivity;
        public GardenActivityState activity {get=>activities[activeActivity];set=>activities[activeActivity]=value;}
        public string language="tr";
        public bool sound=true,reducedMotion,visited;
        public GardenState Clone(){var copy=JsonUtility.FromJson<GardenState>(JsonUtility.ToJson(this));copy.Validate();return copy;}
        static T[] Resize<T>(T[] values,int length,T[] defaults=null)
        {var result=defaults==null?new T[length]:(T[])defaults.Clone();if(values!=null)Array.Copy(values,result,Math.Min(length,values.Length));return result;}
        public void Validate()
        {
            if(version<1||version>2)throw new InvalidDataException("Unsupported save version");
            bool legacy=version==1;
            plants=Resize(plants,6);colors=Resize(colors,6);
            flowerIds=Resize(flowerIds,6,new[]{"cosmos","cosmos","cosmos","daisy","cosmos","daisy"});
            if(legacy)for(int i=0;i<3;i++){flowerIds[i]=colors[i]==1?"daisy":"cosmos";colors[i]=0;}
            picnic=Resize(picnic,4);decorations=Resize(decorations,12,GardenCatalog.DecorPositions);
            placed=Resize(placed,12,new[]{true,true,true,false,false,false,false,false,false,false,false,false});
            discoveries=Resize(discoveries,12);activityPlays=Resize(activityPlays,3);
            var owned=(unlocked??new string[0]).Where(x=>GardenCatalog.Find(x)!=null).ToList();
            foreach(string id in new[]{"cosmos","daisy","pinwheel","stones","mushroom"})if(!owned.Contains(id))owned.Add(id);
            unlocked=owned.Distinct().ToArray();stamps=Mathf.Clamp(stamps,0,1000000);activityCompletions=Math.Max(0,activityCompletions);
            plantedSpecies=(plantedSpecies??new string[0]).Where(x=>GardenCatalog.FlowerIndex(x)>=0).Distinct().ToArray();
            for(int i=0;i<6;i++)
            {
                plants[i]=Mathf.Clamp(plants[i],0,2);colors[i]=Mathf.Clamp(colors[i],0,2);
                if(GardenCatalog.FlowerIndex(flowerIds[i])<0||!GardenCatalog.Owns(this,flowerIds[i]))flowerIds[i]="cosmos";
            }
            for(int i=0;i<12;i++)
            {
                var p=decorations[i];if(float.IsNaN(p.x)||float.IsInfinity(p.x)||float.IsNaN(p.y)||float.IsInfinity(p.y))p=GardenCatalog.DecorPositions[i];
                decorations[i]=new Vector2(Mathf.Clamp(p.x,-7.8f,7.8f),Mathf.Clamp(p.y,-3.7f,1.8f));
                if(!GardenCatalog.Owns(this,GardenCatalog.Decorations[i].id))placed[i]=false;
            }
            for(int i=0;i<4;i++)picnic[i]=Mathf.Clamp(picnic[i],0,2);
            for(int i=0;i<3;i++)activityPlays[i]=Mathf.Max(0,activityPlays[i]);
            if(language!="tr"&&language!="en")language="tr";
            activities=Resize(activities,3);activeActivity=Mathf.Clamp(activeActivity,0,2);
            for(int i=0;i<3;i++)if(activities[i]!=null&&(!activities[i].Valid()||activities[i].kind!=i))activities[i]=null;
            version=2;
        }
    }
    public static class GardenRules
    {
        public static bool Rain(GardenState s,int plot)
        {if(plot<0||plot>=s.plants.Length)return false;int before=s.plants[plot];s.plants[plot]=Mathf.Min(2,before+1);return before!=s.plants[plot];}
        public static bool Sun(GardenState s,int plot)
        {if(plot<0||plot>=s.plants.Length||s.plants[plot]!=1)return false;s.plants[plot]=2;return true;}
        public static int Picnic(GardenState s,int fruit)
        {if(fruit<1||fruit>2)return -1;int slot=Array.IndexOf(s.picnic,0);if(slot<0)slot=0;s.picnic[slot]=fruit;return slot;}
    }
    public sealed class GardenSave
    {
        public readonly string Path;
        public bool Recovered {get;private set;}
        public bool ReadOnly {get;private set;}
        public string LastError {get;private set;}
        [Serializable] sealed class Header {public int version;}
        public GardenSave(string path){Path=path;}
        static GardenState Parse(string json)
        {
            if(!json.TrimStart().StartsWith("{")||!json.Contains("\"version\""))throw new InvalidDataException("Invalid save envelope");
            var s=JsonUtility.FromJson<GardenState>(json);if(s==null)throw new InvalidDataException("Empty save");s.Validate();return s;
        }
        public GardenState Load()
        {
            foreach(var candidate in new[]{Path,Path+".bak"})
            {
                if(!File.Exists(candidate))continue;
                try
                {
                    string json=File.ReadAllText(candidate);var header=JsonUtility.FromJson<Header>(json);
                    if(header!=null&&header.version>2){ReadOnly=true;LastError="NewerSaveVersion";return new GardenState();}
                    var s=Parse(json);
                    if(header.version==1&&!File.Exists(Path+".v1-preserved"))File.Copy(candidate,Path+".v1-preserved");
                    Recovered=candidate!=Path;return s;
                }
                catch(Exception e){LastError=e.GetType().Name;Recovered=true;}
            }
            return new GardenState{language=Application.systemLanguage==SystemLanguage.Turkish?"tr":"en"};
        }
        public bool Write(GardenState state)
        {
            if(ReadOnly)return false;
            try
            {
                state.Validate();Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path));
                if(File.Exists(Path))
                {
                    string old=File.ReadAllText(Path);bool valid=false;
                    try
                    {
                        var h=JsonUtility.FromJson<Header>(old);
                        if(h!=null&&h.version>2){ReadOnly=true;LastError="NewerSaveVersion";return false;}
                        Parse(old);valid=true;
                        if(h.version==1&&!File.Exists(Path+".v1-preserved"))File.Copy(Path,Path+".v1-preserved");
                    }
                    catch{}
                    if(valid)File.Copy(Path,Path+".bak",true);
                }
                string temp=Path+".tmp";File.WriteAllText(temp,JsonUtility.ToJson(state,true));
                if(File.Exists(Path))
                {
                    try{File.Replace(temp,Path,null);}
                    catch(PlatformNotSupportedException){File.Copy(temp,Path,true);File.Delete(temp);}
                    catch(IOException){File.Copy(temp,Path,true);File.Delete(temp);}
                }
                else File.Move(temp,Path);
                LastError=null;return true;
            }
            catch(Exception e){LastError=e.GetType().Name;Debug.LogWarning("Garden save failed: "+LastError);return false;}
        }
    }
}
