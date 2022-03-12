using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.IO;

namespace Projet_Brick_Breaker_main
{
    //  ██╗-----███████╗██╗---██╗███████╗██╗-----███████╗
    //  ██║-----██╔════╝██║---██║██╔════╝██║-----██╔════╝
    //  ██║-----█████╗--██║---██║█████╗--██║-----███████╗
    //  ██║-----██╔══╝--╚██╗-██╔╝██╔══╝--██║-----╚════██║
    //  ███████╗███████╗-╚████╔╝-███████╗███████╗███████║
    //  ╚══════╝╚══════╝--╚═══╝--╚══════╝╚══════╝╚══════╝
    class LevelsManager
    {
        internal int up { get; set; }
        internal int left { get; set; }

        internal byte level { get; set; }
        internal bool complished { get; set; }
        internal bool lost { get; set; }
        internal float timer { get; set; }


        internal Vector2 pos_print { get; set; }
        internal Texture2D print1 { get; set; }
        internal Texture2D print2 { get; set; }
        internal Texture2D print3 { get; set; }
        internal Texture2D print4 { get; set; }
        internal float alpha { get; set; }

        internal byte[,] map { get; set; }

        internal byte columns { get; set; }
        internal byte lines { get; set; }
        internal int mapx { get; set; }
        private Bricks brickref { get; set; }

        internal List<Bricks> lstBricks { get; set; }


        //------------------------------------------------------------


        internal LevelsManager(byte pStartLevel)
        {
            up = Scenes.Slate[Scenes.SlateEdges.Up];
            left = Scenes.Slate[Scenes.SlateEdges.Left];
            brickref = Tools.Get<EffectsManager>().brickref;

            pos_print = new Vector2(left, up);
            print1 = Tools.Get<EffectsManager>().print1;
            print2 = Tools.Get<EffectsManager>().print2;
            print3 = Tools.Get<EffectsManager>().print3;
            print4 = Tools.Get<EffectsManager>().print4;
            alpha = 0;

            level = pStartLevel;
            map = new byte[22, 8];
            lstBricks = new List<Bricks>();

            columns = (byte)map.GetLength(1);
            lines = (byte)map.GetLength(0);
            mapx = (Scenes.slate.Width - (brickref.hitboxe.Width * columns)) / 2;

            for (byte lin = 0; lin < lines; lin++)
                for (byte col = 0; col < columns; col++)
                    map[lin, col] = 0;

            complished = false;
            lost = false;
        }


        //------------------------------------------------------------


        internal void LoadLevel(byte pLevel)
        {
            lstBricks.RemoveAll(b => true);

            for (byte lin = 0; lin < lines; lin++)
                for (byte col = 0; col < columns; col++)
                    map[lin, col] = 0;

            string[] extlvl = File.ReadAllLines(AppDomain.CurrentDomain.BaseDirectory + "Levels\\" + "level_" + pLevel + ".txt");

            for (byte lin = 0; lin < lines; lin++)
            {
                string line = extlvl[lin];
                for (byte col = 0; col < columns; col++)
                {
                    map[lin, col] = byte.Parse(line.Substring(col, 1));
                }
            }

            for (byte lin = 0; lin < lines; lin++)
                for (byte col = 0; col < columns; col++)
                    if (map[lin, col] != 0)
                        lstBricks.Add(new Bricks(map[lin, col], (col * brickref.hitboxe.Width) + left + mapx - brickref.hitboxe.X, lin * (brickref.hitboxe.Height) + up - brickref.hitboxe.Y));

            for (short i = (short)(lstBricks.Count - 1); i >= 0; i--)
            {
                var brick = lstBricks[i];
                brick.color.R = 0;
                brick.color.G = 0;
                brick.color.B = 0;
            }

            timer = 0;
        }

        internal void LoadLevel(byte[,] pMap)
        {
            lstBricks.RemoveAll(b => true);

            for (byte lin = 0; lin < lines; lin++)
                for (byte col = 0; col < columns; col++)
                    if (pMap[lin, col] != 0)
                        lstBricks.Add(new Bricks(pMap[lin, col], (col * brickref.hitboxe.Width) + left + mapx - brickref.hitboxe.X, lin * (brickref.hitboxe.Height) + up - brickref.hitboxe.Y));

            for (short i = (short)(lstBricks.Count - 1); i >= 0; i--)
            {
                var brick = lstBricks[i];
                brick.color.R = 0;
                brick.color.G = 0;
                brick.color.B = 0;
            }

            timer = 0;
        }

        internal Texture2D currentPrint(byte pLevel)
        {
            Texture2D lclimg = print1;
            switch (pLevel)
            {
                case 1:
                    lclimg = print1;
                    break;
                case 2:
                    lclimg = print2;
                    break;
                case 3:
                    lclimg = print3;
                    break;
                case 4:
                    lclimg = print4;
                    break;
                case 5:
                    lclimg = Editor.currentEditPrint;
                    break;
            }
            return lclimg;
        }


        //------------------------------------------------------------


        internal void Update(GameTime pGameTime, List<Balls> lstBalls, Bate bate)
        {



            for (short i = (short)(lstBricks.Count - 1); i >= 0; i--)
            {
                var brick = lstBricks[i];
                brick.Update(pGameTime);
            }

            if (lstBricks.Count == 0)
            {
                complished = true;
            }

            if (bate.life <= 0 && lstBalls.Count == 0 && !complished)
            {
                lost = true;
            }

            if (complished)
            {
                lstBalls.RemoveAll(b => true);

                if (alpha >= 1)
                {
                    Tools.Get<EffectsManager>().lvlcomplished.Play();
                }

                alpha -= (1.0f / 180.0f);


                if (alpha <= 0)
                {
                    alpha = 0;
                    if (level < 4)
                    {
                        level++;
                        complished = false;
                        LoadLevel(level);
                        lstBalls.Add(new Balls(true));
                    }
                    else
                    {
                        MediaPlayer.Volume -= (1.0f / 120.0f);
                        if (MediaPlayer.Volume <= 0)
                        {
                            MediaPlayer.Stop();
                            MediaPlayer.Volume = 0.7f;
                            Scenes.score += (short)(bate.life * 100);
                            Tools.Get<GameState>().SceneSwitch(new Victory());
                        }
                    }
                }

            }
            else
            {
                if (!lost)
                {
                    timer += (1.0f / 180.0f);
                    if (timer >= 5)
                        timer = 5;

                    if (timer < 4)
                    {
                        for (short i = (short)(lstBricks.Count - 1); i >= 0; i--)
                        {

                            var brick = lstBricks[i];

                            if (brick.color.R < brick.Rmax)
                            {
                                brick.Rtimer += brick.Rmax / 180.0f;
                                brick.color.R = (byte)brick.Rtimer;
                            }
                            if (brick.Rtimer >= brick.Rmax)
                            {
                                brick.Rtimer = brick.Rmax;
                                brick.color.R = brick.Rmax;
                            }

                            if (brick.color.G < brick.Gmax)
                            {
                                brick.Gtimer += brick.Gmax / 180.0f;
                                brick.color.G = (byte)brick.Gtimer;
                            }
                            if (brick.Gtimer >= brick.Gmax)
                            {
                                brick.Gtimer = brick.Gmax;
                                brick.color.G = brick.Gmax;
                            }

                            if (brick.color.B < brick.Bmax)
                            {
                                brick.Btimer += brick.Bmax / 180.0f;
                                brick.color.B = (byte)brick.Btimer;
                            }
                            if (brick.Btimer >= brick.Bmax)
                            {
                                brick.Btimer = brick.Bmax;
                                brick.color.B = brick.Bmax;
                            }
                        }

                        alpha += (1.0f / 180.0f);

                        if (alpha >= 1)
                        {
                            alpha = 1.0f;
                        }
                    }
                }
                else
                {
                    for (short i = (short)(lstBricks.Count - 1); i >= 0; i--)
                    {
                        var brick = lstBricks[i];

                        if (brick.color.R > 0)
                        {
                            brick.Rtimer -= brick.Rmax / 90.0f;
                            brick.color.R = (byte)brick.Rtimer;
                        }
                        if (brick.Rtimer <= 0)
                        {
                            brick.Rtimer = 0;
                            brick.color.R = 0;
                        }

                        if (brick.color.G > 0)
                        {
                            brick.Gtimer -= brick.Gmax / 90.0f;
                            brick.color.G = (byte)brick.Gtimer;
                        }
                        if (brick.Gtimer <= 0)
                        {
                            brick.Gtimer = 0;
                            brick.color.G = 0;
                        }

                        if (brick.color.B > 0)
                        {
                            brick.Btimer -= brick.Bmax / 90.0f;
                            brick.color.B = (byte)brick.Btimer;
                        }
                        if (brick.Btimer <= 0)
                        {
                            brick.Btimer = 0;
                            brick.color.B = 0;
                        }
                    }

                    alpha -= (1.0f / 90.0f);
                    MediaPlayer.Volume -= (1.0f / 120.0f);
                    if (MediaPlayer.Volume <= 0)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Volume = 0.7f;
                        Tools.Get<GameState>().SceneSwitch(new GameOver());
                    }
                }
            }
        }


        //------------------------------------------------------------


        internal void Draw(GameTime pGameTime)
        {
            Tools.Get<Main>()._spriteBatch.Draw(currentPrint(level), pos_print, Color.White * alpha);

            for (short i = (short)(lstBricks.Count - 1); i >= 0; i--)
            {
                var brick = lstBricks[i];
                brick.Draw(pGameTime);
            }
        }


    }




    //  ███████╗███████╗███████╗███╗---███╗-█████╗-███╗---██╗-█████╗--██████╗-███████╗██████╗-
    //  ██╔════╝██╔════╝██╔════╝████╗-████║██╔══██╗████╗--██║██╔══██╗██╔════╝-██╔════╝██╔══██╗
    //  █████╗--█████╗--█████╗--██╔████╔██║███████║██╔██╗-██║███████║██║--███╗█████╗--██████╔╝
    //  ██╔══╝--██╔══╝--██╔══╝--██║╚██╔╝██║██╔══██║██║╚██╗██║██╔══██║██║---██║██╔══╝--██╔══██╗
    //  ███████╗██║-----██║-----██║-╚═╝-██║██║--██║██║-╚████║██║--██║╚██████╔╝███████╗██║--██║
    //  ╚══════╝╚═╝-----╚═╝-----╚═╝-----╚═╝╚═╝--╚═╝╚═╝--╚═══╝╚═╝--╚═╝-╚═════╝-╚══════╝╚═╝--╚═╝
    class EffectsManager
    {
        internal SoundEffect ballout { get; set; }
        internal SoundEffect batehit { get; set; }
        internal SoundEffect bateswitch { get; set; }
        internal SoundEffect bricktouched { get; set; }
        internal SoundEffect crack { get; set; }
        internal SoundEffect destroy { get; set; }
        internal SoundEffect hitingball { get; set; }
        internal SoundEffect wall { get; set; }
        internal SoundEffect select { get; set; }
        internal SoundEffect choice { get; set; }
        internal SoundEffect lvlcomplished { get; set; }
        internal SoundEffect pop { get; set; }
        internal SoundEffect whistle { get; set; }
        internal SoundEffect firework { get; set; }
        internal SoundEffect stone { get; set; }
        internal SoundEffect brickout { get; set; }
        internal SoundEffect brickpop { get; set; }

        internal Song sndMenu { get; set; }
        internal SoundEffect sndGameover { get; set; }
        internal Song sndVictory { get; set; }
        internal Song sndIngame { get; set; }

        internal Texture2D print1 { get; set; }
        internal Texture2D print2 { get; set; }
        internal Texture2D print3 { get; set; }
        internal Texture2D print4 { get; set; }
        internal Texture2D delete { get; set; }
        internal Texture2D refresh { get; set; }
        internal Texture2D exit { get; set; }

        internal Texture2D imgbrick { get; set; }
        internal Bricks brickref { get; set; }

        internal List<Effects> lstEffects { get; set; }
        internal Texture2D imgimpct { get; set; }

        internal List<Particles> lstParticles { get; set; }
        internal Texture2D particle { get; set; }
        internal Texture2D aura { get; set; }


        //------------------------------------------------------------


        internal EffectsManager()
        {
            Tools.AddService(this);

            ballout = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/ballout");
            batehit = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/batehit");
            bateswitch = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/bateswitch");
            bricktouched = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/bricktouched");
            crack = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/crack");
            destroy = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/destroy");
            hitingball = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/hitingball");
            wall = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/wall");
            select = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/select");
            choice = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/choice");
            lvlcomplished = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/lvlcomplished");
            pop = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/pop");
            whistle = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/whistle");
            firework = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/firework");
            stone = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/stone");
            brickout = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/brickout");
            brickpop = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/brickpop");

            sndMenu = Tools.Get<Main>().Content.Load<Song>("sounds/musics/newmoon");
            sndIngame = Tools.Get<Main>().Content.Load<Song>("sounds/musics/goryokaidan");
            sndGameover = Tools.Get<Main>().Content.Load<SoundEffect>("sounds/effects/gameover");
            sndVictory = Tools.Get<Main>().Content.Load<Song>("sounds/musics/fairlady");

            print1 = Tools.Get<Main>().Content.Load<Texture2D>("images/display/sakura");
            print2 = Tools.Get<Main>().Content.Load<Texture2D>("images/display/night");
            print3 = Tools.Get<Main>().Content.Load<Texture2D>("images/display/mountains");
            print4 = Tools.Get<Main>().Content.Load<Texture2D>("images/display/waves");
            delete = Tools.Get<Main>().Content.Load<Texture2D>("images/display/delete");
            refresh = Tools.Get<Main>().Content.Load<Texture2D>("images/display/refresh");
            exit = Tools.Get<Main>().Content.Load<Texture2D>("images/display/out");

            imgbrick = Tools.Get<Main>().Content.Load<Texture2D>("images/actors/brick");
            brickref = new Bricks(1, 0, 0);

            lstEffects = new List<Effects>();
            imgimpct = Tools.Get<Main>().Content.Load<Texture2D>("images/display/impact");

            lstParticles = new List<Particles>();
            particle = Tools.DrawRec(1, 1, Color.White);
            aura = Tools.Get<Main>().Content.Load<Texture2D>("images/display/aura");
        }

        //------------------------------------------------------------

        internal void PlayImpact(Balls pBall)
        {
            lstEffects.Add(new Effects(imgimpct, (int)(437.0f / 5.0f), 75, new Vector2(44, 41), new Vector2(pBall.hitboxe.X + (pBall.hitboxe.Width) / 2, pBall.hitboxe.Y + (pBall.hitboxe.Height) / 2), 0.04f));
        }

        internal void PlayExplosion(short pNumber, Vector2 pPos, Color pColor)
        {
            for (short i = 0; i < pNumber; i++)
            {
                var pwr = (Tools.Rnd(50, 300)/100.0f);
                var angle = Tools.Rnd(0, 360);
                var vx = (float)Math.Cos(MathHelper.ToRadians(angle));
                var vy = (float)Math.Sin(MathHelper.ToRadians(angle));
                var frct = (vx > 0) ? 0.005f : -0.005f;
                var disp = (1.0f / Tools.Rnd(90, 180) );
                var dtlife = (Tools.Rnd(75, 150)) / 100;
                var lclscale = (Tools.Rnd(100, 200) / 100.0f);

                var part = new Particles(pColor, pPos, new Vector2(vx,vy), new Vector2(pwr,pwr), new Vector2(0,-0.01f), new Vector2(frct,0), 1.0f, disp, 0);
                part.scale = new Vector2(lclscale, lclscale);

                var lclaura = new Particles(pColor, new Vector2(pPos.X+1, pPos.Y+1), new Vector2(vx,vy), new Vector2(pwr,pwr), new Vector2(0,-0.01f), new Vector2(frct,0), 1.0f, disp, 0);
                lclaura.img = aura;
                lclaura.orgn = new Vector2(aura.Width / 2, aura.Height / 2);
                lclaura.scale = new Vector2(lclscale, lclscale);

                lstParticles.Add(part);
                lstParticles.Add(lclaura);
            }
        }

        //------------------------------------------------------------

        internal void Update(GameTime pGameTime)
        {
            for (short i= (short)(lstEffects.Count -1); i >=0; i--)
            {
                var eff = lstEffects[i];
                eff.Update(pGameTime);
                if (eff.current.fnshed)
                {
                    lstEffects.RemoveAt(i);
                }
            }

            for (int i = (lstParticles.Count - 1); i >= 0; i--)
            {
                var part = lstParticles[i];
                part.Update(pGameTime);
                if (part.todelete)
                {
                    lstParticles.RemoveAt(i);
                }
            }
        }


        //------------------------------------------------------------


        internal void Draw(GameTime pGameTime)
        {
            for (short i = (short)(lstEffects.Count - 1); i >= 0; i--)
            {
                var eff = lstEffects[i];
                eff.Draw(pGameTime);
            }

            for (short i = (short)(lstParticles.Count - 1); i >= 0; i--)
            {
                var part = lstParticles[i];
                part.Draw(pGameTime);
            }
        }


    }




    //  ███████╗███████╗███████╗███████╗-██████╗████████╗███████╗
    //  ██╔════╝██╔════╝██╔════╝██╔════╝██╔════╝╚══██╔══╝██╔════╝
    //  █████╗--█████╗--█████╗--█████╗--██║--------██║---███████╗
    //  ██╔══╝--██╔══╝--██╔══╝--██╔══╝--██║--------██║---╚════██║
    //  ███████╗██║-----██║-----███████╗╚██████╗---██║---███████║
    //  ╚══════╝╚═╝-----╚═╝-----╚══════╝-╚═════╝---╚═╝---╚══════╝
    class Effects : Sprites
    {
        internal byte nframes { get; set; }

        //------------------------------------------------------------

        internal Effects(Texture2D pImg, int pW, int pH, Vector2 pOrgn, Vector2 pPos, float pDt) : base(true, pW, pH)
        {
            List<byte> lclanim = new List<byte>();

            img = pImg;
            layer = 0.6f;
            pos = pPos;
            orgn = pOrgn;
            nframes = (byte)((img.Width / pW)*(img.Height/pH));

            for (byte i = 0; i < nframes; i++)
                lclanim.Add(i);

            AddAnim("eff", lclanim, pDt, false, true);
            PlayAnim("eff");
        }

        //------------------------------------------------------------

        internal override void Update(GameTime pGameTime)
        {
            base.Update(pGameTime);
        }

        //------------------------------------------------------------

        internal override void Draw(GameTime pGameTime)
        {
            base.Draw(pGameTime);
        }

    }



    //  ██████╗--█████╗-██████╗-████████╗██╗-██████╗██╗-----███████╗███████╗
    //  ██╔══██╗██╔══██╗██╔══██╗╚══██╔══╝██║██╔════╝██║-----██╔════╝██╔════╝
    //  ██████╔╝███████║██████╔╝---██║---██║██║-----██║-----█████╗--███████╗
    //  ██╔═══╝-██╔══██║██╔══██╗---██║---██║██║-----██║-----██╔══╝--╚════██║
    //  ██║-----██║--██║██║--██║---██║---██║╚██████╗███████╗███████╗███████║
    //  ╚═╝-----╚═╝--╚═╝╚═╝--╚═╝---╚═╝---╚═╝-╚═════╝╚══════╝╚══════╝╚══════╝
    class Particles : Sprites
    {
        internal float disp { get; set; }
        internal float tmralive {get;set;}
        internal float dtalive { get; set; }
        internal bool todelete { get; set; }

        //------------------------------------------------------------

        internal Particles(Color pColor, Vector2 pPos, Vector2 pSpeed, Vector2 pPower, Vector2 pWeight, Vector2 pFriction, float pAlpha, float pDisp, float pDtalive) : base()
        {
            img = Tools.Get<EffectsManager>().particle;
            color = pColor;
            pos = pPos;
            speed = pSpeed;
            power = pPower;
            weight = pWeight;
            friction = pFriction;
            alpha = pAlpha;
            disp = pDisp;
            dtalive = pDtalive;
            tmralive = 0;
            todelete = false;
        }

        //------------------------------------------------------------

        internal override void Update(GameTime pGameTime)
        {
            alpha -= disp;

            if (dtalive != 0)
                tmralive += (1.0f / 60.0f);

            if ((dtalive != 0 && tmralive >= dtalive) || alpha <= 0)
                todelete = true;

            speed -= (weight + friction);

            pos += (speed * power);

            base.Update(pGameTime);
        }

        //------------------------------------------------------------

        internal override void Draw(GameTime pGameTime)
        {
            base.Draw(pGameTime);
        }
    }
}
