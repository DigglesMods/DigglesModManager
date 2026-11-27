namespace DigglesModManager
{
    /// <summary>
    /// Receives progress information of long running modding operations (e.g. to update a progress bar).
    /// </summary>
    public interface IProgressReporter
    {
        void Increment(int increment = 1);
    }

    /// <summary>
    /// Progress reporter that ignores all progress information.
    /// </summary>
    public sealed class NullProgressReporter : IProgressReporter
    {
        public static readonly NullProgressReporter Instance = new NullProgressReporter();

        public void Increment(int increment = 1)
        {
        }
    }
}
