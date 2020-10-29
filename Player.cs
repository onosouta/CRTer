using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;

namespace Game2
{
    //プレイヤー
    class Player : Actor
    {
        private InputComponent input;//入力コンポーネント
        private CollisionComponent cc;

        private float cool_down = 0.0f;//間隔

        public Player(Scene _scene) : base(_scene)
        {
            rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, (float)Math.PI / 2.0f);

            SpriteComponent sprite = new SpriteComponent(this, 100);
            sprite.SetTexture(scene.GetRenderer().GetTexture("texture/player"));

            input = new InputComponent(this);
            cc = new CollisionComponent(this, 16, 16);

            AABB aabb;
            aabb.min = new Vector2(-8, -8);
            aabb.max = new Vector2( 8,  8);
            BoxComponent bc = new BoxComponent(this, aabb);

            GameDevice.GetInstance().GetSE().Load("se/shot");
            GameDevice.GetInstance().GetSE().Load("se/clear");
        }

        public override void ActorUpdate(float _delta_time)
        {
            base.ActorUpdate(_delta_time);

            cool_down = Math.Max(cool_down - _delta_time, 0.0f);

            if (cool_down <= 0.0f &&
                (Input.GetLMB()))// || Input.GetKey(Keys.Z) || Input.GetButton(Buttons.RightShoulder)))//発射
            {
                Bullet b = new Bullet(
                    scene,          //Scene
                    this,           //owner
                    rotation,       //rotation
                    position,       //position
                    3000.0f);       //front_speed
                cool_down = 0.1f;

                Random random = GameDevice.GetInstance().GetRandom();
                if (random.Next(2) == 0)
                {
                    Fire fire = new Fire(scene, this, 1, 0.5f);
                    fire.SetScale(0.1f);
                    fire.SetPosition(position);
                }

                //振動
                Display display = GameDevice.GetInstance().GetDisplay();
                display.is_wave = true;
                display.is_x = true;
                display.is_y = true;
                display.speed = 1.0f;

                GameDevice.GetInstance().GetSE().Play("se/shot");
            }
        }

        //衝突判定
        public override void Hit(Actor _actor, DIRECTION _d)
        {
            base.Hit(_actor, _d);

            if (_actor is Thorn)//刺
            {
                Thorn thorn = (Thorn)_actor;//Thornにキャスト
                //方向を判定
                if ((thorn.GetNum() == 1 && (_d &= DIRECTION.UP)    == DIRECTION.UP)   ||   //上
                    (thorn.GetNum() == 2 && (_d &= DIRECTION.LEFT)  == DIRECTION.LEFT) ||   //左
                    (thorn.GetNum() == 3 && (_d &= DIRECTION.DOWN)  == DIRECTION.DOWN) ||   //下
                    (thorn.GetNum() == 4 && (_d &= DIRECTION.RIGHT) == DIRECTION.RIGHT))    //右
                    Dead();
            }

            if (_actor is Laser) Dead();//レーザー
            if (_actor is Beam) Dead();//ビーム

            if (_actor is Gear) Dead();//歯車

            if (_actor is Goal)
            {
                Wave wave = new Wave(scene, position, 1.0f, 1.0f);
                scene.GetWaveManager().AddWave(wave);

                state = State.Dead;
                
                //振動
                Display display = GameDevice.GetInstance().GetDisplay();
                display.is_wave = true;
                display.is_x = true;
                display.is_y = true;
                display.speed = 12.0f;

                Actor a = new Actor(scene);
                a.SetPosition(scene.GetCamera().GetPosition() - Vector3.UnitZ + new Vector3(0.0f, 100.0f, 0.0f));
                SpriteComponent sc = new SpriteComponent(a, 1000);
                sc.SetTexture(scene.GetRenderer().GetTexture("Texture/clear"));
                GameDevice.GetInstance().GetSE().Play("se/clear");

                if (!scene.GetIsClear())
                {
                    if (scene is Game)
                    {
                        Game g = (Game)scene;
                        NextMenu next = new NextMenu(g);
                        g.SetNext(next);
                    }
                }

                scene.SetIsClear(true);
            }
        }

        private void Dead()
        {
            state = State.Dead;
            
            //ウェーブ
            Wave wave = new Wave(
                scene,                  //scene
                position,               //position
                1.0f,                   //size
                1.0f);                  //time
            scene.GetWaveManager().AddWave(wave);//追加

            //振動
            Display display = GameDevice.GetInstance().GetDisplay();
            display.is_wave = true;
            display.is_x = true;
            display.is_y = true;
            display.speed = 12.0f;
            
            Actor a = new Actor(scene);
            a.SetPosition(scene.GetCamera().GetPosition() - Vector3.UnitZ + new Vector3(0.0f, 100.0f, 0.0f));
            SpriteComponent sc = new SpriteComponent(a, 1000);
            sc.SetTexture(scene.GetRenderer().GetTexture("Texture/gameover"));

            if (!scene.GetIsDead())
            {
                if (scene is Game)
                {
                    Game g = (Game)scene;
                    RetryMenu retry = new RetryMenu(g);
                    g.SetRetry(retry);
                }
            }

            scene.SetIsDead(true);

        }

        public float GetFrontSpeed() { return input.GetFrontSpeed(); }
        public void SetPrePosition(Vector3 _pre_position) { cc.SetPrePosition(_pre_position); }
    }
}
