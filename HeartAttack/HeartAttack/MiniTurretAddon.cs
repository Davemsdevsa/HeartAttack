using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeartAttack
{
    class MiniTurretAddon : AddonBase
    {
        DateTime _lastFireTime;
        List<Enemy> _enemies;
        List<Bullet> _bullets;
        private int rof = 2;
        public MiniTurretAddon(List<Enemy> enemies, List<Bullet> bullets)
        {
            _lastFireTime = DateTime.Now;
            _enemies = enemies;
            _bullets = bullets;
        }

        public override void Update()
        {
            foreach (Enemy e in _enemies)
            {
                if (this.Position.DistanceTo(e.Position) <= 3 && DateTime.Now.Subtract(_lastFireTime).TotalSeconds > rof)
                {
                    _bullets.Add(new Bullet(){
                        Position = this.Position,
                        Velocity = new Vector2D(),
                        TrackEnemy = true,
                        Target = e
                    });
                    _lastFireTime = DateTime.Now;
                    break;
                }
            }
        }

        public override void Draw()
        
        {
            ConsoleColor originalcolor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.CursorTop = (int)Position.Y;
            Console.CursorLeft = (int)Position.X;
            Console.Write("∙");
            Console.ForegroundColor = originalcolor; ;
        }
    }
}
