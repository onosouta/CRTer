using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Game2
{
    //シーン
    abstract class Scene
    {
        protected SceneManager scene_manager;//シーン管理

        protected List<Actor> actors = new List<Actor>();
        protected List<Actor> add_actors = new List<Actor>();
        protected List<SpriteComponent> sprites = new List<SpriteComponent>();//スプライト

        protected bool is_updating;

        protected Physics physics;//物理クラス
        protected Player player;//プレイヤー
        protected Camera camera;//カメラ
        protected Map map;//マップ

        protected bool is_clear;
        protected bool is_dead;
        protected bool is_pause = false;

        protected WaveManager wave_manager;//ウェーブ管理

        public Scene(SceneManager _scene_manager)
        {
            scene_manager = _scene_manager;
        }

        public virtual void Initialize()
        {
            is_updating = false;
            Load();
        }

        public virtual void Update(float _delta_time)
        {
            is_updating = true;

            //アクターを更新
            foreach (var a in actors)
            {
                a.Update(_delta_time);
            }

            is_updating = false;

            foreach (var a in add_actors)
            {
                a.WorldTransform();
                actors.Add(a);
            }

            add_actors.Clear();

            RemoveActor();  //アクターを削除
            RemoveSprite(); //スプライトを削除

            physics.Update(_delta_time);
            wave_manager.Update(_delta_time);
        }

        public virtual void Draw(Renderer _renderer)
        {
            //スプライトを描画
            foreach (var s in sprites)
            {
                s.Draw(_renderer);
            }

            foreach (var a in actors)
            {
                a.Draw(_renderer);
            }

            if (Input.GetKey(Keys.P)) physics.Draw(_renderer);//AABBを描画
            wave_manager.Draw(_renderer);
        }

        public virtual void Load()
        {
            physics = new Physics();//物理クラス
            player = null;//プレイヤー
            camera = new Camera(this);//カメラ
            map = new Map(this);//マップ

            wave_manager = new WaveManager(this);
        }

        public void Pause()
        {
            is_pause = true;

            foreach (var a in actors)
            {
                if (a.GetState() == State.Active)
                {
                    a.SetState(State.Pause);
                }
            }
        }

        public void Active()
        {
            is_pause = false;

            foreach (var a in actors)
            {
                if (a.GetState() == State.Pause)
                {
                    a.SetState(State.Active);
                }
            }
        }

        //アクターを追加
        public void AddActor(Actor _actor)
        {
            if (is_updating)//更新中
            {
                add_actors.Add(_actor);
            }
            else
            {
                actors.Add(_actor);
            }
        }

        //アクターを削除
        private void RemoveActor()
        {
            List<Actor> remove_actors = new List<Actor>();
            foreach (var a in actors)
            {
                if (a.GetState() == State.Dead)
                {
                    remove_actors.Add(a);
                }
            }

            foreach (var a in add_actors)
            {
                if (a.GetState() == State.Dead)
                {
                    remove_actors.Add(a);
                }
            }

            for (int i = 0; i < remove_actors.Count; i++)
            {
                actors.Remove(remove_actors[i]);//削除
            }
        }

        //スプライトを追加
        public void AddSprite(SpriteComponent _sprite)
        {
            int order = _sprite.GetOrder();

            int i = 0;
            for (; i < sprites.Count; i++)
            {
                if (order < sprites[i].GetOrder())
                {
                    break;
                }
            }

            sprites.Insert(i, _sprite);
        }

        //スプライトを削除
        private void RemoveSprite()
        {
            List<SpriteComponent> remove_sprites = new List<SpriteComponent>();
            foreach (var s in sprites)
            {
                if (s.GetOwner().GetState() == State.Dead)
                {
                    remove_sprites.Add(s);
                }
            }

            for (int i = 0; i < remove_sprites.Count; i++)
            {
                sprites.Remove(remove_sprites[i]);//削除
            }
        }

        //ゲッター
        public Renderer GetRenderer() { return GameDevice.GetInstance().GetRenderer(); }
        public Physics GetPhysics() { return physics; }
        public Player GetPlayer() { return player; }
        public Camera GetCamera() { return camera; }
        public Map GetMap() { return map; }
        public bool GetIsClear() { return is_clear; }
        public bool GetIsDead() { return is_dead; }

        public WaveManager GetWaveManager() { return wave_manager; }

        public void SetMap(Map _map) { map = _map; }
        public void SetIsClear(bool _is_clear) { is_clear = _is_clear; }
        public void SetIsDead(bool _is_dead) { is_dead = _is_dead; }
    }
}
