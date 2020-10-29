using Microsoft.Xna.Framework;

namespace Game2
{
    //ON/OFF スイッチブロック
    class Switch : Block
    {
        private bool on;//スイッチオンか

        public Switch(Scene _scene, bool _is_sample, int _num = 1) : base(_scene, _is_sample, _num)
        {
            if (!is_sample)
            {
                scene.GetMap().GetPaint().GetNones().Add(this);//Nonesに追加

                SpriteComponent sc = new SpriteComponent(this, 90);
                sc.SetTexture(scene.GetRenderer().GetTexture("texture/switch"));

                AABB aabb;
                aabb.min = new Vector2(-16.0f, -16.0f);
                aabb.max = new Vector2(16.0f, 16.0f);
                BoxComponent bc = new BoxComponent(this, aabb);
            }

            on = true;
        }

        public override void Hit(Actor _actor, DIRECTION _d)
        {
            base.Hit(_actor, _d);

            if (_actor is Bullet)//バレット
            {
                on = !on;//反転

                Map map = scene.GetMap();
                if (on)  map.ON();  //オン
                if (!on) map.OFF(); //オフ
            }
        }

        public override void ON()
        {
            base.ON();
            on = true;
        }

        public override void OFF()
        {
            base.OFF();
            on = false;
        }

        //コピー
        public override object Clone()
        {
            return new Switch(this);
        }

        //コピーコンストラクタ
        private Switch(Switch _switch) : this(_switch.scene, false, _switch.num) { }
    }
}
