using Microsoft.Xna.Framework;

namespace Game2
{
    struct Menu
    {
        public Actor actor;
        public SpriteComponent sprite;
    }

    //ゲームシーン
    class Game : Scene
    {
        private static readonly int map_max = 8;

        private int map_num;

        private RetryMenu retry;
        private NextMenu next;

        public Game(SceneManager _scene_manager, int _map_num) : base(_scene_manager)
        {
            map_num = _map_num;

            if (map_num == 1)
            {
                Actor a = new Actor(this);
                a.SetScale(1.5f);
                a.SetPosition(new Vector3(504.0f, 500.0f, 0.0f));
                SpriteComponent sc = new SpriteComponent(a, 50);
                sc.SetTexture(GetRenderer().GetTexture("texture/control"));
                sc.a = 0.8f;
            }
            if (map_num == 2)
            {
                Actor a = new Actor(this);
                a.SetScale(1.0f);
                a.SetPosition(new Vector3(504.0f, 400.0f, 0.0f));
                SpriteComponent sc = new SpriteComponent(a, 50);
                sc.SetTexture(GetRenderer().GetTexture("texture/control2"));
                sc.a = 0.8f;
            }
            if (map_num == 3)
            {
                Actor a = new Actor(this);
                a.SetScale(1.0f);
                a.SetPosition(new Vector3(504.0f, 800.0f, 0.0f));
                SpriteComponent sc = new SpriteComponent(a, 50);
                sc.SetTexture(GetRenderer().GetTexture("texture/control3"));
                sc.a = 0.8f;
            }
        }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            if (next != null)
                next.Update();
            if (retry != null)
                retry.Update();
        }

        public override void Load()
        {
            base.Load();

            player = new Player(this);
            map.Load(map_num);
            player.SetPosition(map.GetStartPosition());
            player.SetPrePosition(player.GetPosition());
            camera.SetPosition(player.GetPosition() + Vector3.UnitZ);

            Background background = new Background(this);//背景

            Wave wave = new Wave(this, player.GetPosition(), 0.5f, 0.25f);
            wave_manager.AddWave(wave);
        }

        public int GetMapMax() { return map_max; }
        public int GetMapNum() { return map_num; }

        public void SetNext(NextMenu _next) { next = _next; }
        public void SetRetry(RetryMenu _retry) { retry = _retry; }
    }
}
