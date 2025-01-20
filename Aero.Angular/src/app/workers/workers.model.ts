export interface Worker {
  name: string
  description: string
  modules: Module[]
}

export interface Module {
  name: string
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
