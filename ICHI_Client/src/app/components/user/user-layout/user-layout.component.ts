import { Component } from '@angular/core';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { UserHeaderComponent } from '../user-header/user-header.component';
import { UserHomeComponent } from '../user-home/user-home.component';
import { UserFooterComponent } from '../user-footer/user-footer.component';
import { UserMenuComponent } from "../user-menu/user-menu.component";
import { AdminFooterComponent } from "../../admin/admin-footer/admin-footer.component";

@Component({
    selector: 'app-user-layout',
    standalone: true,
    templateUrl: './user-layout.component.html',
    styleUrls: ['./user-layout.component.css'],
    imports: [UserHeaderComponent, UserFooterComponent, UserHomeComponent, UserMenuComponent, AdminFooterComponent]
})
export class UserLayoutComponent {}
