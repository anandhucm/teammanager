import { Injectable, signal } from '@angular/core';

export type toastrStatus = "Success" | "Error" | "Warning" | "Info";
export interface toastrContent {
   message: string;
   status: toastrStatus;
}

@Injectable({
  providedIn: 'root',
})
export class ToastrService {

  public toast = signal<toastrContent | null>(null);

  showToast(message: string, status: toastrStatus = "Info", duration: number = 3000){
     this.toast.set({message, status});
     setTimeout(() => {
      this.toast.set(null);
     }, duration);
  } 

  closeToast(){
    this.toast.set(null);
  }
  
}
