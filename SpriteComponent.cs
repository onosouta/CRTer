using Microsoft.Xna.Framework.Graphics;

namespace Game2
{
    //スプライトコンポーネント
    class SpriteComponent : Component
    {
        protected Texture2D texture;//テクスチャ

        public SpriteComponent(Actor _owner, int _order) : base(_owner, _order)
        {
            owner.GetScene().AddSprite(this);//追加
            a = 1.0f;
        }

        //描画
        public virtual void Draw(Renderer _renderer)
        {
            _renderer.Draw(texture, owner.GetWorld(), a);
        }
        
        //テクスチャを設定
        public void SetTexture(Texture2D _texture) { texture = _texture; }
        public Texture2D GetTexture() { return texture; }

        public float a { set; get; }
    }
}
