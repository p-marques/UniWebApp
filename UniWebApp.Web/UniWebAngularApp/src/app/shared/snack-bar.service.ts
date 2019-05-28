import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material';

@Injectable({
  providedIn: 'root'
})
export class SnackBarService {

  constructor(private snackBar: MatSnackBar) { }

  public showSnackBar(text: string, actionText?: string, closeAfter?: number): void {
    this.snackBar.open(text, actionText || 'OK', { duration: closeAfter || null });
  }
}
