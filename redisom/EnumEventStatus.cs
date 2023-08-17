using System;
using System.ComponentModel;

namespace redisom
{
    public enum EnumEventStatus
    {
        /// <summary>
        /// Fresh new, whose D-Day hasn't arrived yet.
        /// </summary>
        [Description("In queue")]
        UnExecute = 0,

        /// <summary>
        /// Ongoing.
        /// </summary>
        [Description("Executing")]
        Executing = 1,

        /// <summary>
        /// Well done. Die a natural death.
        /// </summary>
        [Description("Finished")]
        Executed = 2,

        /// <summary>
        /// Something went wrong during start or end.
        /// </summary>
        [Obsolete("Use ExecutedFailedAndToWait if failed, so we can try to recover later.")]
        [Description("Failed")]
        ExecuteFailed = 3,

        /// <summary>
        /// At D-Day, before start, the conditions do not match. Pending for everything getting ready. Such as:
        /// Peer not paired.
        /// Peer is live with someone else.
        /// Peer is not onair.
        /// </summary>
        [Description("Pending")]
        ExecutedFailedAndToWait = 4,

        /// <summary>
        /// Interrupted by user action. Never ever getting back together.
        /// </summary>
        [Description("Interrupted")]
        ExecutedInterrupted = 5,

        /// <summary>
        /// Expired, never got a single chance to run.
        /// </summary>
        [Description("Expired")]
        Expired = 6,

        /// <summary>
        /// At start time, R is already live with target T. So report to CC as "carry on".
        /// At end time, don't stop live. Let the original live session continue.
        /// </summary>
        [Description("Executing in passing")]
        ExecutingInPassing = 7,

        /// <summary>
        /// Add the schedule by Playout
        /// </summary>
        [Description("Added event successfully")]
        EventAdded = 8,

        /// <summary>
        /// Delete the schedule by Playout
        /// </summary>
        [Description("Deleted event successfully")]
        EventDeleted = 9,

        /// <summary>
        /// Update the schedule by Playout
        /// </summary>
        [Description("Updated event successfully")]
        EventUpdated = 10,

        /// <summary>
        /// Event is missing
        /// </summary>
        [Description("Event missing")]
        EventMissing = 11,

        /// <summary>
        /// Event is duplication
        /// </summary>
        [Description("Event missing")]
        EventDuplication = 12,

        /// <summary>
        /// Executing event cant be updated
        /// </summary>
        [Description("Executing event cant be updated")]
        ExecutingEventUpdateFailed = 13,

        /// <summary>
        /// Event source missing
        /// </summary>
        [Description("Event source missing")]
        EventSourceMissing = 14,

        /// <summary>
        /// Event source missing
        /// </summary>
        [Description("Event source premature end")]
        PrematureEnd = 15,

        /// <summary>
        /// Waiting end slate
        /// </summary>
        [Description("Waiting end slate")]
        WaitingEndSlate = 16,

        /// <summary>
        /// Preload event
        /// </summary>
        [Description("Preloading source")]
        Preload = -1,

        /// <summary>
        /// Event doesn't exist
        /// </summary>
        [Description("Event doesn't exist")]
        NoEvent = -2,

        /// <summary>
        /// Event exception
        /// </summary>
        [Description("Event exception")]
        Exception = -3,

        /// <summary>
        /// End preload event
        /// </summary>
        [Description("End preload source")]
        EndPreload = -4,
    }
}