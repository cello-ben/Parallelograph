namespace Parallelograph.Models
{
    internal class ParallelChecker
    {
        private bool hasParallelFifths = false;
        private bool hasParallelOctaves = false;

        public ParallelChecker()
        {
            
        }

        public bool HasParallelFifths()
        {
            return hasParallelFifths;
        }

        public bool HasParallelOctaves()
        {
            return hasParallelOctaves;
        }

        private void CheckParallelFifths()
        {
            hasParallelFifths = true;
        }

        private void CheckParallelOctaves()
        {
            hasParallelOctaves = true;
        }
        public void CheckParallels()
        {
            CheckParallelFifths();
            CheckParallelOctaves();
        }
    }
}