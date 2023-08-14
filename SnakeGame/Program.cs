using System;
using static System.Console;

namespace SnakeGame
{
    class Program
    {
        System.Timers.Timer timer = new System.Timers.Timer();

        bool quit = false;
        int vx;
        int vy;
        int headX;
        int headY;
        int[,] GameField;
        int w = 80, h = 40;
        int score = 0;
        int lifes = 3;
        int apples;
        int level = 0;

        void init()
        {
            Console.CursorVisible = false;
            Console.SetWindowSize(w + 1, h + 3);
            Console.SetBufferSize(w + 1, h + 3);
        }

        void SplashScreen()
        {
            string[] ss = new string[10];
            ss[0] = "-----------------------------------";
            ss[1] = "  sss   s    s   sss s   s   ssssss";
            ss[2] = " s   s  s    s  s  s s  s    s     ";
            ss[3] = "  s     ss   s s   s s s     s     ";
            ss[4] = "   s    s s  s sssss ss      ssssss";
            ss[5] = "    s   s  s s s   s s s     s     ";
            ss[6] = "     s  s   ss s   s s  s    s     ";
            ss[7] = "s    s  s    s s   s s   s   s     ";
            ss[8] = " ssss   s    s s   s s    s  ssssss";
            ss[9] = "-----------------------------------";

            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < ss.Length; i++)
                for (int j = 0; j < ss[i].Length; j++)
                {
                    Console.SetCursorPosition(j + 25, i + 10);
                    Console.Write(ss[i][j]);
                    System.Console.Beep(200, 1);
                    System.Threading.Thread.Sleep(5);
                }
            Console.SetCursorPosition(30, 25);
            Console.WriteLine("Нажмите любую клавишу... ");
            Console.ResetColor();
          
        }

        void Load(int level = 1)
        {
            vx = 0;
            vy = 1;
            headX = w / 2;
            headY = h / 2;
            GameField = new int[w + 1, h + 1];
            GameField[headX, headY] = 1;
            Random random = new Random();
            apples = level + 1;
            //разбрасываем яблоки
            for (int i = 0; i < apples; i++)
                GameField[random.Next(1, w), random.Next(1, h)] = -1;

            //создаём бардюры
            for (int i = 0; i <= w; i++)
            {
                GameField[i, 0] = 10000;
                GameField[i, h] = 10000;
            }

            for (int i = 0; i < h; i++)
            {
                GameField[0, i] = 10000;
                GameField[w, i] = 10000;
            }
        }


        void PrintGameField()
        {
            for (int y = 0; y <= h; y++)
            {
                for (int x = 0; x <= w; x++)
                {
                    Console.SetCursorPosition(x, y);

                    switch (GameField[x, y])
                    {
                        case 0:
                            Console.WriteLine(' ');
                            break;
                        case -1:
                            Console.WriteLine('&');
                            break;
                        case 1:
                            Console.WriteLine('@');
                            break;
                        default:
                            //Console.WriteLine(GameField[x, y]);
                            Console.WriteLine('#');
                            break;
                    }
                }
            }
            Console.SetCursorPosition(10, 0);
            Console.Write($"Level:{level} Score:{score} Lifes:{lifes} Apples:{apples}");
        }

        void KeyBoardUpdate()
        {
            if (Console.KeyAvailable)
            {
                ConsoleKey key = Console.ReadKey().Key;
                System.Diagnostics.Debug.WriteLine(key);
                System.Diagnostics.Debug.WriteLine("X=" + headX + " Y=" + headY + " VX=" + vx + " VY=" + vy);
                Console.Title = DateTime.Now.ToLongTimeString();

                switch (key)
                {
                    case ConsoleKey.LeftArrow:
                        vx = -1;
                        vy = 0;
                        break;
                    case ConsoleKey.RightArrow:
                        vx = 1;
                        vy = 0;
                        break;
                    case ConsoleKey.UpArrow:
                        vx = 0;
                        vy = -1;
                        break;
                    case ConsoleKey.DownArrow:
                        vx = 0;
                        vy = 1;
                        break;
                    case ConsoleKey.Escape:
                        timer.Stop();
                        quit = true;
                        Console.WriteLine("Игра прервана пользователем.");
                        break;
                        
                }
            }
        }

        void Update()
        {
            headX += vx;
            headY += vy;
            if (Collision()) return;

            if (GameField[headX, headY] < 0)
            {
                score++;
                apples--;
                if (apples == 0)
                {
                    quit = true;
                    return;
                }
                GameField[headX, headY] = 1;
                Next(headX - vx, headY - vy, 1, 1);
            }
            else
                Next(headX, headY, 1);
        }

        bool Collision()
        {
            if (GameField[headX, headY] > 0)  quit = true;
            if (headX < 1 || headX >= w || headY < 1 || headY >= h) quit = true;
            return quit;
        }

        //Рисование змейки, её хвоста
        void Next(int tailX, int tailY, int n, int p = 0)
        {
            GameField[tailX, tailY] = n + p;

            if (GameField[tailX + 1, tailY] == n + p) Next(tailX + 1, tailY, n + 1, p);
            else
                if (GameField[tailX - 1, tailY] == n + p) Next(tailX - 1, tailY, n + 1, p);
            else
                if (GameField[tailX, tailY - 1] == n + p) Next(tailX, tailY - 1, n + 1, p);
            else
                if (GameField[tailX, tailY + 1] == n + p) Next(tailX, tailY + 1, n + 1, p);
            else
                if (p == 0) GameField[tailX, tailY] = 0;
        }


        public void Game()
        {
            init();
            SplashScreen();
            Console.ReadKey();
            while (lifes > 0)
            {
                Load(++level);
                PrintGameField();
                Console.ReadKey();
                while(!quit)
                {
                    KeyBoardUpdate();
                    Update();
                    PrintGameField();
                }
                lifes--;
                quit = false;

            }


            
        }
       
        static void Main(string[] args)
        {
            Program program = new Program();
            program.Game();
            Console.ReadKey();
        }
    }
}
