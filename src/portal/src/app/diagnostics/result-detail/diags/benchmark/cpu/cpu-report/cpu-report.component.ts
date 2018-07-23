import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { MatTableDataSource } from '@angular/material';
import { ApiService, Loop } from '../../../../../../services/api.service';
import { TableSettingsService } from '../../../../../../services/table-settings.service';

@Component({
  selector: 'app-cpu-report',
  templateUrl: './cpu-report.component.html',
  styleUrls: ['./cpu-report.component.scss']
})
export class CpuReportComponent implements OnInit {

  @Input() result: any;

  private dataSource = new MatTableDataSource();

  private jobId: string;
  private interval: number;
  private tasksLoop: Object;
  private jobState: string;
  private tasks = [];
  private events = [];
  private nodes = [];
  private aggregationResult: any;

  private componentName = "cpuReport";

  private customizableColumns = [
    { name: 'node', displayName: 'Node', displayed: true },
    { name: 'size', displayName: 'Size', displayed: true },
  ];

  constructor(
    private api: ApiService,
    private settings: TableSettingsService,
  ) {
    this.interval = 5000;
  }

  ngOnInit() {
    this.jobId = this.result.id;
    if (this.result.aggregationResult !== undefined) {
      this.aggregationResult = this.result.aggregationResult;
    }
    this.tasksLoop = this.getTasksInfo();
  }

  get hasError() {
    return this.aggregationResult !== undefined && this.aggregationResult['Error'] !== undefined;
  }

  getTasksInfo(): any {
    return Loop.start(
      this.api.diag.getDiagTasks(this.jobId),
      {
        next: (result) => {
          this.dataSource.data = result;
          this.tasks = result;
          if (this.jobState == 'Finished' || this.jobState == 'Failed' || this.jobState == 'Canceled') {
            this.getAggregationResult();
            return false;
          }
          else {
            this.getJobInfo();
            return true;
          }

        }
      },
      this.interval
    );
  }

  ngOnDestroy() {
    if (this.tasksLoop) {
      Loop.stop(this.tasksLoop);
    }
  }

  getJobInfo() {
    this.api.diag.getDiagJob(this.result.id).subscribe(res => {
      this.jobState = res.state;
      this.result = res;
      this.nodes = res.targetNodes;
      if (res.events !== undefined) {
        this.events = res.events;
      }
    });
  }

  getAggregationResult() {
    this.api.diag.getJobAggregationResult(this.result.id).subscribe(
      res => {
        this.aggregationResult = res;
      },
      err => {
        let errInfo = err;
        if (ApiService.isJSON(err)) {
          if (err.error) {
            errInfo = err.error;
          }
          else {
            errInfo = JSON.stringify(err);
          }
        }
        this.aggregationResult = { Error: errInfo };
      });
  }

  getLink(node) {
    let path = [];
    path.push('/resource');
    path.push(node);
    return path;
  }
}