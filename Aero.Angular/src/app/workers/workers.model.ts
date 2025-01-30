export interface Worker {
  name: string
  description: string
  modules: Module[]
}

export interface Module {
  name: string
  code: string
  description: string
  arguments: Argument[]
}

export interface Argument {
  name: string
  description: string
  type: string
  defaultValue?: string
  isRequired: boolean
}

export interface WorkerLaunchResponse {
  workerId: number;
  workerStatus: WorkerStatus;
}

export interface WorkerLaunchRequest {
  workerName: string;
  module: string;
  arguments?: string[];
}

export enum WorkerStatus {
  Pending = "Pending",
  Running = "Running",
  Succeeded = 'Succeeded',
  CompletedWithWarnings = 'CompletedWithWarnings',
  CompletedWithErrors = 'CompletedWithErrors',
  Failed = 'Failed',
  Canceled = 'Canceled'
}
