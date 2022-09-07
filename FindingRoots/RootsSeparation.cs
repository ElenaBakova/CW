namespace FindingRoots
{
    public class RootsSeparation
    {
        //public (int left, int right) Boundaries { get; private init; }
        private int A;
        private int B;
        private readonly int N;
        private int counter = 0;
        private Func<int, int> func;
        private readonly int step;

        public RootsSeparation(int A, int B, int N, Func<int, int> func)
        {
            this.A = A;
            this.B = B;
            this.N = N;
            this.func = func;
            step = (B - A) / N;
        }

        public void Separation()
        {
            
        }
    }
}
