using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

class Point
{
    public readonly int Y;
    public readonly int X;

    public Point(int y, int x)
    {
        Y = y;
        X = x;
    }

    public static bool operator ==(Point ob1, Point ob2)
    {
        return ob1.Y == ob2.Y && ob1.X == ob2.X;
    }

    public static bool operator !=(Point ob1, Point ob2)
    {
        return true;
    }
}

class Program
{
    enum Route { Top, Left, Bottom, Right };
    const char WALL = '1', PATH = ' ';
    static char[,] arr;

    static void Main()
    {
        Random random = new Random();
        arr = new char[9, 9];
        List<Stack<Point>> lst;
        do
        {
            for (int i = 0; i < arr.GetLength(0); i++)
            {
                for (int j = 0; j < arr.GetLength(1); j++)
                {
                    arr[i, j] = random.Next(3) == 0 ? WALL : PATH;

                }
            }
            arr[0, 0] = arr[arr.GetLength(0) - 1, arr.GetLength(1) - 1] = PATH;
        } while ((lst = AllPath()).Count == 0);

        foreach (var stack in lst)
        {
            PrintArr();
            Console.ForegroundColor = ConsoleColor.Red;
            int saveY = Console.CursorTop;
            foreach (var point in stack.Reverse())
            {
                Console.SetCursorPosition(point.X, point.Y + saveY - arr.GetLength(0) - 1);
                Console.Write('*');
                System.Threading.Thread.Sleep(300);
            }
            Console.ResetColor();
            Console.CursorTop = saveY;
            Console.WriteLine();
        }

        Console.ReadKey();
    }

    static void PrintArr()
    {
        for (int i = 0; i < arr.GetLength(0); i++)
        {
            for (int j = 0; j < arr.GetLength(1); j++)
            {
                Console.Write(arr[i, j]);
            }
            Console.WriteLine('|');
        }
        for (int i = 0; i <= arr.GetLength(0); i++)
            Console.Write('-');
        Console.WriteLine();
    }

    static List<Stack<Point>> AllPath()
    {
        List<Stack<Point>> lst = new List<Stack<Point>>();
        Stack<Point> st = new Stack<Point>();
        st.Push(new Point(0, 0));
        Foo(st, Route.Left);
        return lst;

        void Foo(Stack<Point> stack, Route route)
        {
            Point point = stack.Peek();
            if (point.Y == arr.GetLength(0) - 1 && point.X == arr.GetLength(1) - 1)
            {
                lst.Add(stack);
                return;
            }

            if (route != Route.Top && point.Y > 0 && arr[point.Y - 1, point.X] != WALL && CheckMove(stack, new Point(point.Y - 1, point.X)))
            {
                Stack<Point> newStack = new Stack<Point>(stack.Reverse());
                newStack.Push(new Point(point.Y - 1, point.X));
                Foo(newStack, Route.Bottom);
            }

            if (route != Route.Left && point.X > 0 && arr[point.Y, point.X - 1] != WALL && CheckMove(stack, new Point(point.Y, point.X - 1)))
            {
                Stack<Point> newStack = new Stack<Point>(stack.Reverse());
                newStack.Push(new Point(point.Y, point.X - 1));
                Foo(newStack, Route.Right);
            }

            if (route != Route.Bottom && point.Y < arr.GetLength(0) - 1 && arr[point.Y + 1, point.X] != WALL && CheckMove(stack, new Point(point.Y + 1, point.X)))
            {
                Stack<Point> newStack = new Stack<Point>(stack.Reverse());
                newStack.Push(new Point(point.Y + 1, point.X));
                Foo(newStack, Route.Top);
            }

            if (route != Route.Right && point.X < arr.GetLength(1) - 1 && arr[point.Y, point.X + 1] != WALL && CheckMove(stack, new Point(point.Y, point.X + 1)))
            {
                Stack<Point> newStack = new Stack<Point>(stack.Reverse());
                newStack.Push(new Point(point.Y, point.X + 1));
                Foo(newStack, Route.Left);
            }

        }

    }

    static bool CheckMove(Stack<Point> stack, Point point)
    {
        foreach (var currentPoint in stack)
        {
            if (currentPoint == point)
                return false;
        }
        return true;
    }
}