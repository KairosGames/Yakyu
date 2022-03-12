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
    //  ███████╗██████╗-██╗████████╗-██████╗-██████╗-
    //  ██╔════╝██╔══██╗██║╚══██╔══╝██╔═══██╗██╔══██╗
    //  █████╗--██║--██║██║---██║---██║---██║██████╔╝
    //  ██╔══╝--██║--██║██║---██║---██║---██║██╔══██╗
    //  ███████╗██████╔╝██║---██║---╚██████╔╝██║--██║
    //  ╚══════╝╚═════╝-╚═╝---╚═╝----╚═════╝-╚═╝--╚═╝
    class Editor : Scenes
    {
        internal bool launched { get; set; }

        internal static Texture2D currentEditPrint { get; set; } = Tools.Get<EffectsManager>().print1;
        internal static byte printswitcher { get; set; } = 1;

        internal static byte[,] map { get; set; } =  new byte[22, 8];
        internal byte columns { get; set; }
        internal byte lines { get; set; }
        internal int mapx { get; set; }

        private Bricks brickref { get; set; }

        internal List<Buttons> lstButtons { get; set; }
        internal Buttons btnExit { get; set; }
        internal Buttons btnRefresh { get; set; }
        internal Buttons btnDelete { get; set; }
        internal Buttons btnLaunch { get; set; }

        internal Vector2 pos_print { get; set; }
        internal float alpha { get; set; }
        internal Texture2D print1 { get; set; }
        internal Texture2D print2 { get; set; }
        internal Texture2D print3 { get; set; }
        internal Texture2D print4 { get; set; }
        
        internal Texture2D imgbrick { get; set; }
        internal Color colorB1 { get; set; }
        internal Color colorB2 { get; set; }
        internal Color colorB3 { get; set; }
        internal Color colorB4 { get; set; }


        //------------------------------------------------------------


        internal Editor() : base()
        {
            brickref = Tools.Get<EffectsManager>().brickref;

            pos_print = new Vector2(left, up);
            print1 = Tools.Get<EffectsManager>().print1;
            print2 = Tools.Get<EffectsManager>().print2;
            print3 = Tools.Get<EffectsManager>().print3;
            print4 = Tools.Get<EffectsManager>().print4;
            alpha = 0;

            launched = false;
            
            columns = (byte)map.GetLength(1);
            lines = (byte)map.GetLength(0);
            mapx = (slate.Width - (brickref.hitboxe.Width * columns)) / 2;

            lstButtons = new List<Buttons>();
            btnExit = new Buttons(Tools.Get<EffectsManager>().exit, new Vector2(left + 25, down-100), Color.White, Color.Red);
            btnRefresh = new Buttons(Tools.Get<EffectsManager>().refresh, new Vector2(left + ((right-left)/2) - 50, down - 100), Color.SaddleBrown, Color.Red);
            btnDelete = new Buttons(Tools.Get<EffectsManager>().delete, new Vector2(right - 125, down - 100), Color.White, Color.Red);
            btnLaunch = new Buttons(new Vector2(750, 200), "Launch", Tools.Fonts["osake72"], Color.Black, Color.DarkRed);
            lstButtons.Add(btnExit);
            lstButtons.Add(btnRefresh);
            lstButtons.Add(btnDelete);
            lstButtons.Add(btnLaunch);

            for (byte lin = 0; lin < lines; lin++)
                for (byte col = 0; col < columns; col++)
                {
                    var brickbtn = new Buttons(new Vector2((col * brickref.hitboxe.Width) + left + mapx - brickref.hitboxe.X, lin * (brickref.hitboxe.Height) + up - brickref.hitboxe.Y), lin, col);
                    lstButtons.Add(brickbtn);
                }

            imgbrick = Tools.Get<EffectsManager>().imgbrick;
            colorB1 = new Color(228, 145, 97, 255);
            colorB2 = new Color(144, 95, 67, 255);
            colorB3 = new Color(74, 47, 33, 255);
            colorB4 = new Color(229, 159, 181, 255);

        }
    

        //------------------------------------------------------------


        internal override void Update(GameTime pGameTime)
        {
            if (!choice)
            {
                timer += (1.0f / 60.0f);

                alpha += (1.0f / 60.0f);
                if (alpha >= 1)
                {
                    alpha = 1.0f;
                }

                for (short i = 0; i < lstButtons.Count; i++)
                {
                    var btn = lstButtons[i];

                    if (btn.usable)
                    {
                        if (btn.hitboxe.Contains(Tools.mouse.Position))
                        {
                            if (!btn.clickable)
                            {
                                btn.clickable = true;
                                if (btn.kind != 4)
                                {
                                    Tools.Get<EffectsManager>().select.Play();
                                }
                            }
                        }
                        else
                        {
                            btn.clickable = false;
                        }


                        if (btn.clickable && btn.kind == 4 && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                        {
                            map[btn.line, btn.column]++;
                            if (map[btn.line, btn.column] >= 5)
                            {
                                Tools.Get<EffectsManager>().brickout.Play();
                                map[btn.line, btn.column] = 0;
                            }
                            else
                            {
                                Tools.Get<EffectsManager>().stone.Play();
                            }
                        }
                        if (btn.clickable && btn.kind == 4 && Tools.mouse.RightButton == ButtonState.Pressed && Tools.prv_mouse.RightButton != ButtonState.Pressed)
                        {
                            if (map[btn.line, btn.column] > 0)
                            {
                                map[btn.line, btn.column]--;
                                if (map[btn.line, btn.column] == 0)
                                {
                                    Tools.Get<EffectsManager>().brickout.Play();
                                }
                                else
                                {
                                    Tools.Get<EffectsManager>().stone.Play();
                                }
                            } 
                            else
                            {
                                Tools.Get<EffectsManager>().stone.Play();
                                map[btn.line, btn.column] = 4;
                            }
                        }
                    } 
                }

                if (btnDelete.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    Tools.Get<EffectsManager>().brickout.Play();
                    for (byte lin = 0; lin < lines; lin++)
                        for (byte col = 0; col < columns; col++)
                            map[lin, col] = 0;
                }

                if (btnRefresh.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    printswitcher++;
                    if (printswitcher >= 5)
                    {
                        printswitcher = 1;
                    }
                    switch (printswitcher)
                    {
                        case 1:
                            currentEditPrint = print1;
                            break;
                        case 2:
                            currentEditPrint = print2;
                            break;
                        case 3:
                            currentEditPrint = print3;
                            break;
                        case 4:
                            currentEditPrint = print4;
                            break;
                    }
                    Tools.Get<EffectsManager>().pop.Play();
                }

                if (btnExit.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    Tools.Get<EffectsManager>().choice.Play();
                    choice = true;
                }

                if (btnLaunch.clickable && Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
                {
                    Tools.Get<EffectsManager>().choice.Play();
                    choice = true;
                    launched = true;
                }

            } 
            else
            {
                alpha -= (1.0f / 60.0f);
                if (alpha <= 0)
                {
                    if (launched)
                    {
                        Tools.Get<GameState>().current = new InGame(true);
                    }
                    else
                    {
                        Tools.Get<GameState>().current = new Menu();
                    }
                }
                
            }

            base.Update(pGameTime);
        }


        //------------------------------------------------------------

        internal override void Draw(GameTime pGameTime)
        {
            Tools.Get<Main>()._spriteBatch.Draw(currentEditPrint, pos_print, null, Color.White*alpha, 0, Vector2.Zero, 1.0f, SpriteEffects.None, 0);

            for (byte lin = 0; lin < lines; lin++)
                for (byte col = 0; col < columns; col++)
                {
                    switch (map[lin, col])
                    {
                        case 1:
                            Tools.Get<Main>()._spriteBatch.Draw(imgbrick, new Vector2((col * brickref.hitboxe.Width) + left + mapx - brickref.hitboxe.X, lin * (brickref.hitboxe.Height) + up - brickref.hitboxe.Y), brickref.frmrec, colorB1*alpha);
                            break;
                        case 2:
                            Tools.Get<Main>()._spriteBatch.Draw(imgbrick, new Vector2((col * brickref.hitboxe.Width) + left + mapx - brickref.hitboxe.X, lin * (brickref.hitboxe.Height) + up - brickref.hitboxe.Y), brickref.frmrec, colorB2*alpha);
                            break;
                        case 3:
                            Tools.Get<Main>()._spriteBatch.Draw(imgbrick, new Vector2((col * brickref.hitboxe.Width) + left + mapx - brickref.hitboxe.X, lin * (brickref.hitboxe.Height) + up - brickref.hitboxe.Y), brickref.frmrec, colorB3*alpha);
                            break;
                        case 4:
                            Tools.Get<Main>()._spriteBatch.Draw(imgbrick, new Vector2((col * brickref.hitboxe.Width) + left + mapx - brickref.hitboxe.X, lin * (brickref.hitboxe.Height) + up - brickref.hitboxe.Y), brickref.frmrec, colorB4*alpha);
                            break;
                    }
                }

            for (short i = 0; i < lstButtons.Count; i++)
            {
                var btn = lstButtons[i];
                btn.Draw(pGameTime);
            }

            base.Draw(pGameTime);
        }
    }
}
