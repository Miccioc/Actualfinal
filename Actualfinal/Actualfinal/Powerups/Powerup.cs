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


namespace Actualfinal.Powerups
{

    public class Powerup
    {
        public Sprite powerup;
        public Powerup()
        {
            //powerup = new Sprite(new Vector2(x, y), Game1.square, new Rectangle(0, 0, 10, 10), new Vector2(-600, 0));
        }
        public virtual void Update(GameTime time, Rectangle box)
        {
            powerup.Update(time);
        }
        public virtual void Draw(SpriteBatch batch)
        {
            powerup.Draw(batch);
        }
    }
}
