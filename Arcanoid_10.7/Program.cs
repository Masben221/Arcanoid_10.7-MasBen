using System;
using SFML.Audio;
using SFML.Graphics;
using SFML.Window;
using SFML.System;
using System.Collections.Generic;
class Program 
{
    static RenderWindow win;

    static Texture ballTexture;
    static Texture stickTexture;
    static Texture blockTexture;

    static Stick stick;
    static List<Block> blocks;

    static Ball ball;

    static Text textAttempt1 = new Text();
    static Text textAttempt2 = new Text();
    static Text textChoice = new Text();
    static Text textGameOver = new Text();
    static Text textLevel = new Text();
    static Text textNumberLevel = new Text();

    static Font mainFont = new Font("comic.ttf");

    static bool Endgame = false;
    static int level = 1;
    static int blockCount;

    static int sumblocks;

    static SoundBuffer bufer = new SoundBuffer("zvuk-iz-igryi-super-mario-23490.ogg");
    static Music backgroundSound; //фоновая музыка
    static SoundBuffer bufferCongratulation = new SoundBuffer("aplodismentyi-gruppyi-lyudey-s-voplem.ogg");
    static Sound congratulationSound; //Звук Победы
    static SoundBuffer bufferGameOver = new SoundBuffer("game_over.ogg");
    static Sound gameOverSound; //Звук конца игры
    static SoundBuffer bufferGameLost = new SoundBuffer("game-lost.ogg");
    static Sound gameLostSound; //Звук проигрыша
    static void Initialization()
    {
        win = new RenderWindow(new VideoMode(800, 600), "Arcanoid");
        win.SetFramerateLimit(60);
        win.SetMouseCursorVisible(false);
        win.Closed += Win_Closed;

        // Загрузка текстур
        ballTexture = new Texture("Ball.png");
        stickTexture = new Texture("Stick.png");
        blockTexture = new Texture("Block.png");

        ball = new Ball(ballTexture);

        stick = new Stick(stickTexture);

        blockCount = level * 3 * 10;       
        
        textAttempt1.Font = mainFont;
        textAttempt2.Font = mainFont;
        textGameOver.Font = mainFont;
        textChoice.Font = mainFont;
        textLevel.Font = mainFont;
        textNumberLevel.Font = mainFont;

        textAttempt1.Color = Color.Green;
        textAttempt2.Color = Color.Red;
        textGameOver.Color = Color.Red;
        textChoice.Color = Color.Blue;
        textLevel.Color = Color.Cyan;
        textNumberLevel.Color = Color.Magenta;

        textAttempt1.CharacterSize = 20;
        textAttempt2.CharacterSize = 20;
        textGameOver.CharacterSize = 40;
        textChoice.CharacterSize = 24;
        textLevel.CharacterSize = 20;
        textNumberLevel.CharacterSize = 30;

        textAttempt1.Position = new Vector2f(660, 10);
        textAttempt2.Position = new Vector2f(770, 10);
        textGameOver.Position = new Vector2f(270, 250);
        textChoice.Position = new Vector2f(130, 220);
        textLevel.Position = new Vector2f(30, 10);

        textAttempt1.DisplayedString = "Попытки: ";
        textAttempt2.DisplayedString = ball.attempt.ToString();
        textGameOver.DisplayedString = "GAME OVER";
        textChoice.DisplayedString = "ESC - выход из игры, Space - начать сначала";
        textLevel.DisplayedString = $"Уровень: { level }";

        backgroundSound = new Music("zvuk-iz-igryi-super-mario-23490.ogg");
        backgroundSound.Volume = 30;
        backgroundSound.Loop = true;

        congratulationSound = new Sound(bufferCongratulation);
        gameOverSound = new Sound(bufferGameOver);
        gameLostSound = new Sound(bufferGameLost);
    }
    public static void SetStartPosition() 
    {
        backgroundSound.Play();
        
        // Объявление номера уровня
        win.Clear();
        textNumberLevel.Position = new Vector2f(330, 260);
        textNumberLevel.DisplayedString = $"Уровень { level }";
        win.Draw(textNumberLevel);
        win.Display();

        System.Threading.Thread.Sleep(3000);

        // обнуление счетчика блоков и скорости мяча
        sumblocks = 0;
        ball.speed = 0;

        // инициализация блоков
        blocks = new List<Block>();
        Random rnd = new Random();
        blockCount = level * 3 * 10;
       
        for (int i = 0; i < blockCount; i ++)
        {
            int health = rnd.Next(1, 4);
            blocks.Add(new Block(blockTexture, health)); //blocks[i] = new Block(blockTexture, health);
        }

        // вычисление местоположения блоков в зависимости от level
        int index = 0;
        for (int y = 0; y < level * 3; y++) 
        { 
            for (int x = 0; x < 10; x++) 
            {
                blocks[index].sprite.Position = new Vector2f(x * (blocks[index].sprite.TextureRect.Width + 15) + 75, 
                    y * (blocks[index].sprite.TextureRect.Height + 15) + 50);
                index++;
            }
        }

        stick.sprite.Position = new Vector2f(400, 500);
        ball.sprite.Position = new Vector2f(375, 400);
    }

    public static void OutPosition()
    {
        if (ball.outball == true && ball.attempt > 0)
        {
            stick.sprite.Position = new Vector2f(400, 500);
            ball.sprite.Position = new Vector2f(375, 400);
            ball.outball = false;
            ball.speed = 0;            
        }        
    }
    static void Main(string[] args)
    {
        // Инициализация
        Initialization();

        // Стартовые настройки игры
        SetStartPosition();

        // Главный цикл игры 
        while (win.IsOpen == true)
        {            
            win.Clear();

            win.DispatchEvents();
                        
            if (Endgame == false)
            {
                // Проверка вылета мяча за экран
                OutPosition();
                
                // Если нажата клавиша мыши то начинается движение мяча
                if (Mouse.IsButtonPressed(Mouse.Button.Left) == true)
                {
                    ball.Start(5 + level, new Vector2f(0, -1));
                }
                
                // Проверка что если мяч не начал движение, то его позиция на платформе
                if (ball.speed == 0)
                {
                    ball.sprite.Position = new Vector2f(stick.sprite.Position.X + stick.sprite.TextureRect.Width * 0.5f - 
                       ball.sprite.TextureRect.Width * 0.5f, stick.sprite.Position.Y - 25);
                }

                // Движение мяча
                ball.Move(new Vector2i(0, 0), new Vector2i(800, 600));

                // Проверка на столкновение мяча с платформой
                ball.CheckCollision(stick);

                // Проверка на столкновение мяча с блоками
                for (int i = 0; i < blocks.Count; i++)
                {
                    if (ball.CheckCollision(blocks[i]) == true)
                    {   
                        if (blocks[i].health_1 <= 0) 
                        {
                            blocks.Remove(blocks[i]); //blocks[i].sprite.Position = new Vector2f(1000, 1000);
                            sumblocks++;
                        }                        
                        break;
                    }
                }

                // Перемещение стика
                stick.Move(win); //stick.sprite.Position = new Vector2f(Mouse.GetPosition(win).X - stick.sprite.Texture.Size.X * 0.5f, stick.sprite.Position.Y);
                
                // Проверка на проигрыш
                if (ball.attempt <= 0) 
                {
                    Endgame = true;
                    backgroundSound.Stop();
                    gameLostSound.Play();
                }
                
                // Проверка на конец уровня или победу
                if (sumblocks >= level * 3 * 10)
                {
                    ball.attempt = 3;
                    level += 1;
                    if (level > 4)
                    {
                        win.Clear();
                        textNumberLevel.Position = new Vector2f(330, 260);
                        textNumberLevel.DisplayedString = $"Победа!";
                        win.Draw(textNumberLevel);
                        win.Display();

                        backgroundSound.Stop();
                        congratulationSound.Play();

                        System.Threading.Thread.Sleep(3000);

                        Endgame = true;
                    }
                    else SetStartPosition();
                }

                // Отрисовка в буфер
                win.Draw(ball.sprite);
                win.Draw(stick.sprite);

                win.Draw(textAttempt1);

                textAttempt2.DisplayedString = ball.attempt.ToString();
                win.Draw(textAttempt2);

                textLevel.DisplayedString = $"Уровень: { level }";
                win.Draw(textLevel);

                for (int i = 0; i < blocks.Count; i++)
                {
                    win.Draw(blocks[i].sprite);
                }

                // Вывод на экран из буфера
                win.Display();
            }
            
            //Меню выбора выход или рестарт
            if (Endgame == true)
            {
                win.Clear();
                win.Draw(textChoice);
                win.Display();
            }

            // Выход из игры
            if (Keyboard.IsKeyPressed(Keyboard.Key.Escape))
            {
                win.Clear();
                win.Draw(textGameOver);
                win.Display();

                gameOverSound.Play();

                System.Threading.Thread.Sleep(4000);
                break;
            }

            //рестарт игры
            if (Keyboard.IsKeyPressed(Keyboard.Key.Space))
            {
                ball.attempt = 3;
                Endgame = false;
                level = 1;                
                SetStartPosition();
            }
            // Читкод L
            if (Keyboard.IsKeyPressed(Keyboard.Key.L))
            {
                sumblocks = level * 3 * 10;                
            }
        }       
    }
    // Закрытие окна win
    private static void Win_Closed(object sender, EventArgs e)
    {
        win.Close();
    }
}

