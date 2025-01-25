import { Component, Input, OnChanges, OnInit, SimpleChanges } from '@angular/core';
import { Worker } from './workers.model';
import { WorkerService } from './workers.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-workers',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './workers.component.html',
})
export class WorkersComponent implements OnInit {

  workers: Worker[] = [];
  formGroups = new Map<string, FormGroup>();

  selectedWorker: string | null = null;
  selectedModule: string | null = null;
  constructor(private workerService: WorkerService, private fb: FormBuilder) {}

  ngOnInit() {
    this.workerService.getWorkers().subscribe((data) => {
      this.workers = data;
      if (this.workers.length > 0) {
        this.selectedWorker = this.workers[0].name;
        this.initializeForms();
      }
    });
  }

  private initializeForms() {
    this.workers.forEach(worker => {
      worker.modules.forEach(module => {
        const group: Record<string, any> = {};
        module.arguments.forEach(arg => {

          let defaultValue : string | boolean | number | undefined | null = arg.defaultValue;
          switch(arg.type) {
            case 'boolean':
              defaultValue = defaultValue === 'true';
              break;
            case 'number':
              defaultValue = defaultValue ? parseFloat(defaultValue) : null;
              break;
            case 'text':
              defaultValue = defaultValue || '';
              break;
            case 'date':
              defaultValue = defaultValue || null;
              break;
          }

          group[arg.name] = [[defaultValue]];
        });

        this.formGroups.set(this.getFormGroupKey(worker.name, module.name), this.fb.group(group));
      });
    });
  }

  getFormGroup(workerName: string, moduleName: string): FormGroup {
    const formGroup = this.formGroups.get(this.getFormGroupKey(workerName, moduleName));
    if (!formGroup) {
      console.error(`Form group not found for worker: ${workerName}, module: ${moduleName}`);
      return this.fb.group({});
    }
    return formGroup;
  }

  private getFormGroupKey(workerName: string, moduleName: string): string {
    return `${workerName}-${moduleName}`;
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
    const formGroup = this.formGroups.get(this.getFormGroupKey(workerName, moduleName));
    if (formGroup?.valid) {
      console.log('Running module with arguments:', formGroup.value);
      // TODO : run module here
    }
  }
}
