import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ToastrService } from '../../core/service/toastr-service';

@Component({
  selector: 'app-toastr',
  imports: [CommonModule],
  templateUrl: './toastr.html',
  styleUrl: './toastr.css',
})
export class Toastr {

  protected toastrService = inject(ToastrService);
  public toast = this.toastrService.toast;

  protected closeToast(){
    this.toastrService.closeToast();
  }

}
