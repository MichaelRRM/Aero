import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { TranslateModule } from '@ngx-translate/core';
import { SystemStatusService } from './system.status.service';

@Component({
  selector: 'app-system-status',
  standalone: true,
  imports: [CommonModule, TranslateModule],
  templateUrl: './system-status.component.html',
})
export class SystemStatusComponent implements OnInit {
  webStatus = false;
  workerStatus = false;

  constructor(private statusService: SystemStatusService) {}

  ngOnInit() {
    this.statusService
      .checkWebStatus()
      .subscribe((status) => (this.workerStatus = true));


    this.statusService
      .checkWorkerStatus()
      .subscribe((status) => (this.workerStatus = true));
  }
}
