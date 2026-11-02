using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace RainGarden
{
    public sealed class GardenSmoke : MonoBehaviour
    {
        [Serializable] sealed class Result {public string status;public int checks;public string[] failures;public string screenshot;}
        readonly List<string> failures=new List<string>();int checks;
        void Check(bool condition,string name){checks++;if(!condition){failures.Add(name);Debug.LogError("SMOKE: "+name);}}
        IEnumerator Start()
        {
            var input=FindFirstObjectByType<UnityEngine.EventSystems.StandaloneInputModule>();if(input!=null)input.enabled=false;
            var game=GetComponent<GardenGame>();
            string output=Argument("--garden-output")??Application.persistentDataPath;
            Directory.CreateDirectory(output);
            yield return null;
            yield return new WaitForEndOfFrame();Capture(Path.Combine(output,"home.png"));yield return new WaitForSeconds(1);
            game.StartGarden();yield return null;
            Check(game.Playing,"Start button begins garden");
            game.SelectTool(GardenTool.Rain);
            var plot=game.Find(EntityKind.Plot);
            Vector2 screen=Camera.main.WorldToScreenPoint(plot.rest+Vector3.up*.2f);
            game.PointerDown(screen,-1);game.PointerUp(screen,-1);
            Check(game.State.plants[0]==1,"World pointer waters first plot");
            game.SelectTool(GardenTool.Sun);game.Interact(plot);
            Check(game.State.plants[0]==2,"Sun opens watered flower");
            game.Water(1);game.Water(1);game.Water(2);game.Water(2);
            game.SelectTool(GardenTool.Explore);game.Interact(game.Find(EntityKind.Plot,1));
            var boat=game.Find(EntityKind.Boat);
            Drag(game,boat,GardenGame.PondCenter);game.Wind();
            Check(game.State.boatLaunched && game.WindUntil>Time.time,"Boat and wind chain");
            Drag(game,game.Find(EntityKind.Fruit),GardenGame.PicnicCenter);
            game.ShareFruit(2);game.ShareFruit(1);game.ShareFruit(2);
            Check(game.State.picnic[0]==1 && game.State.picnic[1]==2,"Fruit shared into picnic");
            var decor=game.Find(EntityKind.Decoration);
            Vector2 start=Camera.main.WorldToScreenPoint(decor.rest+Vector3.up*.2f);
            Vector2 end=Camera.main.WorldToScreenPoint(new Vector3(-1,1,0));
            game.PointerDown(start,-1);game.PointerMove(end,-1);game.PointerUp(end,-1);
            Check(Vector2.Distance(game.State.decorations[0],new Vector2(-1,1))<.1f,"Decoration drag persists position");
            Check(game.Save(),"Save writes successfully");
            var reloaded=new GardenSave(game.SavePath).Load();Check(reloaded.plants[0]==2 && reloaded.boatLaunched && reloaded.picnic[1]==2,"Save restores the garden");
            game.ToggleLanguage();Check(game.State.language=="en","English language switch");game.ToggleLanguage();
            yield return Expansion(game,output);
            game.SelectTool(GardenTool.Explore);game.UI.Hint(game.L("Birlikte büyüyen küçük bir dünya.","A little world, growing together."));
            yield return new WaitForSeconds(2);
            string screenshot=Path.Combine(output,"garden.png");yield return new WaitForEndOfFrame();Capture(screenshot);
            yield return new WaitForSeconds(1);
            Check(File.Exists(screenshot),"Rendered screenshot captured");
            File.WriteAllText(Path.Combine(output,"runtime-tests.json"),JsonUtility.ToJson(new Result{status=failures.Count==0?"passed":"failed",checks=checks,failures=failures.ToArray(),screenshot=screenshot},true));
            Debug.Log("RAIN_GARDEN_SMOKE_FINISHED: "+checks+" checks, "+failures.Count+" failures");
            Application.Quit(failures.Count==0?0:1);
        }
        IEnumerator Expansion(GardenGame game,string output)
        {
            Check(game.entities.Length==29 && game.Find(EntityKind.Plot,5)!=null,"Six plots, twelve decorations and five friends exist");
            game.UI.OpenActivity(0);yield return null;
            Press("Picnic fruit 1");yield return null;string runId=game.State.activity.id;
            Check(game.State.activity.step==1,"Picnic UI accepts matching fruit");
            game.UI.CloseModal();game.UI.OpenActivity(1);yield return null;game.UI.CloseModal();game.UI.OpenActivity(0);
            Check(game.State.activity.id==runId && game.State.activity.step==1,"Switching games preserves picnic progress");
            yield return new WaitForEndOfFrame();Capture(Path.Combine(output,"picnic.png"));
            int guard=0;while(!GardenActivities.Complete(game.State.activity)&&guard++<30){Press("Picnic fruit "+GardenActivities.PicnicOrder(game.State.activity)[game.State.activity.step]);yield return null;}
            Check(game.State.stamps==1 && game.State.activity.claimed,"Picnic reward is persisted from UI");
            Check(!game.ClaimActivity(runId) && game.State.stamps==1,"Repeated completion callback cannot grant extra reward");
            Press("Reward collection");yield return null;
            yield return new WaitForEndOfFrame();Capture(Path.Combine(output,"collection.png"));
            Press("Catalog bluebell");yield return null;Press("Plant slot 3");yield return null;
            Check(game.State.stamps==0 && game.State.flowerIds[3]=="bluebell" && GardenCatalog.Owns(game.State,"bluebell"),"Earned stamp buys a flower and UI plants it");
            game.Water(3);game.Plant("daisy",4);game.Water(4);
            Check(game.State.plantedSpecies.Length>=3,"Growing three different species is remembered");
            for(int kind=1;kind<3;kind++)
            {
                game.UI.OpenActivity(kind);yield return null;
                yield return new WaitForEndOfFrame();Capture(Path.Combine(output,kind==1?"cloud.png":"boat.png"));
                guard=0;
                while(!GardenActivities.Complete(game.State.activity)&&guard++<50)
                {
                    if(kind==1)
                    {
                        int cell=Array.Find(GardenActivities.CloudPath(game.State.activity),c=>!GardenActivities.Connected(game.State.activity,c));Press("Cloud cell "+cell);
                    }
                    else
                    {
                        var next=JsonUtility.FromJson<GardenActivityState>(JsonUtility.ToJson(game.State.activity));GardenActivities.Help(next);
                        int delta=next.boatCell-game.State.activity.boatCell;Press("Boat direction "+(delta==-5?0:delta==1?1:delta==5?2:3));
                    }
                    yield return null;
                }
                Check(GardenActivities.Complete(game.State.activity)&&game.State.activity.claimed,"Activity completes through actual UI "+kind);
            }
            Check(GardenProjects.Sumi(game.State) && game.Find(EntityKind.Friend,4).gameObject.activeSelf,"Discoveries reveal Sumi in the garden");
            game.UI.ShowCollection(1);yield return null;Press("Catalog chime");yield return null;
            Check(game.State.placed[3] && game.State.stamps==1,"Decoration purchase places new item with correct balance");
            Press("Catalog chime");yield return null;Press("Catalog chime");yield return null;
            Check(game.State.placed[3] && game.State.stamps==1,"Putting away and replacing owned decor is free");
            game.UI.ShowProjects();yield return null;
            for(int p=0;p<4;p++)Check(GardenProjects.Finished(game.State,p),"Project has all three discoveries "+p);
            yield return new WaitForEndOfFrame();Capture(Path.Combine(output,"discoveries.png"));
            game.UI.CloseModal();game.CaptureMemory();yield return new WaitForSeconds(.8f);
            Check(Directory.Exists(game.AlbumDirectory)&&Directory.GetFiles(game.AlbumDirectory,"*.png").Length==1,"Local album stores one image in isolated test directory");
            game.UI.ShowAlbum();yield return new WaitForEndOfFrame();Capture(Path.Combine(output,"album.png"));game.UI.CloseModal();
            var restored=new GardenSave(game.SavePath).Load();
            Check(restored.stamps==1 && restored.flowerIds[3]=="bluebell" && restored.placed[3] && GardenProjects.Sumi(restored),"Expanded garden, wallet and discoveries survive reload");
            // Simulate an unwritable future-format save only in this run's isolated test folder.
            string original=File.ReadAllText(game.SavePath);File.WriteAllText(game.SavePath,"{\"version\":99}");
            Check(!game.Purchase("can") && game.State.stamps==1 && !GardenCatalog.Owns(game.State,"can"),"Failed save rolls back purchase and balance in memory");
            File.WriteAllText(game.SavePath,original);
        }
        void Press(string name)
        {
            foreach(var button in FindObjectsByType<Button>(FindObjectsSortMode.None))
                if(button.name==name && button.gameObject.activeInHierarchy){button.onClick.Invoke();return;}
            Check(false,"Missing UI button: "+name);
        }
        void Capture(string path)
        {
            var texture=ScreenCapture.CaptureScreenshotAsTexture();
            if(texture==null){failures.Add("Screenshot texture is unavailable");return;}
            File.WriteAllBytes(path,texture.EncodeToPNG());Destroy(texture);
        }
        static void Drag(GardenGame game,GardenEntity entity,Vector2 destination)
        {
            Vector2 start=Camera.main.WorldToScreenPoint(entity.rest+Vector3.up*.2f);
            Vector2 end=Camera.main.WorldToScreenPoint(destination);
            Physics2D.SyncTransforms();game.PointerDown(start,-1);game.PointerMove(end,-1);game.PointerUp(end,-1);
        }
        static string Argument(string name){var args=Environment.GetCommandLineArgs();int i=Array.IndexOf(args,name);return i>=0 && i+1<args.Length?args[i+1]:null;}
    }
}
