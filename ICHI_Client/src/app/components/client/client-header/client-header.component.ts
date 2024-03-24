import { Component } from '@angular/core';
import { TokenService } from '../../../service/token.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-client-header',
  templateUrl: './client-header.component.html',
  styleUrl: './client-header.component.css',
})
export class ClientHeaderComponent {
  constructor(private tokenService: TokenService, private router: Router) {}
  signout() {
    this.tokenService.removeToken();
    this.router.navigate(['login']);
  }

  resetPassword() {
    this.router.navigate(['reset-password']);
  }
}
