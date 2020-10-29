using Microsoft.Xna.Framework;

namespace Game2
{
    //レーザー
    class Laser : Actor
    {
        public Laser(Scene _scene) : base(_scene)
        {
            state = State.Dead;//即破棄
        }
    }

    //レーザーコンポーネント
    class LaserComponent : Component
    {
        private LaserSprite sprite;//レーザーのスプライト

        private bool is_detected;//検知したか
        private int lag;//ラグ(フレーム)

        public LaserComponent(Battery _owner, int _order = 100) : base(_owner, _order)
        {
            sprite = new LaserSprite((Battery)owner, 80);

            Initialize();
        }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            float length = 500.0f;

            //線分
            Line line = new Line(
                owner.GetPosition(),                                //始点
                owner.GetPosition() + owner.GetFront() * length);   //終点
            Information information = new Information();//情報

            Physics physics = owner.GetScene().GetPhysics();
            if (physics.Intersect(line, owner, ref information))//衝突
            {
                length = (owner.GetPosition() - information.point).Length();//長さを計算

                if (is_detected)//検知した
                {
                    lag--;
                    if (lag <= 0)
                    {
                        //発射(疑似)
                        Laser laser = new Laser(owner.GetScene());//レーザー(即破棄)
                        information.actor.Hit(laser, 0x00);

                        for (int i = 0; i < 4; i++)
                        {
                            LaserChip lc = new LaserChip(owner.GetScene(), information.point);
                        }
                        
                        Trail trail = new Trail(information.point);
                        Scene scene = owner.GetScene();
                        scene.GetMap().GetPaint().GetTrails().Add(trail);//追加

                        Initialize();
                    }
                }

                if (information.actor is Player)
                {
                    is_detected = true;//AABBを検知!

                    Fire fire = new Fire(owner.GetScene(), owner, 2, 0.5f);
                    fire.SetScale(0.2f);
                    fire.SetPosition(owner.GetPosition());
                    
                    //ウェーブ
                    Scene scene = owner.GetScene();
                    Wave wave = new Wave(
                        scene,                  //scene
                        owner.GetPosition(),    //position
                        0.4f,                   //size
                        0.6f);                 //time
                    scene.GetWaveManager().AddWave(wave);//追加
                }
            }
            else
            {
                Initialize();
            }

            sprite.SetLength((int)length);//※int型
        }

        //初期化
        private void Initialize()
        {
            is_detected = false;
            lag = 2;
        }
    }
}
