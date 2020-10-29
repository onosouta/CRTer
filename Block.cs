using System;

namespace Game2
{
    //ブロック
    abstract class Block : Actor, ICloneable
    {
        protected bool is_sample;//サンプル
        protected int num;

        protected int x, y;//インデックス

        public Block(Scene _scene, bool _is_sample, int _num = 1) : base(_scene)
        {
            is_sample = _is_sample;
            num = _num;
        }

        //ON/OFF
        public virtual void ON() { }
        public virtual void OFF() { }
        
        //コピー
        public abstract object Clone();

        //ゲッター
        public int GetNum() { return num; }
        public int GetX() { return x; }
        public int GetY() { return y; }

        //セッター
        public void SetX(int _x) { x = _x; }
        public void SetY(int _y) { y = _y; }
    }
}
