namespace Microsoft.HpcAcm.Frontend.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading;
    using T = System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Microsoft.HpcAcm.Common.Dto;
    using Microsoft.HpcAcm.Common.Utilities;
    using Microsoft.WindowsAzure.Storage.Queue;
    using Microsoft.WindowsAzure.Storage.Table;
    using Newtonsoft.Json;

    [Route("v1/diagnostics")]
    public class DiagnosticsV1Controller : Controller
    {
        private readonly DataProvider provider;

        public DiagnosticsV1Controller(DataProvider provider)
        {
            this.provider = provider;
        }

        // GET v1/diagnostics/tests
        [HttpGet("tests")]
        public T.Task<IEnumerable<DiagnosticsTest>> GetDiagnosticsTestsAsync(CancellationToken token)
        {
            return this.provider.GetDiagnosticsTestsAsync(token);
        }

        // GET v1/diagnostcis?lastid=3&count=10&reverse=true
        [HttpGet()]
        public async T.Task<IActionResult> GetJobsAsync([FromQuery] int lastId, [FromQuery] int count = 1000, [FromQuery] bool reverse = false, CancellationToken token = default(CancellationToken))
        {
            return new OkObjectResult(await this.provider.GetJobsAsync(lastId, count, JobType.Diagnostics, reverse, token));
        }

        // GET v1/diagnostics/5
        [HttpGet("{jobid}")]
        public async T.Task<IActionResult> GetJobAsync(int jobId, CancellationToken token = default(CancellationToken))
        {
            var j = await this.provider.GetJobAsync(jobId, JobType.Diagnostics, token);
            return j == null ? (IActionResult)new NotFoundResult() : new OkObjectResult(j);
        }

        // GET v1/diagnostics/5/tasks?lastid=5&count=10&requeueCount=0
        [HttpGet("{jobid}/tasks")]
        public async T.Task<IActionResult> GetJobTasksAsync(
            int jobId,
            [FromQuery] int lastId = 0,
            [FromQuery] int count = 1000,
            [FromQuery] int requeueCount = 0,
            CancellationToken token = default(CancellationToken))
        {
            var tasks = await this.provider.GetTasksAsync(
                jobId,
                requeueCount,
                lastId,
                count,
                JobType.Diagnostics,
                token);

            return new OkObjectResult(tasks);
        }

        // GET v1/diagnostics/5/tasks/10?requeueCount=0
        [HttpGet("{jobid}/tasks/{taskid}")]
        public async T.Task<IActionResult> GetJobTaskAsync(
            int jobId,
            int taskId = 0,
            [FromQuery] int requeueCount = 0,
            CancellationToken token = default(CancellationToken))
        {
            var task = await this.provider.GetTaskAsync(
                jobId,
                requeueCount,
                taskId,
                JobType.Diagnostics,
                token);

            return task == null ? (IActionResult)new NotFoundResult() : new OkObjectResult(task);
        }

        // GET v1/diagnostics/5/tasks/10/result?requeueCount=0
        [HttpGet("{jobid}/tasks/{taskid}/result")]
        public async T.Task<IActionResult> GetJobTaskResultAsync(
            int jobId,
            int taskId = 0,
            [FromQuery] int requeueCount = 0,
            CancellationToken token = default(CancellationToken))
        {
            var task = await this.provider.GetTaskResultAsync(
                jobId,
                requeueCount,
                taskId,
                JobType.Diagnostics,
                token);

            return task == null ? (IActionResult)new NotFoundResult() : new OkObjectResult(task);
        }

        [HttpGet("testnewjob/{nodes}")]
        public async T.Task<int> TestCreateJobAsync(string nodes, CancellationToken token)
        {
            var job = new Job()
            {
                Name = "diag-test-test",
                RequeueCount = 0,
                State = JobState.Queued,
                TargetNodes = nodes.Split(','),
                Type = JobType.Diagnostics,
                DiagnosticTest = new DiagnosticsTest() { Category = "test", Name = "test", Arguments = "some arg" }
            };

            return await this.provider.CreateJobAsync(job, token);
        }

        // POST v1/diagnostics
        [HttpPost()]
        public async T.Task<IActionResult> CreateJobAsync([FromBody] Job job, CancellationToken token)
        {
            job.Type = JobType.Diagnostics;
            int id = await this.provider.CreateJobAsync(job, token);
            return new CreatedResult($"/v1/diagnostics/{id}", null);
        }

        // PATCH v1/diagnostics/5
        [HttpPatch("{jobid}")]
        public async T.Task<IActionResult> PatchJobAsync(int jobId, [FromBody] Job job, CancellationToken token)
        {
            job.Id = jobId;
            job.Type = JobType.Diagnostics;
            return await this.provider.PatchJobAsync(job, token);
        }
    }
}
