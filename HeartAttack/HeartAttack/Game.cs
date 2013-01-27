using System;
using System.Collections.Generic;

namespace HeartAttack
{
    public enum GameState
    {
        IntroScreen,
        MainMenu,
        GameScreen,
        BuyScreen,
        GameOver
    }

    public class Game
    {
        #region Common Variables
        private GameState _state = GameState.GameScreen;
        private Ship _ship;
        private bool EXIT_GAME = false;
        private DateTime _gametime = new DateTime();
        private TimeSpan _deltaTime = new TimeSpan();

        #endregion

        #region Game Screen Variables
        private HealthBar _bar = new HealthBar();
        private List<Enemy> _enemies = new List<Enemy>();
        private List<Bullet> _bullets = new List<Bullet>();
        private int _moola = 0;
        private bool _initGameScreen = false;
        private DateTime _shipLastFireTime;
        private int _shipRof = 1;
        #endregion

        #region Buy Screen Variables
        private bool _initBuyScreen = false;
        private Vector2D _currentPosition = null;
        private bool _showAvailablePoints = true;
        private bool _showPurchaseList = false;
        private bool _showingPurchaseList = false;
        private bool _showingAvailablePoints = false;
        List<Vector2D> _addonNodes = new List<Vector2D>();
        Dictionary<string, int> _availableAddons = new Dictionary<string, int>();
        #endregion


        internal void Start()
        {
            Random r = new Random();

            _gametime = DateTime.Now;
            Console.Clear();
            while (!EXIT_GAME)
            {

                switch (_state)
                {
                    case GameState.GameScreen:
                        #region Handle GameScreen

                        #region Initialise
                        if (null == _ship)
                        {
                            _ship = new Ship();
                            _ship.Position = new Vector2D(Console.WindowWidth / 2, Console.WindowHeight / 2);
                            _ship.Health = 135;
                            _ship.Sheilds = 0;
                            _shipLastFireTime = DateTime.Now;
                            _bar.Health = _ship.Health;
                        }

                        if (!_initGameScreen)
                        {
                            Console.Clear();
                            _initGameScreen = true;
                            _initBuyScreen = false;
                        }
                        #endregion

                        #region Key Handling
                        if (Console.KeyAvailable)
                        {
                            ConsoleKeyInfo c = Console.ReadKey(true);

                            switch (c.Key)
                            {
                                //Buy
                                case ConsoleKey.P:
                                    if (_moola > 0)
                                    {
                                        _state = GameState.BuyScreen;
                                    }
                                    break;
                                case ConsoleKey.Escape:
                                    break;
                            }
                        }
                        #endregion

                        #region Update States

                        foreach (Enemy e in _enemies)
                        {
                            if (_ship.Position.DistanceTo(e.Position) <= 5 && _ship.Position.DistanceTo(e.Position) > 0 && DateTime.Now.Subtract(_shipLastFireTime).TotalSeconds > _shipRof)
                            {
                                _bullets.Add(new Bullet()
                                {
                                    Position = _ship.Position,
                                    Velocity = new Vector2D(1, 1).ScalarMultiply(5f),
                                    TrackEnemy = true,
                                    Target = e
                                });
                                _shipLastFireTime = DateTime.Now;
                                break;
                            }
                        }

                        _deltaTime = DateTime.Now.Subtract(_gametime);
                        if (_deltaTime.TotalMilliseconds > 300)
                        {
                            // Check if out of bounds
                            List<Enemy> rem = new List<Enemy>();
                            foreach (Enemy e in _enemies)
                            {
                                Vector2D temp = e.Position.Add(e.Velocity);
                                if (temp.X < 0 || temp.Y < 0 || temp.X >= Console.WindowWidth || temp.Y >= Console.WindowHeight)
                                {
                                    rem.Add(e);
                                }
                                else
                                {
                                    e.Position = temp;
                                }
                            }

                            List<Enemy> death = new List<Enemy>();
                            List<Bullet> rembullet = new List<Bullet>();
                            foreach (Bullet b in _bullets)
                            {                                
                                if (b.TrackEnemy)
                                {
                                    // Adjust velocity
                                    b.Velocity = b.Position.Subtract(b.Target.Position).Unit();
                                }
                                Vector2D temp = b.Position.Add(b.Velocity);
                                if (temp.X < 0 || temp.Y < 0 || temp.X >= Console.WindowWidth || temp.Y >= Console.WindowHeight)
                                {
                                    rembullet.Add(b);
                                }
                                else
                                {
                                    b.Position = temp;
                                }

                                foreach (Enemy e in _enemies)
                                {
                                    if ((int)e.Position.X == (int)b.Position.X && (int)e.Position.Y == (int)b.Position.Y)
                                    {
                                        if(!rem.Contains(e))rem.Add(e);
                                        if(!rembullet.Contains(b))rembullet.Add(b);
                                        _moola += 50;                                        
                                        break;
                                    }
                                }                                
                            }

                            foreach (AddonBase addon in _ship.Addons)
                            {
                                addon.Update();
                                Console.Beep(2000, 1);
                            }

                            bool clearscreen = false;
                            foreach (Enemy e in rem)
                            {                               
                                foreach (Bullet b in _bullets)
                                {
                                    if (b.Target == e)
                                    {
                                        if(!rembullet.Contains(b))rembullet.Add(b);
                                        break;
                                    }
                                }
                                _enemies.Remove(e);
                                clearscreen = true;
                            }

                            
                            foreach (Bullet b in rembullet)
                            {
                                _bullets.Remove(b);
                                clearscreen = true;
                            }

                            if (_enemies.Count <= 0)
                            {
                                _bullets.Clear();
                                
                                for (int j = 0; j < 20; j++)
                                {
                                    int x = r.Next(0, Console.WindowWidth);
                                    int y = r.Next(0, Console.WindowHeight);
                                    Enemy ne = new Enemy()
                                    {
                                        Position = new Vector2D(x, y),
                                        Velocity = new Vector2D(x, y).Subtract(_ship.Position).Unit()
                                    };
                                    _enemies.Add(ne);
                                }
                            }

                            if (clearscreen)
                            {
                                Console.Clear();
                                clearscreen = false;
                            }

                            _gametime = DateTime.Now;
                        }
                        #endregion

                        DrawGame();

                        #endregion
                        break;
                    case GameState.BuyScreen:
                        #region Handle Buy Screen
                        if (!_initBuyScreen)
                        {
                            Console.Clear();
                            _currentPosition = _ship.Position;
                            _initBuyScreen = true;
                            _initGameScreen = false;
                            _showAvailablePoints = true;

                            if (_availableAddons.Count == 0)
                            {
                                _availableAddons.Add("Mini Turret Addon", 100);
                            }
                        }

                        if (Console.KeyAvailable)
                        {
                            string c = Console.ReadKey(true).KeyChar.ToString();

                            int index;
                            int.TryParse(c, out index);

                            if (index > 0 && index < 5 && _addonNodes.Count > 0 && !_showPurchaseList)
                            {
                                _currentPosition = _addonNodes[index - 1];
                                AddonBase addon = _ship.GetAddonForPosition(_currentPosition);
                                if (addon == null)
                                {
                                    _showAvailablePoints = false;                                    
                                    _showPurchaseList = true;
                                }
                                else
                                {
                                    _showingAvailablePoints = false;
                                    _showingPurchaseList = false;
                                    _showPurchaseList = false;
                                    _showAvailablePoints = true;
                                }
                            }
                            else if(_showPurchaseList && index > 0)
                            {
                                switch (index)
                                {
                                    case 1:                                        
                                        MiniTurretAddon t = new MiniTurretAddon(_enemies,_bullets)
                                        {
                                            Position = _currentPosition
                                        };
                                        if (_moola > t.Cost)
                                        {
                                            _ship.Addons.Add(t);
                                            _ship.Health -= 20;
                                            _moola -= t.Cost;
                                            _bar.Health = _ship.Health;
                                        }
                                        break;
                                }
                                _showingAvailablePoints = false;
                                _showPurchaseList = false;
                                _initGameScreen = false;
                                _state = GameState.GameScreen;
                            }
                        }

                        if (_showAvailablePoints)
                        {
                            _addonNodes.Clear();
                            _addonNodes.Add(new Vector2D(_currentPosition.X, _currentPosition.Y - 1));
                            _addonNodes.Add(new Vector2D(_currentPosition.X + 1, _currentPosition.Y));
                            _addonNodes.Add(new Vector2D(_currentPosition.X, _currentPosition.Y + 1));
                            _addonNodes.Add(new Vector2D(_currentPosition.X - 1, _currentPosition.Y));
                            DrawBuyPoints();
                        }
                        else if (_showPurchaseList)
                        {
                            ShowPurchaseMenu();
                        }
                        
                        #endregion
                        break;
                }
            }
        }

        private void ShowPurchaseMenu()
        {
            if (!_showingPurchaseList)
            {
                Console.Clear();
                Console.CursorTop = 0;
                Console.CursorLeft = 0;

                int counter = 1;
                foreach (string a in _availableAddons.Keys)
                {
                    Console.CursorTop = counter;
                    Console.Write(counter.ToString() + ". " + a + " - Price - $" + _availableAddons[a]);
                    counter++;
                }

                _showingPurchaseList = true;
            }
        }

        private void DrawBuyPoints()
        {
            if (!_showingAvailablePoints)
            {
                // Init draw;
                Console.CursorLeft = 0;
                Console.CursorTop = 0;

                Console.Clear();
                _ship.Draw();

                int counter = 1;
                foreach (Vector2D node in _addonNodes)
                {
                    Console.CursorLeft = (int)node.X;
                    Console.CursorTop = (int)node.Y;
                    Console.Write(counter);
                    counter++;
                }
                _showingAvailablePoints = true;
            }
        }

        private void DrawGame()
        {
            // Init draw;
            Console.CursorLeft = 0;
            Console.CursorTop = 0;

            #region Draw stuff
            _ship.Draw();

            //Draw Enemies      
            foreach (Enemy e in _enemies)
            {
                e.Draw();
            }


            foreach (Bullet b in _bullets)
            {
                b.Draw();
            }
            //Draw health
            _bar.Draw();

            Console.CursorTop = 1;
            Console.CursorLeft = 1;            
            Console.Write("$" + _moola);


            Console.CursorLeft = 0;
            Console.CursorTop = 0;
            #endregion

        }
    }
}
