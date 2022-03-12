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
    //  ██████╗--█████╗-████████╗███████╗
    //  ██╔══██╗██╔══██╗╚══██╔══╝██╔════╝
    //  ██████╔╝███████║---██║---█████╗--
    //  ██╔══██╗██╔══██║---██║---██╔══╝--
    //  ██████╔╝██║--██║---██║---███████╗
    //  ╚═════╝-╚═╝--╚═╝---╚═╝---╚══════╝
    class Bate : Sprites
    {
        internal int up { get; set; }
        internal int down { get; set; }
        internal int left { get; set; }
        internal int right { get; set; }

        internal enum BateStates { Right, Left}
        internal BateStates bateState { get; set; }
        internal bool hiting { get; set; }


        //------------------------------------------------------------



        internal Bate() : base(true, 91, 86)
        {
            shadow = true;

            up = Scenes.Slate[Scenes.SlateEdges.Up];
            down = Scenes.Slate[Scenes.SlateEdges.Down];
            left = Scenes.Slate[Scenes.SlateEdges.Left];
            right = Scenes.Slate[Scenes.SlateEdges.Right];

            life = 5;
            bateState = BateStates.Right;
            hiting = false;

            hitboxe = new Rectangle(31, 11, 59, 30);

            img = Tools.Get<Main>().Content.Load<Texture2D>("images/actors/bate");
            pos = new Vector2(0, down - h);
            AddAnim("hit", new List<byte>() { 0, 1, 4, 4, 4, 4, 3, 2, 1, 1, 0 }, 0.008f);
        }


        //------------------------------------------------------------


        internal override void Update(GameTime pGameTime)
        {
            if (bateState == BateStates.Right)
            {
                pos.X = Tools.mouse.X;
                sprteff = SpriteEffects.None;

                hitboxe.X = (int)(pos.X + 31);
                hitboxe.Y = (int)(pos.Y + 11);

            } else
            {
                pos.X = Tools.mouse.X - w;
                sprteff = SpriteEffects.FlipHorizontally;

                hitboxe.X = (int)(pos.X + w - hitboxe.Width - 33);
                hitboxe.Y = (int)(pos.Y + 11);
            }

            if (frm >= 2)
            {
                hiting = true;
            } else
            {
                hiting = false;
            }

            if (pos.X <= left)
            {
                pos.X = left;
                hitboxe.X = (bateState == BateStates.Right) ? (int)(pos.X + 31) : (int)(pos.X + w - hitboxe.Width - 33);
            } else if (pos.X >= right - w)
            {
                pos.X = right - w;
                hitboxe.X = (bateState == BateStates.Right) ? (int)(pos.X + 31) : (int)(pos.X + w - hitboxe.Width - 33);
            }

            if (Tools.mouse.LeftButton == ButtonState.Pressed && Tools.prv_mouse.LeftButton != ButtonState.Pressed)
            {
                PlayAnim("hit");
                Tools.Get<EffectsManager>().batehit.Play();
            }

            if (Tools.mouse.RightButton == ButtonState.Pressed && Tools.prv_mouse.RightButton != ButtonState.Pressed)
            {
                bateState = (bateState == BateStates.Right) ? BateStates.Left : BateStates.Right;
                Tools.Get<EffectsManager>().bateswitch.Play();
            }

            base.Update(pGameTime);
        }


        //------------------------------------------------------------


        internal override void Draw(GameTime pGameTime)
        {
            base.Draw(pGameTime);
        }


    }




    //  ██████╗--█████╗-██╗-----██╗-----███████╗
    //  ██╔══██╗██╔══██╗██║-----██║-----██╔════╝
    //  ██████╔╝███████║██║-----██║-----███████╗
    //  ██╔══██╗██╔══██║██║-----██║-----╚════██║
    //  ██████╔╝██║--██║███████╗███████╗███████║
    //  ╚═════╝-╚═╝--╚═╝╚══════╝╚══════╝╚══════╝
    class Balls : Sprites
    {
        internal int up { get; set; }
        internal int down { get; set; }
        internal int left { get; set; }
        internal int right { get; set; }

        internal bool overbate { get; set; }
        internal bool outside { get; set; }

        internal List<Trails> lstTrails { get; set; }

        internal Balls(bool pOverBate) : base()
        {
            shadow = true;

            lstTrails = new List<Trails>();

            up = Scenes.Slate[Scenes.SlateEdges.Up];
            down = Scenes.Slate[Scenes.SlateEdges.Down];
            left = Scenes.Slate[Scenes.SlateEdges.Left];
            right = Scenes.Slate[Scenes.SlateEdges.Right];

            img = Tools.Get<Main>().Content.Load<Texture2D>("images/actors/ball");
            color = Color.LightPink;
            w = img.Width;
            h = img.Height;

            hitboxe = new Rectangle(1, 1, 13, 13);
            nxtXhitboxe = new Rectangle(1, 7, 13, 1);
            nxtYhitboxe = new Rectangle(7, 1, 1, 13);

            outside = false;
            overbate = pOverBate;
            

            speed = new Vector2();
            power = new Vector2(10.0f, 10.0f);
            weight = new Vector2(0, 0.05f);
            friction = new Vector2(0.005f, 0);


        }

        //------------------------------------------------------------


        internal override void Update(GameTime pGameTime)
        {
            if (!overbate)
            {
                pos += speed;
                speed += weight;
                if (speed.X > 0)
                {
                    speed -= friction;
                    if (speed.X <= 0)
                    {
                        speed.X = 0;
                    }
                } else if (speed.X < 0)
                {
                    speed += friction;
                    if (speed.X >= 0)
                    {
                        speed.X = 0;
                    }
                }

                if (speed.Y >= 10.0f)
                {
                    speed.Y = 10.0f;
                }

                if ((hitboxe.X >= right-hitboxe.Width) || (hitboxe.X <= left))
                {
                    Tools.Get<EffectsManager>().PlayImpact(this);
                    Tools.Get<EffectsManager>().wall.Play();
                    pos.X = (pos.X > left + (right / 2)) ? right - w : left;
                    speed.X *= -1;
                }

                if ((hitboxe.Y <= up)) 
                {
                    Tools.Get<EffectsManager>().PlayImpact(this);
                    Tools.Get<EffectsManager>().wall.Play();
                    pos.Y = up;
                    speed.Y *= -1;
                }

                if (pos.Y >= down+(down/4))
                {
                    outside = true;
                    Tools.Get<EffectsManager>().ballout.Play();
                }

                lstTrails.Add(new Trails(img, color, pos, 0.6f, (1.0f / 10.0f)));
            }

            hitboxe.X = (int)(pos.X + 1);
            hitboxe.Y = (int)(pos.Y + 1);

            nxtXhitboxe.X = (int)(pos.X + 1 + speed.X);
            nxtXhitboxe.Y = (int)(pos.Y + 7);

            nxtYhitboxe.X = (int)(pos.X + 7);
            nxtYhitboxe.Y = (int)(pos.Y + 1 + speed.Y);

            for (short i = (short)(lstTrails.Count-1); i >= 0; i--)
            {
                var trail = lstTrails[i];

                trail.Update(pGameTime);
                
                if (trail.alpha <= 0)
                {
                    lstTrails.RemoveAt(i);
                }
            }


            base.Update(pGameTime);
        }


        //------------------------------------------------------------


        internal override void Draw(GameTime pGameTime)
        {
            for (short i = 0;  i < lstTrails.Count; i++)
            {
                var trail = lstTrails[i];
                trail.Draw(pGameTime);
            }

            base.Draw(pGameTime);
        }


        //------------------------------------------------------------
    }




    //  ██████╗-██████╗-██╗-██████╗██╗--██╗███████╗
    //  ██╔══██╗██╔══██╗██║██╔════╝██║-██╔╝██╔════╝
    //  ██████╔╝██████╔╝██║██║-----█████╔╝-███████╗
    //  ██╔══██╗██╔══██╗██║██║-----██╔═██╗-╚════██║
    //  ██████╔╝██║--██║██║╚██████╗██║--██╗███████║
    //  ╚═════╝-╚═╝--╚═╝╚═╝-╚═════╝╚═╝--╚═╝╚══════╝
    class Bricks : Sprites
    {
        internal int up { get; set; }
        internal int down { get; set; }
        internal int left { get; set; }
        internal int right { get; set; }

        internal bool solid { get; set; }

        internal Balls lastball { get; set; }

        internal float Rtimer { get; set; }
        internal float Gtimer { get; set; }
        internal float Btimer { get; set; }
        internal byte Rmax { get; set; }
        internal byte Gmax { get; set; }
        internal byte Bmax { get; set; }


        //------------------------------------------------------------


        internal Bricks(byte pKind, int pX, int pY) : base(true, 70, 47)
        {
            img = Tools.Get<EffectsManager>().imgbrick;
            shadow = true;
            Rtimer = 0;
            Gtimer = 0;
            Btimer = 0;

            kind = pKind;
            switch (pKind)
            {
                case 1:
                    life = 1;
                    color = new Color(228,145,97,255);
                    AddAnim("destroy", new List<byte> { 0, 1, 2, 3, 4, 4, 5 }, 0.04f, false, true);
                    break;
                case 2:
                    life = 2;
                    color = color = new Color(144, 95, 67, 255);
                    AddAnim("hardbreak", new List<byte> { 0, 1, 1, 2 }, 0.04f, false, true);
                    AddAnim("destroy", new List<byte> { 2, 3, 4, 4, 5 }, 0.04f, false, true);
                    break;
                case 3:
                    life = 3;
                    color = color = new Color(74, 47, 33, 255);
                    AddAnim("break", new List<byte> { 0, 1 }, 0.04f, false, true);
                    AddAnim("hardbreak", new List<byte> { 1, 2 }, 0.04f, false, true);
                    AddAnim("destroy", new List<byte> { 2, 3, 4, 4, 5 }, 0.04f, false, true);
                    break;
                case 4:
                    life = 1;
                    lastball = new Balls(false);
                    color = color = new Color(229, 159, 181, 255);
                    AddAnim("destroy", new List<byte> { 0, 1, 2, 3, 4, 4, 5 }, 0.04f, false, true);
                    break;
            }

            Rmax = color.R;
            Gmax = color.G;
            Bmax = color.B;

            pos = new Vector2(pX, pY);
            hitboxe = new Rectangle((int)pos.X+8, (int)pos.Y+10, 56,28);
            solid = true;
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




    //  ██████╗-██╗---██╗████████╗████████╗-██████╗-███╗---██╗███████╗
    //  ██╔══██╗██║---██║╚══██╔══╝╚══██╔══╝██╔═══██╗████╗--██║██╔════╝
    //  ██████╔╝██║---██║---██║------██║---██║---██║██╔██╗-██║███████╗
    //  ██╔══██╗██║---██║---██║------██║---██║---██║██║╚██╗██║╚════██║
    //  ██████╔╝╚██████╔╝---██║------██║---╚██████╔╝██║-╚████║███████║
    //  ╚═════╝--╚═════╝----╚═╝------╚═╝----╚═════╝-╚═╝--╚═══╝╚══════╝
    class Buttons : Sprites
    {
        internal string text { get; set; }
        internal Vector2 size { get; set; }
        internal SpriteFont font { get; set; }
        internal bool usable { get; set; }
        internal bool clickable { get; set; }
        internal Color selectcolor { get; set; }

        internal Bricks brickref { get; set; } 
        internal byte column { get; set; }
        internal byte line { get; set; }


        //------------------------------------------------------------


        internal Buttons(Vector2 pPos, string pText, SpriteFont pFont, Color pColor) : base()
        {
            kind = 1;
            pos = pPos;
            text = pText;
            font = pFont;
            color = pColor;
            size = pFont.MeasureString(pText);
            w = (int)size.X;
            h = (int)size.Y;

            usable = false;
        }

        internal Buttons(Vector2 pPos, string pText, SpriteFont pFont, Color pColor, Color pSelectcolor) : base()
        {
            kind = 2;
            pos = pPos;
            text = pText;
            font = pFont;
            color = pColor;
            selectcolor = pSelectcolor;
            size = pFont.MeasureString(pText);
            w = (int)size.X;
            h = (int)size.Y;

            usable = true;
            clickable = false;
            hitboxe = new Rectangle((int)pPos.X, (int)pPos.Y, w, h);
        }

        internal Buttons(Texture2D pImg, Vector2 pPos, Color pColor, Color pSelectcolor) : base()
        {
            kind = 3;
            img = pImg;
            w = img.Width;
            h = img.Height;
            pos = pPos;
            color = pColor;
            selectcolor = pSelectcolor;

            usable = true;
            clickable = false;
            hitboxe = new Rectangle((int)pPos.X, (int)pPos.Y, w, h);
        }

        internal Buttons(Vector2 pPos, byte pLine, byte pColumn) : base(true, 70, 47)
        {
            kind = 4;
            brickref = Tools.Get<EffectsManager>().brickref;
            line = pLine;
            column = pColumn;

            img = Tools.Get<EffectsManager>().imgbrick;
            pos = pPos;
            color = Color.Black;
            alpha = 0.5f;

            usable = true;
            clickable = false;
            hitboxe = new Rectangle((int)pPos.X + brickref.hitboxe.X, (int)pPos.Y + brickref.hitboxe.Y, brickref.hitboxe.Width, brickref.hitboxe.Height);
        }


        //------------------------------------------------------------


        internal new void Draw(GameTime pGameTime)
        {
            switch (kind)
            {
                case 1:
                    Tools.Get<Main>()._spriteBatch.DrawString(font, text, pos, color * alpha, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
                    break;
                case 2:
                    if (clickable)
                    {
                        Tools.Get<Main>()._spriteBatch.DrawString(font, text, pos, selectcolor * alpha, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
                    }
                    else
                    {
                        Tools.Get<Main>()._spriteBatch.DrawString(font, text, pos, color * alpha, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
                    }
                    break;
                case 3:
                    if (clickable)
                    {
                        Tools.Get<Main>()._spriteBatch.Draw(img, pos, null, selectcolor * alpha, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
                    }
                    else
                    {
                        Tools.Get<Main>()._spriteBatch.Draw(img, pos, null, color * alpha, 0, Vector2.Zero, 1, SpriteEffects.None, 0.8f);
                    }
                    break;
                case 4:
                    if (clickable)
                    {
                        Tools.Get<Main>()._spriteBatch.Draw(img, pos, frmrec, color * alpha, 0, Vector2.Zero, 1, SpriteEffects.None, 0.2f);
                    }
                    break;
            }

        }
    }
}
