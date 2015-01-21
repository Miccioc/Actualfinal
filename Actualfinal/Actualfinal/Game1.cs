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
        Texture2D kar;
        Texture2D kar2;
        Texture2D trail2;
        SpriteFont pericles;
        Texture2D texture1;
        Rectangle rectangle1;
        Rectangle rectangle2;
        Sprite Player;
        //Sprite Player2;
        List<Sprite> Trail = new List<Sprite>();
        List<Sprite> Enemys = new List<Sprite>();
        Random randy = new Random(System.Environment.TickCount);
        List<Song> songs = new List<Song>();
        int songindex = 0;
        bool isPlayingSong = false;
        int score = 0, Deaths = -1;
        //Rectangle Player = new Rectangle(10, 10, 100, 100);
        Speed sped;
        KeyboardState old;
        public static float Speed = 1;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1920;
            graphics.PreferredBackBufferHeight = 1000;
            Content.RootDirectory = "Content";
            //Components.Add(new GamerServicesComponent(this));
        }


        protected override void Initialize()
        {
            rectangle1 = new Rectangle(0, 0, 1920, 1080);
            rectangle2 = new Rectangle(0, 1080, 1920, 1080);
            base.Initialize();
        }


        protected override void LoadContent()
        {
                
                
                //KeyboardState keybState = Keyboard.GetState();

                    songs.Add(Content.Load<Song>("Sound Remdy and Nitro Fun - Turbo Penguin"));
                    songs.Add(Content.Load<Song>("OVERWERK - House"));
                    songs.Add(Content.Load<Song>("Rogue - Adventure Time"));
                    songs.Add(Content.Load<Song>("Pegboard Nerds and Tristam - Razor Sharp"));
                    songs.Add(Content.Load<Song>("Ephixa - Awesome to the Max"));
                    songs.Add(Content.Load<Song>("Tristam and Stephen Walking - Too Simple"));
                    songs.Add(Content.Load<Song>("Tristam and Braken - Flight"));
                    songs.Add(Content.Load<Song>("Laszlo - Messiah"));

                    
                    //MediaPlayer.Play(song);
                    MediaPlayer.Volume = 10.0f;
              
            
            spriteBatch = new SpriteBatch(GraphicsDevice);
            square = Content.Load<Texture2D>(@"square");
            pericles = Content.Load<SpriteFont>(@"pericles14");
            kar = Content.Load<Texture2D>(@"kar");
            kar2 = Content.Load<Texture2D>(@"kar2");
            trail2 = Content.Load<Texture2D>(@"trail2");
            Player = new Sprite(new Vector2(200, 500), kar, new Rectangle(0, 0, 114, 231), Vector2.Zero);
            sped = new Speed(1920, 500);
            texture1 = Content.Load<Texture2D>("background");
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
            Sprite temp = new Sprite(Player.Location, trail2, new Rectangle(0, 0, 114, 231), new Vector2(0, 600), Color.Gray, 1f);
            Speed = 1;
            Trail.Add(temp);
            Enemys.Clear();

            Player = new Sprite(new Vector2(200, 500), kar, new Rectangle(0, 0, 114, 231), Vector2.Zero);
            foreach (Sprite sp in Trail)
                sp.Texture = trail2; 
                
                

        }
        protected override void Update(GameTime gameTime)
        {
            score++;
            sped.Update(gameTime, Player.BoundingBoxRect);
         ;
            KeyboardState KEYS = Keyboard.GetState(); 
            if (KEYS.IsKeyDown(Keys.E) && !isPlayingSong)
            {
                isPlayingSong = true;
                MediaPlayer.Play(songs[songindex]);
            }
            if (KEYS.IsKeyDown(Keys.D1)&& old.IsKeyUp(Keys.D1))
            {
                MediaPlayer.Stop();
                if (songindex == 0)
                    songindex = songs.Count - 1;
                else songindex--;

                MediaPlayer.Play(songs[songindex]);
               
            }
            if (KEYS.IsKeyDown(Keys.D3) && old.IsKeyUp(Keys.D3
                ))
            {
                MediaPlayer.Stop();
                if (songindex == songs.Count-1)
                    songindex =0;
                else songindex++;

                MediaPlayer.Play(songs[songindex]);

            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here
            // Simple bounds check. If the right edge of rectangle1 is offscreen to the left, 
            // the code moves it to the right side of rectangle2.
            if (rectangle1.Y + texture1.Height <= 0)
                rectangle1.Y = rectangle2.X + texture1.Height;
            // Then repeat this check for rectangle2.
            if (rectangle2.Y + texture1.Height <= 0)
                rectangle2.Y = rectangle1.X + texture1.Height;

            // 6. Incrementally move the rectangles to the left. 
            // Optional: Swap X for Y if you want to scroll vertically.

            rectangle1.Y -= 20;
            rectangle2.Y -= 20;

            if (Enemys.Count < 4)
            {
                Enemys.Add(new Sprite(new Vector2(randy.Next(100,1800), randy.Next(-30,-30)), kar2, new Rectangle(0, 0, 119, 228), new Vector2(0, 500 * Speed)));
            }
            
          //  Trail.Add(new Sprite(Player.Location, trail2, new Rectangle(0, 0, 114, 231), new Vector2(0, 231), Color.Gray, .8f));

            foreach (Sprite sp in Enemys)
            {
                sp.Update(gameTime);
                if (sp.IsBoxColliding(Player.BoundingBoxRect))
                {
                    Restart();
                    break;
                }
            }
            for (int i = 0; i < Enemys.Count; i++)
                if (Enemys[i].Location.Y > 1075)
                    Enemys.RemoveAt(i);

            foreach (Sprite sp in Trail)
                sp.Update(gameTime);
            for (int i = 0; i < Trail.Count; i++)
                if (Trail[i].Location.Y > 1075)
                    Trail.RemoveAt(i);
            HandleInputs(PlayerIndex.One, Player);
  old = KEYS;
            base.Update(gameTime);
        }

        void HandleInputs(PlayerIndex index, Sprite player)
        {
            KeyboardState keybState = Keyboard.GetState();
            if (player.Location.X <= 1600)
                if (keybState.IsKeyDown (Keys.Right))
                    player.Location = new Vector2(player.Location.X + 10 * Speed, player.Location.Y);
            

            if (player.Location.X >= 300)
                if (keybState.IsKeyDown(Keys.Left))
                    player.Location = new Vector2(player.Location.X - 10 * Speed, player.Location.Y);

            if (player.Location.Y <= 900)
                if (keybState.IsKeyDown(Keys.Down))
                    player.Location = new Vector2(player.Location.X, player.Location.Y + 10 * Speed);

            if (player.Location.Y >= 0)
                if (keybState.IsKeyDown(Keys.Up))
                    player.Location = new Vector2(player.Location.X, player.Location.Y - 10 * Speed);

        }
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);


            spriteBatch.Begin();
            spriteBatch.Draw(texture1, rectangle1, Color.White);
            spriteBatch.Draw(texture1, rectangle2, Color.White);
            foreach (Sprite sp in Trail)
                sp.Draw(spriteBatch);
            foreach (Sprite sp in Enemys)
                sp.Draw(spriteBatch);
            Player.Draw(spriteBatch);
            
            spriteBatch.DrawString(pericles, "Deaths:" + Deaths, new Vector2(1750, 100), Color.White);
            spriteBatch.DrawString(pericles, "Song: " + songs[songindex].ToString(), new Vector2(10, 10), Color.White);
            spriteBatch.DrawString(pericles, "Press e to start music, press 1 or 3 to change songs,", new Vector2(850, 10), Color.White);
            spriteBatch.DrawString(pericles, "Score:" + score, new Vector2(1750, 70), Color.White);   
            sped.Draw(spriteBatch);
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}