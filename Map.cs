using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace Game2
{
    enum DIRECTION
    {
        UP    = 1 << 1, //0x02
        DOWN  = 1 << 2, //0x04
        LEFT  = 1 << 3, //0x08
        RIGHT = 1 << 4  //0x16
    }

    //マップ
    class Map
    {
        private static readonly int BLOCK_WIDTH = 24;   //ブロックの幅
        private static readonly int BLOCK_HEIGHT = 24;  //ブロックの高さ

        private Dictionary<int, Block> sample;//サンプル
        private Block[][] blocks;

        private CSV csv = new CSV();//csvファイル

        private Paint paint;//ペイント

        private Vector3 start_position;//スタート
        private Vector3 goal_position;//ゴール

        public Map(Scene _scene)
        {
            paint = new Paint(_scene);

            //サンプル
            sample = new Dictionary<int, Block>()
            {
                { -2, new Goal(_scene, true) },//ゴール
                { -1, new Start(_scene, true) },//スタート

                { 0, new None(_scene, true) },
                
                //AABBあり
                { 1, new Normal(_scene, true, 1) },
                { 2, new Normal(_scene, true, 2) },
                { 3, new Normal(_scene, true, 3) },
                { 4, new Normal(_scene, true, 4) },
                { 5, new Normal(_scene, true, 5) },
                { 6, new Normal(_scene, true, 6) },
                { 7, new Normal(_scene, true, 7) },
                { 8, new Normal(_scene, true, 8) },
                { 9, new Normal(_scene, true, 9) },
                //AABBなし
                { 11, new Normal(_scene, true, 11) },
                { 12, new Normal(_scene, true, 12) },
                { 13, new Normal(_scene, true, 13) },
                { 14, new Normal(_scene, true, 14) },
                { 15, new Normal(_scene, true, 15) },
                { 16, new Normal(_scene, true, 16) },
                { 17, new Normal(_scene, true, 17) },
                { 18, new Normal(_scene, true, 18) },
                { 19, new Normal(_scene, true, 19) },

                //刺
                { 21, new Thorn(_scene, true, 1) }, //上
                { 22, new Thorn(_scene, true, 2) }, //左
                { 23, new Thorn(_scene, true, 3) }, //下
                { 24, new Thorn(_scene, true, 4) }, //右

                //スイッチ
                { 25, new Switch(_scene, true) },
                { 26, new ON(_scene, true) },   //オン
                { 27, new OFF(_scene, true) },  //オフ

                //砲台
                { 31, new Battery(_scene, true, 1) },   //上
                { 32, new Battery(_scene, true, 2) },   //左
                { 33, new Battery(_scene, true, 3) },   //下
                { 34, new Battery(_scene, true, 4) },   //右

                //ビームの砲台
                { 36, new BeamBattery(_scene, true) },   //上
                { 37, new BeamBattery(_scene, true) },   //左
                { 38, new BeamBattery(_scene, true) },   //下
                { 39, new BeamBattery(_scene, true) },   //右

                //ガラス
                { 40, new Glass(_scene, true) },

                //歯車
                { 41, new Gear(_scene, true) },
                
                //ビームの砲台
                { 46, new BeamBattery(_scene, true, false, 1) },   //上
                { 47, new BeamBattery(_scene, true, false, 2) },   //左
                { 48, new BeamBattery(_scene, true, false, 3) },   //下
                { 49, new BeamBattery(_scene, true, false, 4) },   //右
            };
        }

        //マップを読み込む
        public void Load(int num)
        {
            if (blocks != null)
            {
                for (int i = 0; i < blocks.Length; i++)
                {
                    for (int j = 0; j < blocks[i].Length; j++)
                    {
                        blocks[i][j].SetState(State.Dead);
                    }
                }
            }

            paint.GetNones().Clear();
            paint.GetTrails().Clear();

            int[][] int_array = csv.Load("csv/map" + num);//csvファイル

            blocks = new Block[int_array.Length][];         //行の配列

            for (int y = 0; y < int_array.Length; y++)
            {
                blocks[y] = new Block[int_array[y].Length]; //列の配列

                for (int x = 0; x < int_array[y].Length; x++)
                {
                    Block b = (Block)sample[int_array[y][x]].Clone();

                    b.SetPosition(new Vector3(x * BLOCK_WIDTH, y * BLOCK_HEIGHT, 0.0f));//座標
                    b.SetX(x);//x
                    b.SetY(y);//y

                    blocks[y][x] = b;//追加
                    
                    if (int_array[y][x] == -1)
                        start_position = b.GetPosition();
                    if (int_array[y][x] == -2)
                        goal_position = b.GetPosition();
                }
            }

            ON();
        }
        
        public void ON()
        {
            for (int y = 0; y < blocks.Length; y++)
            {
                for (int x = 0; x < blocks[y].Length; x++)
                {
                    blocks[y][x].ON();
                }
            }
        }//オン
        
        public void OFF()
        {
            for (int y = 0; y < blocks.Length; y++)
            {
                for (int x = 0; x < blocks[y].Length; x++)
                {
                    blocks[y][x].OFF();
                }
            }
        }//オフ

        //衝突
        public bool Intersect(Vector3 _v, Actor _owner, ref DIRECTION _d)
        {
            //インデックスを計算
            int x = (int)((_v.X + BLOCK_WIDTH / 2.0f) / BLOCK_WIDTH);
            int y = (int)((_v.Y + BLOCK_HEIGHT / 2.0f) / BLOCK_HEIGHT);

            //範囲外か
            if (x < 0 || y < 0 || x > blocks[0].Length - 1 || y > blocks.Length - 1)
                return false;

            if (blocks[y][x] is None ||
                blocks[y][x] is OFF)
                return false;

            if (blocks[y][x] is Start ||
                blocks[y][x] is Goal)
            {
                _owner.Hit(blocks[y][x], _d);
                return false;
            }

            //DIRECTIONを計算
            Vector3 v = _v - blocks[y][x].GetPosition();//点へのベクトル
            if (Math.Abs(v.Y) > Math.Abs(v.X))
            {
                if (v.Y > 0.0f) _d |= DIRECTION.UP;    //上
                if (v.Y < 0.0f) _d |= DIRECTION.DOWN;  //下
            }
            if (Math.Abs(v.Y) < Math.Abs(v.X))
            {
                if (v.X < 0.0f) _d |= DIRECTION.LEFT;  //左
                if (v.X > 0.0f) _d |= DIRECTION.RIGHT; //右
            }

            _owner.Hit(blocks[y][x], _d);
            blocks[y][x].Hit(_owner, _d);

            return true;
        }

        //ゲッター
        public Block[][] GetBlocks() { return blocks; }
        public Paint GetPaint() { return paint; }
        public Vector3 GetStartPosition() { return start_position; }
        public Vector3 GetGoalPosition() { return goal_position; }
        public int GetBlockWidth() { return BLOCK_WIDTH; }
        public int GetBlockHeight() { return BLOCK_HEIGHT; }
    }

    //ゴール
    class Goal : Block
    {
        public Goal(Scene _scene, bool _is_sample, int _num = 1) : base(_scene, _is_sample, _num)
        {
            if (!is_sample)
            {
                SpriteComponent sc = new SpriteComponent(this, 90);
                sc.SetTexture(scene.GetRenderer().GetTexture("Texture/goal"));
            }
        }

        //コピー
        public override object Clone()
        {
            return new Goal(this);
        }

        //コピーコンストラクタ
        private Goal(Goal _goal) : this(_goal.scene, false, _goal.num) { }
    }

    //スタート
    class Start : Block
    {
        public Start(Scene _scene, bool _is_sample, int _num = 1) : base(_scene, _is_sample, _num) { }

        //コピー
        public override object Clone()
        {
            return new Start(this);
        }

        //コピーコンストラクタ
        private Start(Start _start) : this(_start.scene, false, _start.num) { }
    }
}
