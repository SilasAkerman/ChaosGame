using System;
using System.Numerics;
using System.Collections.Generic;
using Raylib_cs;

namespace Chaos_Game
{
    class Program
    {
        const int WIDTH = 1400;
        const int HEIGHT = 1000;

        const int SIZE = 1;

        enum GlobalState
        {
            Intro,
            Simulation
        }

        static GlobalState currentState = GlobalState.Intro;

        static List<Vector2> points = new List<Vector2>();
        static List<Vector2> targetPoints = new List<Vector2>();
        static Vector2 activePoint;

        static Random random = new Random();

        static void Main(string[] args)
        {
            Raylib.InitWindow(WIDTH, HEIGHT, "Chaos Game");
            Raylib.SetTargetFPS(30);

            while (!Raylib.WindowShouldClose())
            {
                int amout = currentState == GlobalState.Intro ? 1 : 50;
                for (int i = 0; i < 50; i++)
                {
                    Update();
                }
                Display();
            }
        }

        static void Update()
        {
            switch (currentState)
            {
                case GlobalState.Intro:
                    if (Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON) && !(Raylib.IsKeyDown(KeyboardKey.KEY_SPACE)))
                    {
                        targetPoints.Add(Raylib.GetMousePosition());
                    }
                    if (Raylib.IsKeyDown(KeyboardKey.KEY_SPACE) && Raylib.IsMouseButtonPressed(MouseButton.MOUSE_LEFT_BUTTON))
                    {
                        ChangeState(GlobalState.Simulation);
                    }
                    if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
                    {
                        ChangeState(GlobalState.Intro);
                    }
                    break;

                case GlobalState.Simulation:
                    ActivePointUpdate();
                    if (Raylib.IsKeyPressed(KeyboardKey.KEY_R))
                    {
                        ChangeState(GlobalState.Intro);
                    }
                    if (Raylib.IsKeyPressed(KeyboardKey.KEY_BACKSPACE))
                    {
                        points.Clear();
                        activePoint = Raylib.GetMousePosition();
                    }
                    break;
            }
        }

        static void Display()
        {
            Raylib.BeginDrawing();
            Raylib.ClearBackground(Color.BLACK);
            foreach (Vector2 targetPoint in targetPoints)
            {
                Raylib.DrawCircleV(targetPoint, SIZE, Color.WHITE);
            }
            foreach (Vector2 point in points)
            {
                Raylib.DrawCircleV(point, SIZE, Color.WHITE);
            }
            Raylib.DrawFPS(10, 10);
            Raylib.EndDrawing();
        }

        static void ChangeState (GlobalState change)
        {
            switch (change)
            {
                case GlobalState.Intro:
                    points.Clear();
                    targetPoints.Clear();
                    Raylib.SetTargetFPS(30);
                    currentState = change;
                    break;

                case GlobalState.Simulation:
                    activePoint = Raylib.GetMousePosition();
                    points.Add(activePoint);
                    Raylib.SetTargetFPS(0);
                    currentState = change;
                    break;
            }
        }

        static void ActivePointUpdate()
        {
            int randInt = random.Next(targetPoints.Count);
            Vector2 target = targetPoints[randInt];
            Vector2 activeToTarget = target - activePoint;
            activeToTarget *= 1/2f;
            activePoint += activeToTarget;
            points.Add(activePoint);
        }
    }
}
