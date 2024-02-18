import { Component } from '@angular/core';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { UserHeaderComponent } from '../user-header/user-header.component';
import { UserHomeComponent } from '../user-home/user-home.component';
import { UserFooterComponent } from '../user-footer/user-footer.component';

@Component({
  selector: 'app-user-layout',
  standalone: true,
  imports: [UserHeaderComponent, UserFooterComponent, UserHomeComponent],
  templateUrl: './user-layout.component.html',
  styleUrls: ['./user-layout.component.css'],
})
export class UserLayoutComponent {}
