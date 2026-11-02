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
        check(!GardenRules.Rain(state,-1) && !GardenRules.Sun(state,3),"Invalid plot indices are rejected");
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
        check(new GardenSave(save.Path).Load().version==1,"Unknown or damaged saves fall back without crashing");
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
        check(game.art.font!=null && game.art.notes.Length==5 && game.art.ambience!=null,"Font and audio references");
        check(!PlayerSettings.Android.forceInternetPermission && !PlayerSettings.Android.forceSDCardPermission,"No forced network or storage permission");
        string output=Path.Combine(Directory.GetParent(Application.dataPath).Parent.FullName,"Logs","editor-tests.json");
        File.WriteAllText(output,JsonUtility.ToJson(new Report{status=failures.Count==0?"passed":"failed",checks=checks,failures=failures.ToArray()},true));
        Debug.Log("RAIN_GARDEN_CHECKS: "+checks+" checks, "+failures.Count+" failures");
        if(failures.Count>0)throw new Exception(string.Join("; ",failures));
    }
}
