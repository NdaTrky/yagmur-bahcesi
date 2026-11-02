using System;
using System.IO;
using UnityEngine;

namespace RainGarden
{
    [Serializable] public sealed class GardenState
    {
        public int version = 1;
        public int[] plants = new int[3];
        public int[] colors = new int[3];
        public bool boatLaunched;
        public int[] picnic = new int[4];
        public Vector2[] decorations = { new Vector2(-1.6f, -.9f), new Vector2(6.4f, -2.8f), new Vector2(-3.7f, -2.9f) };
        public string language = "tr";
        public bool sound = true;
        public bool reducedMotion;
        public bool visited;
        public void Validate()
        {
            if (version != 1) throw new InvalidDataException("Unsupported save version");
            if (plants == null || plants.Length != 3) plants = new int[3];
            if (colors == null || colors.Length != 3) colors = new int[3];
            if (picnic == null || picnic.Length != 4) picnic = new int[4];
            if (decorations == null || decorations.Length != 3) decorations = new GardenState().decorations;
            for (int i=0;i<3;i++)
            {
                plants[i] = Mathf.Clamp(plants[i], 0, 2); colors[i] = Mathf.Clamp(colors[i], 0, 1);
                var p = decorations[i];
                if (float.IsNaN(p.x) || float.IsInfinity(p.x) || float.IsNaN(p.y) || float.IsInfinity(p.y)) p = new GardenState().decorations[i];
                decorations[i] = new Vector2(Mathf.Clamp(p.x,-7.8f,7.8f),Mathf.Clamp(p.y,-3.7f,1.4f));
            }
            for(int i=0;i<4;i++) picnic[i]=Mathf.Clamp(picnic[i],0,2);
            if(language!="tr" && language!="en") language="tr";
        }
    }
    public static class GardenRules
    {
        public static bool Rain(GardenState state,int plot)
        {
            if(plot<0 || plot>=3) return false;
            int before=state.plants[plot]; state.plants[plot]=Mathf.Min(2,before+1); return state.plants[plot]!=before;
        }
        public static bool Sun(GardenState state,int plot)
        {
            if(plot<0 || plot>=3 || state.plants[plot]!=1) return false;
            state.plants[plot]=2; return true;
        }
        public static int Picnic(GardenState state,int fruit)
        {
            if(fruit<1 || fruit>2) return -1;
            int slot=Array.IndexOf(state.picnic,0); if(slot<0) slot=0;
            state.picnic[slot]=fruit; return slot;
        }
    }
    public sealed class GardenSave
    {
        public readonly string Path;
        public bool Recovered {get;private set;}
        public string LastError {get;private set;}
        public GardenSave(string path) { Path=path; }
        public GardenState Load()
        {
            foreach(var candidate in new[]{Path,Path+".bak"})
            {
                if(!File.Exists(candidate)) continue;
                try
                {
                    string json=File.ReadAllText(candidate);
                    if(!json.TrimStart().StartsWith("{") || !json.Contains("\"version\"")) throw new InvalidDataException("Invalid save envelope");
                    var result=JsonUtility.FromJson<GardenState>(json);
                    if(result==null) throw new InvalidDataException("Empty save");
                    result.Validate(); Recovered=candidate!=Path; return result;
                }
                catch(Exception e) { LastError=e.GetType().Name; Recovered=true; }
            }
            return new GardenState { language=Application.systemLanguage==SystemLanguage.Turkish?"tr":"en" };
        }
        public bool Write(GardenState state)
        {
            try
            {
                state.Validate(); Directory.CreateDirectory(System.IO.Path.GetDirectoryName(Path));
                string temp=Path+".tmp"; File.WriteAllText(temp,JsonUtility.ToJson(state,true));
                if(File.Exists(Path))
                {
                    bool valid=false;
                    try { string old=File.ReadAllText(Path); var prior=JsonUtility.FromJson<GardenState>(old); if(old.Contains("\"version\"") && prior!=null) {prior.Validate();valid=true;} } catch {}
                    if(valid) File.Copy(Path,Path+".bak",true);
                }
                if(File.Exists(Path))
                {
                    try { File.Replace(temp,Path,null); }
                    catch(PlatformNotSupportedException) { File.Copy(temp,Path,true);File.Delete(temp); }
                    catch(IOException) { File.Copy(temp,Path,true);File.Delete(temp); }
                }
                else File.Move(temp,Path);
                LastError=null; return true;
            }
            catch(Exception e) { LastError=e.GetType().Name; Debug.LogWarning("Garden save failed: "+LastError); return false; }
        }
    }
}
