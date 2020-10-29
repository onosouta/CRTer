using System;

namespace Game2
{
    //発射
    class Fire : Actor
    {
        private Actor owner;
        private int life;

        public Fire(Scene _scene, Actor _owner, int _life, float _a) : base(_scene)
        {
            owner = _owner;
            life = _life;

            SpriteComponent sprite = new SpriteComponent(this, 200);
            sprite.SetTexture(scene.GetRenderer().GetTexture("Texture/fire"));
            sprite.a = _a;
        }

        public override void ActorUpdate(float _delta_time)
        {
            base.ActorUpdate(_delta_time);

            if (life > 0)
            {
                life--;
            }

            if (life == 0)
            {
                state = State.Dead;
            }

            if (owner.GetState() == State.Dead)
            {
                state = State.Dead;
            }
        }
    }
}
