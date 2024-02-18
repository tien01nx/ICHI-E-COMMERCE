import { Component } from '@angular/core';
import { AdminMenuComponent } from "../admin-menu/admin-menu.component";
import { AdminHomeComponent } from "../admin-home/admin-home.component";
import { AdminHeaderComponent } from "../admin-header/admin-header.component";
import { AdminFooterComponent } from "../admin-footer/admin-footer.component";

@Component({
    selector: 'app-admin-layout',
    standalone: true,
    templateUrl: './admin-layout.component.html',
    styleUrl: './admin-layout.component.css',
    imports: [AdminMenuComponent, AdminHomeComponent, AdminHeaderComponent, AdminFooterComponent]
})
export class AdminLayoutComponent {

}
