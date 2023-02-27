using SFML.Audio;
using SFML.Graphics;
using SFML.System;
class Ball : GameObject
{
    public float speed;
    private Vector2f direction;

    private SoundBuffer bufferBlock = new SoundBuffer("udar-kulakom-krutogo-boytsa.ogg");
    private Sound blockSound; //Звук удара об блок
    private SoundBuffer bufferStick = new SoundBuffer("Stick.ogg");
    private Sound stickSound; //Звук удара об платформу
    private SoundBuffer bufferOut = new SoundBuffer("jujjanie-v-kompyuternoy-igre-oboznachayuschee-promah.ogg");
    private Sound outSound; //Звук удара об платформу
    public Ball(Texture texture)
    {
        sprite = new Sprite(texture);
        blockSound = new Sound(bufferBlock);
        stickSound = new Sound(bufferStick);
        outSound = new Sound(bufferOut);    }

    public int attempt = 3;
    public bool outball = false;
    public void Start(float speed, Vector2f direction)
    {
        if (this.speed != 0) return;

        this.speed = speed;
        this.direction = direction;
    }

    public void Move(Vector2i boundsPos, Vector2i boundSize)
    {
        sprite.Position += direction * speed;

        if (sprite.Position.X > boundSize.X - sprite.Texture.Size.X || sprite.Position.X < boundsPos.X)
        {
            direction.X *= -1;
        }
        if (sprite.Position.Y < boundsPos.Y)
        {
            direction.Y *= -1;
        }
        if (sprite.Position.Y > boundSize.Y - sprite.Texture.Size.Y)
        {
            if (attempt > 0)
                attempt--;
            outSound.Play();
            outball = true;
        }
    }
    public bool CheckCollision(GameObject obj)
    {
        if (sprite.GetGlobalBounds().Intersects(obj.sprite.GetGlobalBounds()) == true)
        {
            if (obj.GetType() == typeof(Stick))
            {
                direction.Y *= -1;
                float f = ((sprite.Position.X + sprite.Texture.Size.X * 0.5f) - (obj.sprite.Position.X + obj.sprite.Texture.Size.X * 0.5f)) / obj.sprite.Texture.Size.X;
                direction.X = f * 2;
                stickSound.Play();
            }

            if (obj.GetType() == typeof(Block))
            {
                direction.Y *= -1;
                (obj as Block).TakeDamage();
                blockSound.Play();
            }

            return true;                        
        }
        return false;
    }
}
