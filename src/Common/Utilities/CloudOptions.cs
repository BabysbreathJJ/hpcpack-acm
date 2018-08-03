﻿using System;

namespace Microsoft.HpcAcm.Common.Utilities
{
    public class CloudOptions
    {
        #region Credential
        //public string StorageKeyOrSas { get; set; } = "?sv=2017-07-29&ss=bfqt&srt=sco&sp=rwdlacup&se=2019-04-02T18:35:39Z&st=2018-03-05T10:35:39Z&spr=https&sig=MO2wFOAcvGCQr7h1sasw8SWQAWME%2BfM58XKdJgKkOuY%3D";
        //public string AccountName { get; set; } = "evanc";
        //public string StorageKeyOrSas { get; set; } = "?sv=2017-11-09&ss=bfqt&srt=sco&sp=rwdlacup&se=2019-07-18T05:11:07Z&st=2018-07-17T21:11:07Z&spr=https,http&sig=ptJpQOi0A24XYe6UOsnswTG%2FcE6o2BOx%2BwdwG4%2FG9BM%3D";
        //public string AccountName { get; set; } = "evancslurmst";
        public string StorageKeyOrSas { get; set; } = "?sv=2017-07-29&ss=bfqt&srt=sco&sp=rwdlacup&se=2019-04-24T18:19:28Z&st=2018-04-25T10:19:28Z&spr=https&sig=pYCVmT40eW54msV7P9F%2BMhBwPUbHr0HYGHvogafCs1I%3D";
        public string AccountName { get; set; } = "evanchpcacm";
        public string ConnectionString { get; set; }

        #endregion

        #region Server operations

        public int QueueServerTimeoutSeconds { get; set; } = 30;
        public int TableServerTimeoutSeconds { get; set; } = 30;
        public int VisibleTimeoutSeconds { get; set; } = 60;

        #endregion

        #region Metrics table

        public string MetricsTableName { get; set; } = "metricstable";

        // TODO: scale out by metric category
        public string MetricsValuesPartitionKey { get; set; } = "metricsvalues";
        public string MetricsCategoriesPartitionKey { get; set; } = "metricscategories";

        #endregion

        #region Ids table

        public string IdsTableName { get; set; } = "idstable";

        #endregion

        #region Nodes table

        public string NodesTableName { get; set; } = "nodestable";
        public string NodePartitionPattern { get; internal set; } = "node-{0}";
        public string MinuteHistoryPattern { get; internal set; } = "history-minute-{0}";
        public string MinuteHistoryKey { get; internal set; } = "history-minute";
        public string ScheduledEventsKey { get; internal set; } = "scheduled-events";
        public string MetadataKey { get; internal set; } = "metadata-instance";
        public string RegistrationPattern { get; set; } = "registration-{0}";
        public string HeartbeatPattern { get; internal set; } = "heartbeat-{0}";
        public string NodesPartitionKey { get; internal set; } = "nodes";

        public int HeartbeatIntervalSeconds { get; set; } = 30;
        public int MaxMissedHeartbeats { get; set; } = 3;
        public int RegistrationIntervalSeconds { get; set; } = 300;
        public int RetryOnFailureSeconds { get; set; } = 5;

        #endregion

        #region Jobs table

        public string DashboardTableName { get; set; } = "dashboardtable";
        public string JobsTableName { get; set; } = "jobstable";
        public string JobEntryKey { get; internal set; } = "jobentry";
        public string JobAggregationResultPattern { get; internal set; } = "aggregationresult-{0}";
        public string NodeTaskResultPattern { get; internal set; } = "nodejobresult-{0}-{1}";
        public string JobReversePartitionPattern { get; internal set; } = "jobreverse-{0}-{1}";
        public string JobPartitionPattern { get; internal set; } = "job-{0}-{1}";
        public string DiagnosticCategoryPattern { get; internal set; } = "diag-{0}";
        public string DashboardRowKeyPattern { get; internal set; } = "range-{0}";
        public string DashboardPartitionPattern { get; internal set; } = "dashboard-{0}";
        public string DashboardEntryKey { get; internal set; } = "entry";

        #endregion

        #region Blobs

        public string JobResultContainerPattern { get; set; } = "jobresults-{0}";
        public string TaskChildrenContainerName { get; set; } = "taskchildren";

        #endregion

        #region Queue

        public string TaskCompletionQueueName { get; set; } = "taskcompletionqueue";
        public string JobEventQueueName { get; set; } = "jobeventqueue";
        public string NodeDispatchQueuePattern { get; set; } = "nodedispatchqueue-{0}";
        public string NodeCancelQueuePattern { get; set; } = "nodecancelqueue-{0}";

        #endregion
    }
}
