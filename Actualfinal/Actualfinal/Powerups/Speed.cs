using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Actualfinal.Powerups
{
    public class Speed : Powerup
    {
        public Speed(int x, int y)
        {
            powerup = new Sprite(new Vector2(x, y), Game1.square, new Rectangle(0, 0, 10, 10), new Vector2(-600, 0));
        }
        public override void Update(GameTime time, Rectangle box)
        {
            if (powerup.IsBoxColliding(box))
                Game1.Speed = 2f;
            base.Update(time, box);
        }
        public override void Draw(SpriteBatch batch)
        {
            base.Draw(batch);
        }
    }
}