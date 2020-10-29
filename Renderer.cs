using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace Game2
{
    //レンダラー
    class Renderer
    {
        private Game1 game1;//Game1クラス

        private ContentManager content;  //コンテンツ管理
        private GraphicsDevice graphics; //グラフィックス

        private Matrix view;        //ビュー
        private Matrix projection;  //プロジェクション

        private Effect sprite_e;//スプライト
        private Dictionary<string, Texture2D> textures;//テクスチャ

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="_game1">Game1クラス</param>
        public Renderer(Game1 _game1)
        {
            game1 = _game1;

            content = _game1.Content;
            graphics = _game1.GraphicsDevice;

            VertexArrayObject();//VAO

            View(Vector3.UnitZ);    //ビュー
            Projection();           //プロジェクション

            sprite_e = content.Load<Effect>("fx/sprite_e");//スプライト
            textures = new Dictionary<string, Texture2D>();//テクスチャ
        }

        //VAO
        private void VertexArrayObject()
        {
            //頂点
            VertexPositionTexture[] vertices = new VertexPositionTexture[]
            {
                new VertexPositionTexture(new Vector3(-0.5f,  0.5f, 0.0f), new Vector2(0.0f, 0.0f)),    //左上
                new VertexPositionTexture(new Vector3( 0.5f,  0.5f, 0.0f), new Vector2(1.0f, 0.0f)),    //右上
                new VertexPositionTexture(new Vector3( 0.5f, -0.5f, 0.0f), new Vector2(1.0f, 1.0f)),    //右下
                new VertexPositionTexture(new Vector3(-0.5f, -0.5f, 0.0f), new Vector2(0.0f, 1.0f))     //左下
            };

            //頂点バッファ
            VertexBuffer vertex_buffer = new VertexBuffer(
                graphics,                       //GraphicsDevice
                typeof(VertexPositionTexture),  //System.Type
                vertices.Length,                //int vertexCount
                BufferUsage.None);              //BufferUsage
            vertex_buffer.SetData(vertices);

            //インデックス
            ushort[] indices = new ushort[]
            {
                0, 1, 2,
                2, 3, 0
            };

            //インデックスバッファ
            IndexBuffer index_buffer = new IndexBuffer(
                graphics,                       //GraphicsDevice
                IndexElementSize.SixteenBits,   //IndexElementSize
                indices.Length,                 //int indexCount
                BufferUsage.None);              //BufferUsage
            index_buffer.SetData(indices);

            //設定
            graphics.SetVertexBuffer(vertex_buffer);
            graphics.Indices = index_buffer;
        }

        //ビュー
        public void View(Vector3 _camera_pos)
        {
            view = Matrix.CreateLookAt(
                _camera_pos,                    //cameraPosition
                _camera_pos - Vector3.UnitZ,    //cameraTarget
                Vector3.Up);                    //cameraUpVector
        }

        //プロジェクション
        private void Projection()
        {
            projection = Matrix.CreateOrthographic(
                game1.GetScreenWidth(),     //width
                game1.GetScreenHeight(),    //height
                1.0f,                       //zNearPlane
                10.0f);                     //zFarPlane
        }

        //テクスチャを取得
        public Texture2D GetTexture(string _name)
        {
            Texture2D tex;

            if (textures.ContainsKey(_name))
            {
                tex = textures[_name];
            }
            else
            {
                tex = content.Load<Texture2D>(_name);
                textures.Add(_name, tex);//追加
            }

            return tex;
        }

        #region 描画

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="_tex">テクスチャ</param>
        /// <param name="_world">ワールド行列</param>
        /// <param name="_a">アルファ値</param>
        public void Draw(Texture2D _tex, Matrix _world, float _a = 1.0f)
        {
            sprite_e.Parameters["a"].SetValue(_a);
            Draw(sprite_e, _tex, _world);
        }

        /// <summary>
        /// 描画
        /// </summary>
        /// <param name="_effect">シェーダー</param>
        /// <param name="_tex">テクスチャ</param>
        /// <param name="_world">ワールド行列</param>
        /// <param name="_a">アルファ値</param>
        public void Draw(Effect _effect, Texture2D _tex, Matrix _world, float _a = 1.0f)
        {
            Matrix world =
                Matrix.CreateScale(_tex.Width, _tex.Height, 0.0f) *
                _world;

            _effect.Parameters["tex0"].SetValue(_tex);
            _effect.Parameters["world"].SetValue(world);
            _effect.Parameters["view_projection"].SetValue(view * projection);

            //描画
            foreach (var e in _effect.CurrentTechnique.Passes)
            {
                e.Apply();
                graphics.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 2);
            }
        }

        #endregion 描画

        //ゲッター
        public ContentManager GetContent() { return content; }
        public GraphicsDevice GetGraphics() { return graphics; }
        public int GetScreenWidth() { return game1.GetScreenWidth(); }
        public int GetScreenHeight() { return game1.GetScreenHeight(); }
    }
}
