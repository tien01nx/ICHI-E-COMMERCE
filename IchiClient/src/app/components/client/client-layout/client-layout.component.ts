import { Component } from '@angular/core';
import { ClientHeaderComponent } from "../client-header/client-header.component";
import { ClientMenuComponent } from "../client-menu/client-menu.component";
import { HomeComponent } from "../home/home.component";
import { ClientFooterComponent } from "../client-footer/client-footer.component";

@Component({
    selector: 'app-client-layout',
    standalone: true,
    templateUrl: './client-layout.component.html',
    styleUrl: './client-layout.component.css',
    imports: [ClientHeaderComponent, ClientMenuComponent, HomeComponent, ClientFooterComponent]
})
export class ClientLayoutComponent {

}
