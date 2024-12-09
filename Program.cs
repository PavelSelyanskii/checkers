using System;

class Checkers
{
    const int BoardSize = 8; // Размер доски 8x8
    static char[,] board = new char[BoardSize, BoardSize];

    // Инициализация доски
    static void InitializeBoard()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                if ((row + col) % 2 == 1)
                {
                    if (row < 3)
                        board[row, col] = 'b'; // Черные шашки
                    else if (row > 4)
                        board[row, col] = 'w'; // Белые шашки
                    else
                        board[row, col] = '.'; // Пустые клетки
                }
                else
                {
                    board[row, col] = ' '; // Пустые клетки для черных
                }
            }
        }
    }

    // Вывод доски на экран
    static void PrintBoard()
    {
        for (int row = 0; row < BoardSize; row++)
        {
            for (int col = 0; col < BoardSize; col++)
            {
                Console.Write(board[row, col] + " ");
            }
            Console.WriteLine();
        }
    }

    // Проверка на возможность хода
    static bool IsValidMove(int startX, int startY, int endX, int endY, char player)
    {
        if (endX < 0 || endX >= BoardSize || endY < 0 || endY >= BoardSize)
            return false;

        if (board[startX, startY] != player) return false; // Ходить может только на своей шашке
        if (board[endX, endY] != '.') return false; // Целевая клетка должна быть пустой

        int dx = endX - startX;
        int dy = endY - startY;

        // Простой ход по диагонали (на 1 клетку)
        if (Math.Abs(dx) == 1 && Math.Abs(dy) == 1) return true;

        // Бить шашку через клетку
        if (Math.Abs(dx) == 2 && Math.Abs(dy) == 2)
        {
            int middleX = (startX + endX) / 2;
            int middleY = (startY + endY) / 2;
            char opponent = player == 'w' ? 'b' : 'w';
            if (board[middleX, middleY] == opponent) return true;
        }

        return false;
    }

    // Выполнение хода
    static void MakeMove(int startX, int startY, int endX, int endY)
    {
        char player = board[startX, startY];
        board[endX, endY] = player;
        board[startX, startY] = '.';

        // Если шашка достигла последней строки, она становится дамкой
        if ((player == 'w' && endX == 0) || (player == 'b' && endX == BoardSize - 1))
        {
            board[endX, endY] = char.ToUpper(player); // Сделать дамкой
        }

        // Если был "побит" противник, удаляем его шашку
        if (Math.Abs(endX - startX) == 2 && Math.Abs(endY - startY) == 2)
        {
            int middleX = (startX + endX) / 2;
            int middleY = (startY + endY) / 2;
            board[middleX, middleY] = '.'; // Удаляем побитую шашку
        }
    }

    // Игровой цикл
    static void PlayGame()
    {
        char currentPlayer = 'w'; // Начинает белый
        while (true)
        {
            Console.Clear();
            PrintBoard();
            Console.WriteLine($"Ход игрока: {(currentPlayer == 'w' ? "Белые" : "Черные")}");

            int startX, startY, endX, endY;
            while (true)
            {
                Console.WriteLine("Введите координаты хода (startX startY endX endY):");
                string[] input = Console.ReadLine().Split();
                if (input.Length == 4 &&
                    int.TryParse(input[0], out startX) &&
                    int.TryParse(input[1], out startY) &&
                    int.TryParse(input[2], out endX) &&
                    int.TryParse(input[3], out endY))
                {
                    if (IsValidMove(startX, startY, endX, endY, currentPlayer))
                    {
                        break;
                    }
                    else
                    {
                        Console.WriteLine("Невозможный ход. Попробуйте снова.");
                    }
                }
                else
                {
                    Console.WriteLine("Неверный ввод. Попробуйте снова.");
                }
            }

            MakeMove(startX, startY, endX, endY);

            // Проверка на победу
            bool whiteLeft = false, blackLeft = false;
            for (int row = 0; row < BoardSize; row++)
            {
                for (int col = 0; col < BoardSize; col++)
                {
                    if (board[row, col] == 'w' || board[row, col] == 'W') whiteLeft = true;
                    if (board[row, col] == 'b' || board[row, col] == 'B') blackLeft = true;
                }
            }

            if (!whiteLeft)
            {
                Console.Clear();
                PrintBoard();
                Console.WriteLine("Черные победили!");
                break;
            }
            else if (!blackLeft)
            {
                Console.Clear();
                PrintBoard();
                Console.WriteLine("Белые победили!");
                break;
            }

            // Смена игрока
            currentPlayer = (currentPlayer == 'w') ? 'b' : 'w';
        }
    }

    // Точка входа в программу
    static void Main()
    {
        InitializeBoard();
        PlayGame();
    }
}
