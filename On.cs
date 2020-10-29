using Microsoft.Xna.Framework;

namespace Game2
{
    //ONブロック
    class ON : Block
    {
        public ON(Scene _scene, bool _is_sample, int _num = 1) : base(_scene, _is_sample, _num)
        {
            if (!is_sample)
            {
                SpriteComponent sc = new SpriteComponent(this, 90);
                sc.SetTexture(scene.GetRenderer().GetTexture("texture/normal"));

                AABB aabb;
                aabb.min = new Vector2(-12.0f, -12.0f);
                aabb.max = new Vector2(12.0f, 12.0f);
                BoxComponent bc = new BoxComponent(this, aabb);
            }
        }

        //スイッチオフ!
        public override void OFF()
        {
            base.OFF();

            state = State.Dead;

            //OFFブロック
            OFF off = new OFF(scene, false);
            off.SetPosition(position);
            off.SetX(x);
            off.SetY(y);

            Map map = scene.GetMap();
            map.GetBlocks()[y][x] = off;//追加
        }

        //コピー
        public override object Clone()
        {
            return new ON(this);
        }

        //コピーコンストラクタ
        private ON(ON _on) : this(_on.scene, false, _on.num) { }
    }
}
