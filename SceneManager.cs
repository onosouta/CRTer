using System.Collections.Generic;

namespace Game2
{
    //シーン管理(シングルトン)
    sealed class SceneManager
    {
        private static SceneManager instance;//インスタンス

        private Stack<Scene> scenes = new Stack<Scene>();
        private Scene scene;
        
        private SceneManager()
        {
            Load(new Title(this));
        }

        public static SceneManager GetInstance()
        {
            if (instance == null)
            {
                instance = new SceneManager();
            }

            return instance;
        }
        
        public void Load(Scene _scene)
        {
            scenes.Clear();
            Push(_scene);
        }//読み込み

        public void Push(Scene _scene)
        {
            scene = _scene;
            scene.Initialize();
            scenes.Push(scene);//追加
        }//プッシュ

        public void Pop()
        {
            if (scenes.Count > 1)
            {
                scenes.Pop();//削除
                scene = scenes.Peek();
            }
        }//ポップ

        //Game1クラスのみ
        public void Update(float _delta_time)
        {
            scene.Update(_delta_time);
        }

        //Game1クラスのみ
        public void Draw(Renderer _renderer)
        {
            scene.Draw(_renderer);
        }

        //ゲッター
        public Scene GetScene() { return scene; }
    }
}
