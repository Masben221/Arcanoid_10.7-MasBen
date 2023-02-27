using SFML.Graphics;

class Block : GameObject
{
    public int health_1;
    public Texture blockBlueTexture;
    public Texture blockYellowTexture;
    public Texture blockRedTexture;
    public Block(Texture texture, int health)
    {
        health_1 = health;
        blockBlueTexture = new Texture("Block_Blue.png");
        blockYellowTexture = new Texture("Block_Yellow.png");
        blockRedTexture = new Texture("Block_Red.png");
        if (health_1 == 1) texture = blockBlueTexture;
        if (health_1 == 2) texture = blockYellowTexture;
        if (health_1 == 3) texture = blockRedTexture;
        sprite = new Sprite(texture);
    }
    public void TakeDamage() 
    {
        health_1 -= 1;
        if (health_1 == 1) sprite.Texture = blockBlueTexture;
        if (health_1 == 2) sprite.Texture = blockYellowTexture;        
    }

}

