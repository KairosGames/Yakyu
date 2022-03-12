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
    //  -██████╗--█████╗-███╗---███╗███████╗███████╗████████╗-█████╗-████████╗███████╗
    //  ██╔════╝-██╔══██╗████╗-████║██╔════╝██╔════╝╚══██╔══╝██╔══██╗╚══██╔══╝██╔════╝
    //  ██║--███╗███████║██╔████╔██║█████╗--███████╗---██║---███████║---██║---█████╗--
    //  ██║---██║██╔══██║██║╚██╔╝██║██╔══╝--╚════██║---██║---██╔══██║---██║---██╔══╝--
    //  ╚██████╔╝██║--██║██║-╚═╝-██║███████╗███████║---██║---██║--██║---██║---███████╗
    //  -╚═════╝-╚═╝--╚═╝╚═╝-----╚═╝╚══════╝╚══════╝---╚═╝---╚═╝--╚═╝---╚═╝---╚══════╝
    class GameState
    {
        internal Scenes current { get; set; }
        

        //------------------------------------------------------------


        internal GameState()
        {
            Tools.AddService(this);
        }


        //------------------------------------------------------------


        internal void SceneSwitch(Scenes pScene)
        {
            current = null;
            current = pScene;
        }


    }




    //  ███████╗-██████╗███████╗███╗---██╗███████╗███████╗
    //  ██╔════╝██╔════╝██╔════╝████╗--██║██╔════╝██╔════╝
    //  ███████╗██║-----█████╗--██╔██╗-██║█████╗--███████╗
    //  ╚════██║██║-----██╔══╝--██║╚██╗██║██╔══╝--╚════██║
    //  ███████║╚██████╗███████╗██║-╚████║███████╗███████║
    //  ╚══════╝-╚═════╝╚══════╝╚═╝--╚═══╝╚══════╝╚══════╝
    abstract class Scenes
    {
        static internal Rectangle slate = new Rectangle(125, 51, 500, 800);
        static internal Vector2 slatecorner = new Vector2(125, 51);
        internal enum SlateEdges { Up, Down, Left, Right }
        static internal Dictionary<SlateEdges, int> Slate = new Dictionary<SlateEdges, int>()
        {
            [SlateEdges.Up] = slate.Y,
            [SlateEdges.Down] = slate.Y + slate.Height,
            [SlateEdges.Left] = slate.X,
            [SlateEdges.Right] = slate.X + slate.Width
        };
        internal int up { get; set; }
        internal int down { get; set; }
        internal int left { get; set; }
        internal int right { get; set; }

        Texture2D bg_img;

        internal float timer { get; set; }
        internal bool choice { get; set; }

        internal static short score;

        internal Sprites framework { get; set; }


        //------------------------------------------------------------


        internal Scenes()
        {
            up = Slate[SlateEdges.Up];
            down = Slate[SlateEdges.Down];
            left = Slate[SlateEdges.Left];
            right = Slate[SlateEdges.Right];

            bg_img = Tools.Get<Main>().Content.Load<Texture2D>("images/display/background");

            timer = 0;
            choice = false;

            framework = new Sprites();
            framework.img = Tools.Get<Main>().Content.Load<Texture2D>("images/display/framework");
            framework.pos = slatecorner;
            framework.alpha = 0;
        }


        //------------------------------------------------------------


        internal virtual void Update(GameTime pGameTime)
        {
            Tools.Get<EffectsManager>().Update(pGameTime);
        }
        

        //------------------------------------------------------------


        internal virtual void Draw(GameTime pGameTime)
        {
            Tools.Get<EffectsManager>().Draw(pGameTime);
            Tools.Get<Main>()._spriteBatch.Draw(bg_img, Vector2.Zero, null, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 0.7f);
        }
    }




    //  ███╗---███╗███████╗███╗---██╗██╗---██╗
    //  ████╗-████║██╔════╝████╗--██║██║---██║
    //  ██╔████╔██║█████╗--██╔██╗-██║██║---██║
    //  ██║╚██╔╝██║██╔══╝--██║╚██╗██║██║---██║
    //  ██║-╚═╝-██║███████╗██║-╚████║╚██████╔╝
    //  ╚═╝-----╚═╝╚══════╝╚═╝--╚═══╝-╚═════╝-
    class Menu : Scenes
    {
        internal List<Buttons> lstButtons { get; set; }
        internal Color selectcolor { get; set; }
        internal Buttons btnTitle { get; set; }
        internal Buttons btnCredits { get; set; }
        internal Buttons btn1ActorsCredits { get; set; }
        internal Buttons btn2ActorsCredits { get; set; }
        internal Buttons btn3ActorsCredits { get; set; }
        internal Buttons btnPlay { get; set; }
        internal Buttons btnEdit { get; set; }
        internal Buttons btnExit { get; set; }

        internal Sprites light { get; set; }
        internal Sprites flowers { get; set; }

        internal bool to_edit { get; set; }



        //------------------------------------------------------------


        internal Menu() : base()
        {
            lstButtons = new List<Buttons>();
            selectcolor = new Color(229, 159, 181);
            to_edit = false;

            btnTitle = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["japanola72"].MeasureString("Yakyu").X / 2)), up + 150), "Yakyu", Tools.Fonts["japanola72"], Color.White);
            btnCredits = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["japanola18"].MeasureString("by  Kayros").X / 2)), up + btnTitle.pos.Y + btnTitle.h - 60), "by  Kayros", Tools.Fonts["japanola18"], Color.White);
            btnPlay = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["osake36"].MeasureString("Play").X / 2)), up + (((down - up) / 6) * 3)), "Play", Tools.Fonts["osake36"], Color.White, selectcolor);
            btnEdit = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["osake36"].MeasureString("Edit").X / 2)), up + btnPlay.pos.Y + 30), "Edit", Tools.Fonts["osake36"], Color.White, selectcolor);
            btnExit = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["osake36"].MeasureString("Exit").X / 2)), up + btnEdit.pos.Y + 30), "Exit", Tools.Fonts["osake36"], Color.White, selectcolor);
            btn1ActorsCredits = new Buttons(new Vector2(left + 70, down - (Tools.Fonts["japanola8"].MeasureString("H").Y*2) - 65), "Bate-Balls-Bricks", Tools.Fonts["japanola8"], Color.White);
            btn2ActorsCredits = new Buttons(new Vector2(left + 125 , btn1ActorsCredits.pos.Y + btn1ActorsCredits.h), "by", Tools.Fonts["japanola8"], Color.White);
            btn3ActorsCredits = new Buttons(new Vector2(left + 75 , btn2ActorsCredits.pos.Y + btn2ActorsCredits.h), "Lara Cougot", Tools.Fonts["japanola12"], Color.White);

            lstButtons.Add(btnTitle);
            lstButtons.Add(btnCredits);
            lstButtons.Add(btnPlay);
            lstButtons.Add(btnEdit);
            lstButtons.Add(btnExit);
            //lstButtons.Add(btn1ActorsCredits);
            //lstButtons.Add(btn2ActorsCredits);
            //lstButtons.Add(btn3ActorsCredits);

            foreach (Buttons b in lstButtons)
                b.alpha = 0;

            flowers = new Sprites();
            flowers.img = Tools.Get<Main>().Content.Load<Texture2D>("images/display/flowers");
            flowers.pos = slatecorner;
            flowers.alpha = 0;

            light = new Sprites();
            light.img = Tools.Get<Main>().Content.Load<Texture2D>("images/display/light1");
            light.pos = slatecorner;
            light.alpha = 0;

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Tools.Get<EffectsManager>().sndMenu);
            MediaPlayer.Volume = 0.7f;
            score = 0;
        }


        //------------------------------------------------------------


        internal override void Update(GameTime pGameTime)
        {
            if (!choice)
            {
                framework.alpha += (1.0f / 60.0f);
                if (framework.alpha >= 1)
                    framework.alpha = 1.0f;
            }

            timer += (1.0f / 60.0f);

            if (timer >= 2)
            {
                light.alpha += (1.0f / 1200.0f);
                if (light.alpha >= 0.2f)
                {
                    flowers.alpha += (1.0f / 60.0f);
                    if (flowers.alpha >= 1)
                        flowers.alpha = 1.0f;
                    if (light.alpha >= 0.6)
                        light.alpha = 0.6f;
                }

                for (byte i = 0; i <= lstButtons.Count - 1; i++)
                {
                    var button = lstButtons[i];
                    if (!choice)
                    {
                        button.alpha += (1.0f / 60.0f);
                        if (button.alpha >= 1)
                        {
                            button.alpha = 1.0f;
                        }
                    } 
                    else
                    {
                        button.alpha -= (1.0f / 60.0f);
                        if (button.alpha <= 0)
                        {
                            button.alpha = 0;
                        }
                        
                    }

                    if (button.usable)
                    {
                        if (button.hitboxe.Contains(Tools.mouse.Position) && !choice)
                        {
                            if (!button.clickable)
                            {
                                Tools.Get<EffectsManager>().select.Play();
                                button.clickable = true;
                            }
                        } else
                        {
                            button.clickable = false;
                        }
                    }
                }

                if (!choice && btnPlay.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    Tools.Get<EffectsManager>().choice.Play();
                    choice = true;
                }

                if (!choice && btnEdit.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    Tools.Get<EffectsManager>().choice.Play();
                    choice = true;
                    to_edit = true;
                }

                if (btnExit.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    Tools.Get<Main>().Exit();
                }

                if (choice)
                {
                    framework.alpha -= (1.0f / 30.0f);
                    flowers.alpha -= (1.0f / 30.0f);
                    light.alpha -= (1.0f / 90.0f);
                    MediaPlayer.Volume -= (1.0f / 120.0f);
                    if (MediaPlayer.Volume <= 0)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Volume = 0.7f;
                        if (to_edit)
                        {
                            Tools.Get<GameState>().SceneSwitch(new Editor());
                        }
                        else
                        {
                            Tools.Get<GameState>().SceneSwitch(new InGame());
                        }
                    }
                }
            }



            base.Update(pGameTime);
        }


        //------------------------------------------------------------


        internal override void Draw(GameTime pGameTime)
        {
            framework.Draw(pGameTime);
            flowers.Draw(pGameTime);
            light.Draw(pGameTime);

            for (byte i = 0; i <= lstButtons.Count - 1; i++)
            {
                var button = lstButtons[i];
                button.Draw(pGameTime);
            }

            base.Draw(pGameTime);
        }
    }




    //  ██╗███╗---██╗-██████╗--█████╗-███╗---███╗███████╗
    //  ██║████╗--██║██╔════╝-██╔══██╗████╗-████║██╔════╝
    //  ██║██╔██╗-██║██║--███╗███████║██╔████╔██║█████╗--
    //  ██║██║╚██╗██║██║---██║██╔══██║██║╚██╔╝██║██╔══╝--
    //  ██║██║-╚████║╚██████╔╝██║--██║██║-╚═╝-██║███████╗
    //  ╚═╝╚═╝--╚═══╝-╚═════╝-╚═╝--╚═╝╚═╝-----╚═╝╚══════╝
    class InGame : Scenes
    {
        internal byte startlevel {get;set;}
        internal LevelsManager lvlsmanager { get; set; }
        internal Bate bate { get; set; }
        internal List<Balls> lstBalls { get; set; }

        internal Buttons btn1Level { get; set; }
        internal Buttons btn2Level { get; set; }
        internal Buttons btn1Score { get; set; }
        internal Buttons btn2Score { get; set; }
        internal List<Buttons> lstButtons { get; set; }
        internal Texture2D imglifes { get; set; }
        internal Vector2 lifespos;


        //------------------------------------------------------------


        internal InGame(bool pEdit = false) : base()
        {
            if (!pEdit)
            {
                startlevel = 1;
                lvlsmanager = new LevelsManager(startlevel);
                lvlsmanager.LoadLevel(startlevel);
            } 
            else
            {
                startlevel = 5;
                lvlsmanager = new LevelsManager(startlevel);
                lvlsmanager.LoadLevel(Editor.map);
            }
            
            score = 0;
            bate = new Bate();

            lstBalls = new List<Balls>();
            lstBalls.Add(new Balls(true));

            lstButtons = new List<Buttons>();
            btn1Level = new Buttons(new Vector2(910, 650), "Level", Tools.Fonts["osake18"], Color.Black);
            btn2Level = new Buttons(new Vector2(920, 680), lvlsmanager.level.ToString(), Tools.Fonts["osake72"], Color.Black);
            btn1Score = new Buttons(new Vector2(right+((Tools.Width-right)/2)-((Tools.Fonts["japanola36"].MeasureString("Score :").X)/2),140), "Score :", Tools.Fonts["japanola36"], Color.Black);
            btn2Score = new Buttons(new Vector2(right + ((Tools.Width - right) / 2) - ((Tools.Fonts["japanola72"].MeasureString(score.ToString()).X) / 2), btn1Score.pos.Y+ btn1Score.h), "Score : " + score, Tools.Fonts["japanola72"], Color.Black);
            imglifes = Tools.Get<Main>().Content.Load<Texture2D>("images/actors/ball");
            lifespos = new Vector2(40, 0);
            lstButtons.Add(btn1Level);
            lstButtons.Add(btn2Level);
            lstButtons.Add(btn1Score);
            lstButtons.Add(btn2Score);

            MediaPlayer.IsRepeating = true;
            MediaPlayer.Play(Tools.Get<EffectsManager>().sndIngame);
        }


        //------------------------------------------------------------


        internal override void Update(GameTime pGameTime)
        {
            btn2Level.text = lvlsmanager.level.ToString();
            btn2Score.text = score.ToString();
            btn2Score.pos.X = right + ((Tools.Width - right) / 2) - ((Tools.Fonts["japanola72"].MeasureString(score.ToString()).X) / 2);

            bate.Update(pGameTime);

            lvlsmanager.Update(pGameTime, lstBalls, bate);

            for (sbyte i = (sbyte)(lstBalls.Count -1); i >= 0 ; i--)
            { 
                var ball = lstBalls[i];

                ball.Update(pGameTime);

                if (ball.overbate)
                {
                    ball.pos.Y = bate.pos.Y;
                    ball.pos.X = bate.hitboxe.X + (bate.hitboxe.Width / 2) - (ball.img.Width/2);
                }
                
                // Bate collision
                if (bate.hitboxe.Intersects(ball.hitboxe) && bate.hiting)
                {
                    Tools.Get<EffectsManager>().PlayImpact(ball);
                    Tools.Get<EffectsManager>().hitingball.Play();

                    if (ball.overbate)
                    {
                        ball.overbate = false;
                    }

                    var xball = (float)(ball.hitboxe.X + ball.hitboxe.Width);
                    var xbate1 = (float)bate.hitboxe.X;
                    var xbate2 = (float)(bate.hitboxe.X + bate.hitboxe.Width + ball.hitboxe.Width);
                    var xd = xball - xbate1;
                    var xq = xd / (xbate2 - xbate1);

                    ball.pos.Y = bate.pos.Y - ball.h;
                    ball.speed.Y = (bate.bateState == Bate.BateStates.Right) ? (((float)Math.Sin(MathHelper.ToRadians(-112.5f + (105.0f * xq)))) * ball.power.Y) : (((float)Math.Sin(MathHelper.ToRadians(-172.5f + (105.0f * xq)))) * ball.power.Y);
                    ball.speed.X = (bate.bateState == Bate.BateStates.Right) ? (((float)Math.Cos(MathHelper.ToRadians(-112.5f + (105.0f * xq)))) * ball.power.X) : (((float)Math.Cos(MathHelper.ToRadians(-172.5f + (105.0f * xq)))) * ball.power.X);
                }  

                // Bricks collisions
                for (short n = (short)(lvlsmanager.lstBricks.Count-1); n >= 0; n--)
                {
                    var brick = lvlsmanager.lstBricks[n];
                    if (brick.solid)
                    {
                        if (ball.nxtXhitboxe.Intersects(brick.hitboxe) || ball.nxtYhitboxe.Intersects(brick.hitboxe))
                        {
                            Tools.Get<EffectsManager>().PlayImpact(ball);
                            Tools.Get<EffectsManager>().bricktouched.Play();
                        }
                        if (ball.nxtXhitboxe.Intersects(brick.hitboxe))
                        {
                            ball.speed.X *= -1;
                            if (Math.Abs(ball.speed.X) < 1)
                            {
                                if ((brick.hitboxe.X+brick.hitboxe.Width) < (ball.nxtXhitboxe.X + ball.nxtXhitboxe.Width))
                                {
                                    ball.speed.X = 2;
                                }
                                if (brick.hitboxe.X > ball.nxtXhitboxe.X)
                                {
                                    ball.speed.X = -2;
                                }
                            }
                            brick.life -= 1;
                        }
                        if (ball.nxtYhitboxe.Intersects(brick.hitboxe))
                        {
                            ball.speed.Y *= -1;
                            if (Math.Abs(ball.speed.Y) < 1)
                            {
                                if ((brick.hitboxe.Y + brick.hitboxe.Height) < (ball.nxtYhitboxe.Y + ball.nxtYhitboxe.Height))
                                {
                                    ball.speed.Y = 2;
                                }
                                if (brick.hitboxe.Y > ball.nxtYhitboxe.Y)
                                {
                                    ball.speed.Y = -2;
                                }
                            }
                            brick.life -= 1;
                        }
                        if (ball.nxtXhitboxe.Intersects(brick.hitboxe) || ball.nxtYhitboxe.Intersects(brick.hitboxe))
                        {
                            if (brick.life == 2 && brick.kind == 3)
                                brick.PlayAnim("break");

                            if (brick.life == 1 && (brick.kind == 2 || brick.kind == 3))
                            {
                                Tools.Get<EffectsManager>().crack.Play();
                                brick.PlayAnim("hardbreak");
                            }

                            if (brick.kind == 4)
                            {
                                brick.lastball = ball;
                            }

                            if (brick.life <= 0)
                            {
                                brick.solid = false;
                                Tools.Get<EffectsManager>().destroy.Play();
                                brick.PlayAnim("destroy");
                            }
                        }
                    }

                    if (brick.life <= 0 && brick.frm >= 5)
                    {
                        switch (brick.kind)
                        {
                            case 1:
                                score += 10;
                                break;
                            case 2:
                                score += 20;
                                break;
                            case 3:
                                score += 30;
                                break;
                            case 4:
                                score += 10;
                                Tools.Get<EffectsManager>().pop.Play();
                                var newball = new Balls(false);
                                newball.pos = brick.lastball.pos;
                                newball.speed = brick.lastball.speed;
                                newball.speed.X *= -1;
                                newball.speed.Y *= -1;
                                newball.hitboxe.X = (int)(newball.pos.X + 1);
                                newball.hitboxe.Y = (int)(newball.pos.Y + 1);
                                newball.nxtXhitboxe.X = (int)(newball.pos.X + 1 + newball.speed.X);
                                newball.nxtXhitboxe.Y = (int)(newball.pos.Y + 7);
                                newball.nxtYhitboxe.X = (int)(newball.pos.X + 7);
                                newball.nxtYhitboxe.Y = (int)(newball.pos.Y + 1 + newball.speed.Y);
                                lstBalls.Add(newball);
                                break;
                        }
                        lvlsmanager.lstBricks.RemoveAt(n);
                    }
                }

                if (ball.outside)
                {
                    lstBalls.RemoveAt(i);
                }

                if (lstBalls.Count == 0 && lvlsmanager.lstBricks.Count != 0)
                {
                    if (bate.life > 0)
                    {
                        lstBalls.Add(new Balls(true));
                        bate.life--;
                    }
                }
            }

            //Cheat
            if (Tools.keyboard.IsKeyDown(Keys.K))
            {
                bate.life++;
                foreach (Bricks b in lvlsmanager.lstBricks)
                {
                    b.life = 0;
                    b.PlayAnim("destroy");
                }
            }

            base.Update(pGameTime);
        }


        //------------------------------------------------------------


        internal override void Draw(GameTime pGameTime)
        {
            lvlsmanager.Draw(pGameTime);

            for (sbyte i = (sbyte)(lstBalls.Count - 1); i >= 0; i--)
            {
                var ball = lstBalls[i];
                ball.Draw(pGameTime);
            }

            bate.Draw(pGameTime);

            for (byte i = 0; i < lstButtons.Count; i++)
            {
                var button = lstButtons[i];
                button.Draw(pGameTime);
            }

            for (byte i = 0; i <= bate.life-1; i++)
            {
                lifespos.Y = (770) - (imglifes.Height + 10) * i;
                Tools.Get<Main>()._spriteBatch.Draw(imglifes, lifespos, null, Color.LightPink, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
            }

            base.Draw(pGameTime);
        }


    }




    //  -██████╗--█████╗-███╗---███╗███████╗-██████╗-██╗---██╗███████╗██████╗-
    //  ██╔════╝-██╔══██╗████╗-████║██╔════╝██╔═══██╗██║---██║██╔════╝██╔══██╗
    //  ██║--███╗███████║██╔████╔██║█████╗--██║---██║██║---██║█████╗--██████╔╝
    //  ██║---██║██╔══██║██║╚██╔╝██║██╔══╝--██║---██║╚██╗-██╔╝██╔══╝--██╔══██╗
    //  ╚██████╔╝██║--██║██║-╚═╝-██║███████╗╚██████╔╝-╚████╔╝-███████╗██║--██║
    //  -╚═════╝-╚═╝--╚═╝╚═╝-----╚═╝╚══════╝-╚═════╝---╚═══╝--╚══════╝╚═╝--╚═╝
    class GameOver : Scenes
    {
        internal List<Buttons> lstButtons { get; set; }
        internal Color selectcolor { get; set; }
        internal Buttons btnGame { get; set; }
        internal Buttons btnOver { get; set; }
        internal Buttons btnScore { get; set; }
        internal Buttons btnRetry { get; set; }
        internal Buttons btnExit { get; set; }

        internal Sprites light { get; set; }


        //------------------------------------------------------------


        internal GameOver() : base()
        {
            lstButtons = new List<Buttons>();
            selectcolor = new Color(229, 159, 181);

            btnGame = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["japanola72"].MeasureString("Game").X / 2)), up +90), "Game", Tools.Fonts["japanola72"], Color.White);
            btnOver = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["japanola72"].MeasureString("Over").X / 2)), up + btnGame.pos.Y + btnGame.h - 60), "Over", Tools.Fonts["japanola72"], Color.White);
            btnScore = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["japanola18"].MeasureString("Score : "+score).X / 2)), up + btnOver.pos.Y + btnOver.h), "Score : "+score, Tools.Fonts["japanola18"], Color.White);
            btnRetry = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["osake36"].MeasureString("Retry").X / 2)), up + (((down - up) / 5) * 3)), "Retry", Tools.Fonts["osake36"], Color.White, selectcolor);
            btnExit = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["osake36"].MeasureString("Exit").X / 2)), up + btnRetry.pos.Y + 30), "Exit", Tools.Fonts["osake36"], Color.White, selectcolor);

            lstButtons.Add(btnGame);
            lstButtons.Add(btnOver);
            lstButtons.Add(btnScore);
            lstButtons.Add(btnRetry);
            lstButtons.Add(btnExit);

            foreach (Buttons b in lstButtons)
                b.alpha = 0;

            light = new Sprites();
            light.img = Tools.Get<Main>().Content.Load<Texture2D>("images/display/light2");
            light.orgn = new Vector2(light.img.Width/2, light.img.Height/2);
            light.pos = new Vector2( btnGame.pos.X+(btnGame.w/2), btnGame.pos.Y+(btnGame.h) );
            light.color = Color.Red;
            light.alpha = 0;

            MediaPlayer.IsRepeating = true;
            Tools.Get<EffectsManager>().sndGameover.Play();
            MediaPlayer.Volume = 0.7f;
        }


        //------------------------------------------------------------


        internal override void Update(GameTime pGameTime)
        {
            if (!choice)
            {
                framework.alpha += (1.0f / 60.0f);
                if (framework.alpha >= 1)
                {
                    framework.alpha = 1.0f;
                    light.alpha += (1.0f / 120.0f);
                    if (light.alpha >= 0.8)
                        light.alpha = 0.8f;
                }
            }

            timer += (1.0f / 60.0f);

            if (timer >= 0.8)
            {

                for (byte i = 0; i <= lstButtons.Count - 1; i++)
                {
                    var button = lstButtons[i];
                    if (!choice)
                    {
                        button.alpha += (1.0f / 60.0f);
                        if (button.alpha >= 1)
                        {
                            button.alpha = 1.0f;
                        }
                    }
                    else
                    {
                        button.alpha -= (1.0f / 60.0f);
                        if (button.alpha <= 0)
                        {
                            button.alpha = 0;
                        }
                    }

                    if (button.usable)
                    {
                        if (button.hitboxe.Contains(Tools.mouse.Position) && !choice)
                        {
                            if (!button.clickable)
                            {
                                Tools.Get<EffectsManager>().select.Play();
                                button.clickable = true;
                            }
                        }
                        else
                        {
                            button.clickable = false;
                        }
                    }
                }

                if (!choice && btnRetry.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    Tools.Get<EffectsManager>().choice.Play();
                    choice = true;
                }

                if (btnExit.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    Tools.Get<Main>().Exit();
                }

                if (choice)
                {
                    framework.alpha -= (1.0f / 30.0f);
                    light.alpha -= (1.0f / 90.0f);

                    MediaPlayer.Volume -= (1.0f / 120.0f);
                    if (MediaPlayer.Volume <= 0)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Volume = 0.7f;
                        Tools.Get<GameState>().SceneSwitch(new Menu());
                    }
                }
            }

            base.Update(pGameTime);
        }


        //------------------------------------------------------------


        internal override void Draw(GameTime pGameTime)
        {
            framework.Draw(pGameTime);
            light.Draw(pGameTime);

            for (byte i = 0; i <= lstButtons.Count - 1; i++)
            {
                var button = lstButtons[i];
                button.Draw(pGameTime);
            }

            base.Draw(pGameTime);
        }


    }




    //  ██╗---██╗██╗-██████╗████████╗-██████╗-██████╗-██╗---██╗
    //  ██║---██║██║██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗╚██╗-██╔╝
    //  ██║---██║██║██║--------██║---██║---██║██████╔╝-╚████╔╝-
    //  ╚██╗-██╔╝██║██║--------██║---██║---██║██╔══██╗--╚██╔╝--
    //  -╚████╔╝-██║╚██████╗---██║---╚██████╔╝██║--██║---██║---
    //  --╚═══╝--╚═╝-╚═════╝---╚═╝----╚═════╝-╚═╝--╚═╝---╚═╝---
    class Victory : Scenes
    {
        internal List<Buttons> lstButtons { get; set; }
        internal Color selectcolor { get; set; }
        internal Buttons btnVictory { get; set; }
        internal Buttons btnScore { get; set; }
        internal Buttons btnMenu { get; set; }
        internal Buttons btnExit { get; set; }

        internal Sprites bird { get; set; }

        internal List<Fireworks> lstFireworks { get; set; }
        internal byte nfireworks { get; set; }
        internal float frwrktimer { get; set; }


        //------------------------------------------------------------


        internal Victory() : base()
        {
            lstButtons = new List<Buttons>();
            selectcolor = new Color(229, 159, 181);

            btnVictory = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["japanola72"].MeasureString("Victory").X / 2)), up + 160), "Victory", Tools.Fonts["japanola72"], Color.White);
            btnScore = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["japanola18"].MeasureString("Score : " + score).X / 2)), up + btnVictory.pos.Y + btnVictory.h), "Score : " + score, Tools.Fonts["japanola18"], Color.White);
            btnMenu = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["osake36"].MeasureString("Menu").X / 2)), up + (((down - up) / 5) * 3)), "Menu", Tools.Fonts["osake36"], Color.White, selectcolor);
            btnExit = new Buttons(new Vector2(left + (((right - left) / 2) - (Tools.Fonts["osake36"].MeasureString("Exit").X / 2)), up + btnMenu.pos.Y + 30), "Exit", Tools.Fonts["osake36"], Color.White, selectcolor);

            lstButtons.Add(btnVictory);
            lstButtons.Add(btnScore);
            lstButtons.Add(btnMenu);
            lstButtons.Add(btnExit);

            foreach (Buttons b in lstButtons)
                b.alpha = 0;

            bird = new Sprites();
            bird.img = Tools.Get<Main>().Content.Load<Texture2D>("images/display/bird");
            bird.pos = slatecorner;
            bird.alpha = 0;

            lstFireworks = new List<Fireworks>();
            nfireworks = 0;
            frwrktimer = 1.0f;

            MediaPlayer.IsRepeating = false;
            MediaPlayer.Play(Tools.Get<EffectsManager>().sndVictory);
            MediaPlayer.Volume = 0.7f;
        }


        //------------------------------------------------------------


        internal override void Update(GameTime pGameTime)
        {
            if (!choice)
            {
                framework.alpha += (1.0f / 60.0f);
                if (framework.alpha >= 1)
                {
                    framework.alpha = 1.0f;
                    bird.alpha += (1.0f / 120.0f);
                    if (bird.alpha >= 1)
                        bird.alpha = 1.0f;
                }
            }

            timer += (1.0f / 60.0f);

            if (timer >= 0.8)
            {
                if (!choice)
                {
                    if ((timer >= 1.8 && nfireworks == 0) || (timer >= 2.3 && nfireworks == 1) || (timer >= 3.3 && nfireworks == 2) || (timer >= 3.8 && nfireworks == 3) || (timer >= 4.05 && nfireworks == 4))
                    {
                        lstFireworks.Add(new Fireworks());
                        nfireworks++;
                    }
                    else if (nfireworks > 4)
                    {
                        frwrktimer -= (1.0f / 60.0f);
                        if (frwrktimer <= 0)
                        {
                            frwrktimer = Tools.Rnd(5, 20)/10.0f;
                            lstFireworks.Add(new Fireworks());
                        }
                    }
                }

                for (byte i = 0; i <= lstButtons.Count - 1; i++)
                {
                    var button = lstButtons[i];
                    if (!choice)
                    {
                        button.alpha += (1.0f / 60.0f);
                        if (button.alpha >= 1)
                        {
                            button.alpha = 1.0f;
                        }
                    }
                    else
                    {
                        button.alpha -= (1.0f / 60.0f);
                        if (button.alpha <= 0)
                        {
                            button.alpha = 0;
                        }
                    }

                    if (button.usable)
                    {
                        if (button.hitboxe.Contains(Tools.mouse.Position) && !choice)
                        {
                            if (!button.clickable)
                            {
                                Tools.Get<EffectsManager>().select.Play();
                                button.clickable = true;
                            }
                        }
                        else
                        {
                            button.clickable = false;
                        }
                    }
                }

                if (!choice && btnMenu.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    Tools.Get<EffectsManager>().choice.Play();
                    choice = true;
                }

                if (btnExit.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    Tools.Get<Main>().Exit();
                }

                if (choice)
                {
                    framework.alpha -= (1.0f / 30.0f);
                    MediaPlayer.Volume -= (1.0f / 120.0f);
                    if (MediaPlayer.Volume <= 0)
                    {
                        MediaPlayer.Stop();
                        MediaPlayer.Volume = 0.7f;
                        Tools.Get<GameState>().SceneSwitch(new Menu());
                    }
                }
            }

            for (sbyte i = (sbyte)(lstFireworks.Count-1); i >= 0; i--)
            {
                var frwrk = lstFireworks[i];
                frwrk.Update(pGameTime);
                if (frwrk.done)
                    lstFireworks.RemoveAt(i);
            }

            Tools.Get<EffectsManager>().Update(pGameTime);

            base.Update(pGameTime);
        }


        //------------------------------------------------------------


        internal override void Draw(GameTime pGameTime)
        {
            framework.Draw(pGameTime);
            bird.Draw(pGameTime);

            Tools.Get<EffectsManager>().Draw(pGameTime);

            for (byte i = 0; i <= lstButtons.Count - 1; i++)
            {
                var button = lstButtons[i];
                button.Draw(pGameTime);
            }

            base.Draw(pGameTime);
        }


    }




    //  ███████╗██╗██████╗-███████╗██╗----██╗-██████╗-██████╗-██╗--██╗███████╗
    //  ██╔════╝██║██╔══██╗██╔════╝██║----██║██╔═══██╗██╔══██╗██║-██╔╝██╔════╝
    //  █████╗--██║██████╔╝█████╗--██║-█╗-██║██║---██║██████╔╝█████╔╝-███████╗
    //  ██╔══╝--██║██╔══██╗██╔══╝--██║███╗██║██║---██║██╔══██╗██╔═██╗-╚════██║
    //  ██║-----██║██║--██║███████╗╚███╔███╔╝╚██████╔╝██║--██║██║--██╗███████║
    //  ╚═╝-----╚═╝╚═╝--╚═╝╚══════╝-╚══╝╚══╝--╚═════╝-╚═╝--╚═╝╚═╝--╚═╝╚══════╝
    class Fireworks
    {
        internal float timer { get; set; }
        internal bool explosed { get; set; }
        internal bool done { get; set; }
        internal List<Color> lstColors {get;set;}

        //------------------------------------------------------------
        internal Fireworks()
        {
            timer = 0;
            explosed = false;
            done = false;
            lstColors = new List<Color>() { Color.Red, Color.LimeGreen, Color.Blue, Color.Yellow, Color.Violet, Color.Aqua, Color.Orange, Color.Green };
            Tools.Get<EffectsManager>().whistle.Play();
        }
        //------------------------------------------------------------
        internal void Update(GameTime pGameTime)
        {
            timer += (1.0f / 60.0f);

            if (timer >= 1 && !explosed)
            {
                explosed = true;
                var icolor = Tools.Rnd(0, 5);
                var lclx = Tools.Rnd(Scenes.Slate[Scenes.SlateEdges.Left] + 100, Scenes.Slate[Scenes.SlateEdges.Right] - 100);
                var lcly = Tools.Rnd(Scenes.Slate[Scenes.SlateEdges.Up] + 100, Scenes.Slate[Scenes.SlateEdges.Up] + (Scenes.slate.Height / 2));
                Tools.Get<EffectsManager>().PlayExplosion(200, new Vector2(lclx, lcly), lstColors[icolor]);
                Tools.Get<EffectsManager>().firework.Play();
            }

            if (timer >= 3)
                done = true;
        }
    }
}
