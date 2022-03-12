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
    public class Main : Game
    {

        private GraphicsDeviceManager _graphics;
        internal SpriteBatch _spriteBatch;

        GameState gameState;
        EffectsManager effsmanager;
        



        //  -██████╗-██████╗-███╗---██╗███████╗████████╗██████╗-██╗---██╗-██████╗████████╗-██████╗-██████╗-
        //  ██╔════╝██╔═══██╗████╗--██║██╔════╝╚══██╔══╝██╔══██╗██║---██║██╔════╝╚══██╔══╝██╔═══██╗██╔══██╗
        //  ██║-----██║---██║██╔██╗-██║███████╗---██║---██████╔╝██║---██║██║--------██║---██║---██║██████╔╝
        //  ██║-----██║---██║██║╚██╗██║╚════██║---██║---██╔══██╗██║---██║██║--------██║---██║---██║██╔══██╗
        //  ╚██████╗╚██████╔╝██║-╚████║███████║---██║---██║--██║╚██████╔╝╚██████╗---██║---╚██████╔╝██║--██║
        //  -╚═════╝-╚═════╝-╚═╝--╚═══╝╚══════╝---╚═╝---╚═╝--╚═╝-╚═════╝--╚═════╝---╚═╝----╚═════╝-╚═╝--╚═╝
        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            Tools.AddService(_graphics);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            Tools.AddService(this);
        }




        //  ██╗███╗---██╗██╗████████╗██╗-█████╗-██╗-----██╗███████╗███████╗
        //  ██║████╗--██║██║╚══██╔══╝██║██╔══██╗██║-----██║╚══███╔╝██╔════╝
        //  ██║██╔██╗-██║██║---██║---██║███████║██║-----██║--███╔╝-█████╗--
        //  ██║██║╚██╗██║██║---██║---██║██╔══██║██║-----██║-███╔╝--██╔══╝--
        //  ██║██║-╚████║██║---██║---██║██║--██║███████╗██║███████╗███████╗
        //  ╚═╝╚═╝--╚═══╝╚═╝---╚═╝---╚═╝╚═╝--╚═╝╚══════╝╚═╝╚══════╝╚══════╝
        protected override void Initialize()
        {
            _graphics.PreferredBackBufferWidth = Tools.Width;
            _graphics.PreferredBackBufferHeight = Tools.Height;
            _graphics.ApplyChanges();
            //--------------------


            gameState = new GameState();
            effsmanager = new EffectsManager();
            

            //--------------------
            base.Initialize();
        }




        //  ██╗------██████╗--█████╗-██████╗-
        //  ██║-----██╔═══██╗██╔══██╗██╔══██╗
        //  ██║-----██║---██║███████║██║--██║
        //  ██║-----██║---██║██╔══██║██║--██║
        //  ███████╗╚██████╔╝██║--██║██████╔╝
        //  ╚══════╝-╚═════╝-╚═╝--╚═╝╚═════╝-
        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            Tools.DefaultFont = Content.Load<SpriteFont>("DefaultFont");
            //--------------------

            
            Tools.Fonts.Add("japanola8", Content.Load<SpriteFont>("japanola8"));
            Tools.Fonts.Add("japanola12", Content.Load<SpriteFont>("japanola12"));
            Tools.Fonts.Add("japanola18", Content.Load<SpriteFont>("japanola18"));
            Tools.Fonts.Add("japanola36", Content.Load<SpriteFont>("japanola36"));
            Tools.Fonts.Add("japanola72", Content.Load<SpriteFont>("japanola72"));
            Tools.Fonts.Add("osake18", Content.Load<SpriteFont>("osake18"));
            Tools.Fonts.Add("osake36", Content.Load<SpriteFont>("osake36"));
            Tools.Fonts.Add("osake72", Content.Load<SpriteFont>("osake72"));

            gameState.current = new Menu();


            //--------------------
        }




        //  ██╗---██╗██████╗-██████╗--█████╗-████████╗███████╗
        //  ██║---██║██╔══██╗██╔══██╗██╔══██╗╚══██╔══╝██╔════╝
        //  ██║---██║██████╔╝██║--██║███████║---██║---█████╗--
        //  ██║---██║██╔═══╝-██║--██║██╔══██║---██║---██╔══╝--
        //  ╚██████╔╝██║-----██████╔╝██║--██║---██║---███████╗
        //  -╚═════╝-╚═╝-----╚═════╝-╚═╝--╚═╝---╚═╝---╚══════╝
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            Tools.update();
            //--------------------


            gameState.current.Update(gameTime);


            //--------------------
            Tools.prv_update();
            base.Update(gameTime);
        }




        //  ██████╗-██████╗--█████╗-██╗----██╗
        //  ██╔══██╗██╔══██╗██╔══██╗██║----██║
        //  ██║--██║██████╔╝███████║██║-█╗-██║
        //  ██║--██║██╔══██╗██╔══██║██║███╗██║
        //  ██████╔╝██║--██║██║--██║╚███╔███╔╝
        //  ╚═════╝-╚═╝--╚═╝╚═╝--╚═╝-╚══╝╚══╝-
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Tools.BackGroundColor);
            _spriteBatch.Begin(SpriteSortMode.FrontToBack);
            //--------------------


            gameState.current.Draw(gameTime);
            

            //--------------------
            //DebugDisplay(gameTime);
            _spriteBatch.End();
            base.Draw(gameTime);
        }




        //  ██████╗-███████╗██████╗-██╗---██╗-██████╗-
        //  ██╔══██╗██╔════╝██╔══██╗██║---██║██╔════╝-
        //  ██║--██║█████╗--██████╔╝██║---██║██║--███╗
        //  ██║--██║██╔══╝--██╔══██╗██║---██║██║---██║
        //  ██████╔╝███████╗██████╔╝╚██████╔╝╚██████╔╝
        //  ╚═════╝-╚══════╝╚═════╝--╚═════╝--╚═════╝-
        protected void DebugDisplay(GameTime gameTime)
        {
            //_spriteBatch.DrawString(Tools.Fonts["japanola"], "", new Vector2(10, 10), Color.White);
            //_spriteBatch.DrawString(Tools.Fonts["osake"], "", new Vector2(10, 100), Color.White); //Burikkubureka Yakyū burikkuburēkā

            var lclscene = gameState.current;
            var lclIG = (InGame)gameState.current;
            var lclbate = ((InGame)gameState.current).bate;

            if (lclbate.hiting)
            {
                _spriteBatch.Draw(Tools.DrawRec(lclbate.hitboxe.Width, lclbate.hitboxe.Height, Color.Red), lclbate.hitboxe, Color.White);
            }
            //_spriteBatch.Draw(Tools.DrawRec(lclbate.hitboxe.Width, lclbate.hitboxe.Height, Color.Red), lclbate.hitboxe, Color.White);

            for (sbyte i = (sbyte)((lclIG).lstBalls.Count - 1 ); i >= 0; i--)
            {
                var lclball = ((InGame)gameState.current).lstBalls[i];
                _spriteBatch.Draw(Tools.DrawRec(lclball.hitboxe.Width, lclball.hitboxe.Height, Color.Green), lclball.hitboxe, Color.White);
                _spriteBatch.Draw(Tools.DrawRec(lclball.nxtXhitboxe.Width, lclball.nxtXhitboxe.Height, Color.Red), lclball.nxtXhitboxe, Color.White);
                _spriteBatch.Draw(Tools.DrawRec(lclball.nxtYhitboxe.Width, lclball.nxtYhitboxe.Height, Color.Green), lclball.nxtYhitboxe, Color.White);
            }

            for (short i = (short)(lclIG.lvlsmanager.lstBricks.Count - 1); i >= 0; i--)
            {
                var lclbrick = lclIG.lvlsmanager.lstBricks[i];
                //_spriteBatch.Draw(DrawRec(lclbrick.hitboxe.Width, lclbrick.hitboxe.Height, Color.Red), lclbrick.hitboxe, Color.White);
            }

            _spriteBatch.DrawString(Tools.DefaultFont, "nBrcks "+lclIG.lvlsmanager.lstBricks.Count, new Vector2(10, 600), Color.Black);
            _spriteBatch.DrawString(Tools.DefaultFont, "nBlls "+lclIG.lstBalls.Count, new Vector2(10, 620), Color.Black);
            _spriteBatch.DrawString(Tools.DefaultFont, "lifes "+lclbate.life, new Vector2(10, 640), Color.Black);
            //_spriteBatch.DrawString(Tools.DefaultFont, "", new Vector2(10, 660), Color.Black);
            //_spriteBatch.DrawString(Tools.DefaultFont, "", new Vector2(10, 660), Color.Black);
            //_spriteBatch.DrawString(Tools.DefaultFont, "", new Vector2(10, 680), Color.Black);

        }

    }


}