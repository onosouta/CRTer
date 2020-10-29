using Microsoft.Xna.Framework;

namespace Game2
{
    //歯車
    class Gear : Block
    {
        private int angle;//角度

        public Gear(Scene _scene, bool _is_sample, int _num = 1) : base(_scene, _is_sample, _num)
        {
            if (!is_sample)
            {
                scene.GetMap().GetPaint().GetNones().Add(this);//Nonesに追加

                SpriteComponent sc = new SpriteComponent(this, 80);
                sc.SetTexture(scene.GetRenderer().GetTexture("texture/gear"));

                AABB aabb;
                aabb.min = new Vector2(-12.0f, -12.0f);
                aabb.max = new Vector2(12.0f, 12.0f);
                BoxComponent bc = new BoxComponent(this, aabb);

                angle = 0;
            }
        }

        public override void ActorUpdate(float _delta_time)
        {
            base.ActorUpdate(_delta_time);

            //回転
            angle += 20;
            if (angle >= 360) angle = 0;

            Quaternion q = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(angle));
            SetRotation(q);
        }

        //コピー
        public override object Clone()
        {
            return new Gear(this);
        }

        //コピーコンストラクタ
        private Gear(Gear _gear) : this(_gear.scene, false, _gear.num) { }
    }
}
