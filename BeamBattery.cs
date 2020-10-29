using Microsoft.Xna.Framework;
using System;

namespace Game2
{
    //ビームの砲台
    class BeamBattery : Block
    {
        private float pre_rotation;

        private bool is_chase;

        public BeamBattery(Scene _scene, bool _is_sample, bool _is_chase = true, int _num = 1) : base(_scene, _is_sample, _num)
        {
            is_chase = _is_chase;

            if (!is_sample)
            {
                scene.GetMap().GetPaint().GetNones().Add(this);//Nonesに追加

                SpriteComponent sc = new SpriteComponent(this, 90);
                sc.SetTexture(scene.GetRenderer().GetTexture("texture/beam_battery"));

                AABB aabb;
                aabb.min = new Vector2(-12.0f, -12.0f);
                aabb.max = new Vector2(12.0f, 12.0f);
                BoxComponent bc = new BoxComponent(this, aabb);
                
                BeamComponent beam = new BeamComponent(this);

                rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(90.0f * num));
                pre_rotation = MathHelper.ToRadians(90.0f * num);

                Player player = scene.GetPlayer();

                //if (player != null)
                //{
                //    //回転
                //    float r = MathHelper.ToDegrees(-(float)Math.Atan2(
                //        player.GetPosition().X - position.X,
                //        player.GetPosition().Y - position.Y));

                //    Quaternion q = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(r + 90.0f));
                //    SetRotation(q);

                //    pre_rotation = r;
                //}
            }
        }

        public override void ActorUpdate(float _delta_time)
        {
            base.ActorUpdate(_delta_time);

            if (!is_sample)
            {
                if (is_chase)
                {
                    Player player = scene.GetPlayer();

                    if (player != null)
                    {
                        //回転
                        float r = MathHelper.ToDegrees(-(float)Math.Atan2(
                            player.GetPosition().X - position.X,
                            player.GetPosition().Y - position.Y));

                        if (r < -180.0f && pre_rotation > 180.0f) pre_rotation -= 360.0f;
                        if (r > 180.0f && pre_rotation < -180.0f) pre_rotation += 360.0f;

                        if (r - pre_rotation > 0) r = Math.Min(r, pre_rotation + 0.2f);
                        if (r - pre_rotation < 0) r = Math.Max(r, pre_rotation - 0.2f);

                        Quaternion q = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(r + 90.0f));
                        SetRotation(q);

                        pre_rotation = r;

                        if (!is_sample &&
                            GameDevice.GetInstance().GetRandom().Next(20) == 0)
                        {
                            BeamShards b = new BeamShards(scene, position + GetFront() * 15.0f);
                        }
                    }
                }
            }
        }

        //コピー
        public override object Clone()
        {
            return new BeamBattery(this);
        }

        //コピーコンストラクタ
        private BeamBattery(BeamBattery _beam_battery) : this(_beam_battery.scene, false, _beam_battery.is_chase, _beam_battery.num) { }
    }
}
