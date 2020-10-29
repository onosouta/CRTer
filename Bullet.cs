using Microsoft.Xna.Framework;

namespace Game2
{
    //バレット
    class Bullet : Actor
    {
        private Actor owner;

        public Bullet(Scene _scene, Actor _owner, Quaternion _rotation, Vector3 _position, float _front_speed)
            : base(_scene)
        {
            owner = _owner;
            rotation = _rotation;   //回転
            position = _position;   //移動

            SpriteComponent sc = new SpriteComponent(this, 80);
            sc.SetTexture(scene.GetRenderer().GetTexture("texture/bullet"));

            BulletMoveComponent bmc = new BulletMoveComponent(this);
            bmc.SetFrontSpeed(_front_speed);//前方スピード

            LagComponent lc = new LagComponent(this, sc.GetTexture());
        }

        //ゲッター
        public Actor GetOwner() { return owner; }
    }
}
