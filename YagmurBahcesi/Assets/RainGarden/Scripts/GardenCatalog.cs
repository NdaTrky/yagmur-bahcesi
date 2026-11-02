using System;
using UnityEngine;
namespace RainGarden
{
    public sealed class GardenItem
    {
        public readonly string id,tr,en;
        public readonly int cost,sprite,behavior;
        public GardenItem(string id,string tr,string en,int cost,int sprite,int behavior=0)
        {this.id=id;this.tr=tr;this.en=en;this.cost=cost;this.sprite=sprite;this.behavior=behavior;}
    }
    public static class GardenCatalog
    {
        public static readonly GardenItem[] Flowers={
            new GardenItem("cosmos","Kozmos","Cosmos",0,-1,0),new GardenItem("daisy","Papatya","Daisy",0,-2,2),
            new GardenItem("bluebell","Çan çiçeği","Bluebell",1,0,1),new GardenItem("lavender","Lavanta","Lavender",2,1,0),
            new GardenItem("tulip","Lale","Tulip",2,2,2),new GardenItem("sunflower","Ayçiçeği","Sunflower",2,3,1),
            new GardenItem("lily","Nilüfer","Water lily",2,4,3),new GardenItem("poppy","Gelincik","Poppy",2,5,0)};
        public static readonly GardenItem[] Decorations={
            new GardenItem("pinwheel","Rüzgârgülü","Pinwheel",0,-1),new GardenItem("stones","Denge taşları","Pebble stack",0,-2),
            new GardenItem("mushroom","Mantar","Mushroom",0,-3),new GardenItem("chime","Rüzgâr çanı","Wind chime",1,6),
            new GardenItem("birdhouse","Kuş evi","Birdhouse",2,7),new GardenItem("bench","Bahçe bankı","Garden bench",2,8),
            new GardenItem("lantern","Fener","Lantern",2,9),new GardenItem("can","Sulama kabı","Watering can",1,10),
            new GardenItem("bridge","Minik köprü","Little bridge",3,11),new GardenItem("pot","Saksı","Flower pot",1,12),
            new GardenItem("fountain","Çeşme","Fountain",3,13),new GardenItem("basket","Piknik sepeti","Picnic basket",2,14)};
        public static readonly Vector2[] ExtraPlots={new Vector2(-.8f,.4f),new Vector2(.75f,.75f),new Vector2(2.25f,1.1f)};
        public static readonly Vector2[] DecorPositions={new Vector2(-1.6f,-.9f),new Vector2(6.4f,-2.8f),new Vector2(-3.7f,-2.9f),
            new Vector2(-7.8f,1.1f),new Vector2(-.7f,1.6f),new Vector2(.4f,-3.5f),new Vector2(7.7f,-2.6f),new Vector2(-4.5f,-3.5f),
            new Vector2(4.9f,1.4f),new Vector2(-.4f,-.8f),new Vector2(7.1f,.2f),new Vector2(1.7f,-3.6f)};
        public static GardenItem Find(string id)=>Array.Find(Flowers,x=>x.id==id)??Array.Find(Decorations,x=>x.id==id);
        public static int FlowerIndex(string id)=>Array.FindIndex(Flowers,x=>x.id==id);
        public static bool Owns(GardenState s,string id)=>Array.IndexOf(s.unlocked,id)>=0;
        public static Color Variant(int color)=>color==1?new Color(.83f,.92f,1):color==2?new Color(1,.86f,.82f):Color.white;
        public static Sprite FlowerSprite(GardenArt art,int index)=>index<2?art.Prop(4+index):art.Expansion(Flowers[index].sprite);
        public static Sprite DecorSprite(GardenArt art,int index)=>index<3?art.Prop(9+index):art.Expansion(Decorations[index].sprite);
        public static bool Purchase(GardenState s,string id)
        {
            var item=Find(id);if(item==null||Owns(s,id)||s.stamps<item.cost)return false;
            s.stamps-=item.cost;var list=new System.Collections.Generic.List<string>(s.unlocked){id};s.unlocked=list.ToArray();return true;
        }
    }
}
