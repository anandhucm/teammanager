import { Component, inject } from '@angular/core';
import { AccountService } from '../../core/service/account-service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav-top',
  imports: [],
  templateUrl: './nav-top.html',
  styleUrl: './nav-top.css',
})
export class NavTop {

  protected accountService = inject(AccountService);
  protected router = inject(Router);

  logout(){
    this.accountService.logout();
    this.router.navigate(['/logout']);
  }
}
