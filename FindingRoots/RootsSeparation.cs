namespace FindingRoots
{
    /// <summary>
    /// Class for separating roots
    /// </summary>
    public class RootsSeparation
    {
        public List<(int left, int right)> Result { get; set; } = new List<(int left, int right)> ();

       // private readonly int N;
       // private int counter = 0;
        private readonly int A;
        private readonly int B;
        private readonly Func<int, int> func;
        private readonly int step;

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="A">Left bound of the segment</param>
        /// <param name="B">Right bound of the segment</param>
        /// <param name="N">Constant value for counting step</param>
        /// <param name="func">Function</param>
        public RootsSeparation(int A, int B, int N, Func<int, int> func)
        {
            //this.N = N;
            this.A = A;
            this.B = B;
            this.func = func;
            step = (B - A) / N;
            Separation();
        }

        public void Separation()
        {
           // List<(int left, int right)> boundaries = new();
            int x1 = A;
            int x2 = x1 + step;
            int y1 = func(x1);
            while (x2 <= B)
            {
                int y2 = func(x2);
                if (y1 * y2 <= 0)
                {
                   // counter++;
                    Result.Add((x1, x2));
                }
                x1 = x2;
                x2 += step;
                y1 = y2;
            }
        }
    }
}
