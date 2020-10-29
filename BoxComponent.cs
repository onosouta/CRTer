using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Game2
{
    //AABB
    struct AABB
    {
        public Vector2 min; //最小
        public Vector2 max; //最大

        public bool Contain(ref Vector3 _point)
        {
            bool is_out =
                _point.X < min.X ||
                _point.Y < min.Y ||
                _point.X > max.X ||
                _point.Y > max.Y;

            return !is_out;
        }
    }

    //ボックスコンポーネント
    class BoxComponent : Component
    {
        private AABB aabb;
        private AABB world_aabb;

        public BoxComponent(Actor _owner, AABB _aabb, int _order = 150) : base(_owner, _order)
        {
            aabb = _aabb;
            owner.GetScene().GetPhysics().AddBox(this);

            texture = owner.GetScene().GetRenderer().GetTexture("texture/aabb");//描画用
        }

        public override void WorldTransform()
        {
            base.WorldTransform();

            world_aabb = aabb;

            //拡縮
            world_aabb.min *= owner.GetScale();
            world_aabb.max *= owner.GetScale();

            //座標
            Vector2 position = new Vector2(
                owner.GetPosition().X,
                owner.GetPosition().Y);
            world_aabb.min += position;
            world_aabb.max += position;
        }

        #region 描画

        private Texture2D texture;
        
        //描画
        public void Draw(Renderer _renderer)
        {
            Vector3 scale = new Vector3(
                (-aabb.min.X + aabb.max.X) * owner.GetScale() / texture.Width,
                (-aabb.min.Y + aabb.max.Y) * owner.GetScale() / texture.Height,
                0.0f);

            Matrix world =
                Matrix.CreateScale(scale) *
                Matrix.CreateTranslation(owner.GetPosition());
            
            _renderer.Draw(texture, world, 0.1f);
        }

        #endregion 描画

        public AABB GetWorldAABB() { return world_aabb; }
    }
}
