import { Component } from '@angular/core';
import { AdminMenuComponent } from '../admin-menu/admin-menu.component';
import { AdminHomeComponent } from '../admin-home/admin-home.component';
import { AdminHeaderComponent } from '../admin-header/admin-header.component';
import { AdminFooterComponent } from '../admin-footer/admin-footer.component';
import { RouterOutlet } from '@angular/router';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-admin-layout',
  templateUrl: './admin-layout.component.html',
  styleUrl: './admin-layout.component.css',
})
export class AdminLayoutComponent {}
