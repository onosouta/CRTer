namespace Game2
{
    class None : Block
    {
        public None(Scene _scene, bool _is_sample) : base(_scene, _is_sample)
        {
            if (!_is_sample)
            {
                scene.GetMap().GetPaint().GetNones().Add(this);//Nonesに追加
            }
        }

        //コピー
        public override object Clone()
        {
            return new None(this);
        }

        //コピーコンストラクタ
        private None(None _none) : this(_none.scene, false) { }
    }
}
