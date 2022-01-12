using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

namespace Black_Jack {
    internal class Graphics {
        public static bool clear;
        private Stopwatch sw;
        private Scene _CurrentScene;
        public Scene CurrentScene { get { return _CurrentScene; } set { _CurrentScene = value; clear = true; } }
        public Graphics() {
            Thread GraphicsThread = new Thread(graphics);
            GraphicsThread.Name = "GraphicsThread";
            GraphicsThread.IsBackground = true;
            GraphicsThread.Start();
        }

        private void graphics() {
            sw = new Stopwatch();
            while (true) {
                if (CurrentScene != null) {
                    sw.Restart();
                    if (clear) { Console.ResetColor(); Console.Clear(); clear = false; }
                    CurrentScene.Render();
                    Console.Title = $"BlackJack | {Math.Floor(1 / sw.Elapsed.TotalSeconds)} fps";
                }
            }
        }
    }

    class Scene {
        public virtual void Render() { }
    }
}
