namespace Game2
{
    //オフブロック
    class OFF : Block
    {
        public OFF(Scene _scene, bool _is_sample, int _num = 1) : base(_scene, _is_sample, _num)
        {
            if (!is_sample)
            {
                scene.GetMap().GetPaint().GetNones().Add(this);//Nonesに追加
            }
        }

        //スイッチオン!
        public override void ON()
        {
            base.ON();

            state = State.Dead;
            scene.GetMap().GetPaint().GetNones().Remove(this);//Nonesから削除

            //ONブロック
            ON on = new ON(scene, false);
            on.SetPosition(position);
            on.SetX(x);
            on.SetY(y);

            Map map = scene.GetMap();
            map.GetBlocks()[y][x] = on;//追加
        }

        //コピー
        public override object Clone()
        {
            return new OFF(this);
        }

        //コピーコンストラクタ
        private OFF(OFF _off) : this(_off.scene, false, _off.num) { }
    }
}
