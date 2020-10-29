using Microsoft.Xna.Framework;

namespace Game2
{
    class BeamFireSprite : SpriteComponent
    {
        public BeamFireSprite(BeamBattery _owner, int _order = 100) : base(_owner, _order)
        {
            //テクスチャを設定
            SetTexture(owner.GetScene().GetRenderer().GetTexture("texture/fire"));
        }

        public override void Draw(Renderer _renderer)
        {
            Matrix world =
                Matrix.CreateScale(0.075f + GameDevice.GetInstance().GetRandom().Next(-2, 2) / 500.0f) *
                Matrix.CreateTranslation(owner.GetPosition() + owner.GetFront() * 15);

            _renderer.Draw(texture, world, 0.5f);
        }
    }
}
