using SFML.Graphics;
using SFML.System;
using SFML.Window;

class Stick : GameObject
{
    public Stick(Texture texture)
    {
        sprite = new Sprite(texture);
    }

    public void Move(RenderWindow win)
    {
        sprite.Position = new Vector2f((float)Mouse.GetPosition(win).X - sprite.TextureRect.Width * 0.5f, sprite.Position.Y);
    }
}
