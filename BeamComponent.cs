using System;

namespace Game2
{
    //ビーム
    class Beam : Actor
    {
        public Beam(Scene _scene) : base(_scene)
        {
            state = State.Dead;//即破棄
        }
    }

    //ビームコンポーネント
    class BeamComponent : Component
    {
        private BeamSprite sprite;//レーザーのスプライト

        private Fire fire;

        public BeamComponent(BeamBattery _owner, int _order = 100) : base(_owner, _order)
        {
            sprite = new BeamSprite((BeamBattery)owner, 80);
            fire = new Fire(owner.GetScene(), owner, -1, 1.0f);
        }

        public override void Update(float _delta_time)
        {
            base.Update(_delta_time);

            float length = 1000.0f;

            //線分
            Line line = new Line(
                owner.GetPosition(),                                //始点
                owner.GetPosition() + owner.GetFront() * length);   //終点
            Information information = new Information();//情報

            Physics physics = owner.GetScene().GetPhysics();
            if (physics.Intersect(line, owner, ref information))//衝突
            {
                length = (owner.GetPosition() - information.point).Length();//長さを計算

                if (information.actor is Player)
                {
                    //発射(疑似)
                    Beam beam = new Beam(owner.GetScene());//ビーム(即破棄)
                    information.actor.Hit(beam, 0x00);
                }
                
                Random random = GameDevice.GetInstance().GetRandom();
                fire.SetPosition(information.point);
                fire.SetScale(0.05f + random.Next(-5, 5) / 1000.0f);

                if (random.Next(5) == 0)
                {
                    BeamShards bs = new BeamShards(owner.GetScene(), information.point);
                    
                    Trail trail = new Trail(information.point);
                    Scene scene = owner.GetScene();
                    scene.GetMap().GetPaint().GetTrails().Add(trail);//追加
                }
            }
            else
            {
                fire.SetScale(0.0f);
            }

            sprite.SetLength((int)length);//※int型
        }
    }
}
