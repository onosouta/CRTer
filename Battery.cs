using Microsoft.Xna.Framework;

namespace Game2
{
    //砲台
    class Battery : Block
    {
        public Battery(Scene _scene, bool _is_sample, int _num = 1) : base(_scene, _is_sample, _num)
        {
            if (!is_sample)
            {
                scene.GetMap().GetPaint().GetNones().Add(this);//Nonesに追加

                SpriteComponent sc = new SpriteComponent(this, 90);
                sc.SetTexture(scene.GetRenderer().GetTexture("texture/battery"));

                AABB aabb;
                aabb.min = new Vector2(-12.0f, -12.0f);
                aabb.max = new Vector2(12.0f, 12.0f);
                BoxComponent bc = new BoxComponent(this, aabb);

                rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(90.0f * num));

                LaserComponent lc = new LaserComponent(this);
            }
        }

        //コピー
        public override object Clone()
        {
            return new Battery(this);
        }

        //コピーコンストラクタ
        private Battery(Battery _battery) : this(_battery.scene, false, _battery.num) { }
    }
}
