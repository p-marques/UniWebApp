export interface IDialogData<T, Y> {
  success: boolean;
  responseObject: T;
  options: Y[];
}
