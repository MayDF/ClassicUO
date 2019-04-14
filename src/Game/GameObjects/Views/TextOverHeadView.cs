#region license
//  Copyright (C) 2019 ClassicUO Development Community on Github
//
//	This project is an alternative client for the game Ultima Online.
//	The goal of this is to develop a lightweight client considering 
//	new technologies.  
//      
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <https://www.gnu.org/licenses/>.
#endregion

using System;

using ClassicUO.Configuration;
using ClassicUO.Game.GameObjects;
using ClassicUO.Game.Scenes;
using ClassicUO.Input;
using ClassicUO.Renderer;
using ClassicUO.Utility.Logging;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ClassicUO.Game.GameObjects
{
    internal partial class TextOverhead
    {
        private readonly RenderedText _text;

        protected bool EdgeDetection { get; set; }  

        public override bool Draw(Batcher2D batcher, Vector3 position, MouseOverList objectList)
        {
            if (!AllowedToDraw || IsDestroyed)
            {
                return false;
            }

            Texture.Ticks = Engine.Ticks;

            if (IsSelected && _text.Hue != 0x0035)
            {
                _text.Hue = 0x0035;
                _text.CreateTexture();
                Texture = _text.Texture;
            }
            else if (!IsSelected && Hue != _text.Hue)
            {
                _text.Hue = Hue;
                _text.CreateTexture();
                Texture = _text.Texture;
            }

            HueVector = Vector3.Zero;

            float scale = Engine.SceneManager.GetScene<GameScene>().Scale;

            int x = Engine.Profile.Current.GameWindowPosition.X;
            int y = Engine.Profile.Current.GameWindowPosition.Y;
            int w = Engine.Profile.Current.GameWindowSize.X;
            int h = Engine.Profile.Current.GameWindowSize.Y;


            position /= scale;
            position.Z = 0;

            position.X += x + 6;
            position.Y += y;

            float width = Texture.Width - Bounds.X - 6;
            float height = Texture.Height - Bounds.Y - 6;


            if (EdgeDetection)
            {
                if (position.X < x + Bounds.X + 6)
                    position.X = x + Bounds.X + 6;
                else if (position.X > x + w - width)
                    position.X = x + w - width;

                if (position.Y < y + Bounds.Y)
                    position.Y = y + Bounds.Y;
                else if (position.Y > y + h - height)
                    position.Y = y + h - height;
            }

            base.Draw(batcher, position, objectList);


           // batcher.DrawRectangle(Textures.GetTexture(Color.Gray), (int)(position.X - Bounds.X), (int) (position.Y - Bounds.Y), Bounds.Width - 3, Bounds.Height, Vector3.Zero);

            return true;
        }

        protected override void MousePick(MouseOverList list, SpriteVertex[] vertex, bool istransparent)
        {
            GameScene gs = Engine.SceneManager.GetScene<GameScene>();
            int x1 = Engine.Profile.Current.GameWindowPosition.X + 6;
            int y1 = Engine.Profile.Current.GameWindowPosition.Y + 6;

            int x = (int) (list.MousePosition.X / gs.Scale - vertex[0].Position.X);
            int y = (int) (list.MousePosition.Y / gs.Scale - vertex[0].Position.Y);

            x += x1;
            y += y1;

            if (Texture.Contains(x, y))
                list.Add(this, vertex[0].Position);            
        }
    }
}