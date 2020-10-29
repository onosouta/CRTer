using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Game2
{
    //アクターの状態
    enum State
    {
        Active,
        Pause,
        Dead,
    }

    //アクター
    class Actor
    {
        protected Scene scene;//シーン

        protected State state;

        protected Matrix world;//ワールド行列
        protected bool is_recalculate;
        protected float scale;
        protected Quaternion rotation;
        protected Vector3 position;

        protected List<Component> components = new List<Component>();//コンポーネント

        public Actor(Scene _scene)
        {
            scene = _scene;

            state = State.Active;

            world = Matrix.Identity;
            is_recalculate = true;
            scale = 1.0f;
            rotation = Quaternion.Identity;
            position = Vector3.Zero;

            if (scene != null) scene.AddActor(this);//追加
        }
        
        public void Update(float _delta_time)
        {
            if (state == State.Active)
            {
                WorldTransform();

                //コンポーネントを更新
                for (int i = 0; i < components.Count; i++)
                {
                    components[i].Update(_delta_time);
                }

                ActorUpdate(_delta_time);

                WorldTransform();
            }
        }

        public virtual void ActorUpdate(float _delta_time) { }

        //ワールド行列を計算
        public void WorldTransform()
        {
            if (is_recalculate)//再計算する
            {
                is_recalculate = false;
                world = Matrix.CreateScale(scale);
                world *= Matrix.CreateFromQuaternion(rotation);
                world *= Matrix.CreateTranslation(position);

                foreach (var c in components)
                {
                    c.WorldTransform();
                }
            }
        }

        public virtual void Draw(Renderer _renderer) { }

        public virtual void Hit(Actor _actor, DIRECTION _d) { }

        //コンポーネントを追加
        public void AddComponent(Component _component)
        {
            int order = _component.GetOrder();

            int i = 0;
            for (; i < components.Count; i++)
            {
                if (order < components[i].GetOrder())
                {
                    break;
                }
            }

            components.Insert(i, _component);
        }

        //コンポーネントを削除
        public void RemoveComponent(Component _component)
        {
            components.Remove(_component);
        }

        //ゲッター
        public State GetState() { return state; }
        public Matrix GetWorld() { return world; }
        public float GetScale() { return scale; }
        public Quaternion GetRotation() { return rotation; }
        public Vector3 GetPosition() { return position; }
        public Scene GetScene() { return scene; }

        public Vector3 GetFront() { return Vector3.Transform(Vector3.UnitX, rotation); }//前方ベクトル

        //セッター
        public void SetState(State _state) { state = _state; }
        public void SetWorld(Matrix _world) { world = _world; }
        public void SetScale(float _scale) { scale = _scale; is_recalculate = true; }
        public void SetRotation(Quaternion _rotation) { rotation = _rotation; is_recalculate = true; }
        public void SetPosition(Vector3 _position) { position = _position; is_recalculate = true; }
    }
}
