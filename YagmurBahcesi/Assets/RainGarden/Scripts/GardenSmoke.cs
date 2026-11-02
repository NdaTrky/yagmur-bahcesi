using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace RainGarden
{
    public sealed class GardenSmoke : MonoBehaviour
    {
        [Serializable] sealed class Result {public string status;public int checks;public string[] failures;public string screenshot;}
        readonly List<string> failures=new List<string>();int checks;
        void Check(bool condition,string name){checks++;if(!condition){failures.Add(name);Debug.LogError("SMOKE: "+name);}}
        IEnumerator Start()
        {
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
            game.SelectTool(GardenTool.Explore);game.UI.Hint(game.L("Birlikte büyüyen küçük bir dünya.","A little world, growing together."));
            yield return new WaitForSeconds(2);
            string screenshot=Path.Combine(output,"garden.png");yield return new WaitForEndOfFrame();Capture(screenshot);
            yield return new WaitForSeconds(1);
            Check(File.Exists(screenshot),"Rendered screenshot captured");
            File.WriteAllText(Path.Combine(output,"runtime-tests.json"),JsonUtility.ToJson(new Result{status=failures.Count==0?"passed":"failed",checks=checks,failures=failures.ToArray(),screenshot=screenshot},true));
            Debug.Log("RAIN_GARDEN_SMOKE_FINISHED: "+checks+" checks, "+failures.Count+" failures");
            Application.Quit(failures.Count==0?0:1);
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
