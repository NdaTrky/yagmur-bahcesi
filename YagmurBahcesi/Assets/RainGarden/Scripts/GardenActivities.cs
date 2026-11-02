using System;
using UnityEngine;
namespace RainGarden
{
    public enum GardenActivityKind { Picnic,Cloud,Boat }
    [Serializable] public sealed class GardenActivityState
    {
        public string id;
        public int kind,variant,step,boatCell;
        public int[] turns;
        public bool claimed;
        public bool Valid()=>!string.IsNullOrEmpty(id)&&kind>=0&&kind<3&&variant>=0&&variant<3&&step>=0&&step<=6&&boatCell>=0&&boatCell<15&&turns!=null&&turns.Length==9&&Array.TrueForAll(turns,x=>x>=0&&x<4);
    }
    public static class GardenActivities
    {
        static readonly int[][] picnic={new[]{1,2,1},new[]{2,2,1,1},new[]{1,2,1,2,1}};
        static readonly int[][] paths={new[]{0,1,4,7,8},new[]{3,4,1,2,5},new[]{6,3,4,5,2}};
        static readonly int[][] masks={new[]{10,12,5,3,10},new[]{10,9,6,12,3},new[]{9,6,10,9,6}};
        static readonly int[][] stops={new[]{12,4},new[]{2,14},new[]{0,9,14}};
        static readonly int[][] rocks={new[]{6,8},new[]{6,7},new[]{5,12}};
        public static bool Begin(GardenState s,int kind)
        {
            if(kind<0||kind>2)return false;s.activeActivity=kind;
            // JsonUtility can materialize null inline objects as empty records.
            if(s.activity==null||!s.activity.Valid()||s.activity.claimed)s.activity=Create(kind,s.activityPlays[kind]%3);
            return true;
        }
        public static GardenActivityState Create(int kind,int variant)
        {
            var a=new GardenActivityState{id=Guid.NewGuid().ToString("N"),kind=kind,variant=variant,turns=new int[9],boatCell=variant==1?0:10};
            for(int i=0;i<9;i++)a.turns[i]=1+i%3;return a;
        }
        public static int[] PicnicOrder(GardenActivityState a)=>picnic[a.variant];
        public static int[] CloudPath(GardenActivityState a)=>paths[a.variant];
        public static int BaseMask(GardenActivityState a,int cell){int i=Array.IndexOf(paths[a.variant],cell);return i<0?0:masks[a.variant][i];}
        public static int Mask(GardenActivityState a,int cell)
        {
            int mask=BaseMask(a,cell);for(int i=0;i<a.turns[cell];i++)mask=((mask<<1)&15)|(mask>>3);return mask;
        }
        public static bool Connected(GardenActivityState a,int cell)=>Mask(a,cell)==BaseMask(a,cell);
        public static int[] BoatStops(GardenActivityState a)=>stops[a.variant];
        public static bool Rock(GardenActivityState a,int cell)=>Array.IndexOf(rocks[a.variant],cell)>=0;
        public static int Goal(GardenActivityState a)=>a.kind==0?picnic[a.variant].Length:a.kind==1?5:stops[a.variant].Length;
        public static bool Complete(GardenActivityState a)
        {
            if(a==null||!a.Valid())return false;
            if(a.kind==1){foreach(int cell in paths[a.variant])if(!Connected(a,cell))return false;return true;}
            return a.step>=Goal(a);
        }
        public static bool PickFruit(GardenActivityState a,int fruit)
        {if(a==null||a.kind!=0||Complete(a)||fruit!=picnic[a.variant][a.step])return false;a.step++;return true;}
        public static bool Rotate(GardenActivityState a,int cell)
        {if(a==null||a.kind!=1||cell<0||cell>8||BaseMask(a,cell)==0||Complete(a))return false;a.turns[cell]=(a.turns[cell]+1)%4;return true;}
        public static int NextCell(GardenActivityState a,int direction,int from)
        {
            int x=from%5,y=from/5;
            if(direction==0)y--;else if(direction==1)x++;else if(direction==2)y++;else if(direction==3)x--;else return -1;
            if(x<0||x>=5||y<0||y>=3)return -1;
            int cell=y*5+x;return Rock(a,cell)?-1:cell;
        }
        public static bool Sail(GardenActivityState a,int direction)
        {
            if(a==null||a.kind!=2||Complete(a))return false;int cell=NextCell(a,direction,a.boatCell);if(cell<0)return false;
            a.boatCell=cell;if(cell==stops[a.variant][a.step])a.step++;return true;
        }
        public static bool Help(GardenActivityState a)
        {
            if(a==null||Complete(a))return false;
            if(a.kind==0)return PickFruit(a,picnic[a.variant][a.step]);
            if(a.kind==1){foreach(int cell in paths[a.variant])if(!Connected(a,cell)){a.turns[cell]=0;return true;}}
            if(a.kind==2)
            {
                var queue=new System.Collections.Generic.Queue<int>();var parent=new int[15];var direction=new int[15];
                for(int i=0;i<15;i++)parent[i]=-1;parent[a.boatCell]=a.boatCell;queue.Enqueue(a.boatCell);
                int target=stops[a.variant][a.step];
                while(queue.Count>0)
                {
                    int cell=queue.Dequeue();if(cell==target)break;
                    for(int d=0;d<4;d++){int next=NextCell(a,d,cell);if(next>=0&&parent[next]<0){parent[next]=cell;direction[next]=d;queue.Enqueue(next);}}
                }
                if(parent[target]<0)return false;
                while(parent[target]!=a.boatCell)target=parent[target];
                return Sail(a,direction[target]);
            }
            return false;
        }
        public static bool Claim(GardenState s,string runId)
        {
            var a=s.activity;if(a==null||a.id!=runId||a.claimed||!Complete(a))return false;
            a.claimed=true;s.stamps++;s.activityCompletions++;s.activityPlays[a.kind]++;
            s.discoveries[a.kind==0?7:a.kind==1?9:4]=true;
            if(Array.TrueForAll(s.activityPlays,x=>x>0))s.discoveries[11]=true;
            return true;
        }
    }
    public static class GardenProjects
    {
        public static int Progress(GardenState s,int project)
        {int count=0;for(int i=project*3;i<project*3+3;i++)if(s.discoveries[i])count++;return count;}
        public static bool Finished(GardenState s,int project)=>Progress(s,project)==3;
        public static bool Sumi(GardenState s)=>Finished(s,3);
        public static readonly string[] NamesTR={"Mina'nın çiçek köşesi","Luli'nin iskelesi","Pufi'nin pikniği","Sumi'ye hoş geldin"};
        public static readonly string[] NamesEN={"Mina's flower corner","Luli's landing","Pufi's picnic","Welcome, Sumi"};
        public static readonly string[] StepsTR={"Bir çiçek büyüt","Yeni bir tohum seç","Yeni çiçeğini dik","Yaprağı yüzdür","İskelelere ulaş","Rüzgârla oyna","Bir meyve paylaş","Piknik Sepeti'ni oyna","Yeni bir süs yerleştir","Bulutun Yolu'nu oyna","Üç çiçek türü dik","Üç oyunu da keşfet"};
        public static readonly string[] StepsEN={"Grow a flower","Choose a new seed","Plant your new flower","Float the leaf","Reach the landings","Play with the breeze","Share a fruit","Play Picnic Basket","Place a new decoration","Play Cloud Path","Plant three flower kinds","Discover all three games"};
    }
}
