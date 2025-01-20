import { Component, Input, OnInit } from '@angular/core';
import { Worker } from './workers.model';
import { WorkerService } from './workers.service';
import { FormBuilder, FormGroup, ReactiveFormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-workers',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './workers.component.html',
})
export class WorkersComponent implements OnInit {

  workers: Worker[] = [];

  selectedWorker: string | null = null;
  selectedModule: string | null = null;
  formGroups: Map<string, FormGroup> = new Map();

  constructor(private workerService: WorkerService, private fb: FormBuilder) {}


  ngOnInit() {
    this.workerService.getWorkers().subscribe((data) => {
      this.workers = data;

    // Initialize form groups for each module
      this.workers.forEach(worker => {
        worker.modules.forEach(module => {
          const group: Record<string, any> = {};
          module.arguments.forEach(arg => {
            group[arg.name] = [arg.defaultValue || '', arg.isRequired];
          });
          this.formGroups.set(`${worker.name}-${module.name}`, this.fb.group(group));
        });
      });
    });
  }

  getFormGroup(workerName: string, moduleName: string): FormGroup {
    return this.formGroups.get(`${workerName}-${moduleName}`) || this.fb.group({});
  }

  scrollToWorker(workerName: string) {
    this.selectedWorker = workerName;
    this.selectedModule = null;
    document.getElementById(workerName)?.scrollIntoView({ behavior: 'smooth' });
  }

  scrollToModule(workerName: string, moduleName: string) {
    this.selectedWorker = workerName;
    this.selectedModule = moduleName;
    document.getElementById(`${workerName}-${moduleName}`)?.scrollIntoView({ behavior: 'smooth' });
  }

  runModule(workerName: string, moduleName: string) {
    const formGroup = this.getFormGroup(workerName, moduleName);
    if (formGroup.valid) {
      console.log('Running module with arguments:', formGroup.value);
      // Implement your run logic here
    }
  }
}
