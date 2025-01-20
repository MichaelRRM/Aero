import { Component, Input, OnInit } from '@angular/core';
import { Worker } from './workers.model';
import { WorkerService } from './workers.service';

@Component({
  selector: 'app-workers',
  standalone: true,
  imports: [],
  templateUrl: './workers.component.html',
})
export class WorkersComponent implements OnInit {
  ngOnInit(): void {
    this.workerService.getWorkers().subscribe((data) => (this.workers = data));
  }
  workers: Worker[] = [];

  constructor(private workerService: WorkerService) {}

  scrollIntoView(elementId: string) {
    const el = document.getElementById(elementId);
    if (el) {
      el.scrollIntoView({ behavior: 'smooth', block: 'start' });
    }
  }
}
