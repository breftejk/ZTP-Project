using ZTP_Project.Models;

namespace ZTP_Project.Learning.Activities
{
    /// <summary>
    /// A notifier that informs observers about new activity logs.
    /// </summary>
    public class ActivityNotifier
    {
        private readonly List<IActivityObserver> _observers = new();

        /// <summary>
        /// Attaches an observer to the notifier.
        /// </summary>
        /// <param name="observer">The observer to attach.</param>
        public void Attach(IActivityObserver observer)
        {
            _observers.Add(observer);
        }

        /// <summary>
        /// Detaches an observer from the notifier.
        /// </summary>
        /// <param name="observer">The observer to detach.</param>
        public void Detach(IActivityObserver observer)
        {
            _observers.Remove(observer);
        }

        /// <summary>
        /// Notifies all attached observers with a new activity log entry.
        /// </summary>
        /// <param name="log">The activity log.</param>
        public async Task NotifyAsync(ActivityLog log)
        {
            foreach (var obs in _observers)
            {
                await obs.UpdateAsync(log);
            }
        }
    }
}