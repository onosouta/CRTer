using Microsoft.Xna.Framework;

namespace Game2
{
    //棘ブロック
    class Thorn : Block
    {
        public Thorn(Scene _scene, bool _is_sample, int _num = 1) : base(_scene, _is_sample, _num)
        {
            if (!is_sample)
            {
                scene.GetMap().GetPaint().GetNones().Add(this);//Nonesに追加

                SpriteComponent sc = new SpriteComponent(this, 90);
                sc.SetTexture(scene.GetRenderer().GetTexture("texture/thorn"));

                AABB aabb;
                aabb.min = new Vector2(-12.0f, -12.0f);
                aabb.max = new Vector2(12.0f, 12.0f);
                BoxComponent bc = new BoxComponent(this, aabb);

                rotation = Quaternion.CreateFromAxisAngle(Vector3.UnitZ, MathHelper.ToRadians(90.0f * num));
            }
        }

        //コピー
        public override object Clone()
        {
            return new Thorn(this);
        }

        //コピーコンストラクタ
        private Thorn(Thorn _thorn) : this(_thorn.scene, false, _thorn.num) { }
    }
}
