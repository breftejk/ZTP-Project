using ZTP_Project.Models;
using System.Threading.Tasks;

namespace ZTP_Project.Observers
{
    /// <summary>
    /// Represents an observer that receives activity log updates.
    /// </summary>
    public interface IActivityObserver
    {
        /// <summary>
        /// Updates the observer with a new activity log entry.
        /// </summary>
        /// <param name="log">The activity log.</param>
        Task UpdateAsync(ActivityLog log);
    }
}