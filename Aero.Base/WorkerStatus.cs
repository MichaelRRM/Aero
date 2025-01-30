namespace Aero.Base;

public enum WorkerStatus
{
    /// <summary>
    /// The worker was requested, but hasn't started yet.
    /// </summary>
    Pending,

    /// <summary>
    /// The worker has started and is in progress.
    /// </summary>
    Running,

    /// <summary>
    /// The worker finished successfully.
    /// </summary>
    Succeeded,

    /// <summary>
    /// The worker completed, but encountered warnings.
    /// </summary>
    CompletedWithWarnings,

    /// <summary>
    /// The worker completed, but encountered non-fatal errors.
    /// </summary>
    CompletedWithErrors,

    /// <summary>
    /// The worker ended unexpectedly
    /// </summary>
    Failed,

    /// <summary>
    /// The worker was canceled before finishing.
    /// </summary>
    Canceled
}