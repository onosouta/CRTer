using Microsoft.Xna.Framework;

namespace Game2
{
    //ビームのスプライトコンポーネント
    class BeamSprite : SpriteComponent
    {
        private int length;//ビームの長さ

        public BeamSprite(BeamBattery _owner, int _order = 100) : base(_owner, _order)
        {
            //テクスチャを設定
            SetTexture(owner.GetScene().GetRenderer().GetTexture("texture/beam"));
        }

        public override void Draw(Renderer _renderer)
        {
            for (int i = 0; i < length; i++)
            {
                Matrix world = Matrix.CreateTranslation(
                    owner.GetPosition() +
                    owner.GetFront() * i);
                _renderer.Draw(texture, world, 0.75f);//ビームを描画
            }
        }

        //ゲッター
        public int GetLength() { return length; }//※int型

        //セッター
        public void SetLength(int _length) { length = _length; }//※int型
    }
}
