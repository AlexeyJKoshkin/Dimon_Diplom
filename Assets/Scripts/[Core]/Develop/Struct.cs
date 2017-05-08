namespace Game.Scripts
{
    public interface ICoor
    {
        int X { get; }

        int Y { get; }
    }

    [System.Serializable]
    public struct Coordinata : ICoor
    {
        public int X { get { return _x; } set { _x = value; } }

        private int _x;

        public int Y { get { return _y; } set { _y = value; } }

        private int _y;

        public Coordinata(int x, int y)
        {
            _x = x;
            _y = y;
        }

        public Coordinata(float x, float y)
        {
            _x = (int)x;
            _y = (int)y;
        }

        public Coordinata(Coordinata coor)
        {
            _x = coor.X;
            _y = coor.Y;
        }

        public Coordinata(UnityEngine.Vector2 vec)
        {
            _x = (int)vec.x;
            _y = (int)vec.y;
        }

        public Coordinata(ICoor coor)
        {
            _x = coor.X;
            _y = coor.Y;
        }

        public Coordinata IncreaseX
        {
            get { return new Coordinata() { X = this.X + 1, Y = this.Y }; }
        }

        public Coordinata IncreaseY
        {
            get { return new Coordinata() { X = this.X, Y = this.Y + 1 }; }
        }

        public Coordinata DecreaseX
        {
            get { return new Coordinata() { X = this.X - 1, Y = this.Y }; }
        }

        public Coordinata DecreaseY
        {
            get { return new Coordinata() { X = this.X, Y = this.Y - 1 }; }
        }

        public override bool Equals(object obj)
        {
            if (System.Object.ReferenceEquals(this, obj))
                return true;
            if (obj is Coordinata)
            {
                if (this.X != ((Coordinata)obj).X)
                    return false;
                if (this.Y != ((Coordinata)obj).Y)
                    return false;
                return true;
            }
            else return false;
        }

        public void IncX()
        {
            this = this.IncreaseX;
        }

        public void IncY()
        {
            this = this.IncreaseY;
        }

        public void DecX()
        {
            this = this.DecreaseX;
        }

        public void DecY()
        {
            this = this.DecreaseY;
        }

        public static Coordinata operator +(Coordinata a, Coordinata b)
        {
            a._x += b._x;
            a._y += b._y;
            return a;
        }

        public static bool operator ==(Coordinata a, Coordinata b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // Return true if the fields match:
            return a.X == b.X && a.Y == b.Y;
        }

        public static bool operator !=(Coordinata a, Coordinata b)
        {
            return !(a == b);
        }

        public Coordinata Set(int iX, int iY)
        {
            this._x = iX;
            this._x = iY;
            return this;
        }

        public override string ToString()
        {
            return string.Format("[{0} - {1}]", X, Y);
        }

        public override int GetHashCode()
        {
            return this.X ^ this.Y;
        }
    }
}