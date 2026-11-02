using System;
using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using RainGarden;

public static class GardenChecks
{
    [Serializable] sealed class Report {public string status;public int checks;public string[] failures;}
    public static void Run()
    {
        GardenBuilder.Setup();
        var failures=new List<string>();int checks=0;
        Action<bool,string> check=(condition,name)=>{checks++;if(!condition)failures.Add(name);};
        var state=new GardenState();
        check(!GardenRules.Sun(state,0),"Sun does not skip watering");
        check(GardenRules.Rain(state,0) && state.plants[0]==1,"Rain germinates a seed");
        check(GardenRules.Sun(state,0) && state.plants[0]==2,"Sun blooms a watered plant");
        check(!GardenRules.Rain(state,0) && state.plants[0]==2,"Repeated rain is safe");
        check(!GardenRules.Rain(state,-1) && !GardenRules.Sun(state,6),"Invalid plot indices are rejected");
        check(GardenRules.Picnic(state,1)==0 && GardenRules.Picnic(state,2)==1,"Picnic remembers distinct fruits");
        check(GardenRules.Picnic(state,9)==-1,"Invalid fruits are rejected");
        string directory=Path.Combine(Application.temporaryCachePath,"RainGardenChecks-"+Guid.NewGuid().ToString("N"));
        var save=new GardenSave(Path.Combine(directory,"test.json"));
        check(save.Write(state),"Save creates directory and primary");
        state.boatLaunched=true;state.language="en";state.decorations[1]=new Vector2(2,-1);save.Write(state);
        var restored=save.Load();
        check(restored.boatLaunched && restored.language=="en" && restored.plants[0]==2,"State roundtrip");
        check(restored.decorations[1]==new Vector2(2,-1),"Decoration roundtrip");
        File.WriteAllText(save.Path,"{broken");
        var recovery=new GardenSave(save.Path);var recovered=recovery.Load();
        check(recovery.Recovered && recovered.plants[0]==2,"Corrupt primary restores previous valid backup");
        check(recovery.Write(recovered) && new GardenSave(save.Path).Load().plants[0]==2,"Recovery can save again");
        File.WriteAllText(save.Path,"{\"version\":99}");File.WriteAllText(save.Path+".bak","bad");
        var future=new GardenSave(save.Path);future.Load();
        check(future.ReadOnly && !future.Write(new GardenState()) && File.ReadAllText(save.Path)=="{\"version\":99}","Future saves remain untouched and read only");
        File.WriteAllText(save.Path,"bad");
        check(new GardenSave(save.Path).Load().version==2,"Damaged saves fall back without crashing");
        ExpansionChecks(check,directory);
        var malformed=new GardenState{plants=new[]{100,-1,5},picnic=null,decorations=null,language="bad"};malformed.Validate();
        check(malformed.plants[0]==2 && malformed.plants[1]==0 && malformed.picnic.Length==4 && malformed.language=="tr","State validation bounds data");
        EditorSceneManager.OpenScene(GardenBuilder.ScenePath);
        var game=UnityEngine.Object.FindFirstObjectByType<GardenGame>();
        check(game!=null && game.entities.Length==16,"Scene contains all 16 interactive entities");
        var ids=new HashSet<string>();
        foreach(var entity in game.entities)
        {
            check(entity!=null && entity.visual!=null && entity.GetComponent<Collider2D>()!=null,"Entity component integrity");
            check(ids.Add(entity.kind+"/"+entity.index),"Unique entity identity");
        }
        check(game.art.friends.Length==16 && game.art.props.Length==12,"Sprite atlas counts");
        check(game.art.expansion.Length==16 && game.art.sumi.Length==4,"Expansion and Sumi sprite counts");
        check(game.art.font!=null && game.art.notes.Length==5 && game.art.ambience!=null,"Font and audio references");
        check(!PlayerSettings.Android.forceInternetPermission && !PlayerSettings.Android.forceSDCardPermission,"No forced network or storage permission");
        string output=Path.Combine(Directory.GetParent(Application.dataPath).Parent.FullName,"Logs","editor-tests.json");
        File.WriteAllText(output,JsonUtility.ToJson(new Report{status=failures.Count==0?"passed":"failed",checks=checks,failures=failures.ToArray()},true));
        Debug.Log("RAIN_GARDEN_CHECKS: "+checks+" checks, "+failures.Count+" failures");
        if(failures.Count>0)throw new Exception(string.Join("; ",failures));
    }
    static void ExpansionChecks(Action<bool,string> check,string directory)
    {
        const string legacy="{\"version\":1,\"plants\":[2,1,0],\"colors\":[1,0,1],\"boatLaunched\":true,\"picnic\":[1,2,0,0],\"decorations\":[{\"x\":-2,\"y\":1},{\"x\":3,\"y\":-2},{\"x\":0,\"y\":1}],\"language\":\"tr\",\"sound\":false,\"reducedMotion\":true,\"visited\":true}";
        var save=new GardenSave(Path.Combine(directory,"migration.json"));File.WriteAllText(save.Path,legacy);var s=save.Load();
        check(s.version==2 && s.plants.Length==6 && s.plants[0]==2 && s.plants[1]==1,"Legacy growth migrates to six plots");
        check(s.flowerIds[0]=="daisy" && s.flowerIds[1]=="cosmos" && s.colors[0]==0,"Legacy flower colors preserve species");
        check(s.boatLaunched && s.picnic[1]==2 && !s.sound && s.reducedMotion && s.visited,"Legacy play and preferences survive");
        check(s.decorations.Length==12 && s.decorations[1]==new Vector2(3,-2) && s.placed[0] && !s.placed[3],"Legacy decoration positions survive");
        check(File.ReadAllText(save.Path+".v1-preserved")==legacy,"Exact original save preserved before migration");
        check(save.Write(s) && File.ReadAllText(save.Path+".v1-preserved")==legacy && save.Load().version==2,"Writing new state preserves original snapshot");
        check(!GardenCatalog.Purchase(s,"bluebell") && s.stamps==0,"Insufficient balance cannot unlock an item");
        s.stamps=3;
        check(GardenCatalog.Purchase(s,"bluebell") && s.stamps==2 && GardenCatalog.Owns(s,"bluebell"),"Purchase charges catalog cost and grants ownership");
        check(!GardenCatalog.Purchase(s,"bluebell") && s.stamps==2,"Owned items cannot be charged twice");
        check(!GardenCatalog.Purchase(s,"invalid") && s.stamps==2,"Unknown catalog item cannot be purchased");
        var clone=s.Clone();clone.stamps=0;clone.plants[0]=0;
        check(s.stamps==2 && s.plants[0]==2,"Transaction clone does not mutate original state");
        var fresh=new GardenState().Clone();
        for(int kind=0;kind<3;kind++)
        {
            fresh=fresh.Clone();check(GardenActivities.Begin(fresh,kind)&&fresh.activity.Valid(),"Fresh cloned state can begin activity "+kind);
            save.Write(fresh);fresh=save.Load();check(GardenActivities.Help(fresh.activity),"First input works after fresh activity reload "+kind);
        }
        int completions=0;
        for(int kind=0;kind<3;kind++)for(int variant=0;variant<3;variant++)
        {
            s.activeActivity=kind;s.activity=GardenActivities.Create(kind,variant);string id=s.activity.id;
            check(!GardenActivities.Claim(s,id),"Incomplete activity has no reward "+kind+"/"+variant);
            if(kind==0)check(!GardenActivities.PickFruit(s.activity,9)&&s.activity.step==0,"Wrong fruit has no penalty or progress");
            if(kind==1)check(!GardenActivities.Rotate(s.activity,-1),"Invalid channel rejected");
            if(kind==2)check(!GardenActivities.Sail(s.activity,3),"Boat cannot leave grid");
            check(GardenActivities.Help(s.activity),"First help step succeeds "+kind+"/"+variant);
            string partial=JsonUtility.ToJson(s.activity);save.Write(s);s=save.Load();
            check(JsonUtility.ToJson(s.activity)==partial,"Partial activity roundtrips "+kind+"/"+variant);
            int guard=0;while(!GardenActivities.Complete(s.activity)&&guard++<80)GardenActivities.Help(s.activity);
            check(GardenActivities.Complete(s.activity),"Layout can be completed "+kind+"/"+variant);
            int balance=s.stamps;
            check(!GardenActivities.Claim(s,"wrong-id"),"Wrong run cannot claim reward");
            check(GardenActivities.Claim(s,id) && s.stamps==balance+1,"Completion grants one stamp "+kind+"/"+variant);
            save.Write(s);s=save.Load();
            check(!GardenActivities.Claim(s,id)&&s.stamps==balance+1,"Reward cannot repeat after reload "+kind+"/"+variant);
            completions++;
        }
        check(s.activityCompletions==completions && Array.TrueForAll(s.activityPlays,n=>n==3),"All activity completion counters persist");
        check(s.discoveries[4] && s.discoveries[7] && s.discoveries[9] && s.discoveries[11],"Activity discoveries unlock once");
        s.activeActivity=0;s.activity=GardenActivities.Create(0,2);GardenActivities.Help(s.activity);string picnicId=s.activity.id;
        s.activeActivity=1;s.activity=GardenActivities.Create(1,1);GardenActivities.Help(s.activity);save.Write(s);s=save.Load();
        check(s.activities[0].id==picnicId && s.activities[0].step==1 && s.activities[1].kind==1,"Switching activities preserves each unfinished run");
        s.discoveries[10]=true;check(GardenProjects.Sumi(s),"Three flower types and all games welcome Sumi");
        s.discoveries[10]=false;check(!GardenProjects.Sumi(s),"Sumi waits for all three discoveries");
        for(int project=0;project<4;project++)
        {for(int i=0;i<3;i++)s.discoveries[project*3+i]=true;check(GardenProjects.Progress(s,project)==3 && GardenProjects.Finished(s,project),"Project completes three steps "+project);}
    }
}
