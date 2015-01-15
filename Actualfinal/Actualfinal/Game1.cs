using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Actualfinal.Powerups;

namespace Actualfinal
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Texture2D square, Speedsheet;
        SpriteFont pericles;

        Sprite Player;
        //Sprite Player2;
        List<Sprite> Trail = new List<Sprite>();
        List<Sprite> Enemys = new List<Sprite>();
        Random randy = new Random(System.Environment.TickCount);
        int score = 0, Deaths = -1;
        //Rectangle Player = new Rectangle(10, 10, 100, 100);
        Speed sped;
        public static float Speed = 1;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1650;
            graphics.PreferredBackBufferHeight = 1050;
            Content.RootDirectory = "Content";
            //Components.Add(new GamerServicesComponent(this));
        }


        protected override void Initialize()
        {

            base.Initialize();
        }


        protected override void LoadContent()
        {

            spriteBatch = new SpriteBatch(GraphicsDevice);
            square = Content.Load<Texture2D>(@"square");
            pericles = Content.Load<SpriteFont>(@"pericles14");
            Player = new Sprite(new Vector2(200, 500), square, new Rectangle(0, 0, 100, 100), Vector2.Zero);
            sped = new Speed(1920, 500);
            Enemys.Clear();
            Restart();

        }



        protected override void UnloadContent()
        {

        }

        void Restart()
        {
            score = 0;
            Deaths++;
            Sprite temp = new Sprite(Player.Location, square, new Rectangle(0, 0, 100, 100), new Vector2(-600, 0), Color.Gray, 1f);
            Speed = 1;
            temp.Rotation = Player.Rotation;
            Trail.Add(temp);
            Enemys.Clear();

            Player = new Sprite(new Vector2(200, 500), square, new Rectangle(0, 0, 100, 100), Vector2.Zero);
            foreach (Sprite sp in Trail)
                sp.TintColor = Color.Red;

        }
        protected override void Update(GameTime gameTime)
        {
            score++;
            sped.Update(gameTime, Player.BoundingBoxRect);
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            Player.Rotation += .1f;

            if (Enemys.Count < 5)
            {
                Enemys.Add(new Sprite(new Vector2(randy.Next(1920, 3000), randy.Next(0, 980)), square, new Rectangle(0, 0, 100, 100), new Vector2(-600 * Speed, 0), Color.Blue, 1f));
            }
            Trail.Add(new Sprite(Player.Location, square, new Rectangle(0, 0, 100, 100), new Vector2(-600, 0), Color.Gray, .8f));

            foreach (Sprite sp in Enemys)
            {
                sp.Rotation -= .1f;
                sp.Update(gameTime);
                if (sp.IsBoxColliding(Player.BoundingBoxRect))
                {
                    Restart();
                    break;
                }
            }
            for (int i = 0; i < Enemys.Count; i++)
                if (Enemys[i].Location.X < -90)
                    Enemys.RemoveAt(i);

            foreach (Sprite sp in Trail)
                sp.Update(gameTime);
            for (int i = 0; i < Trail.Count; i++)
                if (Trail[i].Location.X < -90)
                    Trail.RemoveAt(i);
            HandleInputs(PlayerIndex.One, Player);

            base.Update(gameTime);
        }

        void HandleInputs(PlayerIndex index, Sprite player)
        {
            GamePadState Gp = GamePad.GetState(index);
            if (player.Location.Y <= 990)
                if (Gp.ThumbSticks.Left.Y < 0)
                    player.Location = new Vector2(player.Location.X, player.Location.Y + 10 * Speed);

            if (player.Location.Y >= -10)
                if (Gp.ThumbSticks.Left.Y > 0)
                    player.Location = new Vector2(player.Location.X, player.Location.Y - 10 * Speed);

            if (player.Location.X >= 0)
                if (Gp.ThumbSticks.Left.X < 0)
                    player.Location = new Vector2(player.Location.X - 10 * Speed, player.Location.Y);

            if (player.Location.X <= 600)
                if (Gp.ThumbSticks.Left.X > 0)
                    player.Location = new Vector2(player.Location.X + 10 * Speed, player.Location.Y);

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateGray);


            spriteBatch.Begin();

            foreach (Sprite sp in Trail)
                sp.Draw(spriteBatch);
            foreach (Sprite sp in Enemys)
                sp.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            spriteBatch.DrawString(pericles, "Score:" + score, new Vector2(1750, 70), Color.White);
            spriteBatch.DrawString(pericles, "Deaths:" + Deaths, new Vector2(1750, 100), Color.White);
            sped.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}